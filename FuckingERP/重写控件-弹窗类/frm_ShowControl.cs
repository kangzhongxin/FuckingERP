
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
        /// <summary>
        /// 激活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolTipController1_GetActiveObjectInfo(object sender, DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            KzxSimpleButton btn = e.SelectedControl as KzxSimpleButton;
            e.Info = new DevExpress.Utils.ToolTipControlInfo(sender,sysClass.ssLoadMsgOrDefault(btn.ToolTipMessageCode, btn.ToolTipText), DevExpress.Utils.ToolTipIconType.Information);
        }
        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_ShowControl_Load(object sender, EventArgs e)
        { 
            DataTable dataTable = new DataTable("Student"); 
             
            dataTable.Columns.Add("Name", typeof(String));
            dataTable.Columns.Add("RealName", typeof(String)); 
            dataTable.Rows.Add(new String[] {  "James", "张三" });
            dataTable.Rows.Add(new String[] {  "Mary", "李四" });
            dataTable.Rows.Add(new String[] { "Jack", "王五" });
            dataTable.Rows.Add(new String[] { "joy", "赵六" });
            dataTable.Rows.Add(new String[] {"jay", "钱七"});
            dataTable.Rows.Add(new String[] { "stephen", "康忠鑫" });
            kzxSearchComboboxEdit1.SetSourceTableBinding(dataTable.DefaultView, "RealName", "Name");
            kzxGridControl1.DataSource = dataTable;
        }
    }
}
