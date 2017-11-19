namespace X360DebuggerWV
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generalInfosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.memoryDumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.breakOnModuleLoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.breakOnThreadCreateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordBreakpointsToTraceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.traceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.windowToolStripMenuItem,
            this.optionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(988, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generalInfosToolStripMenuItem,
            this.fileBrowserToolStripMenuItem,
            this.memoryDumpToolStripMenuItem,
            this.modulesToolStripMenuItem,
            this.traceToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // generalInfosToolStripMenuItem
            // 
            this.generalInfosToolStripMenuItem.Name = "generalInfosToolStripMenuItem";
            this.generalInfosToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.generalInfosToolStripMenuItem.Text = "General Infos";
            this.generalInfosToolStripMenuItem.Click += new System.EventHandler(this.generalInfosToolStripMenuItem_Click);
            // 
            // fileBrowserToolStripMenuItem
            // 
            this.fileBrowserToolStripMenuItem.Name = "fileBrowserToolStripMenuItem";
            this.fileBrowserToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.fileBrowserToolStripMenuItem.Text = "File Browser";
            this.fileBrowserToolStripMenuItem.Click += new System.EventHandler(this.fileBrowserToolStripMenuItem_Click);
            // 
            // memoryDumpToolStripMenuItem
            // 
            this.memoryDumpToolStripMenuItem.Name = "memoryDumpToolStripMenuItem";
            this.memoryDumpToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.memoryDumpToolStripMenuItem.Text = "Memory Dump";
            this.memoryDumpToolStripMenuItem.Click += new System.EventHandler(this.memoryDumpToolStripMenuItem_Click);
            // 
            // modulesToolStripMenuItem
            // 
            this.modulesToolStripMenuItem.Name = "modulesToolStripMenuItem";
            this.modulesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.modulesToolStripMenuItem.Text = "Modules";
            this.modulesToolStripMenuItem.Click += new System.EventHandler(this.modulesToolStripMenuItem_Click);
            // 
            // optionToolStripMenuItem
            // 
            this.optionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.breakOnModuleLoadToolStripMenuItem,
            this.breakOnThreadCreateToolStripMenuItem,
            this.recordBreakpointsToTraceToolStripMenuItem});
            this.optionToolStripMenuItem.Name = "optionToolStripMenuItem";
            this.optionToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.optionToolStripMenuItem.Text = "Option";
            // 
            // breakOnModuleLoadToolStripMenuItem
            // 
            this.breakOnModuleLoadToolStripMenuItem.CheckOnClick = true;
            this.breakOnModuleLoadToolStripMenuItem.Name = "breakOnModuleLoadToolStripMenuItem";
            this.breakOnModuleLoadToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.breakOnModuleLoadToolStripMenuItem.Text = "Break on Module Load";
            this.breakOnModuleLoadToolStripMenuItem.Click += new System.EventHandler(this.breakOnModuleLoadToolStripMenuItem_Click);
            // 
            // breakOnThreadCreateToolStripMenuItem
            // 
            this.breakOnThreadCreateToolStripMenuItem.CheckOnClick = true;
            this.breakOnThreadCreateToolStripMenuItem.Name = "breakOnThreadCreateToolStripMenuItem";
            this.breakOnThreadCreateToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.breakOnThreadCreateToolStripMenuItem.Text = "Break on Thread Create";
            this.breakOnThreadCreateToolStripMenuItem.Click += new System.EventHandler(this.breakOnThreadCreateToolStripMenuItem_Click);
            // 
            // recordBreakpointsToTraceToolStripMenuItem
            // 
            this.recordBreakpointsToTraceToolStripMenuItem.CheckOnClick = true;
            this.recordBreakpointsToTraceToolStripMenuItem.Name = "recordBreakpointsToTraceToolStripMenuItem";
            this.recordBreakpointsToTraceToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.recordBreakpointsToTraceToolStripMenuItem.Text = "Record Breakpoints to Trace";
            this.recordBreakpointsToTraceToolStripMenuItem.Click += new System.EventHandler(this.recordBreakpointsToTraceToolStripMenuItem_Click);
            // 
            // traceToolStripMenuItem
            // 
            this.traceToolStripMenuItem.Name = "traceToolStripMenuItem";
            this.traceToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.traceToolStripMenuItem.Text = "Trace";
            this.traceToolStripMenuItem.Click += new System.EventHandler(this.traceToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(988, 576);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "XBox 360 Debugger by Warranty Voider";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generalInfosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileBrowserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem memoryDumpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modulesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem breakOnModuleLoadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem breakOnThreadCreateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recordBreakpointsToTraceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem traceToolStripMenuItem;
    }
}



