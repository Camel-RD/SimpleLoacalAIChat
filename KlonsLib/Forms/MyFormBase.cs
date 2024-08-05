﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using KlonsLIB.Misc;

namespace KlonsLIB.Forms
{
    public partial class MyFormBase : Form
    {
        private ToolStrip myToolStrip = null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool DialogCanceled { get; set; } = false;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string SelectedValue { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedValueInt { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<string> OnSelectedValue;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<int> OnSelectedValueInt;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IKlonsSettings Settings
        {
            get
            {
                if (MyData.Settings == null)
                    throw new Exception("Setting not set.");
                return MyData.Settings;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static MyMainFormBase MyMainForm { protected set; get; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMyDialog
        {
            get { return isMyDialog; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsFormClosing { get; private set; }

        public MyFormBase()
        {
            IsFormClosing = false;
            CloseOnEscape = false;
        }

        static MyFormBase()
        {
            MyMainForm = null;
        }

        [DefaultValue(8f)]
        public float DesignTimeFontSize { get; set; } = 8f;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            return;
            DesignTimeFontSize = Math.Min(32f, Math.Max(8f, DesignTimeFontSize));
            if (Font.Size != DesignTimeFontSize)
            {
                float scale_factor = Font.Size / DesignTimeFontSize;
                Scale(new SizeF(scale_factor, scale_factor));
            }
        }

        private bool isMyDialog = false;

        private bool OnSelectedValueCalled = false;

        protected void SetSelectedValue(string value, bool cancel = false)
        {
            if (!this.Validate()) return;

            DialogCanceled = cancel;
            SelectedValue = value;
            if (Modal || IsMyDialog)
            {
                if (OnSelectedValue != null) OnSelectedValue(value);
                OnSelectedValueCalled = true;
            }
            if (Modal)
            {
                DialogResult = value == null || cancel ? DialogResult.Cancel : DialogResult.OK;
            }
            else if (IsMyDialog)
            {
                Close();
            }
        }

        protected void SetSelectedValue(int value, bool cancel = false)
        {
            if (!this.Validate()) return;

            DialogCanceled = cancel;
            SelectedValueInt = value;
            SelectedValue = value.ToString();

            if (Modal || IsMyDialog)
            {
                if (OnSelectedValueInt != null) OnSelectedValueInt(SelectedValueInt);
                if (OnSelectedValue != null) OnSelectedValue(SelectedValue);
                OnSelectedValueCalled = true;
            }
            if (Modal)
            {
                DialogResult = cancel ? DialogResult.Cancel : DialogResult.OK;
            }
            else if (IsMyDialog)
            {
                Close();
            }
        }

        public void ShowMyDialog()
        {
            isMyDialog = true;
            this.Show();
        }

        public DialogResult ShowMyDialogModal()
        {
            isMyDialog = true;
            return this.ShowDialog(MyMainForm);
        }

        [DefaultValue(null)]
        [TypeConverter(typeof(ReferenceConverter))]
        public virtual ToolStrip MyToolStrip
        {
            get { return myToolStrip; }
            set
            {
                if (value == myToolStrip) return;
                myToolStrip = value;
                if (myToolStrip != null && MyMainForm != null &&
                    MyMainForm.MainMenuStrip != null)
                {
                    myToolStrip.Renderer = MyMainForm.MainMenuStrip.Renderer;
                }
            }
        }

        public virtual void SaveParams()
        {

        }
        public virtual bool SaveData()
        {
            return true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (IsFormClosing) return;
            IsFormClosing = true;
            try
            {
                SaveParams();
            }
            catch (Exception) { }

            try
            {
                base.OnFormClosing(e);
                if (e.Cancel)
                {
                    IsFormClosing = false;
                    return;
                }
                if (!SaveData())
                {
                    var s = String.Format("Logs [{0}] tiks aizvērt, bet\niespējams, ka datos bija kļūda.", this.Text);
                    MyMainForm.ShowWarning(s);
                    IsFormClosing = false;
                    return;
                }

                if (Modal || IsMyDialog)
                {
                    if (OnSelectedValue != null && !OnSelectedValueCalled) OnSelectedValue(null);
                    //if (OnSelectedValueInt != null && !OnSelectedValueCalled) OnSelectedValueInt(null);
                    OnSelectedValueCalled = true;
                }
            }
            finally
            {
                if (e.Cancel == true)
                    IsFormClosing = false;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (this.IsMdiChild && this.MdiParent != null)
            {
                var fm = MdiParent as MyMainFormBase;
                if (fm != null) fm.OnMyCloseForm(this);
            }
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        public bool CloseOnEscape { get; set; }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Tab)) return true;
            if (keyData == (Keys.Control | Keys.F6)) return true;
            if (keyData == (Keys.Control | Keys.Shift | Keys.F6)) return true;
            if (keyData == Keys.Escape && CloseOnEscape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        #region +++++++++ Theming and Scaling +++++++++

        protected virtual MyColorTheme MyColorTheme => MyData.Settings.ColorTheme;

        protected void SetColorTheme(MyColorTheme theme)
        {
            ColorThemeHelper.ApplyToForm(this, theme);
        }

        public virtual void CheckMenuColorTheme()
        {
            ColorThemeHelper.MyToolStripRenderer.SetColorTheme(MyColorTheme);
            if (this.MainMenuStrip != null)
                MainMenuStrip.Refresh();
        }
        
        public void CheckMyFontAndColors()
        {
            MyColorTheme cth = Settings.ColorTheme;
            ColorThemeHelper.ApplyToForm(this, cth);
        }

        public void CheckMyFontAndColors2()
        {
            SuspendLayout();
            this.Font = Settings.FormFont;
            foreach (Control c in GetAllControls(this))
            {
                if (c is ToolStrip tsp)
                {
                    if (c.Font != this.Font)
                        c.Font = this.Font;
                    foreach (var ti in GetAllToolsStripItems(tsp))
                    {
                        if (ti is ToolStripComboBox tscb)
                            tscb.Font = this.Font;
                    }
                }
                else
                {
                    if (!c.Font.Equals(this.Font))
                    {
                        c.Font = new Font(Font.FontFamily, Font.SizeInPoints, c.Font.Style);
                    }
                }
            }
            ResumeLayout(true);
        }

        protected virtual void CheckMyFontSize()
        {
            if (this.Font.Size != Settings.FormFontSize)
                this.Font = new Font(this.Font.Name, Settings.FormFontSize, this.Font.Style);
        }

        protected void SetFontSize(int sz)
        {
            if (this.Font.Size != sz)
                this.Font = new Font(this.Font.Name, sz, this.Font.Style);
        }

        protected void CreateAllTabPages()
        {
            foreach (Control c in GetAllControls(this))
            {
                if (c is not TabControl tc) continue;
                int k0 = tc.SelectedIndex;
                for (int k1 = 0; k1 < tc.TabCount; k1++)
                {
                    if (k1 == k0) continue;
                    tc.SelectedIndex = k1;
                }
                tc.SelectedIndex = k0;
            }
        }

        public static IEnumerable<Control> GetAllControls(Control control)
        {
            foreach (Control c in control.Controls)
            {
                yield return c;
                foreach (Control c1 in GetAllControls(c))
                {
                    yield return c1;
                }
            }
        }

        public static IEnumerable<ToolStripItem> GetAllToolsStripItems(ToolStrip tsp)
        {
            foreach (ToolStripItem tsi in tsp.Items)
            {
                foreach (ToolStripItem tsi2 in GetAllToolsStripItems(tsi))
                {
                    yield return tsi2;
                }
            }
        }

        public static IEnumerable<ToolStripItem> GetAllToolsStripItems(ToolStripItem tsi)
        {
            yield return tsi;
            if (tsi is ToolStripDropDownItem tsdd)
            {
                foreach (ToolStripItem tsi2 in tsdd.DropDownItems)
                {
                    foreach (ToolStripItem tsi3 in GetAllToolsStripItems(tsi2))
                    {
                        yield return tsi3;
                    }
                }
            }
        }

        private SizeF scaleFactor = new SizeF(1.0f, 1.0f);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SizeF ScaleFactor => scaleFactor;
        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            scaleFactor = new SizeF(scaleFactor.Width * factor.Width, scaleFactor.Height * factor.Height);
            base.ScaleControl(factor, specified);
            ScaleToolStrips(this, factor);
        }

        protected void ScaleToolStrips(Form form, SizeF factor)
        {
            foreach (var c in GetAllControls(form))
            {
                if (c is ToolStrip tsp && !(c is MenuStrip))
                {
                    ScaleToolStrip(tsp, factor);
                }
            }
        }

        protected void ScaleToolStrip(ToolStrip tsp, SizeF factor)
        {
            if (factor.Height != 1.0f)
            {
                var imgsz = tsp.ImageScalingSize;
                float f = Math.Max(factor.Width, factor.Height);
                imgsz.Width = (int)((float)imgsz.Width * f);
                imgsz.Height = (int)((float)imgsz.Height * f);
                tsp.ImageScalingSize = imgsz;
            }
        }
        #endregion
    }
}
