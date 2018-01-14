using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace X360DebuggerWV
{
    public static class Disassembler
    {
        private static string[] DisassembleBlock(byte[] buff)
        {
            List<string> disasm = new List<string>();
            string cmd = "@echo off\n";
            cmd += "cstool ppc64be \"";
            for (int i = 0; i < buff.Length / 4; i++)
            {
                cmd += buff[i * 4].ToString("X2");
                cmd += buff[i * 4 + 1].ToString("X2");
                cmd += buff[i * 4 + 2].ToString("X2");
                cmd += buff[i * 4 + 3].ToString("X2");
            }
            cmd += "\"\n";
            File.WriteAllText("batch.bat", cmd);
            string result = Helper.RunShell("batch.bat", "");
            string[] lines = result.Split('\n');
            int count = 0;
            foreach (string line in lines)
            {
                if (++count == lines.Length)
                    break;
                result = line;
                while (result.Contains("  "))
                    result = result.Replace("  ", " ");
                result = result.Trim();
                if (line == "")
                {
                    disasm.Add("");
                    continue;
                }
                string[] parts = result.Split(' ');
                result = "";
                for (int i = 2; i < parts.Length; i++)
                    result += parts[i] + " ";
                disasm.Add(result);
            }
            while (disasm.Count < buff.Length / 4)
                disasm.Add("");
            return disasm.ToArray();
        }

        public static string[] Disassemble(byte[] buff, ToolStripProgressBar pb)
        {
            List<string> result = new List<string>();
            uint pos = 0;
            uint size;
            MemoryStream m = new MemoryStream(buff);
            m.Seek(0, 0);
            pb.Maximum = buff.Length;
            while (pos < buff.Length)
            {
                size = 400;
                if (buff.Length - pos < size)
                    size = (uint)buff.Length - pos;
                byte[] buf = new byte[size];
                m.Read(buf, 0, (int)size);
                string[] tmp = DisassembleBlock(buf);
                result.AddRange(tmp);
                pos += size;
                pb.Value = (int)pos;
                Application.DoEvents();
            }
            pb.Value = 0;
            return result.ToArray();
        }

        public static string ExportForDecompiling(string[] input, byte[] buff, uint address)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("# =============== S U B	R O U T	I N E =======================================");
            sb.AppendLine();
            sb.AppendLine("sub_" + address.ToString("X8") + ":");
            uint pos = address;
            List<uint> FoundTargets = new List<uint>();
            for (int i = 0; i < buff.Length; i += 4)
            {
                uint opc = PPC.SwapEndian(BitConverter.ToUInt32(buff, i));
                uint target = 0;
                if (PPC.isBranchOpc(opc) && PPC.calcBranchTarget(opc, pos + (uint)i, out target) && target > address && target < address + buff.Length)
                        FoundTargets.Add(target);
            }
            foreach (string line in input)
            {
                if (FoundTargets.Contains(pos))
                    sb.AppendLine("loc_" + pos.ToString("X8") + ":");
                uint opc = PPC.SwapEndian(BitConverter.ToUInt32(buff, (int)(pos - address)));
                uint target = 0;
                if (PPC.isBranchOpc(opc) && PPC.calcBranchTarget(opc, pos, out target))
                {
                    int end = line.IndexOf("0x");
                    string tmp = line.Substring(0, end);
                    if (FoundTargets.Contains(target))
                        sb.AppendLine("\t\t" + tmp + "loc_" + target.ToString("X8"));
                    else
                        sb.AppendLine("\t\t" + tmp + "sub_" + target.ToString("X8"));
                }
                else
                    sb.AppendLine("\t\t" + line);
                pos += 4;
            }
            sb.AppendLine("# End of function sub_" + address.ToString("X8"));
            return sb.ToString();
        }

        public static Dictionary<uint, int> DFSLookUp;

        public static int GetFunctionSize(byte[] buff, uint start)
        {
            DFSLookUp = new Dictionary<uint, int>();
            return DFSBranchSearch(buff, start, 0);
        }

        public static int DFSBranchSearch(byte[] buff, uint start, int pos)
        {
            if (DFSLookUp.ContainsKey((uint)(start + pos * 4)))
                return DFSLookUp[(uint)(start + pos * 4)];
            uint target = 0;
            uint opc = PPC.SwapEndian(BitConverter.ToUInt32(buff, pos * 4));
            if (PPC.hintSubReturn(opc))
            {
                DFSLookUp.Add((uint)(start + pos * 4), pos);
                return pos;
            }
            if (!PPC.isBranchOpc(opc))
            {
                int next = pos + 1;
                while (true)
                {
                    if (next * 4 >= buff.Length)
                    {
                        next = buff.Length / 4 - 1;
                        DFSLookUp.Add((uint)(start + pos * 4), next);
                        return next;
                    }
                    uint tmp = PPC.SwapEndian(BitConverter.ToUInt32(buff, next * 4));
                    if (PPC.hintSubReturn(tmp))
                    {
                        DFSLookUp.Add((uint)(start + pos * 4), next);
                        return next;
                    }
                    if (PPC.isBranchOpc(tmp))
                    {
                        int result = DFSBranchSearch(buff, start, next);
                        DFSLookUp.Add((uint)(start + pos * 4), result);
                        return result;
                    }
                    next++;
                }
            }
            else
            {
                uint type = PPC.getOPCD(opc);
                int nextN, nextT;
                if (PPC.calcBranchTarget(opc, start + (uint)pos * 4, out target) && target >= start && target < start + buff.Length)
                {
                    if (target >= start + pos * 4)
                    {
                        int next = (int)(target - start) / 4;
                        if (PPC.getLK(opc) || type != 18)
                        {
                            nextT = DFSBranchSearch(buff, start, next);
                            nextN = DFSBranchSearch(buff, start, pos + 1);
                            int result = getBiggest(new int[] { nextT, nextN });
                            DFSLookUp.Add((uint)(start + pos * 4), result);
                            return result;

                        }
                        else
                        {
                            int result = DFSBranchSearch(buff, start, next);
                            DFSLookUp.Add((uint)(start + pos * 4), result);
                            return result;
                        }
                    }
                    else
                    {
                        if (PPC.getLK(opc) || type != 18)
                        {
                            int result = DFSBranchSearch(buff, start, pos + 1);
                            DFSLookUp.Add((uint)(start + pos * 4), result);
                            return result;
                        }
                        else
                        {
                            DFSLookUp.Add((uint)(start + pos * 4), pos);
                            return pos;
                        }
                    }
                }
                else
                {
                    if (pos < buff.Length / 4 - 1 && (PPC.getLK(opc) || type != 18))
                    {
                        int result = DFSBranchSearch(buff, start, pos + 1);
                        DFSLookUp.Add((uint)(start + pos * 4), result);
                        return result;
                    }
                    else
                    {
                        DFSLookUp.Add((uint)(start + pos * 4), pos);
                        return pos;
                    }
                }
            }
        }

        public static int getBiggest(int[] list)
        {
            int result = list[0];
            for (int i = 1; i < list.Length; i++)
                if (list[i] > result)
                    result = list[i];
            return result;
        }
    }
}
