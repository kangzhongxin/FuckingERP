using System.ComponentModel;
namespace Kzx.UserControl
{
    partial class KzxLookUpEditListBox
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
            this.ValueControl = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ValueControl.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // CaptionControl
            // 
            this.CaptionControl.Location = new System.Drawing.Point(-2, 3);
            this.CaptionControl.Name = "CaptionControl";
            this.CaptionControl.Size = new System.Drawing.Size(70, 14);
            this.CaptionControl.TabIndex = 2;
            this.CaptionControl.Text = "labelControl1";
            // 
            // ValueControl
            // 
            this.ValueControl.Location = new System.Drawing.Point(70, 0);
            this.ValueControl.Name = "ValueControl";
            this.ValueControl.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ValueControl.Properties.NullText = "";
            this.ValueControl.Size = new System.Drawing.Size(151, 20);
            this.ValueControl.TabIndex = 0;
            this.ValueControl.QueryPopUp += new System.ComponentModel.CancelEventHandler(this.lookUpEdit_QueryPopUp);
            this.ValueControl.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.lookUpEdit_Closed);
            // 
            // KzxLookUpEditListBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.Controls.Add(this.ValueControl);
            this.Controls.Add(this.CaptionControl);
            this.Name = "KzxLookUpEditListBox";
            this.Size = new System.Drawing.Size(221, 21);
            this.Load += new System.EventHandler(this.KzxLookUpEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ValueControl.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl CaptionControl;
        private DevExpress.XtraEditors.LookUpEdit ValueControl;
    }
}
