
using Kzx.Common;
using Kzx.UserControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace FuckingERP.重写控件_弹窗类
{
    public partial class frm_ShowControl : DockContent
    {
        private static frm_ShowControl fInstance;
        public frm_ShowControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 单例
        /// </summary>
        /// <returns></returns>
        public static frm_ShowControl GetInstance()
        {
            if (fInstance == null)
            {
                fInstance = new frm_ShowControl();
            }
            return fInstance;
        }

        private void frm_ShowControl_FormClosed(object sender, FormClosedEventArgs e)
        {
            fInstance = null;
        }
        /// <summary>
        /// 弹窗提示测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtnOverrideShow_Click(object sender, EventArgs e)
        {
            KzxMessageBox.Show("this is a test!");
            KzxMessageBox.Show("This is a test!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }

        private void toolTipController1_GetActiveObjectInfo(object sender, DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            KzxSimpleButton btn = e.SelectedControl as KzxSimpleButton;
            e.Info = new DevExpress.Utils.ToolTipControlInfo(sender, btn.ToolTipText, DevExpress.Utils.ToolTipIconType.Information);
        }
    }
}
