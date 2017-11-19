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

namespace X360DebuggerWV
{
    public partial class WinTrace : Form
    {
        public string[] lines = new string[0];

        public WinTrace()
        {
            InitializeComponent();
            RefreshTrace();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshTrace();
        }

        private void RefreshTrace()
        {
            listBox1.Items.Clear();
            if (!File.Exists("trace_log.txt")) return;
            lines = File.ReadAllLines("trace_log.txt");
            if (lines.Length == 0) return;
            listBox1.Items.Add(lines[0]);
            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts1 = lines[i - 1].Split(' ');
                string[] parts2 = lines[i].Split(' ');
                string line = parts2[0] + " : ";
                for (int j = 2; j < parts2.Length; j++)
                    if (parts1[j] != parts2[j])
                        line += parts2[j] + " ";
                listBox1.Items.Add(line);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            if (n == -1) return;
            string[] parts = lines[n].Split(' ');
            rtb1.Text = "";
            for (int i = 2; i < parts.Length; i++)
                rtb1.Text += parts[i] + "\n";
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.Filter = "*.txt|*.txt";
            d.FileName = "trace_log.txt";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllLines(d.FileName, lines);
                MessageBox.Show("Done");
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {            
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "*.txt|*.txt";
            d.FileName = "trace_log.txt";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lines = File.ReadAllLines(d.FileName);
                File.WriteAllLines("trace_log.txt", lines);
                RefreshTrace();
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            File.WriteAllText("trace_log.txt", "");
            RefreshTrace();
        }
    }
}
