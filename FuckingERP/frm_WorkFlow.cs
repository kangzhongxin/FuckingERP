using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace FuckingERP
{
    public partial class frmWorkFlow : DockContent
    {
        private static frmWorkFlow fInstance;
        public frmWorkFlow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 单例
        /// </summary>
        /// <returns></returns>
        public static frmWorkFlow GetInstance()
        {
            if (fInstance == null)
            {
                fInstance = new frmWorkFlow();
            }
            return fInstance;
        }

        private void frm_WorkFlow_FormClosed(object sender, FormClosedEventArgs e)
        {
            fInstance = null;
        }
    }
}
