using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection; 

namespace Kzx.UserControl
{
    public partial class Status : Form
    {
        private DataTable _tableColumnSettings; 

        /// <summary>
        /// 是否显示查询关键字列
        /// </summary>
        private bool _isDisplaySearchKeyColumn = false;
        private string _mainTableName = string.Empty;
        private string _sFrmName = string.Empty;
        private string _filterConfigSection = string.Empty;

        public Status(string mainTableName, string filterConfigSection, string sFrmName)
        {
            InitializeComponent();

            this._mainTableName = mainTableName;
            this._filterConfigSection = filterConfigSection;
            this._sFrmName = sFrmName;
        }

        public Status(string mainTableName, string filterConfigSection)
        {
            InitializeComponent();

            this._mainTableName = mainTableName;
            this._filterConfigSection = filterConfigSection;
        }

        public Status()
        {
            InitializeComponent();
        }

        #region 属性

        /// <summary>
        /// 是否显示查询关键词列
        /// </summary>
        public bool IsDisplaySearchKeyColumn
        {
            get { return _isDisplaySearchKeyColumn; }
            set { _isDisplaySearchKeyColumn = value; }
        }

        #endregion

        #region 多语言设置

        private static MethodInfo _methodInfo = null;

        /// <summary>
        /// 获取多语言文本
        /// </summary>
        /// <param name="messageCode">语言文本标识</param>
        /// <param name="defaultMessage">默认的文本</param>
        /// <returns>取到的文本</returns>
        protected virtual string GetLanguage(string messageCode, string defaultMessage)
        {
            string filepath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "YZCommon.dll");
            string text = string.Empty;
            Assembly assembly = null;
            object obj = null;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                if (assemblies[i].GetName().Name.Equals("YZCommon", StringComparison.OrdinalIgnoreCase) == true)
                {
                    assembly = assemblies[i];
                    break;
                }
            }
            if (assembly == null)
            {
                assembly = Assembly.LoadFrom(filepath);
            }
            obj = assembly.CreateInstance("YZCommon.sysClass");
            text = defaultMessage;
            if (_methodInfo == null)
            {
                if (obj != null)
                {
                    _methodInfo = obj.GetType().GetMethod("ssLoadMsgOrDefault", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    if (_methodInfo != null)
                    {
                        text = _methodInfo.Invoke(obj, new object[] { messageCode, text }).ToString();
                    }
                }
            }
            else
            {
                text = _methodInfo.Invoke(obj, new object[] { messageCode, text }).ToString();
            }
            return string.IsNullOrEmpty(text) ? defaultMessage : text;
        }

        #endregion

