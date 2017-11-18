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
    public partial class WinModules : Form
    {
        public WinModules()
        {
            InitializeComponent();
        }

        private void WinModules_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        public void RefreshList()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(Debugger.GetModuleInfos().ToArray());
        }

        private void showInDumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            WinMemoryDump f = new WinMemoryDump();
            f.MdiParent = this.MdiParent;
            f.Show();
            f.toolStripTextBox1.Text = Debugger.GetModuleBaseAddress(n).ToString("X8");
            f.toolStripTextBox2.Text = "100";
            f.Dump();
        }

        private void showEntryPointInDumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            WinMemoryDump f = new WinMemoryDump();
            f.MdiParent = this.MdiParent;
            f.Show();
            f.toolStripTextBox1.Text = Debugger.GetModuleEntryPoint(n).ToString("X8");
            f.toolStripTextBox2.Text = "100";
            f.Dump();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int n = listBox1.SelectedIndex;
            if (n == -1) e.Cancel = true;
        }

        private void showEntryPointInCPUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            WinCPU f = new WinCPU();
            f.MdiParent = this.MdiParent;
            f.Show();
            uint addr = Debugger.GetModuleEntryPoint(n);
            f.toolStripTextBox1.Text = addr.ToString("X8");
            f.GotoAddress(addr);
        }
    }
}
