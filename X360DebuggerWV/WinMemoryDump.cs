using System;
using System.IO;
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

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.Filter = "*.bin|*.bin";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                byte[] buff = new byte[hb1.ByteProvider.Length];
                for (int i = 0; i < hb1.ByteProvider.Length; i++)
                    buff[i] = hb1.ByteProvider.ReadByte(i);
                File.WriteAllBytes(d.FileName, buff);
                MessageBox.Show("Done.");
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "*.bin|*.bin";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                hb1.ByteProvider = new DynamicByteProvider(File.ReadAllBytes(d.FileName));
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            byte[] pat = Helper.StringToByteArray(toolStripTextBox3.Text.Replace(" ", "").Trim());
            int start = 0;
            if (hb1.SelectionStart != -1) start = (int)hb1.SelectionStart + 1;
            for (int i = start; i < hb1.ByteProvider.Length - pat.Length; i++)
            {
                bool found = true;
                for(int j=0;j<pat.Length;j++)
                    if (hb1.ByteProvider.ReadByte(i + j) != pat[j])
                    {
                        found = false;
                        break;
                    }
                if (found)
                {
                    hb1.SelectionStart = i;
                    hb1.SelectionLength = pat.Length;
                    break;
                }
            }
        }
    }
}
