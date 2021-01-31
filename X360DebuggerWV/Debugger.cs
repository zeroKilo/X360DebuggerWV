using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XDevkit;
using JRPC_Client;

namespace X360DebuggerWV
{
    public static class Debugger
    {
        public static IXboxConsole jtag;
        public static XboxConsole con;
        public static string[] drives;
        public static bool isRunning;
        public static bool refreshThreads;
        public static bool refreshExecState;
        public static bool refreshCPU;
        public static uint breakThreadId;
        public static List<uint> breakPoints;
        public static bool breakOnModuleLoad = false;
        public static bool breakOnThreadCreate = false;
        public static bool recordTrace = false;

        public static bool Init()
        {
            try
            {
                bool result = jtag.Connect(out jtag);
                if (result)
                {
                    con = (XboxConsole)jtag;
                    con.OnStdNotify += con_OnStdNotify;
                    con.DebugTarget.ConnectAsDebugger("X360 Debugger WV", XboxDebugConnectFlags.Force);
                    isRunning = true;
                    breakPoints = new List<uint>();
                    if (File.Exists("trace_log.txt"))
                        File.Delete("trace_log.txt");
                }
                return result;
            }
            catch { return false; }
        }

        public static void Detach()
        {
            con.DebugTarget.DisconnectAsDebugger();
        }

        public static void Reboot()
        {
            Detach();
            jtag.Reboot(null, null, null, XboxRebootFlags.Warm);
        }

        private static void con_OnStdNotify(XboxDebugEventType EventCode, IXboxEventInfo EventInfo)
        {
            XBOX_THREAD_INFO t;
            XBOX_MODULE_INFO m;
            if (EventCode == XboxDebugEventType.ModuleLoad && breakOnModuleLoad) Pause();
            if (EventCode == XboxDebugEventType.ThreadCreate && breakOnThreadCreate) Pause();
            Log.Write("Event: " + EventCode.ToString() + " ");
            switch (EventCode)
            {
                case XboxDebugEventType.Exception:
                    jtag.DebugTarget.FreeEventInfo(EventInfo.Info);
                    if (EventInfo.Info.Message != null)
                        Log.WriteLine("Exception @0x" + EventInfo.Info.Address.ToString("X8") + " \"" + EventInfo.Info.Message + "\"");
                    else
                        Log.WriteLine("Exception @0x" + EventInfo.Info.Address.ToString("X8"));
                    breakThreadId = EventInfo.Info.Thread.ThreadId;
                    refreshCPU = true;
                    isRunning = false;
                    break;
                case XboxDebugEventType.ExecutionBreak:
                    if (recordTrace)
                    {
                        int n = GetThreadIndexById(EventInfo.Info.Thread.ThreadId);
                        Tracer.AppendLog(EventInfo.Info.Address, GetThreadRegisters32(n), GetThreadRegisters64(n));
                    }
                    breakThreadId = EventInfo.Info.Thread.ThreadId;
                    refreshCPU = true;
                    isRunning = false;
                    break;
                case XboxDebugEventType.ExecStateChange:
                    Log.WriteLine(EventInfo.Info.ExecState.ToString());
                    isRunning = EventInfo.Info.ExecState != XboxExecutionState.Stopped && EventInfo.Info.ExecState != XboxExecutionState.PendingTitle;
                    refreshExecState = true;
                    break;
                case XboxDebugEventType.ThreadCreate:
                    t = EventInfo.Info.Thread.ThreadInfo;
                    Log.WriteLine("TID=" + t.ThreadId.ToString("X8") + " StartAddress=0x" + t.StartAddress.ToString("X8"));
                    //refreshThreads = true;
                    break;
                case XboxDebugEventType.ThreadDestroy:
                    Log.WriteLine("TID=" + EventInfo.Info.Thread.ThreadId.ToString("X8"));
                    //refreshThreads = true;
                    break;
                case XboxDebugEventType.ModuleLoad:
                    m = EventInfo.Info.Module.ModuleInfo;
                    Log.WriteLine("Name=\"" + m.FullName + "\" BaseAddress=0x" + m.BaseAddress.ToString("X8"));
                    break;
                case XboxDebugEventType.ModuleUnload:
                    m = EventInfo.Info.Module.ModuleInfo;
                    Log.WriteLine("Name=\"" + m.FullName + "\"");
                    break;
                case XboxDebugEventType.DebugString:
                    Log.WriteLine("\n" + EventInfo.Info.Message.Replace("\n", "\\n"));
                    break;
                default:
                    Log.WriteLine("");
                    break;
            }
        }

        public static void Play()
        {
            bool res;
            jtag.DebugTarget.Go(out res);
        }

        public static void Pause()
        {
            bool res;
            jtag.DebugTarget.Stop(out res);
        }

