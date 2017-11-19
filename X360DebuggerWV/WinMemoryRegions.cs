using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace X360DebuggerWV
{
    public partial class WinMemoryRegions : Form
    {
        public WinMemoryRegions()
        {
            InitializeComponent();
            RefreshRegions();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshRegions();
        }

        private void RefreshRegions()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(Debugger.GetMemoryRegionInfos());
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int n = listBox1.SelectedIndex;
            if (n == -1) e.Cancel = true;
        }

        private void previewInMemoryDumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            string line = listBox1.Items[n].ToString();
            string[] parts = line.Split(' ');
            parts = parts[0].Split('x');
            WinMemoryDump f = new WinMemoryDump();
            f.toolStripTextBox1.Text = parts[1];
            f.toolStripTextBox2.Text = "100";
            f.MdiParent = this.MdiParent;
            f.Show();
            f.Dump();
        }

        private void viewFullInMemoryDumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            string line = listBox1.Items[n].ToString();
            string[] parts = line.Split(' ');
            parts = parts[0].Split('x');
            string addr = parts[1];
            parts = line.Split(' ');
            parts = parts[1].Split('x');
            WinMemoryDump f = new WinMemoryDump();
            f.toolStripTextBox1.Text = addr;
            f.toolStripTextBox2.Text = parts[1];
            f.MdiParent = this.MdiParent;
            f.Show();
            f.Dump();
        }
    }
}
