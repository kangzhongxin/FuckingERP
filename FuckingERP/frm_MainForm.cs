using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FuckingERP
{
    public partial class frmFuckingERP : Form
    {
        public frmFuckingERP()
        {
            InitializeComponent(); 
        }
        /// <summary>
        /// 主界面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmFuckingERP_Load(object sender, EventArgs e)
        {
            frm_LeftMenu leftMenu = new frm_LeftMenu();
            leftMenu.Show(dockPanel1);
            leftMenu.DockState = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;


        }
    }
}
