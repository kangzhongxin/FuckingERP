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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.sbtnOverrideShow = new DevExpress.XtraEditors.SimpleButton();
            this.toolTipController1 = new DevExpress.Utils.ToolTipController(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.sbtnOverrideShow);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(800, 100);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "弹窗测试";
            // 
            // sbtnOverrideShow
            // 
            this.sbtnOverrideShow.Location = new System.Drawing.Point(24, 34);
            this.sbtnOverrideShow.Name = "sbtnOverrideShow";
            this.sbtnOverrideShow.Size = new System.Drawing.Size(91, 23);
            this.sbtnOverrideShow.TabIndex = 0;
            this.sbtnOverrideShow.Text = "重写弹窗提示";
            this.sbtnOverrideShow.ToolTipController = this.toolTipController1;
            this.sbtnOverrideShow.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Warning;
            this.sbtnOverrideShow.ToolTipTitle = "this is a test !";
            this.sbtnOverrideShow.Click += new System.EventHandler(this.sbtnOverrideShow_Click);
            // 
            // toolTipController1
            // 
            this.toolTipController1.ToolTipLocation = DevExpress.Utils.ToolTipLocation.TopRight;
            this.toolTipController1.ToolTipStyle = DevExpress.Utils.ToolTipStyle.Windows7;
            this.toolTipController1.ToolTipType = DevExpress.Utils.ToolTipType.SuperTip;
            this.toolTipController1.GetActiveObjectInfo += new DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventHandler(this.toolTipController1_GetActiveObjectInfo);
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

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton sbtnOverrideShow;
        private DevExpress.Utils.ToolTipController toolTipController1;
    }
}