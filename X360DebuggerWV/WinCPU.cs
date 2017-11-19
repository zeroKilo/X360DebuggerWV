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
    public partial class WinCPU : Form
    {
        public uint currAddress;
        public WinCPU()
        {
            InitializeComponent();
            RefreshThreadList();
            timer1.Enabled = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Debugger.Play();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Debugger.Pause();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Debugger.refreshThreads)
                {
                    RefreshThreadList();
                    Debugger.refreshThreads = false;
                }
                if (Debugger.refreshExecState)
                {
                    RefreshButtons();
                    Debugger.refreshExecState = false;
                }
                if (Debugger.refreshCPU)
                {
                    int n = Debugger.GetThreadIndexById(Debugger.breakThreadId);
                    if (n != -1)
                    {
                        listBox1.SelectedIndex = n;
                        UpdateCurrrentThread();
                    }
                    Debugger.refreshCPU = false;
                }
            }
            catch { }
        }

        public void RefreshButtons()
        {
            toolStripButton4.Enabled =
            toolStripButton1.Enabled = !Debugger.isRunning;
            toolStripButton2.Enabled = Debugger.isRunning;
        }

        public void RefreshThreadList()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(Debugger.GetThreadsInfos());
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCurrrentThread();
        }

        private void UpdateCurrrentThread()
        {
            int n = listBox1.SelectedIndex;
            if (n == -1) return;
            long[] reg64 = Debugger.GetThreadRegisters64(n);
            int[] reg32 = Debugger.GetThreadRegisters32(n);
            listBox3.Items.Clear();
            listBox3.Items.Add("MSR\t: " + reg32[0].ToString("X8"));
            listBox3.Items.Add("IAR\t: " + reg32[1].ToString("X8"));
            listBox3.Items.Add("LR\t: " + reg32[2].ToString("X8"));
            listBox3.Items.Add("CR\t: " + reg32[3].ToString("X8"));
            listBox3.Items.Add("XER\t: " + reg32[4].ToString("X8"));
            listBox3.Items.Add("CTR\t: " + reg64[0].ToString("X16"));
            for (int i = 0; i < 32; i++)
                listBox3.Items.Add("R" + i + "\t: " + reg64[i + 1].ToString("X16"));
            GotoAddress((uint)reg32[1]);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            uint addr = Convert.ToUInt32(toolStripTextBox1.Text, 16);
            GotoAddress(addr);
        }

        public void GotoAddress(uint address)
        {
            uint count = 100;
            uint pos = currAddress = address;
            listBox2.Items.Clear();
            byte[] buf = Debugger.GetMemoryDump(pos, 4 * count);
            string[] opDisAsm = Disassembler.Disassemble(buf);
            for (uint i = 0; i < count; i++)
            {
                string opBytes = buf[i * 4].ToString("X2") + " " + buf[i * 4 + 1].ToString("X2") + " " + buf[i * 4 + 2].ToString("X2") + " " + buf[i * 4 + 3].ToString("X2");
                string hasBP = Debugger.breakPoints.Contains(pos) ? "* " : "";
                listBox2.Items.Add(hasBP + pos.ToString("X8") + "\t: " + opBytes + "\t" + opDisAsm[i]);
                pos += 4;
            }
        }

        private void setBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int n = listBox2.SelectedIndex;
            uint pos = currAddress + (uint)n * 4;
            if (!Debugger.breakPoints.Contains(pos))
            {
                Debugger.AddBreakpoint(pos);
                GotoAddress(currAddress);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int n = listBox2.SelectedIndex;
            if (n == -1) e.Cancel = true;
        }

        private void removeBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int n = listBox2.SelectedIndex;
            uint pos = currAddress + (uint)n * 4;
            if (Debugger.breakPoints.Contains(pos))
            {
                Debugger.RemoveBreakpoint(pos);
                GotoAddress(currAddress);
            }
        }

        private void removeAllBreakpointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debugger.RemoveAllBreakpoints();
            GotoAddress(currAddress);
        }

        private void listBox3_Click(object sender, EventArgs e)
        {
            int n = listBox3.SelectedIndex;
            if (n == -1) return;
            string[] parts = listBox3.Items[n].ToString().Split(':');
            Clipboard.SetText(parts[1].Trim());
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            int n = Debugger.GetThreadIndexById(Debugger.breakThreadId);
            if (n != -1) listBox1.SelectedIndex = n;
            int[] reg32 = Debugger.GetThreadRegisters32(n);
            Debugger.RemoveBreakpoint((uint)reg32[1]);
            Debugger.AddBreakpoint((uint)reg32[1] + 4);
            Debugger.Play();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            RefreshButtons();
        }
    }
}
