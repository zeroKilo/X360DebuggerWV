using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X360DebuggerWV
{
    public static class Disassembler
    {
        public static string[] Disassemble(byte[] buff)
        {
            List<string> disasm = new List<string>();
            string cmd = "ppc64be \"";
            for (int i = 0; i < buff.Length; i++)
                cmd += buff[i].ToString("X2");
            cmd += "\"";
            string result = Helper.RunShell("cstool", cmd);
            if (result.StartsWith("0 "))
            {
                string[] lines = result.Split('\n');
                foreach (string line in lines)
                {
                    result = line;
                    while (result.Contains("  "))
                        result = result.Replace("  ", " ");
                    result = result.Trim();
                    if (line == "") continue;
                    string[] parts = result.Split(' ');
                    result = "";
                    for (int i = 2; i < parts.Length; i++)
                        result += parts[i] + " ";
                    disasm.Add(result);
                }
            }
            int total = buff.Length / 4;
            disasm.AddRange(new string[total - disasm.Count]);
            return disasm.ToArray();
        }
    }
}
