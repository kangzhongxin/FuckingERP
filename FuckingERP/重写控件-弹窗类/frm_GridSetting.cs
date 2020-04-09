using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Kzx.Common;
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

namespace FuckingERP.重写控件_弹窗类
{
    public delegate void RefreshMDIFormHandler();
    public partial class frm_GridSetting : Form
    {
        public event RefreshMDIFormHandler RefreshMDIFormEvent;
        GridControl gc = null;
        public frm_GridSetting(GridControl _gridControl)
        {
            InitializeComponent();
            gc = _gridControl;
        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSure_Click(object sender, EventArgs e)
        {
            try
            {
                //保存动作
                string sPath = $@"{ Application.StartupPath}\Task.xml";
            loop:
                if (!File.Exists(sPath))
                {
                    File.Create(sPath).Close();
                    XDocument xdoc = new XDocument();
                    //创建跟节点
                    XElement root = new XElement("Mes");
                    //添加跟节点
                    xdoc.Add(root);

                    DataTable dtSetting = gcSetting.DataSource as DataTable;
                    int index = 0;
                    foreach (DataRow item in dtSetting.Rows)
                    {
                        //创建person节点 
                        XElement per = new XElement("Columns");
                        //添加person节点
                        root.Add(per);
                        //创建属性节点
                        XAttribute ColumnsName = new XAttribute("ColumnsName", item["ColumnsName"].ToString());
                        //name节点
                        XElement isDisplay = new XElement("isDisplay", item["isDisplay"].ToString());

                        XElement Freeze = new XElement("Freeze", item["Freeze"].ToString());

                        XElement ColumnsWidth = new XElement("ColumnsWidth", item["ColumnsWidth"].ToString());

                        XElement iOrder = new XElement("iOrder", index);
                        //对person节点添加id属性 以及name sex age节点
                        per.Add(ColumnsName, isDisplay, Freeze, ColumnsWidth, iOrder);
                        index++;
                    }
                    //保存linq to xml 文件
                    xdoc.Save(sPath);
                }
                else
                {
                    File.Delete(sPath);
                    goto loop;
                }

                //刷新原始界面显示
                if (RefreshMDIFormEvent != null)
                {
                    RefreshMDIFormEvent();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

                this.Close();
            }
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 数据加载初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_GridSetting_Load(object sender, EventArgs e)
        {
            //读取配置文件，更新网格状态
            string sPath = $@"{ Application.StartupPath}\Task.xml";
            DataTable dtColumns = this.gc.DataSource as DataTable;
            GridView gvv = gc.MainView as GridView; 
            if (dtColumns != null && dtColumns.Rows.Count > 0)
            {
                DataTable dtSetting = new DataTable("GridSetting");
                dtSetting.Columns.Add("ColumnsName", typeof(String));
                dtSetting.Columns.Add("isDisplay", typeof(Boolean));
                dtSetting.Columns.Add("Freeze", typeof(String));
                dtSetting.Columns.Add("ColumnsWidth", typeof(Int32));
                dtSetting.Columns.Add("Order1", typeof(String));
                dtSetting.Columns.Add("Order2", typeof(String));
                dtSetting.Columns.Add("Top", typeof(String));
                dtSetting.Columns.Add("Button", typeof(String));
                dtSetting.Columns.Add("iOrder", typeof(Int32));

                if (!File.Exists(sPath))
                { 
                    for (int i = 0; i < gvv.VisibleColumns.Count; i++)
                    {
                        string colName = gvv.VisibleColumns[i].Name.Replace("col", "");
                        dtSetting.Rows.Add(new object[] { colName, true, "无", 120, "上移", "下移", "置顶", "置底", gvv.VisibleColumns[i].VisibleIndex });
                    }
                }
                else
                {
                    //读取XML 绑定数据源
                    XDocument xdoc = XDocument.Load(sPath);
                    var query = from a in xdoc.Descendants("Columns")
                                select new
                                {
                                    ColumnsName = a.Attribute("ColumnsName").Value,
                                    isDisplay = a.Element("isDisplay").Value,
                                    Freeze = a.Element("Freeze").Value,
                                    ColumnsWidth = a.Element("ColumnsWidth").Value,
                                    iOrder = a.Element("iOrder").Value
                                };
                    int index = 0;
                    foreach (var item in query.ToList())
                    {
                        dtSetting.Rows.Add(new object[] { item.ColumnsName, Boolean.Parse(item.isDisplay), item.Freeze, int.Parse(item.ColumnsWidth), "上移", "下移", "置顶", "置底", index });
                        index++;//排序重置
                    }
                }
                gcSetting.DataSource = dtSetting;
                repositoryItemButtonEdit1.ButtonClick += RepositoryItemButtonEdit1_ButtonClick;//上移 
                repositoryItemButtonEdit2.ButtonClick += RepositoryItemButtonEdit2_ButtonClick;//下移 
                repositoryItemButtonEdit3.ButtonClick += RepositoryItemButtonEdit3_ButtonClick;//置顶 
                repositoryItemButtonEdit4.ButtonClick += RepositoryItemButtonEdit4_ButtonClick;//置底 
            }
        }

        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepositoryItemButtonEdit3_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DataTable dtPostgreSql = gcSetting.DataSource as DataTable;
            GridView gridView1 = this.gcSetting.MainView as GridView;
            if (gridView1.SelectedRowsCount == 1)
            {
                int top = this.gridView1.GetDataSourceRowIndex(this.gridView1.FocusedRowHandle);
                DataRow drOne = this.gridView1.GetDataRow(top);

                dtPostgreSql.Rows.InsertAt(dtPostgreSql.NewRow(), 0);
                dtPostgreSql.Rows[0].ItemArray = drOne.ItemArray;
                drOne.Delete();//删除原来的数据行(该行所在父表中删除)
                dtPostgreSql.AcceptChanges();//提交表所有的更改
                gridView1.MoveFirst();
            }
            if (gridView1.SelectedRowsCount > 1)
            {
                int[] index = new int[gridView1.SelectedRowsCount];
                int[] rows = gridView1.GetSelectedRows();
                int j = 0;
                if (rows[0] - 1 >= 0)
                {
                    for (int i = rows[0]; i < rows[rows.Length - 1] + 1; i++)
                    {
                        object[] ot = dtPostgreSql.Rows[i].ItemArray;
                        DataRow dr = dtPostgreSql.Rows[i];
                        dtPostgreSql.Rows.Remove(dr);
                        DataRow drs = dtPostgreSql.NewRow();
                        drs.ItemArray = ot;
                        dtPostgreSql.Rows.InsertAt(drs, j);
                        index[j] = j;
                        j++;
                    }
                    gridView1.SelectRows(index[0], index[index.Length - 1]);
                    dtPostgreSql.AcceptChanges();
                }
                else
                    KzxMessageBox.Show("已到第一行不能继续上移");

            }
        }
        /// <summary>
        /// 置底
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepositoryItemButtonEdit4_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DataTable dtPostgreSql = gcSetting.DataSource as DataTable;
            GridView gridView1 = this.gcSetting.MainView as GridView;
            int bottom = dtPostgreSql.Rows.Count - 1;
            if (gridView1.SelectedRowsCount >= 1)
            {
                int[] index = new int[gridView1.SelectedRowsCount];
                int[] rows = gridView1.GetSelectedRows();
                int j = 0;
                if (rows[rows.Length - 1] <= gridView1.RowCount - 2)
                {
                    for (int i = rows[rows.Length - 1]; i > rows[0] - 1; i--)
                    {
                        object[] ot = dtPostgreSql.Rows[i].ItemArray;
                        DataRow dr = dtPostgreSql.Rows[i];
                        dtPostgreSql.Rows.Remove(dr);
                        DataRow drs = dtPostgreSql.NewRow();
                        drs.ItemArray = ot;
                        dtPostgreSql.Rows.InsertAt(drs, bottom - j);
                        index[j] = bottom - j;
                        j++;
                    }
                    gridView1.SelectRows(index[0], index[index.Length - 1]);
                    dtPostgreSql.AcceptChanges();
                }
                else
                    KzxMessageBox.Show("已到最后一条记录不能继续下移");

            }
        }

        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DataTable dtPostgreSql = gcSetting.DataSource as DataTable;
            GridView gridView1 = this.gcSetting.MainView as GridView;
            if (gridView1.SelectedRowsCount < 1)
            {
                KzxMessageBox.Show("未选中任何行");
                return;
            }
            if (gridView1.SelectedRowsCount == 1)
            {
                int selectIndex = this.gridView1.GetDataSourceRowIndex(this.gridView1.FocusedRowHandle);
                if (selectIndex - 1 >= 0)
                {
                    object[] ot = dtPostgreSql.Rows[selectIndex].ItemArray;
                    DataRow dr = dtPostgreSql.Rows[selectIndex];
                    dtPostgreSql.Rows.Remove(dr);
                    DataRow drs = dtPostgreSql.NewRow();
                    drs.ItemArray = ot;
                    dtPostgreSql.Rows.InsertAt(drs, selectIndex - 1); 
                    dtPostgreSql.AcceptChanges();
                }
                else
                    KzxMessageBox.Show("已到第一行不能继续上移");
                gridView1.FocusedRowHandle = selectIndex - 1;
            } 
           
        }
        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepositoryItemButtonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DataTable dtPostgreSql = gcSetting.DataSource as DataTable;
            GridView gridView1 = this.gcSetting.MainView as GridView;
            if (gridView1.SelectedRowsCount < 1)
            {
                MessageBox.Show("未选中任何行");
                return;
            }
            if (gridView1.SelectedRowsCount >= 1)
            {
                int[] index = new int[gridView1.SelectedRowsCount];
                int[] rows = gridView1.GetSelectedRows();
                int j = 0;
                if (rows[rows.Length - 1] <= gridView1.RowCount - 2)
                {
                    for (int i = rows[rows.Length - 1]; i > rows[0] - 1; i--)
                    {
                        object[] ot = dtPostgreSql.Rows[i].ItemArray;
                        DataRow dr = dtPostgreSql.Rows[i];
                        dtPostgreSql.Rows.Remove(dr);
                        DataRow drs = dtPostgreSql.NewRow();
                        drs.ItemArray = ot;
                        dtPostgreSql.Rows.InsertAt(drs, i + 1);
                        index[j] = i + 1;
                        j++;
                    }
                    gridView1.SelectRows(index[0], index[index.Length - 1]);
                    dtPostgreSql.AcceptChanges();
                }
                else
                    KzxMessageBox.Show("已到最后一条记录不能继续下移");

                int selectIndex = this.gridView1.GetDataSourceRowIndex(this.gridView1.FocusedRowHandle);
                gridView1.FocusedRowHandle = selectIndex + 1;
            }
        }
    }
}
