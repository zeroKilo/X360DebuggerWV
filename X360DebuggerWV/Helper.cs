using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace X360DebuggerWV
{
    public static class Helper
    {
        public static string U32ToIP(uint u)
        {
            byte[] b = BitConverter.GetBytes(u);
            return b[3] + "." + b[2] + "." + b[1] + "." + b[0];
        }

        public static string RunShell(string path, string cmd)
        {
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = path;
            proc.StartInfo.Arguments = cmd;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.WaitForExit();
            return proc.StandardOutput.ReadToEnd();
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
