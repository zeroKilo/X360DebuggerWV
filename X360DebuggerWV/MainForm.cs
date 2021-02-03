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
            Form[] forms = new Form[7];
            forms[0] = OpenWindowLog();
            if (!Debugger.Init())
            {
                MessageBox.Show("Cannot connect to console, please check Xbox 360 Neighborhood!");
                this.Close();
                return;
            }
            forms[1] = OpenWindowInfos();
            forms[2] = OpenWindowFileBrowser();
            forms[3] = OpenWindowCPU();
            forms[4] = OpenWindowModules();
            forms[5] = OpenWindowMemRegion();
            forms[6] = OpenWindowScreenshot();
            forms[0].Left =
            forms[0].Top =
            forms[3].Left =
            forms[4].Top =
            forms[5].Top = 0;
            forms[3].Top = forms[0].Top + forms[0].Height;
            forms[4].Left = forms[0].Left + forms[0].Width;
            forms[5].Left = forms[4].Left + forms[4].Width;
            forms[1].Left =
            forms[2].Left = forms[3].Width;
            forms[6].Top =
            forms[1].Top = forms[5].Top + forms[5].Height;
            forms[2].Top = forms[1].Top + forms[1].Height;
            forms[6].Left = forms[1].Left + forms[1].Width;
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

        private void memoryRegionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindowMemRegion();
        }

        private Form OpenWindowInfos()
        {
            return OpenWindow(new WinInformation());
        }

        private Form OpenWindowFileBrowser()
        {
            return OpenWindow(new WinFileBrowser());
        }

        private Form OpenWindowLog()
        {
            return OpenWindow(new WinLog());
        }

        private Form OpenWindowCPU()
        {
            return OpenWindow(new WinCPU());
        }

        private Form OpenWindowMemDump()
        {
            return OpenWindow(new WinMemoryDump());
        }

        private Form OpenWindowModules()
        {
            return OpenWindow(new WinModules());
        }

        private Form OpenWindowTrace()
        {
            return OpenWindow(new WinTrace());
        }

        private Form OpenWindowMemRegion()
        {
            return OpenWindow(new WinMemoryRegions());
        }

        private Form OpenWindowScreenshot()
        {
            return OpenWindow(new WinScreenshot());
        }

        private Form OpenWindow(Form f)
        {
            f.MdiParent = this;
            f.Show();
            return f;
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Debugger.Detach();
            }
            catch { }
        }

        private void screenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindowScreenshot();
        }
    }
}
