
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using Kzx.Common;
using Kzx.UserControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
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
            e.Info = new DevExpress.Utils.ToolTipControlInfo(sender, sysClass.ssLoadMsgOrDefault(btn.ToolTipMessageCode, btn.ToolTipText), DevExpress.Utils.ToolTipIconType.Information);
        }
        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_ShowControl_Load(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable("Student");
            dataTable.Columns.Add("Number", typeof(String));
            dataTable.Columns.Add("Name", typeof(String));
            dataTable.Columns.Add("RealName", typeof(String));
            dataTable.Columns.Add("UserName", typeof(String));
            dataTable.Columns.Add("Address", typeof(String));
            dataTable.Columns.Add("TelePhone", typeof(String));
            dataTable.Columns.Add("Email", typeof(String));
            dataTable.Columns.Add("Blogs", typeof(String));
            dataTable.Columns.Add("motto", typeof(String));
            dataTable.Rows.Add(new String[] { "1", "James", "张三", "james.zhang", "长沙", "1111111111", "123@qq.com", "https://kangzhongxin.github.io", "追求卓越，成功会在不经意间追上你！" });
            dataTable.Rows.Add(new String[] { "2", "Mary", "李四", "mary.xu", "山东", "1111111111", "123@qq.com", "https://kangzhongxin.github.io", "追求卓越，成功会在不经意间追上你！" });
            dataTable.Rows.Add(new String[] { "3", "Jack", "王五", "jack.li", "台湾", "1111111111", "123@qq.com", "https://kangzhongxin.github.io", "追求卓越，成功会在不经意间追上你！" });
            dataTable.Rows.Add(new String[] { "4", "joy", "赵六", "joy.zhou", "济南", "1111111111", "123@qq.com", "https://kangzhongxin.github.io", "追求卓越，成功会在不经意间追上你！" });
            dataTable.Rows.Add(new String[] { "5", "jay", "钱七", "jay.ji", "美国", "1111111111", "123@qq.com", "https://kangzhongxin.github.io", "追求卓越，成功会在不经意间追上你！" });
            dataTable.Rows.Add(new String[] { "6", "stephen", "康忠鑫", "Stephen.Kang", "深圳", "1111111111", "123@qq.com", "https://kangzhongxin.github.io", "追求卓越，成功会在不经意间追上你！" });
            //kzxSearchComboboxEdit1.SetSourceTableBinding(dataTable.DefaultView, "RealName", "Name");
            kzxGridControl1.DataSource = dataTable;
            GridView gv = this.kzxGridControl1.MainView as GridView;
            //读取配置文件，更新网格状态
            string sPath = $@"{ Application.StartupPath}\Task.xml";
            if (File.Exists(sPath))
            {
                XDocument xdoc = XDocument.Load(sPath);
                for (int i = 0; i < gv.Columns.Count; i++)
                {
                    string colName = gv.Columns[i].Name.Replace("col", "");
                    var query = (from a in xdoc.Descendants("Columns")
                                 where a.Attribute("ColumnsName").Value == colName
                                 select new
                                 {
                                     isDisplay = a.Element("isDisplay").Value,
                                     Freeze = a.Element("Freeze").Value,
                                     ColumnsWidth = a.Element("ColumnsWidth").Value,
                                     iOrder = a.Element("iOrder").Value
                                 }).FirstOrDefault();
                    if (query != null)
                    {
                        gv.Columns[i].Visible = Boolean.Parse(query.isDisplay);
                        if (query.Freeze.ToLower().Equals("无"))
                        {
                            gv.Columns[i].Fixed = FixedStyle.None;
                        }
                        else if (query.Freeze.ToLower().Equals("左边"))
                        {
                            gv.Columns[i].Fixed = FixedStyle.Left;
                        }
                        else if (query.Freeze.ToLower().Equals("右边"))
                        {
                            gv.Columns[i].Fixed = FixedStyle.Right;
                        }
                        if (!Boolean.Parse(query.isDisplay))
                        {
                            gv.Columns[i].VisibleIndex = -1;
                            gv.Columns[i].Visible = Boolean.Parse(query.isDisplay);
                        }
                        else
                        {
                            gv.Columns[i].VisibleIndex = int.Parse(query.iOrder);
                        }
                        gv.Columns[i].Width = int.Parse(query.ColumnsWidth);
                    }
                }
            }
        }
        /// <summary>
        /// 网格状态设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kzxSimpleButton2_Click(object sender, EventArgs e)
        {
            frm_GridSetting frm = new frm_GridSetting(kzxGridControl1);
            frm.RefreshMDIFormEvent += Frm_RefreshMDIFormEvent;
            frm.ShowDialog();
        }

        private void Frm_RefreshMDIFormEvent()
        {
            this.frm_ShowControl_Load(null, null);
        }
    }
}
