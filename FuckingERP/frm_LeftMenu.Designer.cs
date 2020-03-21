namespace FuckingERP
{
    partial class frm_LeftMenu
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
            this.navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            this.nbgControlShow = new DevExpress.XtraNavBar.NavBarGroup();
            this.nbShowControl = new DevExpress.XtraNavBar.NavBarItem();
            this.nbDataControl = new DevExpress.XtraNavBar.NavBarItem();
            this.nbOtherControl = new DevExpress.XtraNavBar.NavBarItem();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // navBarControl1
            // 
            this.navBarControl1.ActiveGroup = this.nbgControlShow;
            this.navBarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.nbgControlShow});
            this.navBarControl1.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.nbShowControl,
            this.nbDataControl,
            this.nbOtherControl});
            this.navBarControl1.Location = new System.Drawing.Point(0, 0);
            this.navBarControl1.Name = "navBarControl1";
            this.navBarControl1.OptionsNavPane.ExpandedWidth = 226;
            this.navBarControl1.Size = new System.Drawing.Size(226, 450);
            this.navBarControl1.TabIndex = 0;
            this.navBarControl1.Text = "navBarControl1";
            this.navBarControl1.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinNavigationPaneViewInfoRegistrator("DevExpress Design");
            this.navBarControl1.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarControl1_LinkClicked);
            // 
            // nbgControlShow
            // 
            this.nbgControlShow.Caption = "重写控件演示";
            this.nbgControlShow.Expanded = true;
            this.nbgControlShow.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbShowControl),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbDataControl),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbOtherControl)});
            this.nbgControlShow.Name = "nbgControlShow";
            // 
            // nbShowControl
            // 
            this.nbShowControl.Caption = "弹窗类";
            this.nbShowControl.Name = "nbShowControl";
            // 
            // nbDataControl
            // 
            this.nbDataControl.Caption = "容器类";
            this.nbDataControl.Name = "nbDataControl";
            // 
            // nbOtherControl
            // 
            this.nbOtherControl.Caption = "其他类";
            this.nbOtherControl.Name = "nbOtherControl";
            // 
            // frm_LeftMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 450);
            this.Controls.Add(this.navBarControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_LeftMenu";
            this.Text = "控件菜单";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_LeftMenu_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraNavBar.NavBarControl navBarControl1;
        private DevExpress.XtraNavBar.NavBarGroup nbgControlShow;
        private DevExpress.XtraNavBar.NavBarItem nbShowControl;
        private DevExpress.XtraNavBar.NavBarItem nbDataControl;
        private DevExpress.XtraNavBar.NavBarItem nbOtherControl;
    }
}