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
using System.Drawing.Imaging;
using Imaging.DDSReader;

namespace X360DebuggerWV
{
    public partial class WinScreenshot : Form
    {
        public WinScreenshot()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string file = "screenshot.dds";
            if (File.Exists(file))
                File.Delete(file);
            Debugger.MakeScreenshot(file);
            if (File.Exists(file))
            {
                pb1.Image = DDS.LoadImage(file);
                File.Delete(file);
            }
            GC.Collect();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (pb1.Image == null)
                return;
            SaveFileDialog d = new SaveFileDialog();
            d.Filter = "*.png|*.png";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pb1.Image.Save(d.FileName, ImageFormat.Png);
                MessageBox.Show("Done.");
            }
        }
    }
}