        private void Status_Load(object sender, EventArgs e)
        {
            //初始化列表字段设置数据
            InitOfColumnSetting();

            gridControl1.DataSource = StatusSet.dt;
            this.gridView1.IndicatorWidth = 30;
            this.Text = GetLanguage("SYS001178", "网格状态设置");//网格状态设置

            gridView1.Columns.ColumnByFieldName("bSelect").Caption = GetLanguage("SYS001179", "显示");//显示
            gridView1.Columns.ColumnByFieldName("bSum").Caption = GetLanguage("SYS001180", "合计项目");//合计项目
            gridView1.Columns.ColumnByFieldName("sCaption").Caption = GetLanguage("SYS000065", "名称");//名称
            gridView1.Columns.ColumnByFieldName("sField").Caption = GetLanguage("SYS000236", "字段名");//字段名
            gridView1.Columns.ColumnByFieldName("Sumtype").Caption = GetLanguage("SYS001181", "合计类型");//合计类型
            gridView1.Columns.ColumnByFieldName("StringFormat").Caption = GetLanguage("2A19189AF119BF96", "保留小数位");//自定义格式化字符串
            gridView1.Columns.ColumnByFieldName("bFilter").Caption = GetLanguage("SYS001183", "过滤");//过滤
            gridView1.Columns.ColumnByFieldName("sParent").Caption = GetLanguage("MSG010159", "父级");//父级 HHL加 20170803
            gridView1.Columns.ColumnByFieldName("bSearchKeyField").Caption = GetLanguage("MSG000168", "关键字");//关键字 add by huangyq201720170815

            if (!_isDisplaySearchKeyColumn)
            {
                gridView1.Columns["bSearchKeyField"].Visible = false;
            }

            this.ConfirmBt.Text = GetLanguage("SYS000070", "确定");//确定
            this.CancleBt.Text = GetLanguage("SYS000071", "取消");//取消 

            //HHL modify 20170809
            CreateLookUpEditDataSource(StatusSet.dt);
            this.repositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("sCaption", "名称"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("sField", "字段")});
            repositoryItemLookUpEdit1.DisplayMember = "sCaption";
            repositoryItemLookUpEdit1.ValueMember = "sField";
            repositoryItemLookUpEdit1.NullText = "";
            repositoryItemLookUpEdit1.Columns["sCaption"].Caption = GetLanguage("MSG000117", "名称");
            repositoryItemLookUpEdit1.Columns["sField"].Visible = false;
            gridView1.CellValueChanged += gridView1_CellValueChanged;
            gridView1.ShowingEditor += gridView1_ShowingEditor;
        }

        #region 数据表格·列设置

        /// <summary>
        /// 初始化数据表列设置
        /// </summary>
        private void InitOfColumnSetting()
        {
            if (string.IsNullOrWhiteSpace(_mainTableName))
                return;

            var filterConfig = new KzxGridDataFilter(_mainTableName, _sFrmName, _filterConfigSection);
            var filterItems = filterConfig.GetFilterConfigItems();

            //先重置过滤项
            for(var i = StatusSet.dt.Rows.Count; i > 0; i--)
            {
                var row = StatusSet.dt.Rows[i - 1];
                row["bFilter"] = false;
                row["sParent"] = "";
                //row["bSearchKeyField"] = false;
                var sFiled = row["sField"].ToString();
                if (sFiled == "sCrtUser_Name" || sFiled == "sCheckUser_Name" || sFiled == "sModUser_Name")
                {
                    row.Delete();
                }
            }

            //应用配置过滤项
            foreach (var filterItem in filterItems)
            {
                var rows = StatusSet.dt.Select(string.Format("sField='{0}'", filterItem.FieldName));
                foreach (DataRow row in rows)
                {
                    if (filterItem.IsDataSetFilter)
                    {
                        row["bFilter"] = true;
                        row["sParent"] = filterItem.DataSetParentField;
                    }

                    if (filterItem.IsDatabaseFilter)
                    {
                        row["bSearchKeyField"] = true;
                    }
                }
            }
        }

        private bool IsAllowEditor()
        {  
            return true;
        }

        #endregion

        void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (!IsAllowEditor())
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 网格状态设置值改变 HHL modify 20170809
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (gridView1.RowCount > 0 && e.RowHandle >= 0)
            {
                if (gridView1.FocusedColumn.FieldName == "bFilter")
                {
                    ((DataTable)gridControl1.DataSource).AcceptChanges();
                    CreateLookUpEditDataSource((DataTable)gridControl1.DataSource);
                    DataRow drFocused = gridView1.GetDataRow(e.RowHandle);
                    if (!Convert.ToBoolean(drFocused["bFilter"]))
                    {
                        string sField = string.IsNullOrWhiteSpace(drFocused["sField"].ToString()) ? "" : drFocused["sField"].ToString();
                        DataRow[] drSelect = ((DataTable)gridControl1.DataSource).Select("sParent='" + sField + "'");
                        foreach (DataRow dr in drSelect)
                        {
                            dr["sParent"] = "";//把去掉过滤的字段，其对应在父级的数据清掉
                        }
                    }
                }
                if (gridView1.FocusedColumn.FieldName == "sParent")
                {
                    ((DataTable)gridControl1.DataSource).AcceptChanges();
                    CreateLookUpEditDataSource((DataTable)gridControl1.DataSource);
                    DataRow drFocused = gridView1.GetDataRow(e.RowHandle);
                    if (drFocused["sParent"].ToString() == drFocused["sField"].ToString())
                    {
                        drFocused["sParent"] = "";
                    }
                }
            }
        }

        /// <summary>
        /// 获取父级下拉数据源 HHL modify 20170809
        /// </summary>
        /// <param name="dt">传入数据表</param>
        private void CreateLookUpEditDataSource(DataTable dt)
        {
            DataRow[] drFilter = dt.Select("bFilter ='true'", "sCaption asc");
            DataTable dtCaption = new DataTable();
            dtCaption.Columns.Add("sCaption");
            dtCaption.Columns.Add("sField");
            DataRow newRow = dtCaption.NewRow();
            newRow["sCaption"] = "";
            newRow["sField"] = "";
            dtCaption.Rows.Add(newRow);
            foreach (DataRow dr in drFilter)
            {
                DataRow newRow1 = dtCaption.NewRow();
                newRow1["sCaption"] = dr["sCaption"];
                newRow1["sField"] = dr["sField"];
                dtCaption.Rows.Add(newRow1);
            }
            repositoryItemLookUpEdit1.DataSource = dtCaption;
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmBt_Click(object sender, EventArgs e)
        { 
            SaveDataFilterConfigToLocal();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Status s = (Status)this.Owner; 
            this.Dispose();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancleBt_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void gridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

        }

        /// <summary>
        /// 保存数据过滤设置到本地
        /// </summary>
        private void SaveDataFilterConfigToLocal()
        {
            var configItems = new List<KzxGridDataFilterItem>();

            foreach (DataRow row in StatusSet.dt.Rows)
            {
                var isDatabaseFilter = row["bSearchKeyField"] == DBNull.Value ? false : (bool)row["bSearchKeyField"];
                var isDataSetFilter = row["bFilter"] == DBNull.Value ? false : (bool)row["bFilter"];

                if (isDatabaseFilter || isDataSetFilter)
                {
                    var item = new KzxGridDataFilterItem()
                    {
                        DataSetParentField = StatusSet.dt.Columns.Contains("sParent") ? row["sParent"]?.ToString() : "",
                        FieldDesc = row["sCaption"]?.ToString(),
                        FieldName = row["sField"]?.ToString(),
                        IsDatabaseFilter = isDatabaseFilter,
                        IsDataSetFilter = isDataSetFilter,
                    };

                    configItems.Add(item);
                }
            }
            if (string.IsNullOrEmpty(_filterConfigSection)) return;

            var config = new KzxGridDataFilterLocalConfig(_filterConfigSection);
            config.Reset(configItems);
            config.Save();
        }
    }
}
