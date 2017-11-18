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

        public static bool Init()
        {
            try
            {
                bool result = jtag.Connect(out jtag);
                if (result)
                {
                    XboxConsole con = (XboxConsole)jtag;
                    con.OnStdNotify += con_OnStdNotify;
                    con.DebugTarget.ConnectAsDebugger("X360 Debugger WV", XboxDebugConnectFlags.Force);
                    isRunning = true;
                    breakPoints = new List<uint>();
                }
                return result;
            }
            catch { return false; }
        }

        private static void con_OnStdNotify(XboxDebugEventType EventCode, IXboxEventInfo EventInfo)
        {
            if (EventCode == XboxDebugEventType.ModuleLoad && breakOnModuleLoad) Pause();
            if (EventCode == XboxDebugEventType.ThreadCreate && breakOnThreadCreate) Pause();
            Log.Write("Event: " + EventCode.ToString() + " ");
            switch (EventCode)
            {
                case XboxDebugEventType.ExecutionBreak:
                    breakThreadId = EventInfo.Info.Thread.ThreadId;
                    refreshCPU = true;
                    isRunning = false;
                    break;
                case XboxDebugEventType.ExecStateChange:
                    Log.WriteLine(EventInfo.Info.ExecState.ToString());
                    isRunning = EventInfo.Info.ExecState != XboxExecutionState.Stopped;
                    refreshExecState = true;
                    break;
                case XboxDebugEventType.ThreadCreate:
                    XBOX_THREAD_INFO t = EventInfo.Info.Thread.ThreadInfo;
                    Log.WriteLine("TID=" + t.ThreadId.ToString("X8") + " StartAddress=0x" + t.StartAddress.ToString("X8"));
                    refreshThreads = true;
                    break;
                case XboxDebugEventType.ThreadDestroy:
                    Log.WriteLine("TID=" + EventInfo.Info.Thread.ThreadId.ToString("X8"));
                    refreshThreads = true;
                    break;
                case XboxDebugEventType.ModuleLoad:
                    XBOX_MODULE_INFO m = EventInfo.Info.Module.ModuleInfo;
                    Log.WriteLine("Name=\"" + m.FullName + "\" BaseAddress=0x" + m.BaseAddress.ToString("X8"));
                    break;
                case XboxDebugEventType.ModuleUnload:
                    XBOX_MODULE_INFO m2 = EventInfo.Info.Module.ModuleInfo;
                    Log.WriteLine("Name=\"" + m2.FullName + "\"");
                    break;
                case XboxDebugEventType.DebugString:
                    Log.WriteLine(EventInfo.Info.Message.Replace("\n", "\\n"));
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
        }

        public static void RunPausedXEX(string path)
        {
            jtag.Reboot(path, Path.GetDirectoryName(path), "", XboxRebootFlags.Stop);
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
    }
}
