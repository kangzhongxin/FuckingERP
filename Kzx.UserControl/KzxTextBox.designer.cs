namespace Kzx.UserControl
{
    partial class KzxTextBox
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.CaptionControl = new DevExpress.XtraEditors.LabelControl();
            this.ValueControl = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ValueControl.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // CaptionControl
            // 
            this.CaptionControl.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.CaptionControl.Location = new System.Drawing.Point(0, 4);
            this.CaptionControl.Name = "CaptionControl";
            this.CaptionControl.Size = new System.Drawing.Size(70, 14);
            this.CaptionControl.TabIndex = 0;
            this.CaptionControl.Text = "labelControl1";
            // 
            // ValueControl
            // 
            this.ValueControl.Location = new System.Drawing.Point(181, 1);
            this.ValueControl.Name = "ValueControl";
            this.ValueControl.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.ValueControl.Properties.Appearance.Options.UseBackColor = true;
            this.ValueControl.Size = new System.Drawing.Size(100, 20);
            this.ValueControl.TabIndex = 0;
            this.ValueControl.EditValueChanged += new System.EventHandler(this.ValueControl_EditValueChanged);
            this.ValueControl.Enter += new System.EventHandler(this.ValueControl_Enter);
            this.ValueControl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ValueControl_KeyPress);
            this.ValueControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ValueControl_MouseUp);
            // 
            // YZTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.Controls.Add(this.ValueControl);
            this.Controls.Add(this.CaptionControl);
            this.Name = "YZTextBox";
            this.Size = new System.Drawing.Size(284, 21);
            this.Load += new System.EventHandler(this.YZTextBox_Load);
            this.SizeChanged += new System.EventHandler(this.SetSize);
            ((System.ComponentModel.ISupportInitialize)(this.ValueControl.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl CaptionControl;
        private DevExpress.XtraEditors.TextEdit ValueControl;
    }
}