        public static string GetGeneralInfos()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Name\t\t\t: " + jtag.Name);
            sb.AppendLine("IP Address\t\t: " + Helper.U32ToIP(jtag.IPAddress));
            sb.AppendLine("CPU Key\t\t\t: " + jtag.GetCPUKey());
            sb.AppendLine("Kernel Version\t\t: " + jtag.GetKernalVersion());
            sb.AppendLine("Features\t\t: " +  jtag.ConsoleFeatures);
            sb.AppendLine("Debug Target\t\t: " + jtag.DebugTarget.RunningProcessInfo.ProgramName);
            sb.AppendLine("Is Running\t\t: " + isRunning);             
            return sb.ToString();
        }

        public static List<string> GetFileNames(string dir)
        {
            List<string> result = new List<string>();
            IXboxFiles files = jtag.DirectoryFiles(dir);
            foreach (IXboxFile file in files)
                if (!file.IsDirectory)
                    result.Add(file.Name.Substring(dir.Length));
            return result;
        }

        public static List<string> GetDirectories(string dir)
        {
            List<string> result = new List<string>();
            IXboxFiles files = jtag.DirectoryFiles(dir);
            foreach (IXboxFile file in files)
                if (file.IsDirectory)
                    result.Add(file.Name.Substring(dir.Length));            
            return result;
        }

        public static byte[] GetFileContent(string path)
        {
            jtag.ReceiveFile("temp.bin", path);
            byte[] result = new byte[0];
            if (File.Exists("temp.bin"))
            {
                result = File.ReadAllBytes("temp.bin");
                File.Delete("temp.bin");
            }
            return result;
        }

        public static void RunXEX(string path)
        {
            jtag.Reboot(path, Path.GetDirectoryName(path), "", XboxRebootFlags.Title);
            jtag.DebugTarget.StopOn(XboxStopOnFlags.OnFirstChanceException, true);
        }

        public static void RunPausedXEX(string path)
        {
            jtag.Reboot(path, Path.GetDirectoryName(path), "", XboxRebootFlags.Stop);
            jtag.DebugTarget.StopOn(XboxStopOnFlags.OnFirstChanceException, true);
        }

        public static string[] GetThreadsInfos()
        {
            List<string> result = new List<string>();
            foreach (IXboxThread thread in jtag.DebugTarget.Threads)
            {
                try
                {
                    result.Add(thread.ThreadId.ToString("X8") + " (on core " + thread.CurrentProcessor + ")");
                }
                catch { }
            }
            return result.ToArray();
        }

