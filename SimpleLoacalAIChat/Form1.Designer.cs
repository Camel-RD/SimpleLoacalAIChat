using KlonsLIB.Components;

namespace SimpleLoacalAIChat
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            tbPrompt = new TextBox();
            tbOut = new FlatRichTextBox();
            statusStrip1 = new StatusStrip();
            tslPreset = new ToolStripStatusLabel();
            lbStatus = new ToolStripStatusLabel();
            pbProggress = new ToolStripProgressBar();
            tslTokenCount = new ToolStripStatusLabel();
            toolStrip1 = new MenuStrip();
            tsbAsk = new ToolStripButton();
            tsbContinue = new ToolStripButton();
            tsbCancel = new ToolStripButton();
            miDebugPrompt = new ToolStripMenuItem();
            exTabControl1 = new ExTabControl();
            tabPage1 = new TabPage();
            mySplitContainer1 = new MySplitContainer();
            tabPage2 = new TabPage();
            mySplitContainer4 = new MySplitContainer();
            dgvConfig = new MyDataGridView();
            dgcConfigName = new DataGridViewTextBoxColumn();
            bsConfig = new BindingSource(components);
            pgConfig = new MyPropertyGrid();
            tsConfig = new ToolStrip();
            tsbConfigNew = new ToolStripButton();
            tsbConfigCopy = new ToolStripButton();
            tsbConfigDelete = new ToolStripButton();
            tsbConfigSave = new ToolStripButton();
            tsbConfigLoad = new ToolStripButton();
            statusStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            exTabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mySplitContainer1).BeginInit();
            mySplitContainer1.Panel1.SuspendLayout();
            mySplitContainer1.Panel2.SuspendLayout();
            mySplitContainer1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mySplitContainer4).BeginInit();
            mySplitContainer4.Panel1.SuspendLayout();
            mySplitContainer4.Panel2.SuspendLayout();
            mySplitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvConfig).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bsConfig).BeginInit();
            tsConfig.SuspendLayout();
            SuspendLayout();
            // 
            // tbPrompt
            // 
            tbPrompt.Dock = DockStyle.Fill;
            tbPrompt.Location = new Point(0, 0);
            tbPrompt.Multiline = true;
            tbPrompt.Name = "tbPrompt";
            tbPrompt.ScrollBars = ScrollBars.Both;
            tbPrompt.Size = new Size(1020, 168);
            tbPrompt.TabIndex = 2;
            // 
            // tbOut
            // 
            tbOut.BorderColor = SystemColors.ControlText;
            tbOut.BorderStyle = BorderStyle.None;
            tbOut.Dock = DockStyle.Fill;
            tbOut.Location = new Point(0, 0);
            tbOut.Name = "tbOut";
            tbOut.Size = new Size(1020, 228);
            tbOut.TabIndex = 2;
            tbOut.Text = "";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { tslPreset, lbStatus, pbProggress, tslTokenCount });
            statusStrip1.Location = new Point(0, 498);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1034, 41);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // tslPreset
            // 
            tslPreset.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            tslPreset.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tslPreset.Name = "tslPreset";
            tslPreset.Size = new Size(103, 34);
            tslPreset.Text = "Preset: ...";
            // 
            // lbStatus
            // 
            lbStatus.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            lbStatus.Name = "lbStatus";
            lbStatus.Padding = new Padding(3, 0, 3, 0);
            lbStatus.Size = new Size(38, 34);
            lbStatus.Text = "...";
            // 
            // pbProggress
            // 
            pbProggress.Name = "pbProggress";
            pbProggress.Padding = new Padding(3, 0, 3, 0);
            pbProggress.Size = new Size(106, 33);
            // 
            // tslTokenCount
            // 
            tslTokenCount.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            tslTokenCount.Name = "tslTokenCount";
            tslTokenCount.Padding = new Padding(3, 0, 3, 0);
            tslTokenCount.Size = new Size(38, 34);
            tslTokenCount.Text = "...";
            // 
            // toolStrip1
            // 
            toolStrip1.Dock = DockStyle.Bottom;
            toolStrip1.ImageScalingSize = new Size(24, 24);
            toolStrip1.Items.AddRange(new ToolStripItem[] { tsbAsk, tsbContinue, tsbCancel, miDebugPrompt });
            toolStrip1.Location = new Point(3, 409);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1020, 43);
            toolStrip1.TabIndex = 4;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsbAsk
            // 
            tsbAsk.Image = Resource1.MoveNext;
            tsbAsk.ImageTransparentColor = Color.Magenta;
            tsbAsk.Name = "tsbAsk";
            tsbAsk.Size = new Size(75, 34);
            tsbAsk.Text = "Ask";
            tsbAsk.Click += tsbAsk_Click;
            // 
            // tsbContinue
            // 
            tsbContinue.Image = Resource1.MoveNext;
            tsbContinue.ImageTransparentColor = Color.Magenta;
            tsbContinue.Name = "tsbContinue";
            tsbContinue.Size = new Size(128, 34);
            tsbContinue.Text = "Continue";
            tsbContinue.Click += tsbContinue_Click;
            // 
            // tsbCancel
            // 
            tsbCancel.Image = Resource1.Delete;
            tsbCancel.ImageTransparentColor = Color.Magenta;
            tsbCancel.Name = "tsbCancel";
            tsbCancel.Size = new Size(105, 34);
            tsbCancel.Text = "Cancel";
            tsbCancel.Click += tsbCancel_Click;
            // 
            // miDebugPrompt
            // 
            miDebugPrompt.Alignment = ToolStripItemAlignment.Right;
            miDebugPrompt.Name = "miDebugPrompt";
            miDebugPrompt.Size = new Size(267, 39);
            miDebugPrompt.Text = "Show Formatted Prompt";
            miDebugPrompt.Click += miDebugPrompt_Click;
            // 
            // exTabControl1
            // 
            exTabControl1.Controls.Add(tabPage1);
            exTabControl1.Controls.Add(tabPage2);
            exTabControl1.DefaultStyle = false;
            exTabControl1.Dock = DockStyle.Fill;
            exTabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            exTabControl1.HeaderBackColor = SystemColors.Control;
            exTabControl1.HighlightBackColor = SystemColors.GradientInactiveCaption;
            exTabControl1.Location = new Point(0, 0);
            exTabControl1.Name = "exTabControl1";
            exTabControl1.SelectedIndex = 0;
            exTabControl1.Size = new Size(1034, 498);
            exTabControl1.TabIndex = 5;
            exTabControl1.SelectedIndexChanged += exTabControl1_SelectedIndexChanged;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(mySplitContainer1);
            tabPage1.Controls.Add(toolStrip1);
            tabPage1.Location = new Point(4, 39);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1026, 455);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Chat";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // mySplitContainer1
            // 
            mySplitContainer1.Dock = DockStyle.Fill;
            mySplitContainer1.FixedPanel = FixedPanel.Panel2;
            mySplitContainer1.Location = new Point(3, 3);
            mySplitContainer1.Name = "mySplitContainer1";
            mySplitContainer1.Orientation = Orientation.Horizontal;
            // 
            // mySplitContainer1.Panel1
            // 
            mySplitContainer1.Panel1.Controls.Add(tbOut);
            // 
            // mySplitContainer1.Panel2
            // 
            mySplitContainer1.Panel2.Controls.Add(tbPrompt);
            mySplitContainer1.Size = new Size(1020, 406);
            mySplitContainer1.SplitterDistance = 228;
            mySplitContainer1.SplitterWidth = 10;
            mySplitContainer1.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(mySplitContainer4);
            tabPage2.Controls.Add(tsConfig);
            tabPage2.Location = new Point(4, 39);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1026, 455);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Config";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // mySplitContainer4
            // 
            mySplitContainer4.Dock = DockStyle.Fill;
            mySplitContainer4.FixedPanel = FixedPanel.Panel1;
            mySplitContainer4.Location = new Point(3, 3);
            mySplitContainer4.Name = "mySplitContainer4";
            // 
            // mySplitContainer4.Panel1
            // 
            mySplitContainer4.Panel1.Controls.Add(dgvConfig);
            // 
            // mySplitContainer4.Panel2
            // 
            mySplitContainer4.Panel2.Controls.Add(pgConfig);
            mySplitContainer4.Size = new Size(1020, 410);
            mySplitContainer4.SplitterDistance = 350;
            mySplitContainer4.SplitterWidth = 10;
            mySplitContainer4.TabIndex = 3;
            // 
            // dgvConfig
            // 
            dgvConfig.AllowUserToAddRows = false;
            dgvConfig.AllowUserToResizeColumns = false;
            dgvConfig.AllowUserToResizeRows = false;
            dgvConfig.AutoGenerateColumns = false;
            dgvConfig.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvConfig.BackgroundColor = SystemColors.Control;
            dgvConfig.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvConfig.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvConfig.ColumnHeadersVisible = false;
            dgvConfig.Columns.AddRange(new DataGridViewColumn[] { dgcConfigName });
            dgvConfig.DataSource = bsConfig;
            dgvConfig.Dock = DockStyle.Fill;
            dgvConfig.Location = new Point(0, 0);
            dgvConfig.Name = "dgvConfig";
            dgvConfig.RowHeadersVisible = false;
            dgvConfig.RowHeadersWidth = 62;
            dgvConfig.Size = new Size(350, 410);
            dgvConfig.TabIndex = 0;
            dgvConfig.CellBeginEdit += dgvConfig_CellBeginEdit;
            dgvConfig.CellFormatting += dgvConfig_CellFormatting;
            // 
            // dgcConfigName
            // 
            dgcConfigName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgcConfigName.DataPropertyName = "Name";
            dgcConfigName.HeaderText = "Name";
            dgcConfigName.MinimumWidth = 8;
            dgcConfigName.Name = "dgcConfigName";
            // 
            // bsConfig
            // 
            bsConfig.DataSource = typeof(Models.ConfigPreset);
            bsConfig.CurrentChanged += bsConfig_CurrentChanged;
            // 
            // pgConfig
            // 
            pgConfig.CanShowVisualStyleGlyphs = false;
            pgConfig.CommandsVisibleIfAvailable = false;
            pgConfig.Dock = DockStyle.Fill;
            pgConfig.HelpVisible = false;
            pgConfig.LabelColumnWidth = 250;
            pgConfig.Location = new Point(0, 0);
            pgConfig.Name = "pgConfig";
            pgConfig.Size = new Size(660, 410);
            pgConfig.TabIndex = 0;
            pgConfig.ToolbarVisible = false;
            // 
            // tsConfig
            // 
            tsConfig.Dock = DockStyle.Bottom;
            tsConfig.ImageScalingSize = new Size(24, 24);
            tsConfig.Items.AddRange(new ToolStripItem[] { tsbConfigNew, tsbConfigCopy, tsbConfigDelete, tsbConfigSave, tsbConfigLoad });
            tsConfig.Location = new Point(3, 413);
            tsConfig.Name = "tsConfig";
            tsConfig.Size = new Size(1020, 39);
            tsConfig.TabIndex = 2;
            tsConfig.Text = "toolStrip3";
            // 
            // tsbConfigNew
            // 
            tsbConfigNew.Image = Resource1.AddNew;
            tsbConfigNew.ImageTransparentColor = Color.Magenta;
            tsbConfigNew.Name = "tsbConfigNew";
            tsbConfigNew.Size = new Size(85, 34);
            tsbConfigNew.Text = "New";
            tsbConfigNew.Click += tsbConfigNew_Click;
            // 
            // tsbConfigCopy
            // 
            tsbConfigCopy.Image = Resource1.Copy;
            tsbConfigCopy.ImageTransparentColor = Color.Magenta;
            tsbConfigCopy.Name = "tsbConfigCopy";
            tsbConfigCopy.Size = new Size(92, 34);
            tsbConfigCopy.Text = "Copy";
            tsbConfigCopy.Click += tsbConfigCopy_Click;
            // 
            // tsbConfigDelete
            // 
            tsbConfigDelete.Image = Resource1.Delete;
            tsbConfigDelete.ImageTransparentColor = Color.Magenta;
            tsbConfigDelete.Name = "tsbConfigDelete";
            tsbConfigDelete.Size = new Size(104, 34);
            tsbConfigDelete.Text = "Delete";
            tsbConfigDelete.Click += tsbConfigDelete_Click;
            // 
            // tsbConfigSave
            // 
            tsbConfigSave.Image = Resource1.Save1;
            tsbConfigSave.ImageTransparentColor = Color.Magenta;
            tsbConfigSave.Name = "tsbConfigSave";
            tsbConfigSave.Size = new Size(87, 34);
            tsbConfigSave.Text = "Save";
            tsbConfigSave.Click += tsbConfigSave_Click;
            // 
            // tsbConfigLoad
            // 
            tsbConfigLoad.Image = Resource1.open1;
            tsbConfigLoad.ImageTransparentColor = Color.Magenta;
            tsbConfigLoad.Name = "tsbConfigLoad";
            tsbConfigLoad.Size = new Size(154, 34);
            tsbConfigLoad.Text = "Load Preset";
            tsbConfigLoad.Click += tsbConfigLoad_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1034, 539);
            Controls.Add(exTabControl1);
            Controls.Add(statusStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = toolStrip1;
            Name = "Form1";
            Text = "Simple Chat";
            FormClosing += Form1_FormClosing;
            Shown += Form1_Shown;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            exTabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            mySplitContainer1.Panel1.ResumeLayout(false);
            mySplitContainer1.Panel2.ResumeLayout(false);
            mySplitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)mySplitContainer1).EndInit();
            mySplitContainer1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            mySplitContainer4.Panel1.ResumeLayout(false);
            mySplitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mySplitContainer4).EndInit();
            mySplitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvConfig).EndInit();
            ((System.ComponentModel.ISupportInitialize)bsConfig).EndInit();
            tsConfig.ResumeLayout(false);
            tsConfig.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox tbPrompt;
        private KlonsLIB.Components.FlatRichTextBox tbOut;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lbStatus;
        private ToolStripProgressBar pbProggress;
        private MenuStrip toolStrip1;
        private KlonsLIB.Components.ExTabControl exTabControl1;
        private TabPage tabPage1;
        private KlonsLIB.Components.MySplitContainer mySplitContainer1;
        private TabPage tabPage2;
        private ToolStripButton tsbContinue;
        private ToolStripButton tsbCancel;
        private KlonsLIB.Components.MySplitContainer mySplitContainer4;
        private KlonsLIB.Components.MyDataGridView dgvConfig;
        private KlonsLIB.Components.MyPropertyGrid pgConfig;
        private ToolStrip tsConfig;
        private ToolStripButton tsbConfigCopy;
        private ToolStripButton tsbConfigDelete;
        private ToolStripButton tsbConfigNew;
        private BindingSource bsConfig;
        private ToolStripButton tsbConfigLoad;
        private DataGridViewTextBoxColumn dgcConfigName;
        private ToolStripStatusLabel tslTokenCount;
        private ToolStripButton tsbAsk;
        private ToolStripStatusLabel tslPreset;
        private ToolStripMenuItem miDebugPrompt;
        private ToolStripButton tsbConfigSave;
    }
}
