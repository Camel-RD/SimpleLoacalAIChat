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
            tbPrompt = new FlatRichTextBox();
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
            tsbTools = new ToolStripMenuItem();
            miIncludePrefilledResponse = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            miModelMetaData = new ToolStripMenuItem();
            miDebugPrompt = new ToolStripMenuItem();
            exTabControl1 = new ExTabControl();
            tabChat = new TabPage();
            mySplitContainer1 = new MySplitContainer();
            tabEditChat = new TabPage();
            tbEditChat = new FlatRichTextBox();
            toolStrip2 = new ToolStrip();
            tsbAddRequest = new ToolStripButton();
            tsbAddResponse = new ToolStripButton();
            tsbEditorSave = new ToolStripButton();
            tabConfig = new TabPage();
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
            tabChat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mySplitContainer1).BeginInit();
            mySplitContainer1.Panel1.SuspendLayout();
            mySplitContainer1.Panel2.SuspendLayout();
            mySplitContainer1.SuspendLayout();
            tabEditChat.SuspendLayout();
            toolStrip2.SuspendLayout();
            tabConfig.SuspendLayout();
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
            tbPrompt.BorderColor = SystemColors.ControlText;
            tbPrompt.BorderStyle = BorderStyle.None;
            tbPrompt.Dock = DockStyle.Fill;
            tbPrompt.Location = new Point(0, 0);
            tbPrompt.Margin = new Padding(2);
            tbPrompt.Name = "tbPrompt";
            tbPrompt.Size = new Size(677, 135);
            tbPrompt.TabIndex = 2;
            tbPrompt.Text = "";
            // 
            // tbOut
            // 
            tbOut.BorderColor = SystemColors.ControlText;
            tbOut.BorderStyle = BorderStyle.None;
            tbOut.Dock = DockStyle.Fill;
            tbOut.Location = new Point(0, 0);
            tbOut.Margin = new Padding(2);
            tbOut.Name = "tbOut";
            tbOut.Size = new Size(677, 103);
            tbOut.TabIndex = 2;
            tbOut.Text = "";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(16, 15);
            statusStrip1.Items.AddRange(new ToolStripItem[] { tslPreset, lbStatus, pbProggress, tslTokenCount });
            statusStrip1.Location = new Point(0, 313);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 9, 0);
            statusStrip1.Size = new Size(689, 28);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // tslPreset
            // 
            tslPreset.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            tslPreset.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tslPreset.Name = "tslPreset";
            tslPreset.Size = new Size(67, 23);
            tslPreset.Text = "Preset: ...";
            // 
            // lbStatus
            // 
            lbStatus.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            lbStatus.Name = "lbStatus";
            lbStatus.Padding = new Padding(3, 0, 3, 0);
            lbStatus.Size = new Size(28, 23);
            lbStatus.Text = "...";
            // 
            // pbProggress
            // 
            pbProggress.Name = "pbProggress";
            pbProggress.Padding = new Padding(3, 0, 3, 0);
            pbProggress.Size = new Size(59, 22);
            // 
            // tslTokenCount
            // 
            tslTokenCount.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            tslTokenCount.Name = "tslTokenCount";
            tslTokenCount.Padding = new Padding(3, 0, 3, 0);
            tslTokenCount.Size = new Size(28, 23);
            tslTokenCount.Text = "...";
            // 
            // toolStrip1
            // 
            toolStrip1.Dock = DockStyle.Bottom;
            toolStrip1.ImageScalingSize = new Size(24, 24);
            toolStrip1.Items.AddRange(new ToolStripItem[] { tsbAsk, tsbContinue, tsbCancel, tsbTools });
            toolStrip1.Location = new Point(2, 246);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(4, 1, 0, 1);
            toolStrip1.Size = new Size(677, 33);
            toolStrip1.TabIndex = 4;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsbAsk
            // 
            tsbAsk.Image = Resource1.MoveNext;
            tsbAsk.ImageTransparentColor = Color.Magenta;
            tsbAsk.Name = "tsbAsk";
            tsbAsk.Size = new Size(59, 28);
            tsbAsk.Text = "Ask";
            tsbAsk.Click += tsbAsk_Click;
            // 
            // tsbContinue
            // 
            tsbContinue.Image = Resource1.MoveNext;
            tsbContinue.ImageTransparentColor = Color.Magenta;
            tsbContinue.Name = "tsbContinue";
            tsbContinue.Size = new Size(93, 28);
            tsbContinue.Text = "Continue";
            tsbContinue.Click += tsbContinue_Click;
            // 
            // tsbCancel
            // 
            tsbCancel.Image = Resource1.Delete;
            tsbCancel.ImageTransparentColor = Color.Magenta;
            tsbCancel.Name = "tsbCancel";
            tsbCancel.Size = new Size(77, 28);
            tsbCancel.Text = "Cancel";
            tsbCancel.Click += tsbCancel_Click;
            // 
            // tsbTools
            // 
            tsbTools.DropDownItems.AddRange(new ToolStripItem[] { miIncludePrefilledResponse, toolStripSeparator1, miModelMetaData, miDebugPrompt });
            tsbTools.Name = "tsbTools";
            tsbTools.Size = new Size(52, 31);
            tsbTools.Text = "Tools";
            // 
            // miIncludePrefilledResponse
            // 
            miIncludePrefilledResponse.Name = "miIncludePrefilledResponse";
            miIncludePrefilledResponse.Size = new Size(236, 24);
            miIncludePrefilledResponse.Text = "Include Prefilled Response";
            miIncludePrefilledResponse.Click += miIncludePrefilledResponse_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(233, 6);
            // 
            // miModelMetaData
            // 
            miModelMetaData.Alignment = ToolStripItemAlignment.Right;
            miModelMetaData.Name = "miModelMetaData";
            miModelMetaData.Size = new Size(236, 24);
            miModelMetaData.Text = "Model Meta Data";
            miModelMetaData.Click += miModelMetaData_Click;
            // 
            // miDebugPrompt
            // 
            miDebugPrompt.Alignment = ToolStripItemAlignment.Right;
            miDebugPrompt.Name = "miDebugPrompt";
            miDebugPrompt.Size = new Size(236, 24);
            miDebugPrompt.Text = "Show Formatted Prompt";
            miDebugPrompt.Click += miDebugPrompt_Click;
            // 
            // exTabControl1
            // 
            exTabControl1.Controls.Add(tabChat);
            exTabControl1.Controls.Add(tabEditChat);
            exTabControl1.Controls.Add(tabConfig);
            exTabControl1.DefaultStyle = false;
            exTabControl1.Dock = DockStyle.Fill;
            exTabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            exTabControl1.HeaderBackColor = SystemColors.Control;
            exTabControl1.HighlightBackColor = SystemColors.GradientInactiveCaption;
            exTabControl1.Location = new Point(0, 0);
            exTabControl1.Margin = new Padding(2);
            exTabControl1.Name = "exTabControl1";
            exTabControl1.SelectedIndex = 0;
            exTabControl1.Size = new Size(689, 313);
            exTabControl1.TabIndex = 5;
            exTabControl1.SelectedIndexChanged += exTabControl1_SelectedIndexChanged_1;
            // 
            // tabChat
            // 
            tabChat.Controls.Add(mySplitContainer1);
            tabChat.Controls.Add(toolStrip1);
            tabChat.Location = new Point(4, 28);
            tabChat.Margin = new Padding(2);
            tabChat.Name = "tabChat";
            tabChat.Padding = new Padding(2);
            tabChat.Size = new Size(681, 281);
            tabChat.TabIndex = 0;
            tabChat.Text = "Chat";
            tabChat.UseVisualStyleBackColor = true;
            // 
            // mySplitContainer1
            // 
            mySplitContainer1.Dock = DockStyle.Fill;
            mySplitContainer1.FixedPanel = FixedPanel.Panel2;
            mySplitContainer1.Location = new Point(2, 2);
            mySplitContainer1.Margin = new Padding(2);
            mySplitContainer1.Name = "mySplitContainer1";
            mySplitContainer1.Orientation = Orientation.Horizontal;
            // 
            // mySplitContainer1.Panel1
            // 
            mySplitContainer1.Panel1.Controls.Add(tbOut);
            mySplitContainer1.Panel1MinSize = 16;
            // 
            // mySplitContainer1.Panel2
            // 
            mySplitContainer1.Panel2.Controls.Add(tbPrompt);
            mySplitContainer1.Panel2MinSize = 16;
            mySplitContainer1.Size = new Size(677, 244);
            mySplitContainer1.SplitterDistance = 103;
            mySplitContainer1.SplitterWidth = 6;
            mySplitContainer1.TabIndex = 0;
            // 
            // tabEditChat
            // 
            tabEditChat.Controls.Add(tbEditChat);
            tabEditChat.Controls.Add(toolStrip2);
            tabEditChat.Location = new Point(4, 28);
            tabEditChat.Margin = new Padding(2);
            tabEditChat.Name = "tabEditChat";
            tabEditChat.Padding = new Padding(2);
            tabEditChat.Size = new Size(681, 281);
            tabEditChat.TabIndex = 2;
            tabEditChat.Text = "Edit Chat";
            tabEditChat.UseVisualStyleBackColor = true;
            // 
            // tbEditChat
            // 
            tbEditChat.BorderColor = SystemColors.ControlText;
            tbEditChat.BorderStyle = BorderStyle.None;
            tbEditChat.Dock = DockStyle.Fill;
            tbEditChat.Location = new Point(2, 2);
            tbEditChat.Margin = new Padding(2);
            tbEditChat.Name = "tbEditChat";
            tbEditChat.Size = new Size(677, 251);
            tbEditChat.TabIndex = 1;
            tbEditChat.Text = "";
            // 
            // toolStrip2
            // 
            toolStrip2.Dock = DockStyle.Bottom;
            toolStrip2.Items.AddRange(new ToolStripItem[] { tsbAddRequest, tsbAddResponse, tsbEditorSave });
            toolStrip2.Location = new Point(2, 253);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.Size = new Size(677, 26);
            toolStrip2.TabIndex = 0;
            toolStrip2.Text = "toolStrip2";
            // 
            // tsbAddRequest
            // 
            tsbAddRequest.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsbAddRequest.Image = (Image)resources.GetObject("tsbAddRequest.Image");
            tsbAddRequest.ImageTransparentColor = Color.Magenta;
            tsbAddRequest.Name = "tsbAddRequest";
            tsbAddRequest.Size = new Size(91, 23);
            tsbAddRequest.Text = "Add Request";
            tsbAddRequest.Click += tsbAddRequest_Click;
            // 
            // tsbAddResponse
            // 
            tsbAddResponse.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsbAddResponse.Image = (Image)resources.GetObject("tsbAddResponse.Image");
            tsbAddResponse.ImageTransparentColor = Color.Magenta;
            tsbAddResponse.Name = "tsbAddResponse";
            tsbAddResponse.Size = new Size(100, 23);
            tsbAddResponse.Text = "Add Response";
            tsbAddResponse.Click += tsbAddResponse_Click;
            // 
            // tsbEditorSave
            // 
            tsbEditorSave.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsbEditorSave.Image = (Image)resources.GetObject("tsbEditorSave.Image");
            tsbEditorSave.ImageTransparentColor = Color.Magenta;
            tsbEditorSave.Name = "tsbEditorSave";
            tsbEditorSave.Size = new Size(41, 23);
            tsbEditorSave.Text = "Save";
            tsbEditorSave.Click += tsbEditorSave_Click;
            // 
            // tabConfig
            // 
            tabConfig.Controls.Add(mySplitContainer4);
            tabConfig.Controls.Add(tsConfig);
            tabConfig.Location = new Point(4, 28);
            tabConfig.Margin = new Padding(2);
            tabConfig.Name = "tabConfig";
            tabConfig.Padding = new Padding(2);
            tabConfig.Size = new Size(681, 281);
            tabConfig.TabIndex = 1;
            tabConfig.Text = "Config";
            tabConfig.UseVisualStyleBackColor = true;
            // 
            // mySplitContainer4
            // 
            mySplitContainer4.Dock = DockStyle.Fill;
            mySplitContainer4.FixedPanel = FixedPanel.Panel1;
            mySplitContainer4.Location = new Point(2, 2);
            mySplitContainer4.Margin = new Padding(2);
            mySplitContainer4.Name = "mySplitContainer4";
            // 
            // mySplitContainer4.Panel1
            // 
            mySplitContainer4.Panel1.Controls.Add(dgvConfig);
            mySplitContainer4.Panel1MinSize = 17;
            // 
            // mySplitContainer4.Panel2
            // 
            mySplitContainer4.Panel2.Controls.Add(pgConfig);
            mySplitContainer4.Panel2MinSize = 17;
            mySplitContainer4.Size = new Size(677, 251);
            mySplitContainer4.SplitterDistance = 233;
            mySplitContainer4.SplitterWidth = 7;
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
            dgvConfig.Margin = new Padding(2);
            dgvConfig.Name = "dgvConfig";
            dgvConfig.RowHeadersVisible = false;
            dgvConfig.RowHeadersWidth = 62;
            dgvConfig.RowTemplate.Height = 25;
            dgvConfig.Size = new Size(233, 251);
            dgvConfig.TabIndex = 0;
            dgvConfig.CellBeginEdit += dgvConfig_CellBeginEdit;
            dgvConfig.CellFormatting += dgvConfig_CellFormatting;
            // 
            // dgcConfigName
            // 
            dgcConfigName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgcConfigName.DataPropertyName = "Name";
            dgcConfigName.HeaderText = "Name";
            dgcConfigName.MinimumWidth = 4;
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
            pgConfig.LabelColumnWidth = 200;
            pgConfig.Location = new Point(0, 0);
            pgConfig.Margin = new Padding(2);
            pgConfig.Name = "pgConfig";
            pgConfig.Size = new Size(437, 251);
            pgConfig.TabIndex = 0;
            pgConfig.ToolbarVisible = false;
            // 
            // tsConfig
            // 
            tsConfig.Dock = DockStyle.Bottom;
            tsConfig.ImageScalingSize = new Size(16, 15);
            tsConfig.Items.AddRange(new ToolStripItem[] { tsbConfigNew, tsbConfigCopy, tsbConfigDelete, tsbConfigSave, tsbConfigLoad });
            tsConfig.Location = new Point(2, 253);
            tsConfig.Name = "tsConfig";
            tsConfig.Size = new Size(677, 26);
            tsConfig.TabIndex = 2;
            tsConfig.Text = "toolStrip3";
            // 
            // tsbConfigNew
            // 
            tsbConfigNew.Image = Resource1.AddNew;
            tsbConfigNew.ImageTransparentColor = Color.Magenta;
            tsbConfigNew.Name = "tsbConfigNew";
            tsbConfigNew.Size = new Size(56, 23);
            tsbConfigNew.Text = "New";
            tsbConfigNew.Click += tsbConfigNew_Click;
            // 
            // tsbConfigCopy
            // 
            tsbConfigCopy.Image = Resource1.Copy;
            tsbConfigCopy.ImageTransparentColor = Color.Magenta;
            tsbConfigCopy.Name = "tsbConfigCopy";
            tsbConfigCopy.Size = new Size(61, 23);
            tsbConfigCopy.Text = "Copy";
            tsbConfigCopy.Click += tsbConfigCopy_Click;
            // 
            // tsbConfigDelete
            // 
            tsbConfigDelete.Image = Resource1.Delete;
            tsbConfigDelete.ImageTransparentColor = Color.Magenta;
            tsbConfigDelete.Name = "tsbConfigDelete";
            tsbConfigDelete.Size = new Size(68, 23);
            tsbConfigDelete.Text = "Delete";
            tsbConfigDelete.Click += tsbConfigDelete_Click;
            // 
            // tsbConfigSave
            // 
            tsbConfigSave.Image = Resource1.Save1;
            tsbConfigSave.ImageTransparentColor = Color.Magenta;
            tsbConfigSave.Name = "tsbConfigSave";
            tsbConfigSave.Size = new Size(57, 23);
            tsbConfigSave.Text = "Save";
            tsbConfigSave.Click += tsbConfigSave_Click;
            // 
            // tsbConfigLoad
            // 
            tsbConfigLoad.Image = Resource1.open1;
            tsbConfigLoad.ImageTransparentColor = Color.Magenta;
            tsbConfigLoad.Name = "tsbConfigLoad";
            tsbConfigLoad.Size = new Size(101, 23);
            tsbConfigLoad.Text = "Load Preset";
            tsbConfigLoad.Click += tsbConfigLoad_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(689, 341);
            Controls.Add(exTabControl1);
            Controls.Add(statusStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = toolStrip1;
            Margin = new Padding(2);
            Name = "Form1";
            Text = "Simple Chat";
            FormClosing += Form1_FormClosing;
            Shown += Form1_Shown;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            exTabControl1.ResumeLayout(false);
            tabChat.ResumeLayout(false);
            tabChat.PerformLayout();
            mySplitContainer1.Panel1.ResumeLayout(false);
            mySplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mySplitContainer1).EndInit();
            mySplitContainer1.ResumeLayout(false);
            tabEditChat.ResumeLayout(false);
            tabEditChat.PerformLayout();
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
            tabConfig.ResumeLayout(false);
            tabConfig.PerformLayout();
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
        private KlonsLIB.Components.FlatRichTextBox tbPrompt;
        private KlonsLIB.Components.FlatRichTextBox tbOut;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lbStatus;
        private ToolStripProgressBar pbProggress;
        private MenuStrip toolStrip1;
        private KlonsLIB.Components.ExTabControl exTabControl1;
        private TabPage tabChat;
        private KlonsLIB.Components.MySplitContainer mySplitContainer1;
        private TabPage tabConfig;
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
        private ToolStripButton tsbConfigSave;
        private ToolStripMenuItem tsbTools;
        private ToolStripMenuItem miIncludePrefilledResponse;
        private ToolStripMenuItem miModelMetaData;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem miDebugPrompt;
        private TabPage tabEditChat;
        private FlatRichTextBox tbEditChat;
        private ToolStrip toolStrip2;
        private ToolStripButton tsbAddRequest;
        private ToolStripButton tsbAddResponse;
        private ToolStripButton tsbEditorSave;
    }
}
