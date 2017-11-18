using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace X360DebuggerWV
{
    public static class Log
    {
        public static RichTextBox box = null;
        

        public static void Write(string s)
        {
            if (box == null) return;
            box.Invoke(new Action(delegate
            {
                box.Text += s;
                box.SelectionStart = box.Text.Length;
                box.ScrollToCaret();
            }));
        }

        public static void WriteLine(string s)
        {
            Write(s + "\n");
        }
    }
}
