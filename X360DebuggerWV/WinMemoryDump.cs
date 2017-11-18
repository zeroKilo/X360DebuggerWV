using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Be.Windows.Forms;

namespace X360DebuggerWV
{
    public partial class WinMemoryDump : Form
    {
        public WinMemoryDump()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Dump();
        }

        public void Dump()
        {
            uint start = Convert.ToUInt32(toolStripTextBox1.Text, 16);
            uint size = Convert.ToUInt32(toolStripTextBox2.Text, 16);
            hb1.ByteProvider = new DynamicByteProvider(Debugger.GetMemoryDump(start, size));
            hb1.LineInfoOffset = start;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            uint start = Convert.ToUInt32(toolStripTextBox1.Text, 16);
            byte[] data = new byte[hb1.ByteProvider.Length];
            for (int i = 0; i < hb1.ByteProvider.Length; i++)
                data[i] = hb1.ByteProvider.ReadByte(i);
            Debugger.SetMemoryDump(start, data);
        }
    }
}
