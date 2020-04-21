namespace Kzx.UserControl
{
    partial class KzxLabel
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
            this.CaptionControl = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // CaptionControl
            // 
            this.CaptionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CaptionControl.Location = new System.Drawing.Point(0, 0);
            this.CaptionControl.Name = "CaptionControl";
            this.CaptionControl.Size = new System.Drawing.Size(70, 14);
            this.CaptionControl.TabIndex = 0;
            this.CaptionControl.Text = "labelControl1";
            // 
            // KzxLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.Controls.Add(this.CaptionControl);
            this.Name = "KzxLabel";
            this.Size = new System.Drawing.Size(70, 21);
            this.Load += new System.EventHandler(this.KzxLabel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl CaptionControl;
    }
}
