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
    public partial class WinInformation : Form
    {
        public WinInformation()
        {
            InitializeComponent();
        }

        private void WinInformation_Load(object sender, EventArgs e)
        {
            rtb1.Text = Debugger.GetGeneralInfos();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            rtb1.Text = Debugger.GetGeneralInfos();
            status.Text = (Debugger.isRunning ? "Running" : "Stopped");
            status.BackColor = (Debugger.isRunning ? Color.Green : Color.Yellow);
        }
    }
}
