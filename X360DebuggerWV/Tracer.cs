using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X360DebuggerWV
{
    public static class Tracer
    {

        public static string[] reg32Names = { "MSR", "IAR", "LR", "CR", "XER" };
        public static void AppendLog(uint addr, int[] reg32, long[] reg64)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(addr.ToString("X8") + " : ");
            for (int i = 0; i < 5; i++)
                sb.Append(reg32Names[i] + "=" + reg32[i].ToString("X8") + " ");
            sb.Append("CTR=" + reg64[0].ToString("X16") + " ");
            for (int i = 0; i < 32; i++)
                sb.Append("R" + i + "=" + reg64[i + 1].ToString("X16") + " ");
            sb.AppendLine();
            File.AppendAllText("trace_log.txt", sb.ToString());
        }

    }
}
