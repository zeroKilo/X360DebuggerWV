namespace X360DebuggerWV
{
    partial class WinModules
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinModules));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showInDumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showEntryPointInDumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showEntryPointInCPUToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.previewInMemoryDumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewFullInMemoryDumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showInDumpToolStripMenuItem,
            this.showEntryPointInDumpToolStripMenuItem,
            this.showEntryPointInCPUToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(225, 70);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // showInDumpToolStripMenuItem
            // 
            this.showInDumpToolStripMenuItem.Name = "showInDumpToolStripMenuItem";
            this.showInDumpToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.showInDumpToolStripMenuItem.Text = "Show Base Address in Dump";
            this.showInDumpToolStripMenuItem.Click += new System.EventHandler(this.showInDumpToolStripMenuItem_Click);
            // 
            // showEntryPointInDumpToolStripMenuItem
            // 
            this.showEntryPointInDumpToolStripMenuItem.Name = "showEntryPointInDumpToolStripMenuItem";
            this.showEntryPointInDumpToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.showEntryPointInDumpToolStripMenuItem.Text = "Show Entry Point in Dump";
            this.showEntryPointInDumpToolStripMenuItem.Click += new System.EventHandler(this.showEntryPointInDumpToolStripMenuItem_Click);
            // 
            // showEntryPointInCPUToolStripMenuItem
            // 
            this.showEntryPointInCPUToolStripMenuItem.Name = "showEntryPointInCPUToolStripMenuItem";
            this.showEntryPointInCPUToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.showEntryPointInCPUToolStripMenuItem.Text = "Show Entry Point in CPU";
            this.showEntryPointInCPUToolStripMenuItem.Click += new System.EventHandler(this.showEntryPointInCPUToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(642, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(50, 22);
            this.toolStripButton1.Text = "Refresh";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listBox2);
            this.splitContainer1.Size = new System.Drawing.Size(642, 334);
            this.splitContainer1.SplitterDistance = 151;
            this.splitContainer1.TabIndex = 3;
            // 
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(642, 151);
            this.listBox1.TabIndex = 2;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // listBox2
            // 
            this.listBox2.ContextMenuStrip = this.contextMenuStrip2;
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.listBox2.FormattingEnabled = true;
            this.listBox2.IntegralHeight = false;
            this.listBox2.Location = new System.Drawing.Point(0, 0);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(642, 179);
            this.listBox2.TabIndex = 3;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.previewInMemoryDumpToolStripMenuItem,
            this.viewFullInMemoryDumpToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(219, 48);
            this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip2_Opening);
            // 
            // previewInMemoryDumpToolStripMenuItem
            // 
            this.previewInMemoryDumpToolStripMenuItem.Name = "previewInMemoryDumpToolStripMenuItem";
            this.previewInMemoryDumpToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.previewInMemoryDumpToolStripMenuItem.Text = "Preview in Memory Dump";
            this.previewInMemoryDumpToolStripMenuItem.Click += new System.EventHandler(this.previewInMemoryDumpToolStripMenuItem_Click);
            // 
            // viewFullInMemoryDumpToolStripMenuItem
            // 
            this.viewFullInMemoryDumpToolStripMenuItem.Name = "viewFullInMemoryDumpToolStripMenuItem";
            this.viewFullInMemoryDumpToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.viewFullInMemoryDumpToolStripMenuItem.Text = "View Full in Memory Dump";
            this.viewFullInMemoryDumpToolStripMenuItem.Click += new System.EventHandler(this.viewFullInMemoryDumpToolStripMenuItem_Click);
            // 
            // WinModules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 359);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "WinModules";
            this.Text = "Modules";
            this.Load += new System.EventHandler(this.WinModules_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showInDumpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showEntryPointInDumpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showEntryPointInCPUToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem previewInMemoryDumpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewFullInMemoryDumpToolStripMenuItem;
    }
}