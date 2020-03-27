namespace FuckingERP.重写控件_弹窗类
{
    partial class frm_ShowControl
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
            this.toolTipController1 = new DevExpress.Utils.ToolTipController(this.components);
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.kzxTextBox1 = new Kzx.UserControl.KzxTextBox();
            this.kzxSimpleButton1 = new Kzx.UserControl.KzxSimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTipController1
            // 
            this.toolTipController1.ToolTipLocation = DevExpress.Utils.ToolTipLocation.TopRight;
            this.toolTipController1.ToolTipStyle = DevExpress.Utils.ToolTipStyle.Windows7;
            this.toolTipController1.ToolTipType = DevExpress.Utils.ToolTipType.SuperTip;
            this.toolTipController1.GetActiveObjectInfo += new DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventHandler(this.toolTipController1_GetActiveObjectInfo);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.kzxTextBox1);
            this.groupControl1.Controls.Add(this.kzxSimpleButton1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(800, 81);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "自定义控件测试";
            // 
            // kzxTextBox1
            // 
            this.kzxTextBox1.AllowEdit = true;
            this.kzxTextBox1.AllowValueRange = false;
            this.kzxTextBox1.CaptionLabelWidth = 50;
            this.kzxTextBox1.DataSource = null;
            this.kzxTextBox1.DataType = Kzx.UserControl.KzxDataType.Str;
            this.kzxTextBox1.DefaultValue = "";
            this.kzxTextBox1.DesigeCaption = "用户名";
            this.kzxTextBox1.DesigeEnabled = true;
            this.kzxTextBox1.DesigeVisible = true;
            this.kzxTextBox1.DisplayMemberPath = "";
            this.kzxTextBox1.DllName = "";
            this.kzxTextBox1.Field = "";
            this.kzxTextBox1.FormatString = "";
            this.kzxTextBox1.IsNull = true;
            this.kzxTextBox1.IsUnique = false;
            this.kzxTextBox1.IsValidation = true;
            this.kzxTextBox1.Key = "KzxTextBox";
            this.kzxTextBox1.KzxBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.kzxTextBox1.KzxEditMask = "";
            this.kzxTextBox1.KzxFormatString = "";
            this.kzxTextBox1.KzxFormatType = DevExpress.Utils.FormatType.None;
            this.kzxTextBox1.KzxMaskType = "None";
            this.kzxTextBox1.LabelForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.kzxTextBox1.LayoutColumn = 0;
            this.kzxTextBox1.LayoutColumnSpan = 1;
            this.kzxTextBox1.LayoutRow = 0;
            this.kzxTextBox1.LayoutRowSpan = 1;
            this.kzxTextBox1.Location = new System.Drawing.Point(152, 39);
            this.kzxTextBox1.MaxLength = 0;
            this.kzxTextBox1.MaxValue = "";
            this.kzxTextBox1.MessageCode = "0";
            this.kzxTextBox1.MinValue = "";
            this.kzxTextBox1.Name = "kzxTextBox1";
            this.kzxTextBox1.ParentTable = null;
            this.kzxTextBox1.PasswordChar = '\0';
            this.kzxTextBox1.ReadOnly = false;
            this.kzxTextBox1.RegexString = "";
            this.kzxTextBox1.SelectedValuePath = "";
            this.kzxTextBox1.Size = new System.Drawing.Size(284, 21);
            this.kzxTextBox1.SourceTable = "";
            this.kzxTextBox1.TabIndex = 2;
            this.kzxTextBox1.Table = "";
            this.kzxTextBox1.TextBackColor = System.Drawing.Color.White;
            this.kzxTextBox1.TextForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.kzxTextBox1.ToolTipMaxLengthText = "";
            this.kzxTextBox1.ToolTipMessageCode = "";
            this.kzxTextBox1.ToolTipText = "";
            this.kzxTextBox1.ValidateGroup = "";
            this.kzxTextBox1.Value = "";
            this.kzxTextBox1.ValueDependencyField = "";
            // 
            // kzxSimpleButton1
            // 
            this.kzxSimpleButton1.DesigeCaption = "测试弹窗提示";
            this.kzxSimpleButton1.DesigeEnabled = true;
            this.kzxSimpleButton1.DesigeVisible = true;
            this.kzxSimpleButton1.DllName = "";
            this.kzxSimpleButton1.Key = "kzxSimpleButton1";
            this.kzxSimpleButton1.KzxButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.kzxSimpleButton1.KzxImage = null;
            this.kzxSimpleButton1.KzxImageLocation = DevExpress.XtraEditors.ImageLocation.Default;
            this.kzxSimpleButton1.LabelForeColor = System.Drawing.Color.Empty;
            this.kzxSimpleButton1.LayoutColumn = 0;
            this.kzxSimpleButton1.LayoutColumnSpan = 1;
            this.kzxSimpleButton1.LayoutRow = 0;
            this.kzxSimpleButton1.LayoutRowSpan = 1;
            this.kzxSimpleButton1.Location = new System.Drawing.Point(12, 38);
            this.kzxSimpleButton1.MessageCode = "0";
            this.kzxSimpleButton1.Name = "kzxSimpleButton1";
            this.kzxSimpleButton1.Size = new System.Drawing.Size(75, 23);
            this.kzxSimpleButton1.TabIndex = 1;
            this.kzxSimpleButton1.Text = "测试弹窗提示";
            this.kzxSimpleButton1.ToolTipController = this.toolTipController1;
            this.kzxSimpleButton1.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.kzxSimpleButton1.ToolTipMessageCode = "";
            this.kzxSimpleButton1.ToolTipText = "点击后，会弹出重写的弹出提示框！";
            // 
            // frm_ShowControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupControl1);
            this.HideOnClose = true;
            this.Name = "frm_ShowControl";
            this.Text = "重写控件-弹窗类";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_ShowControl_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.Utils.ToolTipController toolTipController1;
        private Kzx.UserControl.KzxSimpleButton kzxSimpleButton1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private Kzx.UserControl.KzxTextBox kzxTextBox1;
    }
}