        public static string GetThreadInfo(int n)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                IXboxThread thread = jtag.DebugTarget.Threads[n];
                result.AppendLine("ID\t\t: 0x" + thread.ThreadId.ToString("X8"));
                result.AppendLine("Processor\t: " + thread.CurrentProcessor);
                result.AppendLine("Suspend Count\t: 0x" + thread.ThreadInfo.SuspendCount);
            }
            catch { }
            return result.ToString();
        }

        public static byte[] GetMemoryDump(uint offset, uint size)
        {
            return jtag.GetMemory(offset, size);
        }

        public static void SetMemoryDump(uint offset, byte[] data)
        {
            jtag.SetMemory(offset, data);
        }

        public static List<string> GetModuleInfos()
        {
            List<string> result = new List<string>();
            foreach (IXboxModule mod in jtag.DebugTarget.Modules)
            {
                string info = "Base=0x" + mod.ModuleInfo.BaseAddress.ToString("X8") + " Name=\"" + mod.ModuleInfo.FullName + "\"";
                result.Add(info);
            }
            return result;
        }

        public static uint GetModuleBaseAddress(int n)
        {
            IXboxModule mod = jtag.DebugTarget.Modules[n];
            return mod.ModuleInfo.BaseAddress;
        }

        public static uint GetModuleEntryPoint(int n)
        {
            IXboxModule mod = jtag.DebugTarget.Modules[n];
            return mod.GetEntryPointAddress();
        }

        public static void AddBreakpoint(uint address)
        {
            jtag.DebugTarget.SetBreakpoint(address);
            breakPoints.Add(address);
        }

        public static void RemoveBreakpoint(uint address)
        {
            if (breakPoints.Contains(address))
            {
                for (int i = 0; i < breakPoints.Count; i++)
                    if (breakPoints[i] == address)
                    {
                        breakPoints.RemoveAt(i);
                        break;
                    }
                jtag.DebugTarget.RemoveBreakpoint(address);
            }
        }

        public static void RemoveAllBreakpoints()
        {
            breakPoints.Clear();
            jtag.DebugTarget.RemoveAllBreakpoints();
        }

        public static long[] GetThreadRegisters64(int n)
        {
            long[] result = new long[33];
            IXboxThread thread = jtag.DebugTarget.Threads[n];
            IXboxStackFrame frame = thread.TopOfStack;
            frame.FlushRegisterChanges();
            for (int i = 0; i < 33; i++)
                frame.GetRegister64((XboxRegisters64)i, out result[i]);
            return result;
        }

        public static int[] GetThreadRegisters32(int n)
        {
            int[] result = new int[5];
            IXboxThread thread = jtag.DebugTarget.Threads[n];
            IXboxStackFrame frame = thread.TopOfStack;
            frame.FlushRegisterChanges();
            for (int i = 0; i < 5; i++)
                frame.GetRegister32((XboxRegisters32)i, out result[i]);            
            return result;
        }

        public static int GetThreadIndexById(uint id)
        {
            for (int i = 0; i < jtag.DebugTarget.Threads.Count; i++)
                if (jtag.DebugTarget.Threads[i].ThreadId == id)
                    return i;
            return -1;
        }

        public static string[] GetModuleSections(int n)
        {
            IXboxModule mod = jtag.DebugTarget.Modules[n];
            List<string> result = new List<string>();
            foreach (IXboxSection sec in mod.Sections)
            {
                StringBuilder sb = new StringBuilder();
                XBOX_SECTION_INFO info = sec.SectionInfo;
                string flags = "";
                if ((info.Flags & XboxSectionInfoFlags.Executable) != 0) flags += "Executable ";
                if ((info.Flags & XboxSectionInfoFlags.Loaded) != 0) flags += "Loaded ";
                if ((info.Flags & XboxSectionInfoFlags.Readable) != 0) flags += "Readable ";
                if ((info.Flags & XboxSectionInfoFlags.Uninitialized) != 0) flags += "Uninitialized ";
                if ((info.Flags & XboxSectionInfoFlags.Writeable) != 0) flags += "Writeable ";
                sb.AppendFormat("Section {0}: BaseAddress=0x{2} Size=0x{3} \"{1}\" Flags={4}", info.Index, info.Name, info.BaseAddress.ToString("X8"), info.Size.ToString("X8"), flags);
                result.Add(sb.ToString());
            }
            return result.ToArray();
        }

        public static string[] GetMemoryRegionInfos()
        {
            List<string> result = new List<string>();
            List<XBOX_SECTION_INFO> sections = new List<XBOX_SECTION_INFO> ();
            List<string> secModuleNames = new List<string>();

            foreach (IXboxModule mod in jtag.DebugTarget.Modules)
                foreach (IXboxSection sec in mod.Sections)
                {
                    secModuleNames.Add(mod.ModuleInfo.Name);
                    sections.Add(sec.SectionInfo);
                }
            foreach (IXboxMemoryRegion reg in jtag.DebugTarget.MemoryRegions)
            {
                string contains = "";
                uint baseAddress = (uint)reg.BaseAddress;
                uint regionSize = (uint)reg.RegionSize;
                for (int i = 0; i < sections.Count; i++)
                    if (sections[i].BaseAddress >= baseAddress && sections[i].BaseAddress + sections[i].Size <= baseAddress + regionSize)
                        contains += "[" + secModuleNames[i] + "->" + sections[i].Name + "]";
                string flags = " ";
                if ((reg.Flags & XboxMemoryRegionFlags.NoAccess) != 0) flags += "NoAccess ";
                if ((reg.Flags & XboxMemoryRegionFlags.ReadOnly) != 0) flags += "ReadOnly ";
                if ((reg.Flags & XboxMemoryRegionFlags.ReadWrite) != 0) flags += "ReadWrite ";
                if ((reg.Flags & XboxMemoryRegionFlags.WriteCopy) != 0) flags += "WriteCopy ";
                if ((reg.Flags & XboxMemoryRegionFlags.Execute) != 0) flags += "Execute ";
                if ((reg.Flags & XboxMemoryRegionFlags.ExecuteRead) != 0) flags += "ExecuteRead ";
                if ((reg.Flags & XboxMemoryRegionFlags.ExecuteReadWrite) != 0) flags += "ExecuteReadWrite ";
                if ((reg.Flags & XboxMemoryRegionFlags.ExecuteWriteCopy) != 0) flags += "ExecuteWriteCopy ";
                if ((reg.Flags & XboxMemoryRegionFlags.Guard) != 0) flags += "Guard ";
                if ((reg.Flags & XboxMemoryRegionFlags.NoCache) != 0) flags += "NoCache ";
                if ((reg.Flags & XboxMemoryRegionFlags.WriteCombine) != 0) flags += "WriteCombine ";
                if ((reg.Flags & XboxMemoryRegionFlags.UserReadOnly) != 0) flags += "UserReadOnly ";
                if ((reg.Flags & XboxMemoryRegionFlags.UserReadWrite) != 0) flags += "UserReadWrite ";
                result.Add("BaseAddress=0x" + reg.BaseAddress.ToString("X8") + " RegionSize=0x" + reg.RegionSize.ToString("X8") + " Flags=\"" + flags + (contains != "" ? "\" Contains=\"" + contains + "\"" : ""));
            }
            return result.ToArray();
        }

        public static void MakeScreenshot(string filename)
        {
            jtag.ScreenShot(filename);
        }
    }
}
