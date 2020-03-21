namespace Kzx.Common
{
    partial class frm_MessageBox
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
            this.lblMsg = new DevExpress.XtraEditors.LabelControl();
            this.panelButton = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelButton)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.lblMsg.Location = new System.Drawing.Point(96, 44);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(350, 14);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "显示内容";
            // 
            // panelButton
            // 
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButton.Location = new System.Drawing.Point(0, 181);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(623, 72);
            this.panelButton.TabIndex = 1;
            // 
            // frm_MessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 253);
            this.Controls.Add(this.panelButton);
            this.Controls.Add(this.lblMsg);
            this.Name = "frm_MessageBox";
            this.Text = "frm_MessageBox";
            this.Load += new System.EventHandler(this.frm_MessageBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblMsg;
        private DevExpress.XtraEditors.PanelControl panelButton;
    }
}