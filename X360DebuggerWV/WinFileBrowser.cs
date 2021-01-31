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
    public partial class WinFileBrowser : Form
    {
        public string currDrive;
        public string currDir;
        public string currPreview;
        public WinFileBrowser()
        {
            InitializeComponent();
        }

        private void WinFileBrowser_Load(object sender, EventArgs e)
        {
            c1.Items.Clear();
            c1.Items.AddRange(Debugger.jtag.Drives.Split(','));
            c1.SelectedIndex = 0;
        }

        private void c1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = c1.SelectedIndex;
            if (n == -1) return;
            currDrive = Debugger.jtag.Drives.Split(',')[n];
            if (currDrive != c1.Items[n].ToString())
            {
                c1.Items.Clear();
                c1.Items.AddRange(Debugger.jtag.Drives.Split(','));
            }
            currDrive += ":";
            currDir = "\\";
            RefreshFiles();
        }

        private void RefreshFiles()
        {
            string path = currDrive + currDir;
            List<string> files = Debugger.GetFileNames(path);
            List<string> dirs = Debugger.GetDirectories(path);
            listBox1.Items.Clear();
            if (currDir.Length > 1)
                listBox1.Items.Add("..");
            foreach (string dir in dirs)
                listBox1.Items.Add("> " + dir);
            listBox1.Items.AddRange(files.ToArray());
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            if (n == -1) return;
            string item = listBox1.Items[n].ToString();
            if (item == "..")
            {
                currDir = currDir.Substring(0, currDir.Length - 1);
                currDir = Path.GetDirectoryName(currDir);
                if (currDir.Length != 1)
                    currDir += "\\";
                status.Text = currDrive + currDir;
                RefreshFiles();
            }
            else if (item.StartsWith("> "))
            {
                currDir += item.Substring(2) + "\\";
                status.Text = currDrive + currDir;
                RefreshFiles();
            }
            else
                Preview(currDrive + currDir + item);
        }

        private void Preview(string file, bool getContent = true)
        {
            status.Text = 
            currPreview = file;
            if (getContent)
            {
                byte[] data = Debugger.GetFileContent(file);
                hb1.ByteProvider = new DynamicByteProvider(data);
            }
            toolStripButton1.Enabled = true;
            toolStripButton2.Enabled =
            toolStripButton4.Enabled = file.ToLower().EndsWith(".xex");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            string ext = "*" + Path.GetExtension(currPreview);
            d.Filter = ext + "|" + ext;
            d.FileName = Path.GetFileName(currPreview);
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                int size = (int)hb1.ByteProvider.Length;
                byte[] data = new byte[size];
                for (int i = 0; i < size; i++)
                    data[i] = hb1.ByteProvider.ReadByte(i);
                File.WriteAllBytes(d.FileName, data);
                MessageBox.Show("Done.");
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Debugger.RunXEX(currPreview);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            c1.Items.Clear();
            c1.Items.AddRange(Debugger.jtag.Drives.Split(','));
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            if (n == -1) return;
            string item = listBox1.Items[n].ToString();
            if (item.StartsWith("..") || item.StartsWith(">")) return;
            Preview(currDrive + currDir + item, false);
            hb1.ByteProvider = new DynamicByteProvider(new byte[0]);
            toolStripButton1.Enabled = false;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Debugger.RunPausedXEX(currPreview);
        }
    }
}
