using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XDevkit;
using JRPC_Client;

namespace X360DebuggerWV
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            OpenWindowLog();
            if (!Debugger.Init())
            {
                MessageBox.Show("Cannot connect to console, please check Xbox 360 Neighborhood!");
                this.Close();
                return;
            }
            OpenWindowInfos();
            OpenWindowFileBrowser();
            OpenWindowCPU();
            OpenWindowModules();
            OpenWindowTrace();
        }

        private void generalInfosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindowInfos();
        }

        private void fileBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindowFileBrowser();
        }

        private void cPUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindowCPU();
        }

        private void memoryDumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindowMemDump();
        }

        private void modulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindowModules();
        }

        private void traceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindowTrace();
        }

        private void OpenWindowInfos()
        {
            OpenWindow(new WinInformation());
        }

        private void OpenWindowFileBrowser()
        {
            OpenWindow(new WinFileBrowser());
        }

        private void OpenWindowLog()
        {
            OpenWindow(new WinLog());
        }

        private void OpenWindowCPU()
        {
            OpenWindow(new WinCPU());
        }

        private void OpenWindowMemDump()
        {
            OpenWindow(new WinMemoryDump());
        }

        private void OpenWindowModules()
        {
            OpenWindow(new WinModules());
        }

        private void OpenWindowTrace()
        {
            OpenWindow(new WinTrace());
        }

        private void OpenWindow(Form f)
        {
            f.MdiParent = this;
            f.Show();
        }

        private void breakOnModuleLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debugger.breakOnModuleLoad = breakOnModuleLoadToolStripMenuItem.Checked;
            Log.WriteLine("Info : Breaking on Module Load = " + (Debugger.breakOnModuleLoad ? "ON" : "OFF"));
        }

        private void breakOnThreadCreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debugger.breakOnThreadCreate = breakOnThreadCreateToolStripMenuItem.Checked;
            Log.WriteLine("Info : Breaking on Thread Create = " + (Debugger.breakOnThreadCreate ? "ON" : "OFF"));
        }

        private void recordBreakpointsToTraceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debugger.recordTrace = recordBreakpointsToTraceToolStripMenuItem.Checked;
            Log.WriteLine("Info : Record Trace = " + (Debugger.recordTrace ? "ON" : "OFF"));
        }
    }
}
