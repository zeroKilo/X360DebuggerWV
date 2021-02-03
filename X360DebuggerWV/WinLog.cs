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
    public partial class WinLog : Form
    {
        public WinLog()
        {
            InitializeComponent();
        }

        private void WinLog_Load(object sender, EventArgs e)
        {
            Log.box = rtb1;
            Log.WriteLine("Log initialized");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rtb1.Text = "";
        }
    }
}
