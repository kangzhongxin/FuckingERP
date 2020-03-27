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
using Kzx.UserControl;

namespace Kzx.UserControls
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
            string filepath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Kzx.Common.dll");
            string text = string.Empty;
            Assembly assembly = null;
            object obj = null;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                if (assemblies[i].GetName().Name.Equals("Kzx.Common", StringComparison.OrdinalIgnoreCase) == true)
                {
                    assembly = assemblies[i];
                    break;
                }
            }
            if (assembly == null)
            {
                assembly = Assembly.LoadFrom(filepath);
            }
            obj = assembly.CreateInstance("Kzx.Common.sysClass");
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
