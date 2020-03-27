using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml; 
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid; 
using System.Drawing;
using DevExpress.XtraEditors.Mask;
using System.Collections;
using DevExpress.Utils;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Views.BandedGrid;
using System.Diagnostics;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPrinting; 
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.Data;
using Kzx.AppCore;
using Kzx.Common;
using Kzx.UserControl.UITypeEdit;
using Kzx.UserControl.Extensions;
using Kzx.UserControls;

namespace Kzx.UserControl
{
    public class KzxGridControl : GridControl, IControl
    {
        /// <summary> 标识字符串常量集 </summary>
        private static class Identities
        {  
            public const string bChoose = nameof(bChoose); 
        }

        #region 字段
        protected delegate void UpdateDelegate(object source);

        private static readonly Type _typeOfDecimal = typeof(Decimal);
        private static readonly Type _typeOfDouble = typeof(double);
        private static readonly Type _typeOfFloat = typeof(float);
        private static readonly Type _typeOfSingle = typeof(Single);

        public static int ColumnCount = 0;
        public static string index = "";
        private bool _isChooseAllEvent = false;

        private ImageList buttonimages = new ImageList();
        private ToolTip _ToolTip = new ToolTip();
        private ContextMenuStrip _ContextMenuStrip = new ContextMenuStrip();
        private ToolStripItem ToolSelectAll = null;
        private ToolStripItem ToolNotSelectAll = null;

        protected Binding BindingObject = null;
        private BandedGridView _bandedGridView = new BandedGridView();
        private KzxBandedGridView _KzxBandedGridView = null;
        private List<int> lStartRow = new List<int>();//lfx,20170306 
        private int EndRow = -1;//lfx,20170306 
        private Dictionary<string, string> _FooterCellDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 用于存放加密的列名。
        /// </summary>
        public List<string> _EncryptionColumns = new List<string>();

        private bool _displayRightClickCopyMenu = true;
        private bool _displayRightClickExportExcel = true;
        #endregion

        #region 事件

        /// <summary>
        /// 网格状态确认设置回调 HHL modify 20170809
        /// </summary>
        public event DelegaeOnThrowCheckFilterParam OnThrowCheckedFilteParam = null;

        /// <summary>
        /// 网格全选或全消事件
        /// </summary>
        public event KzxGridControlChooseAllEndHandle OnChooseAllEnd;

        /// <summary>
        /// 网格鼠标右键复制行处理事件
        /// </summary>
        public event KzxGridControlRightClickCopyHandle OnRowCopy;

        #endregion

        #region 行拖放功能属性

        //鼠标坐标
        private GridHitInfo downHitInfo; //鼠标左键按下去时在GridView中的坐标
        private GridHitInfo upHitInfo; //鼠标左键弹起来时在GridView中的坐标
        private int startRow; // 拖拽的第一行
        private int[] rows; //拖拽的所有行

        #endregion

        /// <summary>
        /// 是否显示右键菜单的复制行
        /// </summary>
        [Category("右键菜单"), Description("DisplayRightClickMenu,显示右键菜单的复制行按钮"), Browsable(true)]
        public bool DisplayRightClickMenu
        {
            get { return _displayRightClickCopyMenu; }
            set { _displayRightClickCopyMenu = value; }
        }
        /// <summary>
        /// 是否显示右键菜单的导出Excel
        /// </summary>
        [Category("右键菜单"), Description("DisplayRightExportExcel,显示右键菜单的导出Excel按钮"), Browsable(true)]
        public bool DisplayRightExportExcel
        {
            get { return _displayRightClickExportExcel; }
            set { _displayRightClickExportExcel = value; }
        }

        /// <summary>
        /// 获取 是否为全选/全消模式
        /// </summary>
        public bool IsChooseAllEvent
        {
            get { return _isChooseAllEvent; }
        }

        /// <summary>
        /// 有Load方法
        /// </summary>
        private bool _HasLoad = true;
        public bool HasLoad
        {
            get
            {
                return this._HasLoad;
            }
        }
        /// <summary>
        /// 脚本单元格
        /// </summary>
        public Dictionary<string, string> FooterCellDictionary
        {
            get
            {
                return this._FooterCellDictionary;
            }
            set
            {
                this._FooterCellDictionary = value;
            }
        }

        private int _CurrentRowIndex = -1;
        public int CurrentRowIndex
        {
            get
            {
                return this._CurrentRowIndex;
            }
            set
            {
                this._CurrentRowIndex = value;
            }
        }

        private bool _IsValidation = true;
        public bool IsValidation
        {
            get
            {
                return this._IsValidation;
            }
            set
            {
                this._IsValidation = value;
            }
        }

        private bool _IsValidationOther = true;
        public bool IsValidationOther
        {
            get
            {
                return this._IsValidationOther;
            }
            set
            {
                this._IsValidationOther = value;
            }
        }


        #region 自定义属性

        private bool _AllowEdit = true;
        /// <summary>
        /// 被引用后允许修改
        /// true允许,false不允许
        /// </summary>
        [Category("验证"), Description("AllowEdit,被引用后允许修改,true允许,false不允许"), Browsable(true)]
        [McDisplayName("AllowEdit")]
        public virtual bool AllowEdit
        {
            get
            {
                return this._AllowEdit;
            }
            set
            {
                this._AllowEdit = value;
                if (value == true)
                {
                    this.Enabled = true;
                }
                else
                {
                    this.Enabled = false;
                }
            }
        }

        private Boolean _IsBandedGridView = false;
        /// <summary>
        /// 是否为多列头网格
        /// true是,false不是
        /// </summary>
        [Category("外观"), Description("IsBandedGridView,是否为多列头网格"), Browsable(true)]
        [McDisplayName("IsBandedGridView")]
        public Boolean IsBandedGridView
        {
            get
            {
                return this._IsBandedGridView;
            }
            set
            {
                this._IsBandedGridView = value;
                if (value == true)
                {
                    this.MainView = this._bandedGridView;
                }
            }
        }

        /// <summary>
        /// GridView
        /// 可能会为空
        /// </summary>
        [Category("外观"), Description("KzxGridView,GridView"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GridView KzxGridView
        {
            get
            {
                GridView view = this.MainView as GridView;
                return view;
            }
        }

        /// <summary>
        /// 循环调用指定委托
        /// RowForEachDelegate类型
        /// </summary>
        /// <returns></returns>
        public void ForEachDataRow<T>(T inArgs, Func<T, DataRow, Boolean> func)
        {
            Boolean ret = true;
            DataRow row = null;
            GridView view = this.MainView as GridView;
            if (view != null)
            {
                for (int i = 0; i < view.DataRowCount; i++)
                {
                    row = view.GetDataRow(i);
                    ret = func(inArgs, row);
                    if (ret == false)
                    {
                        break;
                    }
                }
            }
        }

        #endregion

        private static string _GridColorA = string.Empty;

        public static string GridColorA
        {
            get
            {
                return _GridColorA;
            }
            set
            {
                _GridColorA = value;
            }
        }

        private static string _GridColorB = string.Empty;

        public static string GridColorB
        {
            get
            {
                return _GridColorB;
            }
            set
            {
                _GridColorB = value;
            }
        }

        /// <summary>
        /// Ini文件节点名
        /// </summary>
        private string _ColorSet = "";
        private IContainer components;

        [Category("外观"), Description("ColorSet,文件节点名"), Browsable(true)]
        [McDisplayName("ColorSet")]
        public virtual string ColorSet
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ColorSet) == true)
                {
                    this._ColorSet = this.Table;
                }
                return this._ColorSet;
            }
            set
            {
                this._ColorSet = value;
            }

        }

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
            var filepath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "KzxCommon.dll");
            var text = string.Empty;
            Assembly assembly = null;
            object obj = null;

            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                for (var i = 0; i < assemblies.Length; i++)
                {
                    if (assemblies[i].GetName().Name.Equals("KzxCommon", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        assembly = assemblies[i];
                        break;
                    }
                }
                if (assembly == null)
                {
                    assembly = Assembly.LoadFrom(filepath);
                }
                obj = assembly.CreateInstance("KzxCommon.sysClass");
                text = defaultMessage;
                if (_methodInfo == null)
                {
                    if (obj != null)
                    {
                        _methodInfo = obj.GetType().GetMethod("ssLoadMsg", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                        if (_methodInfo != null)
                        {
                            text = _methodInfo.Invoke(obj, new object[] { messageCode }).ToString();
                        }
                    }
                }
                else
                {
                    text = _methodInfo.Invoke(obj, new object[] { messageCode }).ToString();
                }
            }
            catch
            {
                text = defaultMessage;
            }
            return string.IsNullOrWhiteSpace(text) == true ? defaultMessage : text;
        }

        /// <summary>
        /// 返回用'|'分隔的显示标题的最一个
        /// </summary>
        /// <param name="s">用'|'分隔的字符</param>
        /// <returns>最一个字符串</returns>
        protected virtual string GetColumnCaption(string s)
        {
            var list = s.Split(new string[] { _KzxBandedGridView.SplitStr }, StringSplitOptions.RemoveEmptyEntries);
            if (list.Length > 0)
            {
                return list[list.Length - 1];
            }
            return s;
        }

        #endregion

        //右键菜单弹出框
        private DataTable keyTabale = new DataTable();

        private DataTable childTable = new DataTable();
        private List<string> field = new List<string>();

        //颜色设置快捷菜单
        private void ColorSetMenuItem1_Click_1(object sender, EventArgs e)
        {
           
        }

        public void CreateColumns(BindingSource bs)
        {
            if (this.MainView != null)
            {
                if (this.MainView is BandedGridView)
                {
                    CreateBandColumns(bs);
                }
                else
                {
                    CreateGridColumns(bs);
                }
            }
        }

        public void CreateGridColumns(BindingSource bs)
        {
            GridView gv = null;
            DataView view = null;
            GridColumn c = null;
            if (bs != null)
            {
                gv = this.MainView as GridView;
                if (gv != null)
                {
                    gv.Columns.Clear();
                    view = bs.DataSource as DataView;
                    if (view != null)
                    {
                        for (int i = 0; i < view.Table.Columns.Count; i++)
                        {
                            if (view.Table.Columns[i].DataType == typeof(bool))
                            {
                                c = new GridColumn();
                                var t = new RepositoryItemCheckEdit();
                                c.ColumnEdit = t;
                                c.FieldName = view.Table.Columns[i].ColumnName;
                                c.Caption = view.Table.Columns[i].ColumnName;
                                gv.Columns.Add(c);
                            }
                            else
                            {
                                c = new GridColumn();
                                var t = new RepositoryItemTextEdit();
                                c.ColumnEdit = t;
                                c.FieldName = view.Table.Columns[i].ColumnName;
                                c.Caption = view.Table.Columns[i].ColumnName;
                                gv.Columns.Add(c);
                            }

                        }
                    }
                }
            }
            this.DataSource = bs;
        }

        public void CreateBandColumns(BindingSource bs)
        {
            BandedGridView gv = null;
            DataView view = null;
            BandedGridColumn c = null;
            if (bs != null)
            {
                gv = this.MainView as BandedGridView;
                if (gv != null)
                {
                    gv.Columns.Clear();
                    view = bs.DataSource as DataView;
                    if (view != null)
                    {
                        for (int i = 0; i < view.Table.Columns.Count; i++)
                        {
                            if (view.Table.Columns[i].DataType == typeof(bool))
                            {
                                c = new BandedGridColumn();
                                var t = new RepositoryItemCheckEdit();
                                c.ColumnEdit = t;
                                c.FieldName = view.Table.Columns[i].ColumnName;
                                c.Caption = view.Table.Columns[i].ColumnName;
                                gv.Columns.Add(c);
                            }
                            else
                            {
                                c = new BandedGridColumn();
                                var t = new RepositoryItemTextEdit();
                                c.ColumnEdit = t;
                                c.FieldName = view.Table.Columns[i].ColumnName;
                                c.Caption = view.Table.Columns[i].ColumnName;
                                gv.Columns.Add(c);
                            }
                        }
                    }
                }
            }
            this.DataSource = bs;
        }

        private void GetFieldCaption()
        {
            field.Clear();
            foreach (GridColumn gridcolumn in (this.MainView as GridView).Columns)
            {
                if (gridcolumn.Visible == true)
                {
                    string sCaptionName = gridcolumn.Caption + " (" + gridcolumn.FieldName + ")";
                    field.Add(sCaptionName);
                }
            }
        }

        //网格状态设置快捷菜单
        private List<string> lStringFormat = new List<string>();

        //private List<string> lStringFilter = new List<string>();
        //private List<string> lStringCaption = new List<string>();
        private void FormatMenuItem1_Click_2(object sender, EventArgs e)
        {
            string XmlFileName;

            //网格状态设置窗体
            var frm = new Status(Table, _ColorSet + Name, _ColorSet);
            if (string.Equals(SysVar.FCurryFormInfo.sFormType, "qry", StringComparison.OrdinalIgnoreCase))
            {
                frm.IsDisplaySearchKeyColumn = false;
            }

            //网格数据过滤配置
            var filterConfig = new KzxGridDataFilterConfig(_ColorSet + Name);

            StatusSet.dt.Clear();
            StatusSet.dt.Columns.Clear();
            DataColumn dc = null;
            dc = StatusSet.dt.Columns.Add("bSelect", Type.GetType("System.Boolean"));

            dc = StatusSet.dt.Columns.Add("sCaption", Type.GetType("System.String"));
            dc = StatusSet.dt.Columns.Add("sField", Type.GetType("System.String"));
            dc = StatusSet.dt.Columns.Add("bSum", Type.GetType("System.Boolean"));
            dc = StatusSet.dt.Columns.Add("Sumtype", Type.GetType("System.String"));
            dc = StatusSet.dt.Columns.Add("StringFormat", Type.GetType("System.String"));
            dc = StatusSet.dt.Columns.Add("bFilter", Type.GetType("System.Boolean"));
            dc = StatusSet.dt.Columns.Add("sParent", Type.GetType("System.String"));
            dc = StatusSet.dt.Columns.Add("bSearchKeyField", Type.GetType("System.Boolean"));
            dc = StatusSet.dt.Columns.Add("sFieldDesc", Type.GetType("System.String"));
            if (this.MainView != null)//20161226，lfx，解决网格状态列设置时，当前报表存在多层标题时this.gridView为空导致无法设置网格列状态
            {
                //读取INI文件保存的格式化字符串设置
                string formatFilePath = Application.StartupPath + @"\Guid\StringFormat.ini";
                IniFileCore formatIniFile = new IniFileCore(formatFilePath);
                List<string> sFname = new List<string>();

                foreach (GridColumn dtColumn in ((GridView)this.MainView).Columns)
                {
                    if (dtColumn.Caption != "" && dtColumn.Caption.Equals(dtColumn.FieldName) == false)
                    {
                        sFname.Add(dtColumn.FieldName);
                        DataRow row = StatusSet.dt.NewRow();

                        if (dtColumn.Visible)
                        {
                            row["bSelect"] = true;
                            row["bFilter"] = false;
                            row["bSearchKeyField"] = false;
                        }
                        else
                        {
                            row["bSelect"] = false;
                            row["bFilter"] = false;
                            row["bSearchKeyField"] = false;
                        }

                        if (((GridView)this.MainView).Columns[dtColumn.FieldName].SummaryItem.DisplayFormat == "")
                        {
                            row["bSum"] = false;
                            row["StringFormat"] = "";
                        }
                        else
                        {
                            row["bSum"] = true;
                            row["StringFormat"] = ((GridView)this.MainView).Columns[dtColumn.FieldName].SummaryItem.DisplayFormat;
                        }
                        string s = dtColumn.Caption;
                        DataRow[] rows = this.ColumnInfoTable.Select(string.Format("Field='{0}'", dtColumn.FieldName));
                        if (rows.Length > 0)
                        {
                            s = GetLanguage(rows[0]["MessageCode"].ToString(), rows[0]["DesigeCaption"].ToString());
                        }

                        //管理工具中设置列不显示，lfx，20161226，不将此列添加到网格状态设置中
                        if (rows.Length > 0 && !Convert.ToBoolean(rows[0]["Visible"].ToString()))
                        {
                            continue;
                        }

                        row["sCaption"] = s + " (" + dtColumn.FieldName + ")";
                        //row["sCaption"] = dtColumn.Caption + " (" + dtColumn.FieldName + ")";
                        row["sField"] = dtColumn.FieldName;
                        row["sFieldDesc"] = s;
                        switch (((GridView)this.MainView).Columns[dtColumn.FieldName].SummaryItem.SummaryType)
                        {
                            case DevExpress.Data.SummaryItemType.Sum: row["Sumtype"] = "合计"; break;
                            case DevExpress.Data.SummaryItemType.Average: row["Sumtype"] = "平均值"; break;
                            case DevExpress.Data.SummaryItemType.Max: row["Sumtype"] = "最大值"; break;
                            case DevExpress.Data.SummaryItemType.Min: row["Sumtype"] = "最小值"; break;
                            case DevExpress.Data.SummaryItemType.Count: row["Sumtype"] = "数据总数量"; break;
                            case DevExpress.Data.SummaryItemType.None: row["Sumtype"] = ""; break;
                            default: break;
                        }

                        string sContein = formatIniFile.Read("" + _ColorSet + this.Name + "", "Format");
                        index = formatIniFile.Read("" + _ColorSet + this.Name + "", "Freeze");
                        if (!sContein.IsNullOrWhiteSpaceExt())
                        {
                            string sSFormat = sContein.Substring(0, sContein.Length - 1);
                            string[] sCell = sSFormat.Split(',');

                            for (int i = 0; i < sCell.Length; i++)
                            {
                                string[] _sCellSeparated = sCell[i].Split('|');
                                if (dtColumn.FieldName == _sCellSeparated[0])
                                {
                                    dtColumn.FieldName = _sCellSeparated[0];
                                    if (_sCellSeparated.Length > 1)
                                    {
                                        Dictionary<string, string> dic = SetDisplayValue();
                                        string sTemp = dic.Where(q => q.Value == _sCellSeparated[1]).Select(q => q.Key).FirstOrDefault();

                                        row["StringFormat"] = sTemp;
                                    }
                                }
                                //if (dtColumn.FieldName == sCell[i].Split('|')[0].ToString())
                                //{
                                //    row["StringFormat"] = sCell[i].Split('|')[2].ToString();
                                //}
                            }
                        }


                        StatusSet.dt.Rows.Add(row);
                    }
                }

                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //网格状态确认设置回调 HHL modify 20170809
                    List<CheckedFilterParam> filterParamList = new List<CheckedFilterParam>();
                    DataRow[] drParamList = StatusSet.dt.Select("bFilter = 'true'");
                    for (int i = 0; i < drParamList.Length; i++)
                    {
                        string sColumnName = drParamList[i]["sCaption"].ToString().Substring(0, drParamList[i]["sCaption"].ToString().LastIndexOf("("));
                        CheckedFilterParam checkedFilterParam = new CheckedFilterParam();
                        checkedFilterParam.ColumnCode = string.IsNullOrWhiteSpace(drParamList[i]["sField"].ToString()) ? "" : drParamList[i]["sField"].ToString();
                        checkedFilterParam.ColumnParentCode = string.IsNullOrWhiteSpace(drParamList[i]["sParent"].ToString()) ? "" : drParamList[i]["sParent"].ToString();
                        checkedFilterParam.ColumnName = sColumnName;

                        if (drParamList[i]["bFilter"].ToString().ToLower() == "true")
                        {
                            checkedFilterParam.IsFilter = true;
                        }
                        if (drParamList[i]["bSearchKeyField"].ToString().ToLower() == "true")
                        {
                            checkedFilterParam.IsSearchKeyField = true;
                        }

                        filterParamList.Add(checkedFilterParam);
                    }
                    if (OnThrowCheckedFilteParam != null) OnThrowCheckedFilteParam.Invoke(new ThrowCheckFilterParamEventArgs(filterParamList));

                    int dtrows = StatusSet.dt.Rows.Count;

                    //获取前台传过来的数据
                    if (index != "")
                    {
                        int i;
                        try
                        {
                            i = int.Parse(index.ToString());

                            if (i == 0)
                            {
                                for (int count = 0; count < dtrows; ++count)
                                {
                                    ((GridView)this.MainView).VisibleColumns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.None;
                                }
                            }
                            else if (i < 1)
                            {
                                KzxMessageBox.Show(sysClass.ssLoadMsgOrDefault("MSG002426", "数字必须大于1"));
                            }
                            else if (i > dtrows)
                            {
                                KzxMessageBox.Show(sysClass.ssLoadMsgOrDefault("MSG002426", "超过列的最长范围"));
                            }
                            else if (i < dtrows)
                            {
                                //设置新的冻结前去掉原来的冻结
                                for (int indexs1 = 0; indexs1 < dtrows + 1; indexs1++)
                                {
                                    ((GridView)this.MainView).VisibleColumns[i - 1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.None;
                                }
                                //设置新的冻结
                                for (int indexs = 0; indexs < dtrows + 1; indexs++)
                                {
                                    ((GridView)this.MainView).VisibleColumns[i - 1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            KzxMessageBox.Show(sysClass.ssLoadMsgOrDefault("MSG002425", "输入有效数字"));

                        }
                    }
                    int m = 0;
                    //读取INI文件保存的过滤设置(读取原来的INI信息)
                    string ssContein = "";
                    //string sFilterFile = Application.StartupPath + @"\Guid\bFilter.ini";
                    //IniFile sbFilter = new IniFile(sFilterFile);
                    //ssContein = sbFilter.IniReadValue("" + _ColorSet + this.Name + "", "Filter");
                    //string sbBFilter = ssContein.Substring(0, ssContein.Length);
                    //string[] ssCell = sbBFilter.Split(',');
                    bool ssFlag = false;
                    foreach (DataRow dr in StatusSet.dt.Rows)
                    {
                        if ((Boolean)dr["bSum"])
                        {
                            m += 1;
                        }
                        string sFormat = dr["StringFormat"].ToString();
                        string sFileName = System.Text.RegularExpressions.Regex.Replace(@"" + dr["sCaption"].ToString() + "", @"(.*\()(.*)(\).*)", "$2");
                        if (string.IsNullOrWhiteSpace(dr["StringFormat"].ToString()) == false)
                        {
                            Dictionary<string, string> dic = SetDisplayValue();


                            lStringFormat.Add(sFileName + "|" + dic[sFormat]);
                        }
                        string sFilter = dr["bFilter"].ToString();
                        string sSearchKeyFiled = dr["bSearchKeyField"].ToString();
                        string sParent = string.IsNullOrWhiteSpace(dr["sParent"].ToString()) ? "" : dr["sParent"].ToString();//父类
                        string sCaption = dr["sCaption"].ToString().Substring(0, dr["sCaption"].ToString().LastIndexOf("("));
                        sFileName = System.Text.RegularExpressions.Regex.Replace(@"" + dr["sCaption"].ToString() + "", @"(.*\()(.*)(\).*)", "$2");

                        //var filterItem = new KzxGridDataFilterItem()
                        //{
                        //    FieldName = dr["sField"].ToString().Trim(),
                        //    FieldDesc = sCaption,
                        //    DataSetParentField = dr["sParent"].ToString(),
                        //    IsDatabaseFilter = string.Equals(sSearchKeyFiled, "True", StringComparison.OrdinalIgnoreCase),
                        //    IsDataSetFilter = string.Equals(sFilter, "True", StringComparison.OrdinalIgnoreCase),
                        //};
                        //filterConfig.Set(filterItem);

                        //if (String.IsNullOrWhiteSpace(dr["bFilter"].ToString()) == false)
                        //{
                        //    lStringFilter.Add("|" + sFilter + "|" + sParent + "|" + sSearchKeyFiled);
                        //    lStringCaption.Add(sFileName + "|" + sCaption);
                        //}
                        if ((Boolean)dr["bSelect"])
                        {

                            ((GridView)this.MainView).Columns.ColumnByFieldName(dr["sField"].ToString().Trim()).Visible = true;
                            //this.gridView.Columns.ColumnByFieldName(dr["sField"].ToString().Trim()).DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            if (string.IsNullOrWhiteSpace(dr["StringFormat"].ToString()) == false)
                            {
                                Dictionary<string, string> dic = SetDisplayValue();
                                ((GridView)this.MainView).Columns.ColumnByFieldName(dr["sField"].ToString().Trim()).DisplayFormat.FormatString = $"{dic[dr["StringFormat"].ToString()]}";
                            }
                        }
                        else
                        {
                            ((GridView)this.MainView).Columns.ColumnByFieldName(dr["sField"].ToString().Trim()).SummaryItem.DisplayFormat = "";
                            ((GridView)this.MainView).Columns.ColumnByFieldName(dr["sField"].ToString().Trim()).SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.None;
                            ((GridView)this.MainView).Columns.ColumnByFieldName(dr["sField"].ToString().Trim()).Visible = false;
                        }

                        string sSFormat = "";
                        for (int i = 0; i < lStringFormat.Count; i++)
                        {
                            sSFormat += lStringFormat[i].ToString() + ",";
                        }
                        string FormatFile = Application.StartupPath + @"\Guid\StringFormat.ini";
                        IniFileCore SFormat = new IniFileCore(FormatFile);
                        SFormat.Write("" + _ColorSet + this.Name + "", "Format", sSFormat);


                        foreach (char c in index)
                        {
                            try
                            {
                                if (int.Parse(index) < dtrows)
                                {
                                    SFormat.Write("" + _ColorSet + this.Name + "", "Freeze", index);
                                }
                            }
                            catch (Exception ex)
                            {
                            }

                        }
                        string[] _sCaptionArray = Convert.ToString(dr["sCaption"]).Split('(');
                        string _StrsCaption = _sCaptionArray.Length >= 2 ? _sCaptionArray[1].Replace(")", string.Empty).Trim() : Convert.ToString(dr["sCaption"]);
                        if ((Boolean)dr["bSum"] && _EncryptionColumns.Contains(_StrsCaption.Trim()) == false)
                        {
                            GridView TempGridView = ((GridView)this.MainView);
                            ((GridView)this.MainView).OptionsView.ShowFooter = true;
                            ((GridView)this.MainView).Columns.ColumnByFieldName(dr["sField"].ToString().Trim()).SummaryItem.DisplayFormat = GetDisplayFormatByIni(dr["sField"].ToStringExt().Trim());//"{0}";
                            switch (dr["Sumtype"].ToString())
                            {
                                case "合计":
                                    TempGridView.Columns.ColumnByFieldName(dr["sField"].ToString().Trim()).SummaryItem.SummaryType = SummaryItemType.Sum;
                                    break;
                                case "平均值":
                                    TempGridView.Columns.ColumnByFieldName(dr["sField"].ToString().Trim()).SummaryItem.SummaryType = SummaryItemType.Average;
                                    break;
                                case "最大值":
                                    TempGridView.Columns.ColumnByFieldName(dr["sField"].ToString().Trim()).SummaryItem.SummaryType = SummaryItemType.Max;
                                    break;
                                case "最小值":
                                    TempGridView.Columns.ColumnByFieldName(dr["sField"].ToString().Trim()).SummaryItem.SummaryType = SummaryItemType.Min;
                                    break;
                                case "数据总数量":
                                    TempGridView.Columns.ColumnByFieldName(dr["sField"].ToString().Trim()).SummaryItem.SummaryType = SummaryItemType.Count;
                                    break;
                                case "":
                                    TempGridView.Columns.ColumnByFieldName(dr["sField"].ToString().Trim()).SummaryItem.SummaryType = SummaryItemType.None;
                                    break;
                                default: break;
                            }
                        }
                        else
                        {
                            ((GridView)this.MainView).Columns.ColumnByFieldName(dr["sField"].ToString().Trim()).SummaryItem.DisplayFormat = "";
                            ((GridView)this.MainView).Columns.ColumnByFieldName(dr["sField"].ToString().Trim()).SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.None;
                        }
                    }


                    if (m == 0)
                    {
                        ((GridView)this.MainView).OptionsView.ShowFooter = false;
                    }
                    else
                    {
                        ((GridView)this.MainView).OptionsView.ShowFooter = true;
                    }

                    lStringFormat.Clear();
                    //lStringFilter.Clear();
                    //lStringCaption.Clear();
                    lsStatus.Clear();
                    try
                    {
                        //创建Xml文件
                        string path = Application.StartupPath + @"\Guid";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        if (File.Exists(Application.StartupPath + @"\Guid\" + _ColorSet + this.Name + ".xml") == false)
                        {
                            FileStream fs = new FileStream(Application.StartupPath + @"\Guid\" + _ColorSet + this.Name + ".xml", FileMode.Create);
                            XmlFileName = Application.StartupPath + @"\Guid\" + _ColorSet + this.Name + ".xml";
                            fs.Close();
                        }
                        //加载XML网格状态信息
                        XmlFileName = Application.StartupPath + @"\Guid\" + _ColorSet + this.Name + ".xml";
                        BandedGridView bandedView = this.MainView as BandedGridView;
                        if (bandedView != null)
                        {
                            foreach (GridBand item in bandedView.Bands)
                            {
                                if (item.Children.Count > 0) //是否有下级Band  
                                {
                                    #region  存在子标题的情况下，分别对子标题的列的可见性进行网格状态设置，并根据子标题情况设置父标题显示状态。lfx，20161227
                                    bool flag = false;
                                    for (int i = 0; i < item.Children.Count; i++)
                                    {
                                        foreach (GridColumn dtColumn in bandedView.Columns)
                                        {
                                            if (item.Children[i].Caption != "" && dtColumn.Caption.Equals(item.Children[i].Caption) == true)
                                            {
                                                item.Children[i].Visible = dtColumn.Visible;
                                                if (dtColumn.Visible)
                                                {
                                                    flag = true;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    item.Visible = flag;
                                    #endregion
                                }
                                else
                                {
                                    item.Visible = item.Columns[0].Visible;
                                }
                            }
                            bandedView.SaveLayoutToXml(XmlFileName);//多层标题网格，列设置信息完成后将网格状态设置数据保存到xml文件，lfx，20161227
                        }
                        else
                        {
                            this.MainView.SaveLayoutToXml(XmlFileName);//非多层标题网格保存网格状态设置信息到xml文件，lfx，20161227
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }
        /// <summary>
        /// 抽出来公共方法
        /// </summary> 
        /// <returns></returns>
        private Dictionary<string, string> SetDisplayValue()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("0位", "{0:F0}");
            dic.Add("1位", "{0:F1}");
            dic.Add("2位", "{0:F2}");
            dic.Add("3位", "{0:F3}");
            dic.Add("4位", "{0:F4}");
            dic.Add("5位", "{0:F5}");
            dic.Add("6位", "{0:F6}");
            return dic;
        }

        //添加行号
        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        //获取INI文件Conut信息
        private string PsCount = "";
        private void GetIniCountToString()
        {
            string path = Application.StartupPath + @"\Guid";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName;
            if (File.Exists(Application.StartupPath + @"\Guid\ColorSet.ini"))
            {
                fileName = Application.StartupPath + @"\Guid\ColorSet.ini";
            }
            else
            {
                FileStream fs = new FileStream(Application.StartupPath + @"\Guid\ColorSet.ini", FileMode.Create);
                fs.Close();
                fileName = Application.StartupPath + @"\Guid\ColorSet.ini";
            }
            fileName = Application.StartupPath + @"\Guid\ColorSet.ini";
            IniFileCore ColorSet = new IniFileCore(fileName);
            PsCount = ColorSet.Read("" + _ColorSet + this.Name + "", "Count");
            if (PsCount == "")
            {
                ColorSet.Write("" + _ColorSet + this.Name + "", "Count", "0");
                PsCount = "0";
            }
        }

        //获取INI文件颜色信息
        private List<string> LsColor = new List<string>();
        private void GetIniColorToString()
        {
            int iCount = Convert.ToInt16(PsCount);
            for (int i = 1; i <= iCount; i++)
            {
                string key = "ColorSet" + i.ToString();
                string fileName;
                fileName = Application.StartupPath + @"\Guid\ColorSet.ini";
                IniFileCore ColorSet = new IniFileCore(fileName);
                string sContein = ColorSet.Read("" + _ColorSet + this.Name + "", key);
                LsColor.Add(sContein);
            }
        }

        //获取INI文件格式化字符串信息
        private List<string> lsStatus = new List<string>();
        private void GetIniStatusToString()
        {
            string FormatFiles;
            if (File.Exists(Application.StartupPath + @"\Guid\StringFormat.ini"))
            {
                FormatFiles = Application.StartupPath + @"\Guid\StringFormat.ini";
            }
            else
            {
                FileStream fs = new FileStream(Application.StartupPath + @"\Guid\StringFormat.ini", FileMode.Create);
                fs.Close();
                FormatFiles = Application.StartupPath + @"\Guid\StringFormat.ini";
            }
            string FormatFile = Application.StartupPath + @"\Guid\StringFormat.ini";
            IniFileCore SFormat = new IniFileCore(FormatFile);
            string sContein = SFormat.Read("" + _ColorSet + this.Name + "", "Format");
            lsStatus.Add(sContein);
        }

        //获取INI文件过滤信息

        /// <summary>
        /// 获取INI文件中的Freeze冻结列数，加载页面时冻结
        /// </summary>
        /// <param name="gridView"></param>
        private void GetGridView(GridView gridView)
        {
            string GridView;
            if (File.Exists(Application.StartupPath + @"\Guid\StringFormat.ini"))
            {
                GridView = Application.StartupPath + @"\Guid\StringFormat.ini";
            }
            else
            {
                FileStream fs = new FileStream(Application.StartupPath + @"\Guid\StringFormat.ini", FileMode.Create);
                fs.Close();
                GridView = Application.StartupPath + @"\Guid\StringFormat.ini";
            }
            string GridViews = Application.StartupPath + @"\Guid\StringFormat.ini";
            IniFileCore SFormat = new IniFileCore(GridViews);
            string sSum = SFormat.Read("" + _ColorSet + this.Name + "", "Freeze");
            try
            {
                if (sSum != "")
                {

                    if (gridView != null)
                    {
                        int grid = int.Parse(sSum);
                        for (int i = 0; i < grid; i++)
                        {
                            this.gridView.VisibleColumns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                        }
                    }
                    //   if (int.Parse(sSum) < 1)
                    //{
                    //  this.gridView.VisibleColumns[].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.None;
                    // }
                }

            }
            catch (Exception ex)
            {
                KzxMessageBox.Show(ex.Message);
            }
        }

        //读取ini文件颜色设置
        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (PsCount == "")
            {
                GetIniCountToString();
                GetIniColorToString();
            }
            if (lsStatus.Count == 0)
            {
                GetIniStatusToString();
            }

            GridView gridView1 = (GridView)sender;
            gridView = gridView1;

            GetGridView(gridView);
            //设置网格奇数行背景颜色为淡粉色，偶数行为白色
            //this.gridView.Appearance.OddRow.BackColor = Color.AntiqueWhite;
            //try
            //{
            //    this.gridView.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Properties.Settings.Default.GridColorA));
            //    this.gridView.OptionsView.EnableAppearanceOddRow = true;
            //    //this.gridView.Appearance.EvenRow.BackColor = Color.White;
            //    this.gridView.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Properties.Settings.Default.GridColorB));
            //    this.gridView.OptionsView.EnableAppearanceEvenRow = true;
            //}
            //catch
            //{
            //    this.gridView.Appearance.OddRow.BackColor = Color.AntiqueWhite;
            //    this.gridView.OptionsView.EnableAppearanceOddRow = true;
            //    this.gridView.Appearance.EvenRow.BackColor = Color.White;
            //    this.gridView.OptionsView.EnableAppearanceEvenRow = true;
            //}
            string path = Application.StartupPath + @"\Guid";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //格式化单元格字符串
            try
            {
                List<string> sFname = new List<string>();
                foreach (GridColumn dtColumn in gridView1.Columns)
                {
                    if (dtColumn.Caption != "")
                    {
                        sFname.Add(dtColumn.FieldName);
                    }
                    string sContein = lsStatus[0].ToString();
                    if (sContein != "")
                    {
                        string sSFormat = sContein.Substring(0, sContein.Length - 1);
                        string[] sCell = sSFormat.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string value in sCell)
                        {
                            string[] sValue = value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                            if (sValue.Length >= 2)
                            {
                                this.gridView.Columns[sValue[0]].DisplayFormat.FormatString = "" + sValue[1] + "";
                            }
                        }

                        //for (int i = 0; i < sFname.Count; i++)
                        //{
                        //    if (dtColumn.Caption == this.gridView.Columns[sFname[i].ToString()].ToString())
                        //    {
                        //        string sFormat = sCell[i];
                        //        if (string.IsNullOrWhiteSpace(sFormat)==false)
                        //        {
                        //            this.gridView.Columns[dtColumn.FieldName].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        //            this.gridView.Columns[dtColumn.FieldName].DisplayFormat.FormatString = "" + sFormat + "";
                        //        }
                        //    }
                        //}
                    }
                }
            }
            catch (Exception)
            {

            }
            //颜色设置

            string sFitter = "";
            string sColor = "";
            string sRowOrCol = "";
            string sFieldName = "";
            bool bSetColor = false;
            bool bSetColor2 = true;
            int iCount = Convert.ToInt32(PsCount);

            for (int i = 1; i <= iCount; i++)
            {
                string sEqual = "AND";
                string key = "ColorSet" + i.ToString();
                string sContein = LsColor[i - 1].ToString();
                string[] sCell = sContein.Split('|');
                string sid = "";
                for (int j = 0; j < sCell.Length; j++)
                {

                    string[] sKeytable = sCell[j].Split(',');
                    if (j == 0)
                    {
                        sid = sKeytable[0];
                        sColor = sKeytable[1];
                        sRowOrCol = sKeytable[2];
                        continue;
                    }
                    try
                    {
                        Type type = null;
                        string sTable = "";
                        string sFilename = System.Text.RegularExpressions.Regex.Replace(@"" + sKeytable[0].ToString() + "", @"(.*\()(.*)(\).*)", "$2");
                        foreach (GridColumn dtColumn in gridView1.Columns)
                        {
                            if (dtColumn.FieldName == sFilename)
                            {
                                sTable = dtColumn.FieldName.ToString();
                                type = dtColumn.ColumnType;
                            }
                        }
                        sFitter = gridView1.GetRowCellValue(e.RowHandle, sTable).ToString();

                        if (type == typeof(decimal) || type == typeof(int) || type == typeof(float) || type == typeof(long))
                        {
                            decimal f1, f2;
                            Decimal.TryParse(sFitter, out f1);
                            Decimal.TryParse(sKeytable[2], out f2);

                            bSetColor = EqualFitter(f1, f2, sKeytable[1]);
                        }
                        else
                        {
                            bSetColor = EqualFitter(sFitter, sKeytable[2], sKeytable[1]);
                        }
                        sFieldName = sTable;
                    }
                    catch (Exception ex)
                    {

                    }
                    if ("AND" == sEqual)
                    {
                        bSetColor2 = (bSetColor && bSetColor2);
                    }
                    else
                    {
                        bSetColor2 = (bSetColor || bSetColor2);
                    }
                    sEqual = sKeytable[3];

                }
                if (bSetColor2 == true)
                {
                    //判断是行颜色还是列颜色改变
                    string s = System.Text.RegularExpressions.Regex.Replace(@"" + sRowOrCol + "", @"(.*\()(.*)(\).*)", "$2");
                    if (s == "Row BackColor")
                    {
                        e.Appearance.BackColor = GetColorByString(sColor);
                    }
                    if (s == "Row ForeColor")
                    {
                        e.Appearance.ForeColor = GetColorByString(sColor);
                    }
                    if (s == "Column BackColor")
                    {
                        if (e.Column.FieldName == sFieldName)
                            e.Appearance.BackColor = GetColorByString(sColor);
                    }
                    if (s == "Column ForeColor")
                    {
                        if (e.Column.FieldName == sFieldName)
                            e.Appearance.ForeColor = GetColorByString(sColor);
                    }
                }
                bSetColor2 = true;
            }

        }

        //比较判断
        private bool EqualFitter(decimal sF1, decimal sF2, string sFiltter)
        {
            bool bTmp = false;
            switch (sFiltter)
            {
                case "=": bTmp = (sF1.Equals(sF2)); break;
                case ">": bTmp = sF1 > sF2; break;
                case "<": bTmp = sF1 < sF2; break;
                case ">=": bTmp = sF1 >= sF2; break;
                case "<=": bTmp = sF1 <= sF2; break;
                default: break;
            }
            return bTmp;
        }

        /********************************************************
        * 【方法修改记录】
        * {20180408}：BY {jyg}，{添加like操作符。}
        ********************************************************/
        /// <summary>
        /// 比较判断，确定行列是否满足颜色条件。
        /// </summary>
        /// <param name="sF1">行列原值。</param>
        /// <param name="sF2">查询值。</param>
        /// <param name="sFiltter">操作符号。</param>
        /// <returns></returns>
        private bool EqualFitter(string sF1, string sF2, string sFiltter)
        {
            bool bTmp = false;
            switch (sFiltter)
            {
                case "=": bTmp = (sF1.Equals(sF2)); break;
                case ">": bTmp = (string.Compare(sF1, sF2) > 0); break;
                case "<": bTmp = (string.Compare(sF1, sF2) < 0); break;
                case ">=": bTmp = (string.Compare(sF1, sF2) > 0 || (sF1.Equals(sF2))); break;
                case "<=": bTmp = (string.Compare(sF1, sF2) < 0 || (sF1.Equals(sF2))); break;
                case "like": bTmp = (sF1.IndexOf(sF2) > -1); break;
                case "not like": bTmp = (sF1.IndexOf(sF2) == -1); break;
                default: break;
            }
            return bTmp;
        }
        //将Ini中保存的颜色Argb值转化为颜色
        private Color GetColorByString(string sColor)
        {
            Color TmpColor;
            int iColor = Convert.ToInt32(sColor);
            TmpColor = Color.FromArgb(iColor);
            return TmpColor;
        }

        private Dictionary<string, BindingSource> _BillBindingSourceDictionary = new Dictionary<string, BindingSource>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 单据的数据源
        /// </summary>
        [Category("自定义"), Description("BillBindingSourceDictionary,单据的数据源"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, BindingSource> BillBindingSourceDictionary
        {
            get
            {
                return this._BillBindingSourceDictionary;
            }
            set
            {
                this._BillBindingSourceDictionary = value;
            }
        }


        /// <summary>
        /// 指定父表
        /// </summary>
        private string _ParentTable = "";
        [Category("上级表"), Description("ParentTable,指点父表"), Browsable(true)]
        [McDisplayName("ParentTable")]
        public virtual string ParentTable
        {
            get
            {

                return this._ParentTable;
            }
            set
            {
                this._ParentTable = value;
            }
        }

        private string toolTipMaxLengthText = string.Empty;
        /// <summary>
        /// 数据长度不能超过数据库长度提示文本  
        /// </summary>
        public string ToolTipMaxLengthText
        {
            get { return toolTipMaxLengthText; }
            set { toolTipMaxLengthText = value; }
        }

        private string _MessageCode = "0";
        /// <summary>
        /// 多语言环境下显示文本的对应标识
        /// </summary>
        [Category("多语言"), Description("MessageCode,多语言环境下显示文本的对应标识"), Browsable(true)]
        [McDisplayName("MessageCode")]
        public virtual string MessageCode
        {
            get
            {
                return this._MessageCode;
            }
            set
            {
                this._MessageCode = value;
            }
        }

        private string _DesigeCaption = "显示标题";
        /// <summary>
        /// 没有多语言的情况下的默认显示标题
        /// </summary>
        [Category("多语言"), Description("DesigeCaption,没有多语言的情况下的默认显示标题"), Browsable(true)]
        [McDisplayName("DesigeCaption")]
        public virtual string DesigeCaption
        {
            get
            {
                return this._DesigeCaption;
            }
            set
            {
                this._DesigeCaption = value;
            }
        }

        private string _Key = string.Empty;
        /// <summary>
        /// 控件的唯一标识
        /// </summary>
        [Category("数据"), Description("Key,控件的唯一标识"), Browsable(true)]
        [McDisplayName("Key")]
        public virtual string Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        private bool _DesigeEnabled = true;
        /// <summary>
        /// 设计时的可用性
        /// </summary>
        [Category("特性"), Description("DesigeEnabled,设计时的可用性"), Browsable(true)]
        [McDisplayName("DesigeEnabled")]
        public virtual bool DesigeEnabled
        {
            get
            {
                return this._DesigeEnabled;
            }
            set
            {
                this._DesigeEnabled = value;
                this.Enabled = value;
            }
        }

        private bool _DesigeVisible = true;
        /// <summary>
        /// 设计时可见性
        /// </summary>
        [Category("特性"), Description("DesigeVisible,设计时可见性"), Browsable(true)]
        [McDisplayName("DesigeVisible")]
        public virtual bool DesigeVisible
        {
            get
            {
                return this._DesigeVisible;
            }
            set
            {
                this._DesigeVisible = value;
                if (this.IsDesignMode == true)
                {
                }
                else
                {
                    this.Visible = value;
                }
            }
        }

        private string _DefaultValue = string.Empty;
        /// <summary>
        /// 默认值
        /// </summary>
        [Category("自定义"), Description("DefaultValue,控件的默认值"), Browsable(false)]
        [McDisplayName("DefaultValue")]
        public virtual String DefaultValue
        {
            get
            {
                return this._DefaultValue;
            }
            set
            {
                this._DefaultValue = value;

            }
        }

        private string _FormatString = string.Empty;
        /// <summary>
        /// 显示格式字符串
        /// </summary>
        [Category("数据格式"), Description("FormatString,显示格式字符串"), Browsable(true)]
        [McDisplayName("FormatString")]
        public virtual string FormatString
        {
            get
            {
                return this._FormatString;
            }
            set
            {
                this._FormatString = value;
            }
        }

        private string _Field = string.Empty;
        /// <summary>
        /// 数据库的字段名称
        /// </summary>
        [Category("数据"), Description("Field,数据库的字段名称"), Browsable(true)]
        [McDisplayName("Field")]
        public virtual string Field
        {
            get
            {
                return this._Field.Trim();
            }
            set
            {
                this._Field = Regex.Replace(value, @"\s", string.Empty);
                if (string.IsNullOrWhiteSpace(this.Table) == false && string.IsNullOrWhiteSpace(value) == false)
                {
                    if (string.IsNullOrWhiteSpace(this.Key) == true)
                    {
                        this.Key = value.Trim() + "." + this.Field.Trim();
                    }
                }
            }
        }

        private string _Table = string.Empty;
        /// <summary>
        /// 数据库的表名
        /// </summary>
        [Category("数据"), Description("Table,数据库的表名"), Browsable(true)]
        [McDisplayName("Table")]
        public virtual string Table
        {
            get
            {
                return this._Table.Trim();
            }
            set
            {
                this._Table = Regex.Replace(value, @"\s", string.Empty);
                if (string.IsNullOrWhiteSpace(this.Field) == false && string.IsNullOrWhiteSpace(value) == false)
                {
                    if (string.IsNullOrWhiteSpace(this.Key) == true)
                    {
                        this.Key = value.Trim() + "." + this.Field.Trim();
                    }
                }
                if (string.IsNullOrWhiteSpace(this._ColorSet) == true && string.IsNullOrWhiteSpace(value) == false)
                {
                    this.ColorSet = value.Trim();
                }
            }
        }

        private string _SourceTable = string.Empty;
        /// <summary>
        /// 源表名称
        /// </summary>
        [Category("自定义"), Description("SourceTable,源表名称"), Browsable(false), Editor(typeof(TextUiTypEdit), typeof(UITypeEditor))]
        [McDisplayName("SourceTable")]
        public virtual string SourceTable
        {
            get
            {
                return this._SourceTable.Trim();
            }
            set
            {
                this._SourceTable = value;
            }
        }

        private bool _ShowGroupPanel = true;
        /// <summary>
        /// 显示分组面板
        /// </summary>
        [Category("外观"), Description("ShowGroupPanel,显示分组面板"), Browsable(true)]
        [McDisplayName("ShowGroupPanel")]
        public bool ShowGroupPanel
        {
            get
            {
                if (this.MainView != null)
                {
                    for (int i = this.Views.Count - 1; i >= 0; i--)
                    {
                        if (this.Views[i] is GridView)
                        {
                            (this.Views[i] as GridView).OptionsView.ShowGroupPanel = this._ShowGroupPanel;
                        }
                    }
                    return (this.MainView as GridView).OptionsView.ShowGroupPanel;
                }
                else
                {
                    return this._ShowGroupPanel;
                }
            }
            set
            {
                this._ShowGroupPanel = value;
                if (this.MainView != null)
                {
                    (this.MainView as GridView).OptionsView.ShowGroupPanel = value;
                    for (int i = this.Views.Count - 1; i >= 0; i--)
                    {
                        if (this.Views[i] is GridView)
                        {
                            (this.Views[i] as GridView).OptionsView.ShowGroupPanel = this._ShowGroupPanel;
                        }
                    }
                }
            }
        }

        private bool _ShowFooter = false;
        /// <summary>
        /// 显示页脚行
        /// </summary>
        [Category("外观"), Description("ShowFooter,显示页脚行"), Browsable(true)]
        [McDisplayName("ShowFooter")]
        public bool ShowFooter
        {
            get
            {
                if (this.MainView != null)
                {
                    return (this.MainView as GridView).OptionsView.ShowFooter;
                }
                else
                {
                    return this._ShowFooter;
                }
            }
            set
            {
                this._ShowFooter = value;
                if (this.MainView != null)
                {
                    (this.MainView as GridView).OptionsView.ShowFooter = value;
                }
            }
        }

        private bool _ShowAutoFilterRow = false;
        /// <summary>
        /// 显示过滤行
        /// </summary>
        [Category("外观"), Description("ShowAutoFilterRow,显示过滤行"), Browsable(true)]
        [McDisplayName("ShowAutoFilterRow")]
        public bool ShowAutoFilterRow
        {
            get
            {
                if (this.MainView != null)
                {
                    return (this.MainView as GridView).OptionsView.ShowAutoFilterRow;
                }
                else
                {
                    return this._ShowAutoFilterRow;
                }
            }
            set
            {
                this._ShowAutoFilterRow = value;
                if (this.MainView != null)
                {
                    (this.MainView as GridView).OptionsView.ShowAutoFilterRow = value;
                }
            }
        }

        private string _SelectedValuePath = string.Empty;
        /// <summary>
        /// 源表实际值字段
        /// </summary>
        [Category("自定义"), Description("SelectedValuePath,源表实际值字段"), Browsable(false)]
        [McDisplayName("SelectedValuePath")]
        public virtual string SelectedValuePath
        {
            get
            {
                return this._SelectedValuePath.Trim();
            }
            set
            {
                this._SelectedValuePath = value;
            }
        }

        private string _DisplayMemberPath = string.Empty;
        /// <summary>
        /// 源表显示值字段
        /// </summary>
        [Category("自定义"), Description("DisplayMemberPath,源表显示值字段"), Browsable(false)]
        [McDisplayName("DisplayMemberPath")]
        public virtual string DisplayMemberPath
        {
            get
            {
                return this._DisplayMemberPath.Trim();
            }
            set
            {
                this._DisplayMemberPath = value;
            }
        }

        private bool _IsNull = true;
        /// <summary>
        /// 可空性
        /// </summary>
        [Category("自定义"), Description("IsNull,可空性"), Browsable(false)]
        [McDisplayName("IsNull")]
        public virtual bool IsNull
        {
            get
            {
                return this._IsNull;
            }
            set
            {
                this._IsNull = value;
            }
        }

        private int _ReadOnlyInit = 0;
        private bool _ReadOnly = false;
        /// <summary>
        /// 只读性
        /// </summary>
        [Category("验证"), Description("ReadOnly,只读性"), Browsable(true)]
        [McDisplayName("ReadOnly")]
        public virtual bool ReadOnly
        {
            get
            {
                return this._ReadOnly;
            }
            set
            {
                this._ReadOnly = value;
                if (this._ReadOnlyInit == 1)
                {
                    GridView view = this.MainView as GridView;
                    if (view != null)
                    {
                        view.OptionsBehavior.ReadOnly = value;
                    }
                }
            }
        }

        private string _ValidateGroup = string.Empty;
        /// <summary>
        /// 唯一性验证组别
        /// </summary>
        [Category("自定义"), Description("ValidateGroup,唯一性验证组别"), Browsable(false)]
        [McDisplayName("ValidateGroup")]
        public virtual string ValidateGroup
        {
            get
            {
                return this._ValidateGroup;
            }
            set
            {
                this._ValidateGroup = value;
            }
        }

        private bool _IsUnique = false;
        /// <summary>
        /// 唯一性
        /// </summary>
        [Category("自定义"), Description("IsUnique,唯一性"), Browsable(false)]
        [McDisplayName("IsUnique")]
        public virtual bool IsUnique
        {
            get
            {
                return this._IsUnique;
            }
            set
            {
                this._IsUnique = value;
            }
        }

        private bool _AllowValueRange = false;
        /// <summary>
        /// 是否启用值范围验证
        /// </summary>
        [Category("验证"), Description("AllowValueRange,是否启用值范围验证"), Browsable(false)]
        [McDisplayName("AllowValueRange")]
        public virtual bool AllowValueRange
        {
            get
            {
                return this._AllowValueRange;
            }
            set
            {
                this._AllowValueRange = value;
            }
        }

        private KzxDataType _DataType = KzxDataType.Str;
        /// <summary>
        /// 值范围验证中的数据类型
        /// </summary>
        [Category("验证"), Description("DataType,值范围验证中的数据类型"), Browsable(false)]
        [McDisplayName("DataType")]
        public virtual KzxDataType DataType
        {
            get
            {
                return this._DataType;
            }
            set
            {
                this._DataType = value;
            }
        }

        private string _MaxValue = string.Empty;
        /// <summary>
        /// 值范围验证中最大值
        /// </summary>
        [Category("验证"), Description("MaxValue,值范围验证中最大值"), Browsable(false)]
        [McDisplayName("MaxValue")]
        public virtual string MaxValue
        {
            get
            {
                return this._MaxValue;
            }
            set
            {
                this._MaxValue = value;
            }
        }

        private string _MinValue = string.Empty;
        /// <summary>
        /// 值范围验证中最小值
        /// </summary>
        [Category("验证"), Description("MinValue,值范围验证中最大值"), Browsable(false)]
        [McDisplayName("MinValue")]
        public virtual string MinValue
        {
            get
            {
                return this._MinValue;
            }
            set
            {
                this._MinValue = value;
            }
        }

        private string _RegexString = string.Empty;
        private GridView gridView;
        /// <summary>
        /// 正则表达式验证
        /// </summary>
        [Category("验证"), Description("RegexString,正则表达式验证"), Browsable(false)]
        [McDisplayName("RegexString")]
        public virtual string RegexString
        {
            get
            {
                return this._RegexString;
            }
            set
            {
                this._RegexString = value;
            }
        }

        private string _Columns = string.Empty;
        /// <summary>
        /// 列集合
        /// </summary>
        [Category("数据"), Description("Columns,列集合"), Browsable(true), Editor(typeof(KzxGridControlColumnsUiTypeEdit), typeof(UITypeEditor))]
        [McDisplayName("Columns")]
        public string Columns
        {
            get
            {
                return this._Columns;
            }
            set
            {
                this._Columns = value;
                //if (this.DesignMode == true)
                //{
                //    this.ColumnInfoTable = KzxGridControl.SerializeColumns(this._Columns);
                //    CreateColumnByColumnInfo();
                //}
            }
        }

        private string _DllName = string.Empty;
        /// <summary>
        /// 自定义弹出窗口的配置关键字
        /// </summary>
        [Category("弹出窗口"), Description("DllName,自定义弹出窗口的配置关键字,配置后系统将采用自定义弹出窗口处理数据"), Browsable(true), Editor(typeof(TextUiTypEdit), typeof(UITypeEditor))]
        [McDisplayName("DllName")]
        public virtual string DllName
        {
            get
            {
                return this._DllName.Trim();
            }
            set
            {
                this._DllName = value;
            }
        }

        private bool _KzxUseEmbeddedNavigator = true;
        /// <summary>
        /// 启用编辑导航条
        /// </summary>
        [Category("外观"), Description("KzxUseEmbeddedNavigator,启用编辑导航条"), Browsable(true)]
        [McDisplayName("KzxUseEmbeddedNavigator")]
        public virtual Boolean KzxUseEmbeddedNavigator
        {
            get
            {
                return this._KzxUseEmbeddedNavigator;
            }
            set
            {
                this._KzxUseEmbeddedNavigator = value;
                if (this._ReadOnlyInit == 1)
                {
                    this.UseEmbeddedNavigator = value;
                }

                for (int i = 0; i < this._ContextMenuStrip.Items.Count; i++)
                {
                    if (this._ContextMenuStrip.Items[i].Name.Equals("copyline", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        this._ContextMenuStrip.Items[i].Visible = value;
                    }
                }
            }
        }

        private string _ValueDependencyField = string.Empty;
        /// <summary>
        /// 数据携带
        /// </summary>
        [Category("自定义"), Description("ValueDependencyField,此属性不可用"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [McDisplayName("ValueDependencyField")]
        public string ValueDependencyField
        {
            get
            {
                return this._ValueDependencyField;
            }
            set
            {
                this._ValueDependencyField = value;
            }
        }

        private List<object> _ObjectList = new List<object>();
        /// <summary>
        /// 界面上所有对象的集合
        /// </summary>
        [Category("自定义"), Description("ObjectList,界面上所有对象的集合"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [McDisplayName("ObjectList")]
        public List<object> ObjectList
        {
            get
            {
                return this._ObjectList;
            }
        }

        private string _EventList = string.Empty;
        /// <summary>
        /// 事件列表
        /// </summary>
        [Category("自定义"), Description("EventList,事件列表"), Browsable(true), Editor(typeof(ControlEventInfoUiTypeEdit), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [McDisplayName("EventList")]
        public string EventList
        {
            get
            {
                return this._EventList;
            }
            set
            {
                this._EventList = value;
            }
        }

        #region 布局属性

        private int _LayoutRow = 0;
        /// <summary>
        /// 布局行号
        /// </summary>
        [Category("布局"), Description("LayoutRow,布局行号"), Browsable(false)]
        [McDisplayName("LayoutRow")]
        public int LayoutRow
        {
            get
            {
                if (this.DesignMode == true)
                {
                    TableLayoutPanel panel = this.Parent as TableLayoutPanel;
                    if (panel != null)
                    {
                        this._LayoutRow = panel.GetRow(this);
                    }
                }
                return this._LayoutRow;
            }
            set
            {
                this._LayoutRow = value;
                if (this.DesignMode == true)
                {
                    TableLayoutPanel panel = this.Parent as TableLayoutPanel;
                    if (panel != null)
                    {
                        panel.SetRow(this, value);
                    }
                }
            }
        }

        private int _LayoutRowSpan = 1;
        /// <summary>
        /// 布局跨的行数
        /// </summary>
        [Category("布局"), Description("LayoutRowSpan,布局跨的行数"), Browsable(false)]
        [McDisplayName("LayoutRowSpan")]
        public int LayoutRowSpan
        {
            get
            {
                TableLayoutPanel panel = this.Parent as TableLayoutPanel;
                if (panel != null)
                {
                    panel.SetRowSpan(this, this._LayoutRowSpan);
                }
                return this._LayoutRowSpan;
            }
            set
            {
                this._LayoutRowSpan = value;
                TableLayoutPanel panel = this.Parent as TableLayoutPanel;
                if (panel != null)
                {
                    panel.SetRowSpan(this, value);
                }
            }
        }

        private int _LayoutColumn = 0;
        /// <summary>
        /// 布局列号
        /// </summary>
        [Category("布局"), Description("LayoutColumn,布局列号"), Browsable(false)]
        [McDisplayName("LayoutColumn")]
        public int LayoutColumn
        {
            get
            {
                if (this.DesignMode == true)
                {
                    TableLayoutPanel panel = this.Parent as TableLayoutPanel;
                    if (panel != null)
                    {
                        this._LayoutColumn = panel.GetColumn(this);
                    }
                }
                return this._LayoutColumn;
            }
            set
            {
                this._LayoutColumn = value;
                if (this.DesignMode == true)
                {
                    TableLayoutPanel panel = this.Parent as TableLayoutPanel;
                    if (panel != null)
                    {
                        panel.SetColumn(this, value);
                    }
                }
            }
        }

        private int _LayoutColumnSpan = 1;
        /// <summary>
        /// 布局跨的列数
        /// </summary>
        [Category("布局"), Description("LayoutColumnSpan,布局跨的列数"), Browsable(false)]
        [McDisplayName("LayoutColumnSpan")]
        public int LayoutColumnSpan
        {
            get
            {
                TableLayoutPanel panel = this.Parent as TableLayoutPanel;
                if (panel != null)
                {
                    panel.SetColumnSpan(this, this._LayoutColumnSpan);
                }
                return this._LayoutColumnSpan;
            }
            set
            {
                this._LayoutColumnSpan = value;
                TableLayoutPanel panel = this.Parent as TableLayoutPanel;
                if (panel != null)
                {
                    panel.SetColumnSpan(this, value);
                }
            }
        }

        #endregion

        #region 列信息表

        private DataTable _ColumnInfoTable = null;
        private GridView gridView1;
        /// <summary>
        /// 列信息记录表
        /// </summary>
        [Category("自定义"), Description("ColumnInfoTable,列信息记录表"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [McDisplayName("ColumnInfoTable")]
        public DataTable ColumnInfoTable
        {
            get
            {
                return this._ColumnInfoTable;
            }
            set
            {
                this._ColumnInfoTable = value;
            }
        }

        #endregion

        #region 列的下拉数据源

        private Dictionary<string, BindingSource> _ColumnDataSourceDictionary = new Dictionary<string, BindingSource>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 列的下拉数据源
        /// </summary>
        [Category("自定义"), Description("ColumnDataSourceDictionary,列的下拉数据源"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [McDisplayName("ColumnDataSourceDictionary")]
        public Dictionary<string, BindingSource> ColumnDataSourceDictionary
        {
            get
            {
                return this._ColumnDataSourceDictionary;
            }
            set
            {
                this._ColumnDataSourceDictionary = value;
            }
        }

        #endregion

        /// <summary>
        /// 提示信息
        /// </summary>
        [Category("汽泡提示"), Description("ToolTipText,提示信息"), Browsable(true)]
        [McDisplayName("ToolTipText")]
        public virtual string ToolTipText
        {
            get
            {
                return this._ToolTip.GetToolTip(this);
            }
            set
            {
                this._ToolTip.SetToolTip(this, value);
            }
        }

        private string _ToolTipMessageCode = string.Empty;
        /// <summary>
        /// 提示多语言标识
        /// </summary>
        [Category("汽泡提示"), Description("ToolTipMessageCode,提示信息多语言标识"), Browsable(true)]
        [McDisplayName("ToolTipMessageCode")]
        public virtual string ToolTipMessageCode
        {
            get
            {
                return this._ToolTipMessageCode;
            }
            set
            {
                this._ToolTipMessageCode = value;
            }
        }

        /// <summary>
        /// 设置列的密码显示字符
        /// </summary>
        /// <param name="fieldName">列名</param>
        /// <param name="passwordChar">密码显示字符</param>
        public virtual void SetPasswordChar(string fieldName, Char passwordChar)
        {
            GridView gv = null;
            GridColumn column = null;
            RepositoryItemTextEdit textedit = null;
            gv = this.MainView as GridView;
            if (gv != null)
            {
                column = gv.Columns.ColumnByFieldName(fieldName);
                if (column.ColumnEdit != null)
                {
                    textedit = column.ColumnEdit as RepositoryItemTextEdit;
                    if (textedit != null)
                    {
                        textedit.PasswordChar = passwordChar;
                    }
                }
            }
        }

        /// <summary>
        /// 设置数据绑定
        /// </summary>
        /// <param name="binding">数据绑定对象</param>
        /// <return>int</return>
        public int SetDataBinding(object binding)
        {
            PropertyInfo pi = null;
            DataTable table = null;
            GridView gv = null;
            GridColumn column = null;

            this.DataSource = binding;
            if (binding is BindingSource)
            {
                int maxlength = 0;
                if (((BindingSource)binding).DataSource is DataView)
                {
                    table = ((DataView)(((BindingSource)binding).DataSource)).Table;
                    gv = this.MainView as GridView;
                    for (int i = 0; i < gv.Columns.Count; i++)
                    {
                        column = gv.Columns[i];
                        if (table.Columns.Contains(column.FieldName) == true)
                        {
                            if (table.Columns[column.FieldName].DataType == typeof(string))
                            {
                                maxlength = table.Columns[column.FieldName].MaxLength;
                                if (maxlength >= 0)
                                {
                                    if (column.ColumnEdit != null)
                                    {
                                        pi = column.ColumnEdit.GetType().GetProperty("MaxLength");
                                        if (pi != null)
                                        {
                                            pi.SetValue(column.ColumnEdit, maxlength, null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (((BindingSource)binding).DataSource is DataTable)
                {
                    table = ((BindingSource)binding).DataSource as DataTable;
                    gv = this.MainView as GridView;
                    for (int i = 0; i < gv.Columns.Count; i++)
                    {
                        column = gv.Columns[i];
                        if (table.Columns[column.FieldName].DataType == typeof(string))
                        {
                            maxlength = table.Columns[column.FieldName].MaxLength;
                            if (maxlength >= 0)
                            {
                                if (column.ColumnEdit != null)
                                {
                                    pi = column.ColumnEdit.GetType().GetProperty("MaxLength");
                                    if (pi != null)
                                    {
                                        pi.SetValue(column.ColumnEdit, maxlength, null);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //设置网格中的数据值显示格式与列标题居中,数字类型数据居右
            this.SetColumnDisplayFormat();
            return 1;
        }

        private void OnSetDataBinding(object binding)
        {
            this.DataSource = binding;
        }


        /// <summary>
        /// 设置下拉框的数据源
        /// </summary>
        /// <param name="binding">下拉框的数据源</param>
        /// <param name="displayMember">显示值字段名</param>
        /// <param name="valueMember">实际值字段名</param>
        /// <returns>int</returns>
        public int SetSourceTableBinding(object binding, string displayMember, string valueMember)
        {
            return 1;
        }

        /// <summary>
        /// 取控件的值
        /// </summary>
        /// <return>Object</return>
        public object GetValue()
        {
            return null;
        }

        /// <summary>
        /// 设置控件的值
        /// </summary>
        /// <param name="value">控件的值</param>
        /// <return>int</return>
        public int SetValue(object value)
        {
            return 1;
        }

        /// <summary>
        /// 还原默认值
        /// </summary>
        /// <return>void</return>
        public void SetDefaultValue()
        {

        }

        public virtual void SetLayout()
        {
            TableLayoutPanel panel = this.Parent as TableLayoutPanel;
            if (panel != null)
            {
                panel.SetRowSpan(this, this._LayoutRowSpan);
                panel.SetColumnSpan(this, this._LayoutColumnSpan);
            }
        }

        /// <summary> 全选后 </summary>
        public event EventHandler AllChooseSetted;

        /// <summary> 全消后 </summary>
        public event EventHandler AllChooseCleared;

        /// <summary>
        /// 单元格值改变前事件 （防止多级标题事件被抹掉）
        /// </summary>
        public event CellValueChangedEventHandler CellValueChanging;
        /// <summary>
        /// 单元格值改变后事件 （防止多级标题事件被抹掉）
        /// </summary>
        public event CellValueChangedEventHandler CellValueChanged;

        /// <summary> 控件事件 </summary>
        public event KzxControlOperateEventHandler KzxControlOperate;

        /// <summary>
        /// 获取多语言文本事件
        /// </summary>
        public event KzxGetLanguageEventHandler KzxGetLanguage;
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new BaseView MainView
        {
            get
            {
                return base.MainView;
            }
            set
            {
                if (base.MainView != value && value != null)
                {
                    var view = (value as GridView);
                    view.CellValueChanged -= new CellValueChangedEventHandler(OnCellValueChanged);
                    view.CellValueChanged += new CellValueChangedEventHandler(OnCellValueChanged);
                    view.CellValueChanging -= new CellValueChangedEventHandler(OnCellValueChanging);
                    view.CellValueChanging += new CellValueChangedEventHandler(OnCellValueChanging);
                }
                base.MainView = value;
            }
        }

        private DataTable _PluginInfoTable = KzxBaseControl.CreatePluginDataTable();
        [Category("自定义"), Description("PluginInfoTable,事件插件信息表"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [McDisplayName("PluginInfoTable")]
        public DataTable PluginInfoTable
        {
            get
            {
                return this._PluginInfoTable;
            }
            set
            {
                this._PluginInfoTable = value;
            }
        }

        private bool _ColumnAutoWidth = false;

        [Category("外观"), Description("ColumnAutoWidth,列自动设置宽度"), Browsable(true)]
        [McDisplayName("ColumnAutoWidth")]
        public bool ColumnAutoWidth
        {
            get
            {
                return this._ColumnAutoWidth;
            }
            set
            {
                this._ColumnAutoWidth = value;
                if (this.MainView != null)
                {
                    GridView view = this.MainView as GridView;
                    if (view != null)
                    {
                        view.OptionsView.ColumnAutoWidth = value;
                    }
                }
            }
        }

        private bool _CanAutoWidth = false;
        [Category("外观"), Description("CanAutoWidth,列宽自适应"), Browsable(true)]
        [McDisplayName("CanAutoWidth")]
        public bool CanAutoWidth
        {
            get
            {
                return this._CanAutoWidth;
            }
            set
            {
                this._CanAutoWidth = value;
            }
        }

        /// <summary>
        /// 构造
        /// </summary>
        public KzxGridControl()
            : base()
        {
            if (this.MainView == null)
            {
                this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
                ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
                this.SuspendLayout();
                this.gridView1.GridControl = this;
                this.gridView1.Name = "gridView1";
                this.gridView1.VertScrollVisibility = ScrollVisibility.Always;
                this.gridView1.HorzScrollVisibility = ScrollVisibility.Always;
                this.MainView = this.gridView1;
                this.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
                ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
                this.ResumeLayout(false);
            }

            this._KzxBandedGridView = new KzxBandedGridView(false, _bandedGridView, this);
            this.Load += new EventHandler(GridControl_Load);
        }

        /// <summary>
        /// 是否触发验证事件
        /// </summary>
        /// <param name="e">true触发，false不触发</param>
        public void SetCausesValidation(bool e)
        {
            this.CausesValidation = e;
        }

        #region 复制行


        public void CopyLine(object sender, EventArgs e)
        {
            StringBuilder sqlsb = new StringBuilder();
            GridView gridview = this.MainView as GridView;
            DataTable dt = null;
            DataColumn[] columns = null;
            BindingSource bs = null;
            DataRow row = null;
            DataRow rowtemp = null;

            bs = (this.DataSource as BindingSource);
            if (bs != null)
            {
                if (gridview.FocusedRowHandle >= 0)
                {
                    row = gridview.GetDataRow(gridview.FocusedRowHandle);
                    dt = (bs.List as DataView).Table;
                    if (dt.PrimaryKey != null)
                    {
                        columns = dt.PrimaryKey;
                    }

                    DataTable dttemp = dt.Clone();
                    rowtemp = dttemp.Rows.Add(row.ItemArray);
                    foreach (DataColumn c in columns)
                    {
                        rowtemp[c.ColumnName] = Guid.NewGuid();
                    }
                    //判断iOrder是否存在，存在则不复制，且取最大值+1
                    if (dt.Columns.Contains("iOrder"))
                    {
                        int maxIOrder = string.IsNullOrWhiteSpace(dt.Compute("max(iOrder)", "1=1").ToString()) ? 0 : Convert.ToInt32(dt.Compute("max(iOrder)", "1=1").ToString());
                        rowtemp["iOrder"] = maxIOrder + 1;
                    }
                    dt.ImportRow(rowtemp);

                    //执行右键复制行委托
                    OnRowCopy?.Invoke();
                }
            }
            else
            {
                KzxMessageBox.Show(sysClass.ssLoadMsgOrDefault("MSG002204", "数据源不能为空"));
            }
        }

        #endregion

        private void CopyLineMenu()
        {
            //是否显示右键的复制行按钮
            if (this._displayRightClickCopyMenu == true)
            {
                bool iscontain = false;
                ToolStripItem item3;
                if (this.ContextMenuStrip == null)
                {
                    this.ContextMenuStrip = new ContextMenuStrip();
                }
                this._ContextMenuStrip = this.ContextMenuStrip;
                for (int i = 0; i < this._ContextMenuStrip.Items.Count; i++)
                {
                    if (this._ContextMenuStrip.Items[i].Name.Equals("copyline", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        iscontain = true;
                        break;
                    }
                }
                if (iscontain == false)
                {
                    item3 = this._ContextMenuStrip.Items.Add(GetLanguage("RMI000452", "复制行"));//复制行
                    item3.Name = "copyline";
                    item3.Click += new EventHandler(CopyLine);
                }
            }
        } 


        private void GridControl_Load(object sender, EventArgs e)
        {
            ToolStripItem item;
            ToolStripItem item2;
            ToolStripItem item3;
            ToolStripItem item4;
            ToolStripItem item5;//20170103,lfx,清除网格状态

            if (this.ContextMenuStrip == null)
            {
                this.ContextMenuStrip = new ContextMenuStrip();
            }
            this._ContextMenuStrip = this.ContextMenuStrip;
            _ContextMenuStrip.Opened += _ContextMenuStrip_Opened;

            item = this._ContextMenuStrip.Items.Add(GetLanguage("MSG000688", "网格颜色设置"));//网格颜色设置
            item2 = this._ContextMenuStrip.Items.Add(GetLanguage("MSG000686", "网格状态设置"));//网格状态设置
            item5 = this._ContextMenuStrip.Items.Add(GetLanguage("MSG006992", "清除网格状态"));//20170103,lfx,清除网格状态


            item.Click += new EventHandler(this.ColorSetMenuItem1_Click_1);
            item2.Click += new EventHandler(this.FormatMenuItem1_Click_2);

            item5.Click += new EventHandler(this.ClearFormatMenuItem1_Click_3);//清除网格状态事件，lfx，20170103

            //如果需要显示右键复制行
            if (_displayRightClickCopyMenu)
            {
                item3 = this._ContextMenuStrip.Items.Add(GetLanguage("RMI000452", "复制行"));//复制行
                item3.Name = "copyline";
                item3.Click += new EventHandler(CopyLine);
            }
            //如果需要显示右键导出Excel
            if (_displayRightClickExportExcel)
            {
                item4 = this._ContextMenuStrip.Items.Add(GetLanguage("SYS001196", "导出EXCEL"));//导出EXCEL
                item4.Click += new EventHandler((s, ee) =>
                { 
                });
            }
            ToolSelectAll = this._ContextMenuStrip.Items.Add(GetLanguage("MSG000588", "全选"));//全选
            ToolSelectAll.Visible = false;
            ToolSelectAll.Click += ToolSelectAll_Click;

            ToolNotSelectAll = this._ContextMenuStrip.Items.Add(GetLanguage("MSG000589", "全消"));//全选
            ToolNotSelectAll.Visible = false;
            ToolNotSelectAll.Click += ToolNotSelectAll_Click;

            //this.ContextMenuStrip = this._ContextMenuStrip;
            this.Cursor = System.Windows.Forms.Cursors.Default;
            LayoutControl();
            BindingEvent(this, this.PluginInfoTable);

            if (this.CanAutoWidth == true)
            {
                ((GridView)this.MainView).BestFitColumns();
            }
        }

        /// <summary>
        /// 清除网格状态事件，lfx，20170103，如果是单据，则根据管理工具重新设置网格信息，如果是报表或者列表则网格将所有列设置为可将
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearFormatMenuItem1_Click_3(object sender, EventArgs e)
        {
            //删除网格状态设置（*.xml，StringFormat.ini）
            var path = Application.StartupPath + @"\Guid";
            var XmlFileName = string.Empty;
            if (!Directory.Exists(path))
            {
                return;
            }

            //存在网格状态设置文件则删除
            if (File.Exists(Application.StartupPath + @"\Guid\" + _ColorSet + this.Name + ".xml") == true)
            {
                File.Delete(Application.StartupPath + @"\Guid\" + _ColorSet + this.Name + ".xml");
            }

            //删除筛选设置（bFilter.ini）
            var filterConfig = new KzxGridDataFilterLocalConfig(_ColorSet + this.Name);
            filterConfig.SetOfClearGridStatus();
            filterConfig.Save();

            #region  重新加载管理工具中网格状态设置信息
            DataTable dt = null;
            bool isnull = true;
            bool isreadonly = false;

            this.ColumnInfoTable = KzxGridControl.SerializeColumns(this._Columns);//管理工具中当前网格列设置信息（针对单据）
            this.RefreshColumnInfoByModuleSetting();

            GridColumn gridColumn = null;

            for (int i = this.Views.Count - 1; i >= 0; i--)//设置网格标题显示信息
            {
                if (this.Views[i] is GridView)
                {
                    (this.Views[i] as GridView).OptionsView.ShowGroupPanel = this._ShowGroupPanel;
                }
            }
            GridView gridView = this.MainView as GridView;
            if (gridView != null)
            {
                if (this.DataSource is BindingSource)
                {
                    dt = ((this.DataSource as BindingSource).List as DataView).Table;
                }
                if (SysVar.FCurryFormInfo.sFormType == "frm" || SysVar.FCurryFormInfo.sFormType == "oth")//单据、自定义窗体
                {
                    #region  根据管理工具中当前网格已选列设置信息重新设置网格列显示
                    if (this.ColumnInfoTable != null && this.ColumnInfoTable.Select("Visible='True'").Length > 0)
                    {
                        this.ColumnInfoTable.DefaultView.Sort = " ColIndex desc";
                        //循环处理管理工具中当前网格列设置信息
                        for (int i = 0; i < this.ColumnInfoTable.DefaultView.ToTable().Select("Visible='True'").Length; i++)
                        {
                            DataRow dr = this.ColumnInfoTable.DefaultView.ToTable().Select("Visible='True'")[i];
                            //管理工具中设置列可见时，设置当前网格列可见
                            if (gridView.Columns[dr["Field"].ToString()] != null)
                            {
                                if (!gridView.Columns[dr["Field"].ToString()].Visible)//未显示的列
                                {
                                    gridView.Columns[dr["Field"].ToString()].Visible = true;
                                }
                            }
                        }
                    }
                } 
                #endregion
            }
            #endregion

            if (OnThrowCheckedFilteParam != null)
            {
                OnThrowCheckedFilteParam(new ThrowCheckFilterParamEventArgs(new List<CheckedFilterParam>()));
            }
        }

        /// <summary>
        /// 全不选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolNotSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                _isChooseAllEvent = true;
                GridView gv = (GridView)this.MainView;

                //modify by huangyq20170526 调整全选逻辑，仅选择树节点向下的所有行
                GridColumnReadOnlyCollection columnCollections = gv.GroupedColumns;
                if (columnCollections.Count == 0)
                {
                    //TODO:所有行全选
                    for (int i = 0; i < gv.DataRowCount; i++)
                    {
                        DataRowView drv = gv.GetRow(i) as DataRowView;
                        if (drv != null && Convert.ToBoolean(drv.Row["bChoose"].ToString()))
                        {
                            gv.SetRowCellValue(i, "bChoose", false);
                        }
                    }
                }
                else
                {
                    int focusedRowHandle = gv.FocusedRowHandle;
                    if (focusedRowHandle < 0)
                    {
                        object obj = gridView1.GetRow(gv.FocusedRowHandle);
                        if (obj != null)
                        {
                            //对所有的分组字段进行过滤，取相同组所有数据
                            DataRowView fucusedRowView = obj as DataRowView;

                            var lstRows = new List<DataRowView>(gv.DataRowCount);

                            //TODO:选中的是分组节点且 
                            int tempFocusedRowHandle = (focusedRowHandle * -1) - 1;

                            for (int i = 0; i < gv.DataRowCount; i++)
                            {
                                DataRowView tempRowView = gv.GetRow(i) as DataRowView;

                                bool isSame = true;
                                //与分组的所有列都相同
                                for (int k = 0; k < tempFocusedRowHandle + 1; k++)
                                {
                                    GridColumn groupColumn = columnCollections[k];

                                    if (groupColumn == null) continue;
                                    if (tempRowView.Row[groupColumn.FieldName].ToString() != fucusedRowView.Row[groupColumn.FieldName].ToString())
                                    {
                                        isSame = false;
                                        break;
                                    }
                                }

                                if (isSame)
                                {
                                    if (Convert.ToBoolean(tempRowView.Row["bChoose"]))
                                    {
                                        gv.SetRowCellValue(i, "bChoose", false);
                                        lstRows.Add(tempRowView);
                                    }
                                }
                            }

                            gv.SetRowsCellValueExt(lstRows, Identities.bChoose, false);
                        }
                    }
                    else
                    {
                        //TODO:选中的行
                        object obj = gridView1.GetRow(gv.FocusedRowHandle);
                        if (obj != null)
                        {
                            //对所有的分组字段进行过滤，取相同组所有数据
                            DataRowView fucusedRowView = obj as DataRowView;

                            var lstRows = new List<DataRowView>(gv.DataRowCount);

                            for (int i = 0; i < gv.DataRowCount; i++)
                            {
                                DataRowView tempRowView = gv.GetRow(i) as DataRowView;

                                bool isSame = true;
                                //与分组的所有列都相同
                                for (int k = 0; k < columnCollections.Count; k++)
                                {
                                    GridColumn groupColumn = columnCollections[k];

                                    if (tempRowView.Row[groupColumn.FieldName].ToString() != fucusedRowView.Row[groupColumn.FieldName].ToString())
                                    {
                                        isSame = false;
                                        break;
                                    }
                                }

                                if (isSame)
                                {
                                    if (Convert.ToBoolean(tempRowView.Row["bChoose"]))
                                    {
                                        gv.SetRowCellValue(i, "bChoose", false);
                                        lstRows.Add(tempRowView);
                                    }
                                }
                            }
                            gv.SetRowsCellValueExt(lstRows, Identities.bChoose, false);
                        }
                    }
                }
                //end by huangyq20170526
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
            finally
            {
                ChooseAllEndTrigger(false);
                _isChooseAllEvent = false;
            }
             

            //删除所有已选行记录信息，lfx，20170306,设置结束行行号
            lStartRow.Clear();
            EndRow = -1;
             
            AllChooseCleared?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolSelectAll_Click(object sender, EventArgs e)
        {

            var grid = (GridView)MainView;

            //modify by huangyq20170526 调整全选逻辑，仅选择树节点向下的所有行
            var columnCollections = grid.GroupedColumns;
            if (columnCollections.Count == 0)
            {
                //所有行全选
                grid.SetAllRowCellValueExt(Identities.bChoose, true);
            }
            else
            {
                var focusedRowHandle = grid.FocusedRowHandle;
                //点中的是分组
                if (focusedRowHandle < 0)
                {
                    var obj = gridView1.GetRow(grid.FocusedRowHandle);
                    if (obj != null)
                    {
                        //对所有的分组字段进行过滤，取相同组所有数据
                        var fucusedRowView = obj as DataRowView;

                        //选中的是分组节点且
                        var tempFocusedRowHandle = (focusedRowHandle * -1) - 1;

                        var lstRows = new List<DataRowView>(grid.DataRowCount);


                        for (var i = 0; i < grid.DataRowCount; i++)
                        {
                            var rowView = grid.GetRow(i) as DataRowView;

                            var isSame = true;
                            //与分组的所有列都相同
                            for (var k = 0; k < tempFocusedRowHandle + 1; k++)
                            {
                                var groupColumn = columnCollections[k];

                                if (groupColumn == null) continue;
                                if (rowView.Row[groupColumn.FieldName].ToString() !=
                                    fucusedRowView.Row[groupColumn.FieldName].ToString())
                                {
                                    isSame = false;
                                    break;
                                }
                            }

                            if (isSame)
                            {
                                if (rowView.Row[Identities.bChoose] is bool choosed && false == choosed)
                                {
                                    lstRows.Add(rowView);
                                }
                            }
                        }

                        grid.SetRowsCellValueExt(lstRows, Identities.bChoose, true);
                    }
                }
                else
                {
                    //选中的行
                    var obj = gridView1.GetRow(grid.FocusedRowHandle);
                    if (obj != null)
                    {
                        //对所有的分组字段进行过滤，取相同组所有数据
                        var fucusedRowView = obj as DataRowView;

                        var lstRows = new List<DataRowView>(grid.DataRowCount);


                        for (var i = 0; i < grid.DataRowCount; i++)
                        {
                            var rowView = grid.GetRow(i) as DataRowView;

                            var isSame = true;
                            //与分组的所有列都相同
                            for (var k = 0; k < columnCollections.Count; k++)
                            {
                                var groupColumn = columnCollections[k];

                                if (rowView.Row[groupColumn.FieldName].ToString() !=
                                    fucusedRowView.Row[groupColumn.FieldName].ToString())
                                {
                                    isSame = false;
                                    break;
                                }
                            }

                            if (isSame)
                            {
                                if (rowView.Row[Identities.bChoose] is bool choose && false == choose)
                                {
                                    lstRows.Add(rowView);
                                }
                            }
                        }

                        grid.SetRowsCellValueExt(lstRows, Identities.bChoose, true);
                    }
                }
            }
            //end by huangyq20170526

            AllChooseSetted?.Invoke(this, EventArgs.Empty);
        }

        private void ChooseAllEndTrigger(bool pIsChecked)
        {
            try
            {
                if (OnChooseAllEnd != null)
                {
                    OnChooseAllEnd(new KzxGridControlChooseAllEventArg()
                    {
                        IsChecked = pIsChecked,
                    });
                }
            }
            catch (Exception)
            {
                //记录日志
            }
        }

        void _ContextMenuStrip_Opened(object sender, EventArgs e)
        {
            GridView gv = (GridView)this.MainView;
            if (gv.Columns.ColumnByFieldName(Identities.bChoose) != null)
            {
                ToolSelectAll.Visible = true;
                ToolNotSelectAll.Visible = true;

                ToolSelectAll.Enabled = !ReadOnly;
                ToolNotSelectAll.Enabled = !ReadOnly;
            }
            else
            {
                ToolSelectAll.Visible = false;
                ToolNotSelectAll.Visible = false;
            }

            //GridView gv = (GridView)this.MainView;
            //if (gv != null && gv.FocusedColumn != null && gv.DataRowCount > 0 && gv.FocusedColumn.ColumnType == typeof(bool))
            //{
            //    ToolSelectAll.Visible = true;
            //    ToolNotSelectAll.Visible = true;

            //    ToolSelectAll.Enabled = !ReadOnly;
            //    ToolNotSelectAll.Enabled = !ReadOnly;

            //}
            //else
            //{
            //    ToolSelectAll.Visible = false;
            //    ToolNotSelectAll.Visible = false;
            //}
        }

        /// <summary>
        /// 创建列
        /// </summary>
        public void InitGridControl()
        {
            CreateColumnByColumnInfo();
        }

        /// <summary>
        /// 初始化网格
        /// </summary>
        public void LayoutControl()
        {
            try
            {
                string filepath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Images");
                string imagefile = string.Empty;
                GridView view = this.MainView as GridView;

                //屏蔽，由于重复调用 By Chen.Q 2018-05-30
                //this.ColumnInfoTable = KzxGridControl.SerializeColumns(this.Columns);
                InitGridControl();

                if (view != null)
                {
                    try
                    {
                        view.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(GridColorA));
                        view.OptionsView.EnableAppearanceOddRow = true;
                        //this.gridView.Appearance.EvenRow.BackColor = Color.White;
                        view.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(GridColorB));
                        view.OptionsView.EnableAppearanceEvenRow = true;
                        this.AllowDrop = true; // 确保能够拖拽
                    }
                    catch
                    {
                        view.Appearance.OddRow.BackColor = Color.AntiqueWhite;
                        view.OptionsView.EnableAppearanceOddRow = true;
                        view.Appearance.EvenRow.BackColor = Color.White;
                        view.OptionsView.EnableAppearanceEvenRow = true;
                    }
                    this._ReadOnlyInit = 1;
                    view.OptionsBehavior.ReadOnly = this._ReadOnly;
                    view.VertScrollVisibility = ScrollVisibility.Always;
                    view.HorzScrollVisibility = ScrollVisibility.Always;
                    view.ScrollStyle = ScrollStyleFlags.LiveHorzScroll | ScrollStyleFlags.LiveVertScroll;
                    view.IndicatorWidth = 40;
                    this.UseEmbeddedNavigator = this.KzxUseEmbeddedNavigator;
                    this.buttonimages.ImageSize = new Size(16, 16);
                    this.buttonimages.TransparentColor = System.Drawing.Color.Transparent;

                    imagefile = Path.Combine(filepath, "add.png");
                    if (File.Exists(imagefile) == true)
                    {
                        this.buttonimages.Images.Add(Image.FromFile(imagefile));
                    }
                    imagefile = Path.Combine(filepath, "del.png");
                    if (File.Exists(imagefile) == true)
                    {
                        this.buttonimages.Images.Add(Image.FromFile(imagefile));
                    }
                    imagefile = Path.Combine(filepath, "first.png");
                    if (File.Exists(imagefile) == true)
                    {
                        this.buttonimages.Images.Add(Image.FromFile(imagefile));
                    }
                    imagefile = Path.Combine(filepath, "previous.png");
                    if (File.Exists(imagefile) == true)
                    {
                        this.buttonimages.Images.Add(Image.FromFile(imagefile));
                    }
                    imagefile = Path.Combine(filepath, "next.png");
                    if (File.Exists(imagefile) == true)
                    {
                        this.buttonimages.Images.Add(Image.FromFile(imagefile));
                    }
                    imagefile = Path.Combine(filepath, "last.png");
                    if (File.Exists(imagefile) == true)
                    {
                        this.buttonimages.Images.Add(Image.FromFile(imagefile));
                    }
                    imagefile = Path.Combine(filepath, "edit.png");
                    if (File.Exists(imagefile) == true)
                    {
                        this.buttonimages.Images.Add(Image.FromFile(imagefile));
                    }
                    imagefile = Path.Combine(filepath, "ok.png");
                    if (File.Exists(imagefile) == true)
                    {
                        this.buttonimages.Images.Add(Image.FromFile(imagefile));
                    }
                    imagefile = Path.Combine(filepath, "cancel.png");
                    if (File.Exists(imagefile) == true)
                    {
                        this.buttonimages.Images.Add(Image.FromFile(imagefile));
                    }

                    this.EmbeddedNavigator.Buttons.ImageList = this.buttonimages;
                    this.EmbeddedNavigator.Buttons.Append.ImageIndex = 0;
                    this.EmbeddedNavigator.Buttons.Remove.ImageIndex = 1;
                    this.EmbeddedNavigator.Buttons.First.ImageIndex = 2;
                    this.EmbeddedNavigator.Buttons.Prev.ImageIndex = 3;
                    this.EmbeddedNavigator.Buttons.Next.ImageIndex = 4;
                    this.EmbeddedNavigator.Buttons.Last.ImageIndex = 5;
                    this.EmbeddedNavigator.Buttons.Edit.ImageIndex = 6;
                    this.EmbeddedNavigator.Buttons.EndEdit.ImageIndex = 7;
                    this.EmbeddedNavigator.Buttons.CancelEdit.ImageIndex = 8;

                    this.EmbeddedNavigator.Buttons.PrevPage.Visible = false;
                    this.EmbeddedNavigator.Buttons.NextPage.Visible = false;
                    this.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
                    this.EmbeddedNavigator.Buttons.First.Visible = false;
                    this.EmbeddedNavigator.Buttons.Last.Visible = false;
                    this.EmbeddedNavigator.Buttons.Next.Visible = false;
                    this.EmbeddedNavigator.Buttons.Prev.Visible = false;
                    this.EmbeddedNavigator.Buttons.Edit.Visible = false;
                    //this.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
                    this.EmbeddedNavigator.TextLocation = NavigatorButtonsTextLocation.None;

                    view.CustomDrawCell += new RowCellCustomDrawEventHandler(gridView1_CustomDrawCell);
                    view.CustomDrawRowIndicator += new RowIndicatorCustomDrawEventHandler(gridView1_CustomDrawRowIndicator);

                    //view.OptionsView.ShowGroupPanel = false;

                    view.OptionsView.ColumnAutoWidth = false;
                    this.CanAutoWidth = false;
                    view.OptionsView.ShowGroupPanel = this._ShowGroupPanel;
                    view.OptionsView.ShowAutoFilterRow = this._ShowAutoFilterRow;

                    if (CellValueChanged != null)
                    {
                        view.CellValueChanged -= new CellValueChangedEventHandler(CellValueChanged);
                        view.CellValueChanged += new CellValueChangedEventHandler(CellValueChanged);

                    }
                    if (CellValueChanging != null)
                    {
                        view.CellValueChanging -= new CellValueChangedEventHandler(CellValueChanging);
                        view.CellValueChanging += new CellValueChangedEventHandler(CellValueChanging);
                    }

                    view.FocusedRowChanged -= new FocusedRowChangedEventHandler(GridViewFocuseRowChanged);
                    view.FocusedRowChanged += new FocusedRowChangedEventHandler(GridViewFocuseRowChanged);

                    view.FocusedColumnChanged -= new FocusedColumnChangedEventHandler(GridViewFocusedColumnChanged);
                    view.FocusedColumnChanged += new FocusedColumnChangedEventHandler(GridViewFocusedColumnChanged);

                    view.CustomDrawFooterCell -= new FooterCellCustomDrawEventHandler(GridViewCustomDrawFooterCell);
                    view.CustomDrawFooterCell += new FooterCellCustomDrawEventHandler(GridViewCustomDrawFooterCell);

                    view.ShowingEditor -= new CancelEventHandler(UserShowingEditor);
                    view.ShowingEditor += new CancelEventHandler(UserShowingEditor);

                    view.ShownEditor -= new EventHandler(UserShownEditor);
                    view.ShownEditor += new EventHandler(UserShownEditor);

                    view.ValidatingEditor -= new BaseContainerValidateEditorEventHandler(GridViewValidatingEditor);
                    view.ValidatingEditor += new BaseContainerValidateEditorEventHandler(GridViewValidatingEditor);

                    view.MouseDown -= new MouseEventHandler(GridViewMouseDown);
                    view.MouseDown += new MouseEventHandler(GridViewMouseDown);

                    view.MouseMove -= new MouseEventHandler(GridViewMouseMove);
                    view.MouseMove += new MouseEventHandler(GridViewMouseMove);



                    this.DragOver -= new DragEventHandler(GridControlDragOver);
                    this.DragOver += new DragEventHandler(GridControlDragOver);

                    this.DragDrop -= new DragEventHandler(GridControlDragDrop);
                    this.DragDrop += new DragEventHandler(GridControlDragDrop);


                }
                this.EmbeddedNavigator.ButtonClick -= new NavigatorButtonClickEventHandler(OnEmbeddedNavigator_ButtonClick);
                this.EmbeddedNavigator.ButtonClick += new NavigatorButtonClickEventHandler(OnEmbeddedNavigator_ButtonClick);

                CopyLineMenu();
                if (DesignMode == true)
                {
                    SetLanguage();
                }

                //触发事件
                RaiseEvent(this, "KzxGridControlLoad", new EventArgs());
            }
            catch (Exception ex)
            {
                string aa = ex.Message;
            }
        }

        /// <summary>
        /// 刷新表格列信息，通过模块设置
        /// </summary>
        private void RefreshColumnInfoByModuleSetting()
        {
            if (this._ColumnInfoTable == null)
                return; 

            this._ColumnInfoTable.DefaultView.Sort = " ColIndex ASC";
        }

        /// <summary>
        /// 鼠标按下事件,lfx,20170306
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewKeyUp(object sender, KeyEventArgs e)
        {
            if (this.MainView != null)
            {
                GridView gv = sender as GridView;
                if (this.MainView != null)
                {
                    //GridView gv = sender as GridView;
                    if (gv.GetSelectedRows().Length > 0 && e.KeyData == Keys.ShiftKey)
                    {
                        int endrow = gv.GetFocusedDataSourceRowIndex();

                        if (lStartRow != null)
                        {
                            if (lStartRow.Count > 0)
                            {
                                if (lStartRow.Last<int>() <= endrow)
                                {

                                    //正序选时
                                    for (int x = lStartRow.Last<int>(); x <= endrow; x++)
                                    {

                                        gv.SetRowCellValue(x, "bChoose", 1);
                                    }
                                }
                                else
                                {

                                    //倒序选时
                                    for (int x = endrow; x <= lStartRow.Last<int>(); x++)
                                    {
                                        gv.SetRowCellValue(x, "bChoose", 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            GridView gv = this.MainView as GridView;
            gv.PostEditor();
            gv.UpdateCurrentRow();

        }

        //鼠标按下事件
        private void GridViewMouseDown(object sender, MouseEventArgs e)
        {
            this.AllowDrop = true; // 确保能够拖拽
            GridView gv = sender as GridView;
            downHitInfo = gv.CalcHitInfo(new Point(e.X, e.Y));   //鼠标左键按下去时在GridView中的坐标
            if (downHitInfo == null || downHitInfo.RowHandle < 0)
            {
                return;   //判断鼠标的位置是否有效
            }

            if (gv.GetSelectedRows().Length > 0 && (Control.ModifierKeys == Keys.Shift))
            {
                int CurrenEndRow = downHitInfo.RowHandle;

                if (lStartRow != null)
                {
                    if (lStartRow.Count > 0)
                    {
                        if (lStartRow.Last<int>() <= CurrenEndRow)
                        {

                            //正序选时
                            for (int x = lStartRow.Last<int>(); x <= CurrenEndRow; x++)
                            {

                                gv.SetRowCellValue(x, "bChoose", 1);
                                //}
                                //if (EndRow != -1)
                                //{
                                //    //取消之前已选的
                                //    if (EndRow > CurrenEndRow)
                                //    {
                                //        for (int x = CurrenEndRow + 1; x <= EndRow; x++)
                                //        {

                                //            gv.SetRowCellValue(x, "bChoose", 0);
                                //            lStartRow.Remove(x);
                                //        }
                                //    }
                                //    if (EndRow < lStartRow.Last<int>())
                                //    {
                                //        for (int x = EndRow; x < lStartRow.Last<int>(); x++)
                                //        {
                                //            gv.SetRowCellValue(x, "bChoose", 0);
                                //            lStartRow.Remove(x);
                                //        }
                                //    }
                                //}
                                //EndRow = CurrenEndRow;//记录当已前选中行号
                            }
                        }
                        else
                        {

                            //倒序选时
                            for (int x = CurrenEndRow; x <= lStartRow.Last<int>(); x++)
                            {
                                gv.SetRowCellValue(x, "bChoose", 1);
                                //}
                                //if (EndRow != -1)
                                //{
                                //    //取消之前已选的
                                //    if (EndRow < CurrenEndRow)
                                //    {
                                //        for (int x = EndRow; x < CurrenEndRow; x++)
                                //        {
                                //            gv.SetRowCellValue(x, "bChoose", 0);
                                //            lStartRow.Remove(x);
                                //        }
                                //    }
                                //    if (EndRow > lStartRow.Last<int>())
                                //    {
                                //        for (int x = lStartRow.Last<int>()+1; x <= EndRow; x++)
                                //        {
                                //            gv.SetRowCellValue(x, "bChoose", 0);
                                //            lStartRow.Remove(x);
                                //        }
                                //    }
                                //}
                                //EndRow = CurrenEndRow;//记录当已前选中行号
                            }
                        }
                    }
                }
            }
        }

        //鼠标移动事件
        private void GridViewMouseMove(object sender, MouseEventArgs e)
        {
            DataRow row = null;
            List<DataRow> rowList = new List<DataRow>();
            if (e.Button != MouseButtons.Left) return;        //不是左键则无效
            if (downHitInfo == null || downHitInfo.RowHandle < 0) return;   //判断鼠标的位置是否有效

            GridView gv = sender as GridView;
            rows = gv.GetSelectedRows();  //获取所选行的index
            startRow = rows.Length == 0 ? -1 : rows[0];

            foreach (int r in rows)   // 根据所选行的index进行取值，去除所选的行数据，可能是选取的多行
            {
                row = gv.GetDataRow(r);
                if (row != null)
                {
                    rowList.Add(row);
                }
            }
            this.DoDragDrop(rowList, DragDropEffects.Move);//开始拖放操作，将拖拽的数据存储起来
        }

        //拖拽过程事件
        private void GridControlDragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        //拖拽完成后事件
        private void GridControlDragDrop(object sender, DragEventArgs e)
        {
            int targetRowHandle = -1;
            GridView gv = this.MainView as GridView;
            List<DataRow> rowList = new List<DataRow>();
            Point gridviewPoint = this.PointToScreen(this.Location);  //获取鼠标在屏幕上的位置。
            upHitInfo = gv.CalcHitInfo(new Point(e.X - gridviewPoint.X, e.Y - gridviewPoint.Y));   //鼠标左键弹起来时在GridView中的坐标（屏幕位置减去 gridView 开始位置）
            if (upHitInfo == null || upHitInfo.RowHandle < 0) return;
            targetRowHandle = upHitInfo.RowHandle; //获取拖拽的目标行index

            rowList = e.Data.GetData(typeof(List<DataRow>)) as List<DataRow>;

            KzxDragEventArgs args = new KzxDragEventArgs();
            args.DragEventArgs = e;
            args.TargetRowHandle = targetRowHandle;
            args.SelectedRowList = rowList;

            this.RaiseEvent(sender, "DragOver", args);

        }

        private void SetLanguageDesign()
        {
            string s = string.Empty;
            GridColumn column = null;
            int iCnt = (this.MainView as GridView).Columns.Count;
            Dictionary<string, string> bandedDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            for (int j = 0; j < iCnt; j++)
            {
                column = (this.MainView as GridView).Columns[j];
                DataRow[] rows = this.ColumnInfoTable.Select("Field='" + column.FieldName + "'");
                if (rows.Length > 0)
                {
                    s = GetLanguage(rows[0]["MessageCode"].ToString(), rows[0]["DesigeCaption"].ToString());
                    // if (!bandedDictionary.ContainsValue(column.FieldName))
                    bandedDictionary.Add(column.FieldName, s);
                    column.Caption = GetColumnCaption(s);
                }
            }
            if (this.IsBandedGridView == true)
            {
                this._KzxBandedGridView.Data = bandedDictionary;
                this._KzxBandedGridView.CreateBandedGridView();
            }
        }

        private void OnEditorKeyDown(object sender, KeyEventArgs e)
        {
            this.RaiseEvent(sender, "EditorKeyDown", e);
        }

        private void OnEditorKeyPress(object sender, KeyPressEventArgs e)
        {
            this.RaiseEvent(sender, "EditorKeyPress", e);
        }

        private void OnEditorKeyUp(object sender, KeyEventArgs e)
        {
            RepositoryItemKeyDown(sender, e);
            this.RaiseEvent(sender, "EditorKeyUp", e);
        }

        private void RepositoryItemClick(object sender, EventArgs e)
        {
            GridView view = this.MainView as GridView;
            if (view != null)
            {
                this.CurrentRowIndex = view.FocusedRowHandle;
            }

            this.RaiseEvent(sender, "CellClick", e);
        }

        private void RepositoryItemDoubleClick(object sender, EventArgs e)
        {
            this.RaiseEvent(sender, "CellDoubleClick", e);
        }

        private void RepositoryItemEnter(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit t = sender as DevExpress.XtraEditors.TextEdit;
            if (t != null)
            {
                t.SelectAll();
                t.Tag = true;
            }
        }

        private void RepositoryItemMouseUp(object sender, MouseEventArgs e)
        {
            DevExpress.XtraEditors.TextEdit t = sender as DevExpress.XtraEditors.TextEdit;
            Boolean b = true;
            if (t != null)
            {
                if (Boolean.TryParse(t.Tag.ToString(), out b) == true)
                {
                    if (b == true)
                    {
                        t.SelectAll();
                    }
                }
            }
            t.Tag = false;
        }

        private void RepositoryTextEditItemDoubleClick(object sender, EventArgs e)
        {  
        }

        private void RepositoryMemoEditItemDoubleClick(object sender, EventArgs e)
        {
            DataRow[] rows = null;
            string filterstring = string.Empty;
            BindingSource bs = null;
            GridView view = this.MainView as GridView;
            bool canedit = true;

            if (view != null)
            {
                //弹出备注文本
                if (this.DataSource != null)
                {
                    bs = this.DataSource as BindingSource;
                    if (bs != null)
                    {
                        if (bs.AllowEdit == false)
                        {
                            canedit = false;
                        }
                    }
                }
                if (this.ColumnDataSourceDictionary.Keys.Contains(view.FocusedColumn.FieldName) == true)
                {
                    rows = this.ColumnInfoTable.Select("Field='" + view.FocusedColumn.FieldName + "'", " ColIndex ASC ", DataViewRowState.CurrentRows);
                    if (rows.Length > 0)
                    {
                        filterstring = rows[0]["FilterString"] == DBNull.Value || rows[0]["FilterString"] == null ? string.Empty : rows[0]["FilterString"].ToString();
                    }
                    if (!canedit)
                        return; 
                } 
            }
        }

        private void RepositoryItemKeyDown(object sender, KeyEventArgs e)
        {
            BindingSource bs = null;
            GridView view = this.MainView as GridView;
            string currenttext = string.Empty;
            PropertyInfo pi = null;

            e.Handled = true;
            if (view != null)
            {
                if (this.ColumnDataSourceDictionary.Keys.Contains(view.FocusedColumn.FieldName) == true)
                {
                    if (this.DataSource != null)
                    {
                        bs = this.DataSource as BindingSource;
                        if (bs != null)
                        {
                            if (bs.AllowEdit == false)
                            {
                                return;
                            }
                        }
                    }
                     
                }
            }
            this.RaiseEvent(sender, "CellKeyDown", e);
        }

        private void RepositoryItemButtonClick(object sender, EventArgs e)
        {
            this.RaiseEvent(sender, "CellButtonClick", e);
        }

        private void RepositoryItemQueryPopUp(object sender, CancelEventArgs e)
        {
            //下拉框过滤数据
            string filterstring = string.Empty;
            DataRow[] rows;
            string[] expressionarray = null;
            string[] filterarray = null;
            string[] valuearray = null;
            string s = string.Empty;
            StringBuilder sb = new StringBuilder();
            StringBuilder expressionsb = new StringBuilder();
            GridView view = this.MainView as GridView;

            if (view != null)
            {
                if (this.ColumnDataSourceDictionary.ContainsKey(view.FocusedColumn.FieldName) == true)
                {
                    rows = this.ColumnInfoTable.Select("Field='" + view.FocusedColumn.FieldName + "'", " ColIndex ASC ", DataViewRowState.CurrentRows);
                    if (rows.Length > 0)
                    {

                        this.RaiseEvent(sender, "QueryLoadData", e);

                        filterstring = rows[0]["FilterString"] == DBNull.Value || rows[0]["FilterString"] == null ? string.Empty : rows[0]["FilterString"].ToString();
                        if (string.IsNullOrWhiteSpace(filterstring) == false)
                        {
                            if (filterstring.IndexOf("|") >= 0)
                            {
                                expressionarray = filterstring.Split(new char[] { ',' });
                                for (int i = 0; i < expressionarray.Length; i++)
                                {
                                    sb.Clear();
                                    if (string.IsNullOrWhiteSpace(expressionarray[i]) == false)
                                    {
                                        filterarray = expressionarray[i].Split(new string[] { "=", ">=", "<=", "<>", ">", "<", }, StringSplitOptions.None);
                                        if (filterarray.Length >= 2)
                                        {
                                            for (int h = 0; h < 1; h++)
                                            {
                                                sb.Append("Convert(" + filterarray[0] + ",'System.String')");
                                                s = expressionarray[i].Substring(filterarray[0].Length);
                                                if (s.StartsWith("=") == true)
                                                {
                                                    sb.Append("=");
                                                }
                                                else if (s.StartsWith(">=") == true)
                                                {
                                                    sb.Append(">=");
                                                }
                                                else if (s.StartsWith("<=") == true)
                                                {
                                                    sb.Append("<=");
                                                }
                                                else if (s.StartsWith("<>") == true)
                                                {
                                                    sb.Append("<>");
                                                }
                                                else if (s.StartsWith(">") == true)
                                                {
                                                    sb.Append(">");
                                                }
                                                else if (s.StartsWith("<") == true)
                                                {
                                                    sb.Append("<");
                                                }
                                                else
                                                {
                                                    sb.Append("=");
                                                }
                                                valuearray = filterarray[1].Split(new char[] { '|' });
                                                if (valuearray.Length >= 2)
                                                {
                                                    if (this.BillBindingSourceDictionary.ContainsKey(valuearray[0]) == true)
                                                    {
                                                        if (this.Table.Equals(valuearray[0], StringComparison.OrdinalIgnoreCase) == false)
                                                        {
                                                            BindingSource ss = this.BillBindingSourceDictionary[valuearray[0]];
                                                            if (ss.Current != null)
                                                            {
                                                                sb.Append("'" + (((DataRowView)ss.Current)[valuearray[1]] == DBNull.Value ? string.Empty : ((DataRowView)ss.Current)[valuearray[1]].ToString()) + "'");
                                                            }
                                                            else
                                                            {
                                                                sb.Clear();
                                                                continue;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            BindingSource ss = this.DataSource as BindingSource;
                                                            if (ss != null)
                                                            {
                                                                if (ss.Current != null)
                                                                {
                                                                    sb.Append("'" + (((DataRowView)ss.Current)[valuearray[1]] == null || ((DataRowView)ss.Current)[valuearray[1]] == DBNull.Value ? string.Empty : ((DataRowView)ss.Current)[valuearray[1]].ToString()) + "'");
                                                                }
                                                                else
                                                                {
                                                                    sb.Clear();
                                                                    continue;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                sb.Clear();
                                                                continue;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        sb.Clear();
                                                        continue;
                                                    }
                                                }
                                                else
                                                {
                                                    sb.Append(sb + valuearray[0]);
                                                }
                                            }
                                        }
                                    }
                                    if (i == expressionarray.Length - 1)
                                    {
                                        expressionsb.Append(sb);
                                    }
                                    else
                                    {
                                        expressionsb.Append(sb + " and  ");
                                    }
                                }
                                if (expressionsb.Length > 0)
                                {
                                    this.ColumnDataSourceDictionary[view.FocusedColumn.FieldName].Filter = expressionsb.ToString();
                                }
                                else
                                {
                                    this.ColumnDataSourceDictionary[view.FocusedColumn.FieldName].RemoveFilter();
                                }
                            }
                            else
                            {
                                this.ColumnDataSourceDictionary[view.FocusedColumn.FieldName].Filter = filterstring;
                            }
                        }
                    }
                }
            }
            this.RaiseEvent(sender, "QueryPopUp", e);
        }

        private void RepositoryItemPopup(object sender, EventArgs e)
        {
            this.RaiseEvent(sender, "Popup", e);
            return;
        }

        private void RepositoryItemCloseUp(object sender, CloseUpEventArgs e)
        {
            this.RaiseEvent(sender, "CloseUp", e);
            //清除下拉框的过滤
            GridView view = this.MainView as GridView;
            if (view != null)
            {
                if (this.ColumnDataSourceDictionary.ContainsKey(view.FocusedColumn.FieldName) == true)
                {
                    this.ColumnDataSourceDictionary[view.FocusedColumn.FieldName].RemoveFilter();
                }
            }
        }

        private void RepositoryItemClosed(object sender, ClosedEventArgs e)
        {
            GridView view = this.MainView as GridView;
            if (view != null)
            {
                if (this.ColumnDataSourceDictionary.ContainsKey(view.FocusedColumn.FieldName) == true)
                {
                    this.ColumnDataSourceDictionary[view.FocusedColumn.FieldName].RemoveFilter();
                }
            }
            this.RaiseEvent(sender, "Closed", e);
        }

        /// <summary>
        /// 设置列的下拉数源
        /// </summary>
        /// <param name="field">列名</param>
        /// <param name="source">数据源</param>
        public void SetColumnDataSource(string field, BindingSource source)
        {
            PropertyInfo pi = null;
            GridColumn columnedit = null;
            GridColumn column = null;
            GridView view = null;
            DataView dataview = null;
            DataTable table = null;
            DataRow[] rows = null;

            if (this.MainView == null)
            {
                return;
            }
            if (source == null)
            {
                if (this.ColumnDataSourceDictionary.ContainsKey(field) == true)
                {
                    source = this.ColumnDataSourceDictionary[field];
                }
            }
            else
            {
                if (this.ColumnDataSourceDictionary.ContainsKey(field) == true)
                {
                    this.ColumnDataSourceDictionary[field] = source;
                }
                else
                {
                    this.ColumnDataSourceDictionary.Add(field, source);
                }
            }

            view = this.MainView as GridView;
            column = view.Columns.ColumnByFieldName(field);
            if (column != null)
            {
                if (column.ColumnEdit != null)
                {
                    pi = column.ColumnEdit.GetType().GetProperty("DataSource");
                    if (pi != null)
                    {
                        pi.SetValue(column.ColumnEdit, source, null);
                        if (column.ColumnEdit is RepositoryItemGridLookUpEdit)
                        {
                            ((RepositoryItemGridLookUpEdit)column.ColumnEdit).View.OptionsBehavior.AutoPopulateColumns = false;
                            dataview = source.List as DataView;
                            if (dataview != null)
                            {
                                table = dataview.Table;
                            }
                            if (this.ColumnInfoTable != null && this.ColumnInfoTable.Rows.Count > 0)
                            {
                                for (int i = 0; i < table.Columns.Count; i++)
                                {
                                    columnedit = new GridColumn();
                                    columnedit.FieldName = table.Columns[i].ColumnName;
                                    columnedit.Caption = table.Columns[i].ColumnName;
                                    columnedit.Visible = true;
                                    columnedit.VisibleIndex = i + 1;
                                    ((RepositoryItemGridLookUpEdit)(column.ColumnEdit)).View.Columns.Add(columnedit);

                                    rows = this.ColumnInfoTable.Select("Field='" + field + "'", " Field ASC", DataViewRowState.CurrentRows);
                                    if (rows.Length > 0)
                                    {
                                        if (rows[0]["FieldCaption"] != DBNull.Value)
                                        {
                                            if (string.IsNullOrWhiteSpace(rows[0]["FieldCaption"].ToString()) == false)
                                            {
                                                SetLanguage((RepositoryItemGridLookUpEdit)column.ColumnEdit, rows[0]["FieldCaption"].ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        pi = column.ColumnEdit.GetType().GetProperty("Items");
                        if (pi != null)
                        {
                            object v = pi.GetValue(column.ColumnEdit, null);
                            IList list = (IList)v;
                            dataview = source.List as DataView;
                            if (dataview != null)
                            {
                                table = dataview.Table;
                            }
                            rows = this.ColumnInfoTable.Select("Field='" + field + "'", " Field ASC", DataViewRowState.CurrentRows);
                            if (rows.Length > 0)
                            {
                                for (int i = 0; i < table.Rows.Count; i++)
                                {
                                    //list.Add(table.Rows[i][rows[0]["DisplayMemberPath"].ToString()]);
                                    list.Add(table.Rows[i][rows[0]["SelectedValuePath"].ToString()]);

                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 列的值的携带
        /// </summary>
        private void SetReferenceFieldValue(DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            DataRow[] rows = null;
            string[] fieldarray = null;
            string[] expressionarray = null;
            int rowindex = 0;
            GridView view = null;

            if (this.MainView == null)
            {
                return;
            }
            view = this.MainView as GridView;
            if (this.ColumnInfoTable == null)
            {
                this.ColumnInfoTable = KzxGridControl.SerializeColumns(this.Columns);
                this.RefreshColumnInfoByModuleSetting();
            }
            rows = this.ColumnInfoTable.Select("Field='" + e.Column.FieldName + "'", "Field ASC", DataViewRowState.CurrentRows);
            if (rows.Length > 0)
            {
                //if ((rows[0]["ColumnType"] == DBNull.Value || rows[0]["ColumnType"] == null ? string.Empty : rows[0]["ColumnType"].ToString()).Equals("NoSourceComboBox", StringComparison.OrdinalIgnoreCase) == true)
                //{
                //    //无数据源下拉框列不支持数据携带
                //    return;
                //}
                fieldarray = (rows[0]["ValueDependencyField"] == DBNull.Value || rows[0]["ValueDependencyField"] == null ? string.Empty : rows[0]["ValueDependencyField"].ToString()).Split(new char[] { ',' });
                if (fieldarray.Length > 0)
                {
                    if (this.ColumnDataSourceDictionary.ContainsKey(e.Column.FieldName) == true)
                    {
                        if (this.ColumnDataSourceDictionary[e.Column.FieldName] != null)
                        {
                            rowindex = this.ColumnDataSourceDictionary[e.Column.FieldName].Find(rows[0]["SelectedValuePath"].ToString(), e.Value);
                            for (int i = 0; i < fieldarray.Length; i++)
                            {
                                if (string.IsNullOrWhiteSpace(fieldarray[i]) == false)
                                {
                                    expressionarray = fieldarray[i].Split(new char[] { '=' });
                                    if (expressionarray.Length == 2)
                                    {
                                        if (string.IsNullOrWhiteSpace(expressionarray[0]) == false && string.IsNullOrWhiteSpace(expressionarray[1]) == false)
                                        {
                                            if (rowindex >= 0)
                                            {
                                                PropertyInfo pi = this.ColumnDataSourceDictionary[e.Column.FieldName][rowindex].GetType().GetProperty("Item", new Type[] { typeof(string) });
                                                if (pi != null)
                                                {
                                                    view.SetRowCellValue(e.RowHandle, expressionarray[0], pi.GetValue(this.ColumnDataSourceDictionary[e.Column.FieldName][rowindex], new object[] { expressionarray[1] }));
                                                }
                                                else
                                                {
                                                    view.SetRowCellValue(e.RowHandle, expressionarray[0], null);
                                                }
                                            }
                                            else
                                            {
                                                view.SetRowCellValue(e.RowHandle, expressionarray[0], null);
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #region 单元格验证

        /// <summary>
        /// 合法性验证
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <returns>1,验证通过,其他验证失败</returns>
        public virtual int Validation(object sender)
        {
            return 1;
        }


        /// <summary>
        /// 合法性验证
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">系统参数</param>
        /// <param name="fieldName"></param>
        /// <param name="row">当前行</param>
        /// <returns>1,验证通过,其他验证失败</returns>
        public virtual int Validation(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e, string fieldName, DataRow row)
        {
            int ret = 1;
            KzxDataType KzxDataType = KzxDataType.Str;
            DataRow[] dr = ColumnInfoTable.Select("Field='" + fieldName + "'");
            if (dr.Length <= 0)
            {
                return 1;
            }
            if ((dr[0]["IsNull"] == DBNull.Value ? true : Convert.ToBoolean(dr[0]["IsNull"])).Equals(false) &&
                (dr[0]["ReadOnly"] == DBNull.Value ? false : Convert.ToBoolean(dr[0]["ReadOnly"])).Equals(false) &&
                (dr[0]["Enabled"] == DBNull.Value ? true : Convert.ToBoolean(dr[0]["Enabled"])).Equals(true) &&
                (dr[0]["Visible"] == DBNull.Value ? true : Convert.ToBoolean(dr[0]["Visible"])).Equals(true))
            {
                //非空验证
                if (string.IsNullOrWhiteSpace(e.Value == null || e.Value == DBNull.Value ? string.Empty : e.Value.ToString()) == true)
                {
                    SetErrorText(e, GetLanguage("MSG000704", "数据不能为空"), fieldName, row);
                    ret = 0;
                    return ret;
                }
                else
                {
                    SetErrorText(e, string.Empty, fieldName, row);
                }
            }
            if ((dr[0]["AllowValueRange"] == DBNull.Value ? false : Convert.ToBoolean(dr[0]["AllowValueRange"])).Equals(true) &&
                (dr[0]["ReadOnly"] == DBNull.Value ? false : Convert.ToBoolean(dr[0]["ReadOnly"])).Equals(false) &&
                (dr[0]["Enabled"] == DBNull.Value ? true : Convert.ToBoolean(dr[0]["Enabled"])).Equals(true) &&
                (dr[0]["Visible"] == DBNull.Value ? true : Convert.ToBoolean(dr[0]["Visible"])).Equals(true))
            {
                KzxDataType = XmlRow.KzxDataTypeeConverter(dr[0]["DataType"].ToString());
                //启用了范围验证
                switch (KzxDataType)
                {
                    case KzxDataType.Str:
                        ret = ValidationForString(sender, e, dr[0], fieldName, row);
                        break;
                    case KzxDataType.Int:
                        ret = ValidationForInt(sender, e, dr[0], fieldName, row);
                        break;
                    case KzxDataType.Double:
                        ret = ValidationForDouble(sender, e, dr[0], fieldName, row);
                        break;
                    case KzxDataType.Decimal:
                        ret = ValidationForDecimal(sender, e, dr[0], fieldName, row);
                        break;
                    case KzxDataType.Date:
                        ret = ValidationForDate(sender, e, dr[0], fieldName, row);
                        break;
                    default:
                        ret = ValidationForString(sender, e, dr[0], fieldName, row);
                        break;
                }
                if (ret < 1)
                {
                    return ret;
                }
            }
            if (string.IsNullOrWhiteSpace(dr[0]["RegexString"].ToString()) == false)
            {
                //正则表达式验证
                ret = ValidationForRegex(sender, e, dr[0], fieldName, row);
                if (ret < 1)
                {
                    return ret;
                }
            }
            return 1;
        }


        /// <summary>
        /// 字符串范围验证
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">系统参数</param>
        /// <param name="row">列的元数据</param>
        /// <param name="fieldName">当前列的字段名</param>
        /// <param name="datarow">当前行</param>
        /// <returns>1通过，其他验证失败</returns>
        protected virtual int ValidationForString(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e, DataRow row, string fieldName, DataRow datarow)
        {
            int ret = 1;

            string value = e.Value == null || e.Value == DBNull.Value ? string.Empty : e.Value.ToString();

            SetErrorText(e, string.Empty, fieldName, datarow);
            if (string.IsNullOrWhiteSpace(value) == true)
            {
                //空值不验证
                return ret;
            }
            if (string.IsNullOrWhiteSpace(row["MinValue"] == DBNull.Value ? string.Empty : row["MinValue"].ToString()) == false)
            {
                if (string.Compare(value, row["MinValue"] == DBNull.Value ? string.Empty : row["MinValue"].ToString()) < 0)
                {
                    ret = 0;
                    SetErrorText(e, GetLanguage("MSG000705", "数据太小"), fieldName, datarow);
                }
            }
            if (string.IsNullOrWhiteSpace(row["MaxValue"] == DBNull.Value ? string.Empty : row["MaxValue"].ToString()) == false)
            {
                if (string.Compare(value, row["MaxValue"] == DBNull.Value ? string.Empty : row["MaxValue"].ToString()) > 0)
                {
                    ret = 0;
                    SetErrorText(e, GetLanguage("MSG000706", "数据太大"), fieldName, datarow);
                }
            }
            return ret;
        }

        /// <summary>
        /// 整数范围验证
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">系统参数</param>
        /// <param name="row">列的元数据</param>
        /// <param name="fieldName">列的字段名称</param>
        /// <param name="datarow">当前行</param>
        /// <returns>1通过，其他验证失败</returns>
        protected virtual int ValidationForInt(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e, DataRow row, string fieldName, DataRow datarow)
        {
            int ret = 1;
            int min = 0;
            int max = 0;
            int value = 0;
            SetErrorText(e, string.Empty, fieldName, datarow);
            if (string.IsNullOrWhiteSpace(e.Value == null || e.Value == DBNull.Value ? string.Empty : e.Value.ToString()) == true)
            {
                ret = 1;
                return ret;
            }

            if (int.TryParse((e.Value == null || e.Value == DBNull.Value ? "0" : e.Value.ToString()), out value) == true)
            {
                if (int.TryParse(row["MinValue"].ToString(), out min) == true)
                {
                    if (value < min)
                    {
                        ret = 0;
                        SetErrorText(e, GetLanguage("MSG000705", "数据太小"), fieldName, datarow);
                        return ret;
                    }
                }
                if (int.TryParse(row["MaxValue"].ToString(), out max) == true)
                {
                    if (value > max)
                    {
                        ret = 0;
                        SetErrorText(e, GetLanguage("MSG000706", "数据太大"), fieldName, datarow);
                        return ret;
                    }
                }
            }
            else
            {
                SetErrorText(e, GetLanguage("MSG000708", "请录入整数"), fieldName, datarow);
                ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// 实数范围验证
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">系统参数</param>
        /// <param name="row">列的元数据</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="datarow">当前行</param>
        /// <returns>1通过，其他验证失败</returns>
        protected virtual int ValidationForDouble(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e, DataRow row, string fieldName, DataRow datarow)
        {
            int ret = 1;
            Double min = 0;
            Double max = 0;
            Double value = 0;

            SetErrorText(e, string.Empty, fieldName, datarow);
            if (string.IsNullOrWhiteSpace(e.Value == null || e.Value == DBNull.Value ? string.Empty : e.Value.ToString()) == true)
            {
                ret = 1;
                return ret;
            }

            if (Double.TryParse((e.Value == null || e.Value == DBNull.Value ? "0" : e.Value.ToString()), out value) == true)
            {
                if (Double.TryParse(row["MinValue"].ToString(), out min) == true)
                {
                    if (value < min)
                    {
                        ret = 0;
                        SetErrorText(e, GetLanguage("MSG000705", "数据太小"), fieldName, datarow);
                        return ret;
                    }
                }
                if (Double.TryParse(row["MaxValue"].ToString(), out max) == true)
                {
                    if (value > max)
                    {
                        ret = 0;
                        SetErrorText(e, GetLanguage("MSG000706", "数据太大"), fieldName, datarow);
                        return ret;
                    }
                }
            }
            else
            {
                SetErrorText(e, GetLanguage("MSG000707", "请录入数字"), fieldName, datarow);
                ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// DECIMAL范围验证
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">系统参数</param>
        /// <param name="row">列的元数据</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="datarow">当前行</param>
        /// <returns>1通过，其他验证失败</returns>
        protected virtual int ValidationForDecimal(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e, DataRow row, string fieldName, DataRow datarow)
        {
            int ret = 1;
            decimal min = 0;
            decimal max = 0;
            decimal value = 0;
            SetErrorText(e, string.Empty, fieldName, datarow);
            if (string.IsNullOrWhiteSpace(e.Value == null || e.Value == DBNull.Value ? string.Empty : e.Value.ToString()) == true)
            {
                ret = 1;
                return ret;
            }
            if (decimal.TryParse((e.Value == null || e.Value == DBNull.Value ? "0" : e.Value.ToString()), out value) == true)
            {
                if (decimal.TryParse(row["MinValue"].ToString(), out min) == true)
                {
                    if (value < min)
                    {
                        ret = 0;
                        SetErrorText(e, GetLanguage("MSG000705", "数据太小"), fieldName, datarow);
                        return ret;
                    }
                }
                if (decimal.TryParse(row["MaxValue"].ToString(), out max) == true)
                {
                    if (value > max)
                    {
                        ret = 0;
                        SetErrorText(e, GetLanguage("MSG000706", "数据太大"), fieldName, datarow);
                        return ret;
                    }
                }
            }
            else
            {
                SetErrorText(e, GetLanguage("MSG000707", "请录入数字"), fieldName, datarow);
                ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// 日期范围验证
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">系统参数</param>
        /// <param name="row">列的元数据</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="datarow">当前行</param>
        /// <returns>1通过，其他验证失败</returns>
        protected virtual int ValidationForDate(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e, DataRow row, string fieldName, DataRow datarow)
        {
            int ret = 1;
            DateTime min = new DateTime(1900, 1, 1);
            DateTime max = new DateTime(1900, 1, 1);
            DateTime value = new DateTime(1900, 1, 1);

            SetErrorText(e, string.Empty, fieldName, datarow);
            if (string.IsNullOrWhiteSpace(e.Value == null || e.Value == DBNull.Value ? string.Empty : e.Value.ToString()) == true)
            {
                ret = 1;
                return ret;
            }
            if (DateTime.TryParse((e.Value == null || e.Value == DBNull.Value ? (new DateTime(1900, 1, 1)).ToString() : e.Value.ToString()), out value) == true)
            {
                if (DateTime.TryParse(row["MinValue"].ToString(), out min) == true)
                {
                    if (DateTime.Compare(value, min) < 0)
                    {
                        SetErrorText(e, GetLanguage("MSG000705", "数据太小"), fieldName, datarow);
                        ret = 0;
                    }
                }
                if (DateTime.TryParse(row["MaxValue"].ToString(), out max) == true)
                {
                    if (DateTime.Compare(value, max) > 0)
                    {
                        SetErrorText(e, GetLanguage("MSG000706", "数据太大"), fieldName, datarow);
                        ret = 0;
                    }
                }
            }
            else
            {
                SetErrorText(e, GetLanguage("MSG000709", "请录入日期"), fieldName, datarow);
                ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// 正则表达式验证       
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">系统参数</param>
        /// <param name="row">列的元数据</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="datarow">当前行</param>
        /// <returns>1通过，其他验证失败</returns>
        protected virtual int ValidationForRegex(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e, DataRow row, string fieldName, DataRow datarow)
        {
            int ret = 1;
            string value = e.Value == null || e.Value == DBNull.Value ? string.Empty : e.Value.ToString();

            SetErrorText(e, string.Empty, fieldName, datarow);
            if (string.IsNullOrWhiteSpace(row["RegexString"].ToString()) == false && string.IsNullOrWhiteSpace(value) == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(value, row["RegexString"].ToString()).Equals(false))
                {
                    ret = 0;
                    SetErrorText(e, GetLanguage("MSG000710", "数据不合法"), fieldName, datarow);
                    return ret;
                }
            }
            return ret;
        }

        private void SetErrorText(object sender, string errorText, string fieldName, DataRow dataRow)
        {
            PropertyInfo pi = null;
            GridView gv = this.MainView as GridView;
            if (gv != null)
            {
                gv.SetColumnError(gv.Columns[fieldName], errorText);
            }

            pi = sender.GetType().GetProperty("ErrorText");
            if (pi != null)
            {
                pi.SetValue(sender, errorText, null);
            }
            if (dataRow != null)
            {
                dataRow.SetColumnError(fieldName, errorText);
            }
        }

        /// <summary>
        /// 清除所有的错误信息
        /// </summary>
        public virtual void ClearErrors()
        {
            DataRow row = null;
            GridView view = null;
            this.IsValidation = true;
            view = this.MainView as GridView;
            for (int j = 0; j < view.DataRowCount; j++)
            {
                row = view.GetDataRow(j);
                row.ClearErrors();
            }
        }

        /// <summary>
        /// 合法性验证
        /// </summary>
        /// <returns>1,验证通过,其他验证失败</returns>
        public virtual int KzxValidation()
        {
            int ret = 0;
            int count = 0;
            GridView view = null;
            DataRow[] rows = null;
            GridColumn col = null;
            object value = null;
            this.IsValidation = true;
            DataRow row = null;
            DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs args = null;

            if (this.MainView != null)
            {
                ClearErrors();
                rows = this._ColumnInfoTable.Select("IsNull='False' or AllowValueRange='True' or ISNULL(RegexString,'')<>''", string.Empty, DataViewRowState.CurrentRows);
                view = this.MainView as GridView;
                for (int i = 0; i < rows.Length; i++)
                {
                    if (count > 0)
                    {
                        break;
                    }
                    for (int j = 0; j < view.DataRowCount; j++)
                    {

                        col = view.Columns.ColumnByName(rows[i]["Field"].ToString());
                        if (col != null)
                        {
                            if (col.Visible == true && col.ReadOnly == false)
                            {
                                value = view.GetRowCellValue(j, col);
                                row = view.GetDataRow(j);
                                args = new BaseContainerValidateEditorEventArgs(value);
                                ret = this.Validation(view, args, col.FieldName, row);
                                if (ret <= 0)
                                {
                                    count++;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            this.IsValidation = count > 0 ? false : true;
            return (count <= 0 ? 1 : 0);
        }

        private void GridViewValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            string fieldName = string.Empty;
            DataRow row = null;
            if (this.DataSource != null)
            {
                if (this.DataSource is BindingSource)
                {
                    BindingSource bs = this.DataSource as BindingSource;
                    row = ((bs.Current) as DataRowView).Row;
                }
            }
            fieldName = (sender as GridView).FocusedColumn.FieldName;
            if (this.Validation(sender, e, fieldName, row) < 1)
            {
                this.IsValidation = false;
                //e.Cancel = true;  //焦点不能离开
            }
            else
            {
                this.IsValidation = true;
                //e.Cancel = false;
            }
        }

        #endregion


        private void OnCellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            BindingSource bs = this.DataSource as BindingSource;
            bool allowedit = true;
            if (bs != null)
            {
                if (bs.List != null)
                {
                    DataView view = bs.List as DataView;
                    if (view != null)
                    {
                        if (view.AllowEdit == false)
                        {
                            allowedit = false;
                        }
                    }
                }
            }
            if (allowedit == true)
            {
                RaiseEvent(sender, "CellValueChanged", e);
                //数据携带
                SetReferenceFieldValue(e);
            }
            CellValueChanged?.Invoke(sender, e);
        }

        private void OnCellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Value == null)
            {
                return;
            }
            //记录选中的行，lfx，20170306
            if (e.Column.FieldName == "bChoose" && !(Control.ModifierKeys == Keys.Shift))
            {
                if (e.Value.GetType().ToString() == "System.Boolean")
                {
                    //记录已选中记录数
                    if (Convert.ToBoolean(e.Value))
                    {
                        lStartRow.Add(e.RowHandle);
                    }
                    else
                    {
                        lStartRow.Remove(e.RowHandle);
                    }
                }
            }
            RaiseEvent(sender, "CellValueChanging", e);
            CellValueChanging?.Invoke(sender, e);
        }

        private void ColumnChanged(object sender, EventArgs e)
        {
            RaiseEvent(sender, "ColumnChanged", e);
        }

        private void GridViewFocuseRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            RaiseEvent(sender, "FocuseRowChanged", e);
        }

        private void GridViewFocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            RaiseEvent(sender, "FocusedColumnChanged", e);
        }

        private void GridViewCustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            if (this._FooterCellDictionary.ContainsKey(e.Column.FieldName) == true)
            {
                e.Info.DisplayText = this._FooterCellDictionary[e.Column.FieldName];
            }
        }

        /// <summary>
        /// 刷新页脚行
        /// </summary>
        public void RefreshFooter()
        {
            //GridView gv = null;

            //gv = this.MainView as GridView;
            //if (gv != null)
            //{
            //    if (gv.OptionsView.ShowFooter == true)
            //    {
            //        gv.OptionsView.ShowFooter = false;
            //        gv.OptionsView.ShowFooter = true;
            //    }
            //}
        }

        /// <summary>
        /// 触发控件事件
        /// </summary>
        /// <param name="sender">事件发起者</param>
        /// <param name="eventName">事件名称</param>
        /// <param name="e">事件参数</param>
        protected virtual void RaiseEvent(object sender, string eventName, object e)
        {
            ControlEventArgs args = new ControlEventArgs();
            args.CurrentControl = ((GridView)this.MainView);
            args.EventId = eventName;
            args.SystemEventArgs = e;
            if (this.MainView == null || ((GridView)this.MainView).FocusedColumn == null || string.IsNullOrWhiteSpace(((GridView)this.MainView).FocusedColumn.FieldName) == true)
            {
                args.FieldName = this.Field;
            }
            else
            {
                args.FieldName = ((GridView)this.MainView).FocusedColumn.FieldName;
            }
            args.TableName = this.Table;
            args.Key = this.Key;
            if (this.KzxControlOperate != null)
            {
                this.KzxControlOperate(this, args);
                e = args.SystemEventArgs;
            }
        }

        #region 列的序列化与反序列化

        /// <summary>
        /// 创建列元数据表
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateSerializeColumnsTable()
        {
            DataTable table = new DataTable("Columns");
            table.Columns.Add("ColIndex", typeof(int));
            table.Columns.Add("Key", typeof(string));
            table.Columns.Add("Field", typeof(string));
            table.Columns.Add("MessageCode", typeof(string));
            table.Columns.Add("DesigeCaption", typeof(string));
            table.Columns.Add("ColumnType", typeof(string));
            table.Columns.Add("SourceTable", typeof(string));
            table.Columns.Add("DisplayMemberPath", typeof(string));
            table.Columns.Add("SelectedValuePath", typeof(string));
            table.Columns.Add("MaxLength", typeof(int));
            table.Columns.Add("PasswordChar", typeof(string));
            table.Columns.Add("ReadOnly", typeof(Boolean));
            table.Columns.Add("IsNull", typeof(Boolean));
            table.Columns.Add("IsUnique", typeof(Boolean));
            table.Columns.Add("ValidateGroup", typeof(string));
            table.Columns.Add("Enabled", typeof(Boolean));
            table.Columns.Add("Visible", typeof(Boolean));
            table.Columns.Add("Fixed", typeof(string));
            table.Columns.Add("FieldCaption", typeof(string));
            table.Columns.Add("ParentField", typeof(string));   //字段的值依赖于前面的哪个下拉框的字段(就是字段间的携带),Field中的值
            table.Columns.Add("ValueDependencyField", typeof(string));//字段的值来源于ParentField的表示的字段的下拉框中选择行的字段值
            table.Columns.Add("Width", typeof(int));
            table.Columns.Add("FilterString", typeof(string));
            table.Columns.Add("DllName", typeof(string));
            table.Columns.Add("KzxMaskType", typeof(string));
            table.Columns.Add("KzxEditMask", typeof(string));
            table.Columns.Add("KzxAllowMouseWheel", typeof(Boolean));
            table.Columns.Add("KzxSummaryItemType", typeof(string));
            table.Columns.Add("DefaultValue", typeof(string));
            table.Columns.Add("KzxFormatString", typeof(string));
            table.Columns.Add("KzxFormatType", typeof(string));
            table.Columns.Add("AllowValueRange", typeof(Boolean));
            table.Columns.Add("DataType", typeof(string));
            table.Columns.Add("MaxValue", typeof(string));
            table.Columns.Add("MinValue", typeof(string));
            table.Columns.Add("RegexString", typeof(string));
            table.Columns.Add("ToolTipMessageCode", typeof(string));
            table.Columns.Add("ToolTipText", typeof(string));
            table.Columns.Add("AllowEdit", typeof(string));
            table.Columns.Add("AllowSort", typeof(Boolean));
            return table;


        }

        public static DataTable SerializeColumns(string columnsXml)
        {
            DataRow row = null;
            XmlNode node = null;
            DataTable table = KzxGridControl.CreateSerializeColumnsTable();

            if (string.IsNullOrWhiteSpace(columnsXml) == true)
            {
                return table;
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(columnsXml);
            table.TableName = doc.DocumentElement.Name;
            for (int i = 0; i < doc.DocumentElement.ChildNodes.Count; i++)
            {
                node = doc.DocumentElement.ChildNodes[i];
                ////控制不可见的列不读取，提升渲染效率  
                //bool isVisible = false;
                //if (node.Attributes["Visible"] != null)
                //{
                //    isVisible = Convert.ToBoolean(node.Attributes["Visible"].Value);
                //}
                //if (!isVisible) continue;
                ////end by huangyq20170620

                row = table.NewRow();
                if (node.Attributes["ColIndex"] != null)
                {
                    row["ColIndex"] = Convert.ToInt32(node.Attributes["ColIndex"].Value);
                }
                if (node.Attributes["Key"] != null)
                {
                    row["Key"] = node.Attributes["Key"].Value;
                }
                if (node.Attributes["Field"] != null)
                {
                    row["Field"] = node.Attributes["Field"].Value;
                }
                if (node.Attributes["MessageCode"] != null)
                {
                    row["MessageCode"] = node.Attributes["MessageCode"].Value;
                }
                if (node.Attributes["DesigeCaption"] != null)
                {
                    row["DesigeCaption"] = node.Attributes["DesigeCaption"].Value;
                }
                if (node.Attributes["ColumnType"] != null)
                {
                    row["ColumnType"] = node.Attributes["ColumnType"].Value;
                }
                if (node.Attributes["SourceTable"] != null)
                {
                    row["SourceTable"] = node.Attributes["SourceTable"].Value;
                }
                if (node.Attributes["DisplayMemberPath"] != null)
                {
                    row["DisplayMemberPath"] = node.Attributes["DisplayMemberPath"].Value;
                }
                if (node.Attributes["SelectedValuePath"] != null)
                {
                    row["SelectedValuePath"] = node.Attributes["SelectedValuePath"].Value;
                }
                if (node.Attributes["MaxLength"] != null)
                {
                    row["MaxLength"] = node.Attributes["MaxLength"].Value;
                }
                if (node.Attributes["PasswordChar"] != null)
                {
                    row["PasswordChar"] = node.Attributes["PasswordChar"].Value;
                }
                if (node.Attributes["ReadOnly"] != null)
                {
                    row["ReadOnly"] = Convert.ToBoolean(node.Attributes["ReadOnly"].Value);
                }
                if (node.Attributes["IsNull"] != null)
                {
                    row["IsNull"] = Convert.ToBoolean(node.Attributes["IsNull"].Value);
                }
                if (node.Attributes["IsUnique"] != null)
                {
                    row["IsUnique"] = Convert.ToBoolean(node.Attributes["IsUnique"].Value);
                }
                if (node.Attributes["Enabled"] != null)
                {
                    row["Enabled"] = Convert.ToBoolean(node.Attributes["Enabled"].Value);
                }
                if (node.Attributes["Visible"] != null)
                {
                    row["Visible"] = Convert.ToBoolean(node.Attributes["Visible"].Value);
                }
                if (node.Attributes["ValidateGroup"] != null)
                {
                    row["ValidateGroup"] = node.Attributes["ValidateGroup"].Value;
                }
                if (node.Attributes["Fixed"] != null)
                {
                    row["Fixed"] = node.Attributes["Fixed"].Value;
                }
                if (node.Attributes["FieldCaption"] != null)
                {
                    row["FieldCaption"] = node.Attributes["FieldCaption"].Value;
                }
                if (node.Attributes["ParentField"] != null)
                {
                    row["ParentField"] = node.Attributes["ParentField"].Value;
                }
                if (node.Attributes["ValueDependencyField"] != null)
                {
                    row["ValueDependencyField"] = node.Attributes["ValueDependencyField"].Value;
                }
                if (node.Attributes["Width"] != null)
                {
                    row["Width"] = Convert.ToInt32(node.Attributes["Width"].Value);
                }
                if (node.Attributes["FilterString"] != null)
                {
                    row["FilterString"] = node.Attributes["FilterString"].Value;
                }
                if (node.Attributes["DllName"] != null)
                {
                    row["DllName"] = node.Attributes["DllName"].Value;
                }
                if (node.Attributes["KzxMaskType"] != null)
                {
                    row["KzxMaskType"] = node.Attributes["KzxMaskType"].Value;
                }
                if (node.Attributes["KzxEditMask"] != null)
                {
                    row["KzxEditMask"] = node.Attributes["KzxEditMask"].Value;
                }
                if (node.Attributes["KzxAllowMouseWheel"] != null)
                {
                    row["KzxAllowMouseWheel"] = string.IsNullOrWhiteSpace(node.Attributes["KzxAllowMouseWheel"].Value) == true ? false : Convert.ToBoolean(node.Attributes["KzxAllowMouseWheel"].Value);
                }
                else
                {
                    row["KzxAllowMouseWheel"] = false;
                }
                if (node.Attributes["KzxSummaryItemType"] != null)
                {
                    row["KzxSummaryItemType"] = node.Attributes["KzxSummaryItemType"].Value;
                }
                if (node.Attributes["DefaultValue"] != null)
                {
                    row["DefaultValue"] = node.Attributes["DefaultValue"].Value;
                }
                if (node.Attributes["KzxFormatType"] != null)
                {
                    row["KzxFormatType"] = node.Attributes["KzxFormatType"].Value;
                }
                if (node.Attributes["KzxFormatString"] != null)
                {
                    row["KzxFormatString"] = node.Attributes["KzxFormatString"].Value;
                }
                if (node.Attributes["AllowValueRange"] != null)
                {
                    row["AllowValueRange"] = Convert.ToBoolean(node.Attributes["AllowValueRange"].Value);
                }
                if (node.Attributes["DataType"] != null)
                {
                    row["DataType"] = node.Attributes["DataType"].Value;
                }
                if (node.Attributes["MaxValue"] != null)
                {
                    row["MaxValue"] = node.Attributes["MaxValue"].Value;
                }
                if (node.Attributes["MinValue"] != null)
                {
                    row["MinValue"] = node.Attributes["MinValue"].Value;
                }
                if (node.Attributes["RegexString"] != null)
                {
                    row["RegexString"] = node.Attributes["RegexString"].Value;
                }
                if (node.Attributes["ToolTipText"] != null)
                {
                    row["ToolTipText"] = node.Attributes["ToolTipText"].Value;
                }
                if (node.Attributes["ToolTipMessageCode"] != null)
                {
                    row["ToolTipMessageCode"] = node.Attributes["ToolTipMessageCode"].Value;
                }
                if (node.Attributes["AllowEdit"] != null)
                {
                    row["AllowEdit"] = Convert.ToBoolean(node.Attributes["AllowEdit"].Value);
                }
                else
                {
                    row["AllowEdit"] = true;
                }
                if (node.Attributes["AllowSort"] != null)
                {
                    row["AllowSort"] = Convert.ToBoolean(node.Attributes["AllowSort"].Value);
                }
                else
                {
                    row["AllowSort"] = true;
                }
                table.Rows.Add(row);
            }
            return table;
        }

        public static string IsNullString(object value)
        {
            if (value == DBNull.Value || value == null)
            {
                return string.Empty;
            }
            else
            {
                return value.ToString();
            }
        }

        public static string DeserializeColumns(DataTable columnTable)
        {
            string s = string.Empty;
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb);
            writer.WriteStartElement(columnTable.TableName == string.Empty ? "table1" : columnTable.TableName);
            foreach (DataRow row in columnTable.Rows)
            {
                writer.WriteStartElement("Row");
                writer.WriteAttributeString("ColIndex", XmlConvert.ToString(Convert.ToInt32(row["ColIndex"])));
                writer.WriteAttributeString("Key", IsNullString(row["Key"]).ToString());
                writer.WriteAttributeString("Field", IsNullString(row["Field"]).ToString());
                writer.WriteAttributeString("MessageCode", IsNullString(row["MessageCode"]).ToString());
                writer.WriteAttributeString("DesigeCaption", IsNullString(row["DesigeCaption"]).ToString());
                writer.WriteAttributeString("ColumnType", IsNullString(row["ColumnType"]).ToString());
                writer.WriteAttributeString("SourceTable", IsNullString(row["SourceTable"]).ToString());
                writer.WriteAttributeString("DisplayMemberPath", IsNullString(row["DisplayMemberPath"]).ToString());
                writer.WriteAttributeString("SelectedValuePath", IsNullString(row["SelectedValuePath"]).ToString());
                writer.WriteAttributeString("MaxLength", XmlConvert.ToString(Convert.ToInt32(row["MaxLength"])));
                writer.WriteAttributeString("PasswordChar", IsNullString(row["PasswordChar"]).ToString());
                writer.WriteAttributeString("ValidateGroup", IsNullString(row["ValidateGroup"]).ToString());
                writer.WriteAttributeString("ReadOnly", XmlConvert.ToString(row["ReadOnly"] == DBNull.Value ? false : Convert.ToBoolean(row["ReadOnly"])));
                writer.WriteAttributeString("IsNull", XmlConvert.ToString(row["IsNull"] == DBNull.Value ? false : Convert.ToBoolean(row["IsNull"])));
                writer.WriteAttributeString("IsUnique", XmlConvert.ToString(row["IsUnique"] == DBNull.Value ? false : Convert.ToBoolean(row["IsUnique"])));
                writer.WriteAttributeString("Enabled", XmlConvert.ToString(row["Enabled"] == DBNull.Value ? true : Convert.ToBoolean(row["Enabled"])));
                writer.WriteAttributeString("Visible", XmlConvert.ToString(row["Visible"] == DBNull.Value ? true : Convert.ToBoolean(row["Visible"])));
                writer.WriteAttributeString("Fixed", IsNullString(row["Fixed"]).ToString());
                writer.WriteAttributeString("FieldCaption", IsNullString(row["FieldCaption"]).ToString());
                writer.WriteAttributeString("ParentField", IsNullString(row["ParentField"]).ToString());
                writer.WriteAttributeString("ValueDependencyField", IsNullString(row["ValueDependencyField"]).ToString());
                writer.WriteAttributeString("Width", XmlConvert.ToString(Convert.ToInt32(row["Width"])));
                writer.WriteAttributeString("FilterString", IsNullString(row["FilterString"]).ToString());
                writer.WriteAttributeString("DllName", IsNullString(row["DllName"]).ToString());
                writer.WriteAttributeString("KzxMaskType", IsNullString(row["KzxMaskType"]).ToString());
                writer.WriteAttributeString("KzxEditMask", IsNullString(row["KzxEditMask"]).ToString());
                writer.WriteAttributeString("KzxAllowMouseWheel", XmlConvert.ToString(row["KzxAllowMouseWheel"] == DBNull.Value ? false : Convert.ToBoolean(row["KzxAllowMouseWheel"])));
                writer.WriteAttributeString("KzxSummaryItemType", IsNullString(row["KzxSummaryItemType"]).ToString());
                writer.WriteAttributeString("DefaultValue", IsNullString(row["DefaultValue"]).ToString());
                writer.WriteAttributeString("KzxFormatString", IsNullString(row["KzxFormatString"]).ToString());
                writer.WriteAttributeString("KzxFormatType", IsNullString(row["KzxFormatType"]).ToString());
                writer.WriteAttributeString("AllowValueRange", XmlConvert.ToString(row["AllowValueRange"] == DBNull.Value ? false : Convert.ToBoolean(row["AllowValueRange"])));
                writer.WriteAttributeString("DataType", IsNullString(row["DataType"]).ToString());
                writer.WriteAttributeString("MaxValue", IsNullString(row["MaxValue"]).ToString());
                writer.WriteAttributeString("MinValue", IsNullString(row["MinValue"]).ToString());
                writer.WriteAttributeString("RegexString", IsNullString(row["RegexString"]).ToString());
                writer.WriteAttributeString("ToolTipText", IsNullString(row["ToolTipText"]).ToString());
                writer.WriteAttributeString("ToolTipMessageCode", IsNullString(row["ToolTipMessageCode"]).ToString());
                writer.WriteAttributeString("AllowEdit", XmlConvert.ToString(row["AllowEdit"] == DBNull.Value ? true : Convert.ToBoolean(row["AllowEdit"])));
                writer.WriteAttributeString("AllowSort", XmlConvert.ToString(row["AllowSort"] == DBNull.Value ? true : Convert.ToBoolean(row["AllowSort"])));
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
            return sb.ToString();
        }

        #endregion

        #region 扩展方法

        protected virtual void OnEmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            //if (this.DataSource is BindingSource)
            //{
            //    BindingSource binding = this.DataSource as BindingSource;
            //    if (binding != null)
            //    {
            //        e.Handled = true;
            //        if (e.Button.ButtonType == NavigatorButtonType.Append)
            //        {
            //            binding.AddNew();
            //            binding.EndEdit();
            //        }
            //        else if (e.Button.ButtonType == NavigatorButtonType.Remove)
            //        {
            //            binding.RemoveCurrent();
            //            binding.EndEdit();
            //        }
            //    }
            //}
            //else if (this.DataSource is DataTable)
            //{
            //    DataTable binding = this.DataSource as DataTable;
            //    if (binding != null)
            //    {
            //        e.Handled = true;
            //        if (e.Button.ButtonType == NavigatorButtonType.Append)
            //        {
            //            DataRow row=binding.NewRow();
            //            binding.Rows.Add(row);
            //        }
            //        else if (e.Button.ButtonType == NavigatorButtonType.Remove)
            //        {
            //            GridView gridview = this.MainView as GridView;
            //            if (gridview != null)
            //            {
            //                if (gridview.FocusedRowHandle >= 0)
            //                {
            //                    DataRow row = gridview.GetDataRow(gridview.FocusedRowHandle);
            //                    row.Delete();
            //                }
            //            }
            //        }
            //    }
            //}
            //else if (this.DataSource is DataView)
            //{
            //    DataView view = this.DataSource as DataView;
            //    if (view != null)
            //    {
            //        DataTable binding = view.Table;
            //        if (binding != null)
            //        {
            //            e.Handled = true;
            //            if (e.Button.ButtonType == NavigatorButtonType.Append)
            //            {
            //                DataRow row = binding.NewRow();
            //                binding.Rows.Add(row);
            //            }
            //            else if (e.Button.ButtonType == NavigatorButtonType.Remove)
            //            {
            //                GridView gridview = this.MainView as GridView;
            //                if (gridview != null)
            //                {
            //                    if (gridview.FocusedRowHandle >= 0)
            //                    {
            //                        DataRow row = gridview.GetDataRow(gridview.FocusedRowHandle);
            //                        row.Delete();
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            this.RaiseEvent(sender, "ButtonClick", e);
        }

        /// <summary>
        /// 设置多语言
        /// </summary>
        /// <param name="columnEdit">RepositoryItemGridLookUpEdit类型的列</param>
        /// <param name="captions">多语言标识</param>
        public void SetLanguage(RepositoryItemGridLookUpEdit columnEdit, string captions)
        {
            string[] fields = null;
            string[] fieldvalues = null;
            string[] values = null;
            GridColumn column = null;
            Dictionary<string, string> bandedDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            fields = captions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (fields.Length > 0)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    fieldvalues = fields[i].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (fieldvalues.Length == 2)
                    {
                        column = columnEdit.View.Columns.ColumnByFieldName(fieldvalues[0]);
                        if (column != null)
                        {
                            values = fieldvalues[1].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                            if (values.Length == 2)
                            {
                                column.Caption = GetColumnCaption(GetLanguage(values[0], fieldvalues[0]));
                                column.Visible = values[1] == "1" || string.IsNullOrWhiteSpace(values[1]) == true ? true : false;
                            }
                            else
                            {
                                column.Caption = GetColumnCaption(GetLanguage(fieldvalues[1], fieldvalues[0]));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置多语言
        /// </summary>
        public void SetLanguage()
        {
            SetLanguageDesign();

            GridColumn gridColumn = null;
            GridView gridView = this.MainView as GridView;
            if (gridView != null)
            {
                if (this.ColumnInfoTable != null && this.ColumnInfoTable.Rows.Count > 0)
                {
                    foreach (DataRowView dataRowView in this.ColumnInfoTable.DefaultView)
                    {
                        if (ColumnType.ComboBox == XmlRow.KzxColumnTypeConverter(dataRowView["ColumnType"].ToString()))
                        {
                            gridColumn = gridView.Columns.ColumnByFieldName(dataRowView["Field"].ToString());
                            if (gridColumn != null)
                            {
                                if (gridColumn.ColumnEdit is RepositoryItemGridLookUpEdit)
                                {
                                    if (dataRowView["FieldCaption"] != DBNull.Value)
                                    {
                                        if (string.IsNullOrWhiteSpace(dataRowView["FieldCaption"].ToString()) == false)
                                        {
                                            SetLanguage((RepositoryItemGridLookUpEdit)gridColumn.ColumnEdit, dataRowView["FieldCaption"].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 自动列宽
        /// </summary>
        public void CalcColumnBestWidth()
        {
            GridView gridView = this.MainView as GridView;
            if (gridView != null)
            {
                //foreach (GridColumn c in gridView.Columns)
                //{
                //    gridView.CalcColumnBestWidth(c);
                //}
                gridView.BestFitColumns();
            }
        }


        /// <summary>
        /// 根据列的设计信息创建列
        /// </summary>
        public void SetColumnDisplayFormat()
        {
            DataTable dt = null;
            PropertyInfo pi = null;
            object v = null;
            object obj = null;
            GridView gridView = this.MainView as GridView;
            string f = string.Empty;

            if (gridView != null)
            {
                if (this.DataSource is BindingSource)
                {
                    dt = ((this.DataSource as BindingSource).List as DataView).Table;
                }
                foreach (GridColumn gridColumn in gridView.Columns)
                {
                    if (dt != null)
                    {
                        if (dt.Columns.Contains(gridColumn.FieldName) == true)
                        {
                            if (
                                //dt.Columns[gridColumn.FieldName].DataType == typeof(int) ||
                                //dt.Columns[gridColumn.FieldName].DataType == typeof(Int32) ||
                                //dt.Columns[gridColumn.FieldName].DataType == typeof(Int16) ||
                                //dt.Columns[gridColumn.FieldName].DataType == typeof(Int64) ||
                                dt.Columns[gridColumn.FieldName].DataType == typeof(Decimal) ||
                                dt.Columns[gridColumn.FieldName].DataType == typeof(double) ||
                                dt.Columns[gridColumn.FieldName].DataType == typeof(float) ||
                                dt.Columns[gridColumn.FieldName].DataType == typeof(long) ||
                                dt.Columns[gridColumn.FieldName].DataType == typeof(Single)
                                )
                            {

                                //数字全部靠右显示
                                gridColumn.AppearanceCell.Options.UseTextOptions = true;
                                gridColumn.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
                                gridColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                //gridColumn.DisplayFormat.FormatString = "0.######";

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据列的设计信息创建列
        /// </summary>
        public void SetColumnDisplayFormat(DataTable tabledetail, int qtyscale, int pricescale, int moneyscale, bool rightAmount)
        {
            DataTable dt = null;
            PropertyInfo pi = null;
            object v = null;
            object obj = null;
            GridView gridView = this.MainView as GridView;
            string f = string.Empty;
            int scale = 3;
            int type = -1;

            if (gridView != null)
            {
                if (this.DataSource is BindingSource)
                {
                    dt = ((this.DataSource as BindingSource).List as DataView).Table;
                }
                foreach (GridColumn gridColumn in gridView.Columns)
                {
                    scale = 3;
                    type = -1;
                    if (dt != null)
                    {
                        if (dt.Columns.Contains(gridColumn.FieldName) == true)
                        {
                            var fieldType = dt.Columns[gridColumn.FieldName].DataType;

                            if (
                                //dt.Columns[gridColumn.FieldName].DataType == typeof(int) ||
                                //dt.Columns[gridColumn.FieldName].DataType == typeof(Int32) ||
                                //dt.Columns[gridColumn.FieldName].DataType == typeof(Int16) ||
                                //dt.Columns[gridColumn.FieldName].DataType == typeof(Int64) ||
                                fieldType == _typeOfDecimal ||
                                fieldType == _typeOfDouble ||
                                fieldType == _typeOfFloat ||
                                fieldType == _typeOfSingle
                                )
                            {
                                scale = 6;  //表格数字小数位默认6位（没有受参数控制的，比如税率、汇率）

                                //DataRow[] rows = tabledetail.Select("sTableName='" + this.Table + "' and sField='" + gridColumn.FieldName + "' and (sContentType='0' or sContentType='1' or sContentType='2')", string.Empty);
                                DataRow[] rows = tabledetail.Select(string.Format("sTableName='{0}' and sField='{1}'", this.Table, gridColumn.FieldName), string.Empty);
                                if (rows.Length > 0)
                                {
                                    /* 内容类型处理 */
                                    switch (rows[0]["sContentType"].ToString())
                                    {
                                        case "0":
                                            //数量类型
                                            {
                                                type = 0;
                                                scale = qtyscale;
                                            }
                                            break;

                                        case "1":
                                            //价格类型
                                            {
                                                type = 1;
                                                scale = pricescale;
                                            }
                                            break;

                                        case "2":
                                            //金额类型
                                            {
                                                type = 2;
                                                scale = moneyscale;
                                            }
                                            break;
                                    }

                                    gridColumn.DisplayFormat.FormatString = "#,###0." + new string('#', scale);
                                    if (gridColumn.Summary.Count > 0)
                                    {
                                        var sumFormat = rows[0]["sSubFormat"].ToString();
                                        gridColumn.Summary[0].DisplayFormat = string.IsNullOrWhiteSpace(sumFormat) ? "{0:#,###0.###}" : sumFormat;
                                    }
                                }
                                if (gridColumn.ColumnEdit != null)
                                {
                                    pi = gridColumn.ColumnEdit.GetType().GetProperty("Mask");
                                    if (pi != null)
                                    {
                                        obj = pi.GetValue(gridColumn.ColumnEdit, null);
                                        if (obj != null)
                                        {
                                            pi = obj.GetType().GetProperty("EditMask");
                                            if (pi != null)
                                            {
                                                pi.SetValue(obj, "##############0.######", null);
                                                if (gridColumn.Summary.Count > 0)
                                                {
                                                    gridColumn.Summary[0].DisplayFormat = "{0:##############0.######}";
                                                }
                                            }
                                            pi = obj.GetType().GetProperty("MaskType");
                                            if (pi != null)
                                            {
                                                pi.SetValue(obj, MaskType.Numeric, null);
                                            }
                                            if (type == 1 || type == 2)
                                            {
                                                if (rightAmount == true)
                                                {
                                                    pi = gridColumn.ColumnEdit.GetType().GetProperty("PasswordChar");
                                                    if (pi != null)
                                                    {
                                                        pi.SetValue(gridColumn.ColumnEdit, '\0', null);
                                                    }
                                                }
                                                else
                                                {
                                                    pi = gridColumn.ColumnEdit.GetType().GetProperty("PasswordChar");
                                                    if (pi != null)
                                                    {
                                                        pi.SetValue(gridColumn.ColumnEdit, '*', null);

                                                        //将列名加入加密列表
                                                        _EncryptionColumns.Add(gridColumn.FieldName);

                                                        //如果存在页脚行，则移除。
                                                        if (gridColumn.SummaryItem != null)
                                                        {
                                                            gridColumn.Summary.Remove(gridColumn.SummaryItem);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                //设置浮点显示格式
                                if (scale < 1) scale = 3;

                                gridColumn.AppearanceCell.Options.UseTextOptions = true;
                                gridColumn.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
                                gridColumn.DisplayFormat.FormatType = FormatType.Numeric;
                                gridColumn.DisplayFormat.FormatString = "#,###0." + new string('#', scale);
                                if (gridColumn.Summary.Count > 0)
                                {


                                    gridColumn.Summary[0].DisplayFormat = GetDisplayFormatByIni(gridColumn.FieldName);//string.IsNullOrWhiteSpace(sumFormat) ? ("{0:#,###0." + new string('#', scale) + "}") : sumFormat;

                                }
                            }
                        }
                    }
                }
            }
        }
        private string GetDisplayFormatByIni(string sColumns)
        {
            //读取INI文件保存的格式化字符串设置
            string formatFilePath = Application.StartupPath + @"\Guid\StringFormat.ini";
            IniFileCore formatIniFile = new IniFileCore(formatFilePath);

            string sContein = formatIniFile.Read("" + _ColorSet + this.Name + "", "Format");
            index = formatIniFile.Read("" + _ColorSet + this.Name + "", "Freeze");
            if (!sContein.IsNullOrWhiteSpaceExt())
            {
                if (!sContein.IsNullOrWhiteSpaceExt())
                {
                    string sSFormat = sContein.Substring(0, sContein.Length - 1);
                    string[] sCell = sSFormat.Split(',');

                    for (int i = 0; i < sCell.Length; i++)
                    {
                        string[] _sCellSeparated = sCell[i].Split('|');
                        if (sColumns == _sCellSeparated[0])
                        {
                            if (_sCellSeparated.Length > 1)
                            {
                                return _sCellSeparated[1];
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 根据列的设计信息创建列
        /// </summary>
        public void SetColumnDisplayFormat(DataTable tabledetail, GridColumn gridColumn, DataColumn column, int qtyscale, int pricescale, int moneyscale, bool rightAmount)
        {

            PropertyInfo pi = null;
            object obj = null;
            GridView gridView = this.MainView as GridView;
            string f = string.Empty;
            int scale = 3;
            int type = -1;


            if (
                //dt.Columns[gridColumn.FieldName].DataType == typeof(int) ||
                //dt.Columns[gridColumn.FieldName].DataType == typeof(Int32) ||
                //dt.Columns[gridColumn.FieldName].DataType == typeof(Int16) ||
                //dt.Columns[gridColumn.FieldName].DataType == typeof(Int64) ||
                column.DataType == typeof(Decimal) ||
                column.DataType == typeof(double) ||
                column.DataType == typeof(float) ||
                column.DataType == typeof(long) ||
                column.DataType == typeof(Single)
                )
            {
                scale = 0;
                DataRow[] rows = tabledetail.Select("sTableName='" + this.Table + "' and sField='" + column.ColumnName + "' and (sContentType='0' or sContentType='1' or sContentType='2')", string.Empty);
                if (rows.Length > 0)
                {
                    if (rows[0]["sContentType"].ToString().Equals("0") == true)
                    {
                        type = 0;
                        scale = qtyscale;
                    }
                    else if (rows[0]["sContentType"].ToString().Equals("1") == true)
                    {
                        type = 1;
                        scale = pricescale;
                    }
                    else if (rows[0]["sContentType"].ToString().Equals("2") == true)
                    {
                        type = 2;
                        scale = moneyscale;
                    }
                }
                if (gridColumn.ColumnEdit != null)
                {
                    pi = gridColumn.ColumnEdit.GetType().GetProperty("Mask");
                    if (pi != null)
                    {
                        obj = pi.GetValue(gridColumn.ColumnEdit, null);
                        if (obj != null)
                        {
                            pi = obj.GetType().GetProperty("EditMask");
                            if (pi != null)
                            {
                                pi.SetValue(obj, "##############0.######", null);
                                if (gridColumn.Summary.Count > 0)
                                {
                                    gridColumn.Summary[0].DisplayFormat = "{0:##############0.######}";
                                }
                            }
                            pi = obj.GetType().GetProperty("MaskType");
                            if (pi != null)
                            {
                                pi.SetValue(obj, MaskType.Numeric, null);
                            }
                            if (type == 1 || type == 2)
                            {
                                if (rightAmount == true)
                                {
                                    pi = gridColumn.ColumnEdit.GetType().GetProperty("PasswordChar");
                                    if (pi != null)
                                    {
                                        pi.SetValue(gridColumn.ColumnEdit, '\0', null);
                                    }
                                }
                                else
                                {
                                    pi = gridColumn.ColumnEdit.GetType().GetProperty("PasswordChar");
                                    if (pi != null)
                                    {
                                        pi.SetValue(gridColumn.ColumnEdit, '*', null);
                                    }
                                }
                            }
                        }
                    }
                }

                //数字全部靠右显示
                if (scale > 0)
                {
                    gridColumn.AppearanceCell.Options.UseTextOptions = true;
                    gridColumn.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
                    gridColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gridColumn.DisplayFormat.FormatString = "#,###0." + new string('#', scale);
                    if (gridColumn.Summary.Count > 0)
                    {
                        gridColumn.Summary[0].DisplayFormat = "{0:" + "#,###0." + new string('#', scale) + "}";
                    }
                }
                else
                {
                    gridColumn.AppearanceCell.Options.UseTextOptions = true;
                    gridColumn.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
                    gridColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gridColumn.DisplayFormat.FormatString = "#,###0." + new string('#', 3);
                    if (gridColumn.Summary.Count > 0)
                    {
                        gridColumn.Summary[0].DisplayFormat = "{0:" + "#,###0." + new string('#', 3) + "}";
                    }
                }
            }
        }

        /// <summary>
        /// 设置引用后允许修改属性
        /// </summary>
        /// <param name="e">true允许修改,false不允许修改</param>
        public void SetAllowEditForReference(bool e)
        {
            GridColumn column = null;
            GridView gridView = this.MainView as GridView;
            if (gridView != null)
            {
                foreach (DataRowView dataRowView in this.ColumnInfoTable.DefaultView)
                {
                    column = gridView.Columns.ColumnByFieldName(dataRowView["Field"].ToString());
                    if (Convert.ToBoolean(dataRowView["AllowEdit"]) == false)
                    {
                        column.OptionsColumn.ReadOnly = e;
                    }
                }
            }
        }

        /// <summary>
        /// 根据列的设计信息创建列
        /// </summary>
        public void CreateColumnByColumnInfo()
        {
            DataTable dt = null;
            bool isnull = true;
            bool isreadonly = false;

            this._ColumnInfoTable = KzxGridControl.SerializeColumns(this._Columns);
            this.RefreshColumnInfoByModuleSetting();

            GridColumn gridColumn = null;

            for (int i = this.Views.Count - 1; i >= 0; i--)
            {
                if (this.Views[i] is GridView)
                {
                    (this.Views[i] as GridView).OptionsView.ShowGroupPanel = this._ShowGroupPanel;
                }
            }
            GridView gridView = this.MainView as GridView;
            if (gridView != null)
            {
                if (_ColumnInfoTable.Rows.Count <= gridView.Columns.Count)
                {
                    return;
                }
                if (this.DataSource is BindingSource)
                {
                    dt = ((this.DataSource as BindingSource).List as DataView).Table;
                }

                if (_ColumnInfoTable != null && _ColumnInfoTable.Rows.Count > 0)
                {
                    gridView.Columns.Clear();
                    foreach (DataRowView dataRowView in _ColumnInfoTable.DefaultView)
                    {
                        DataRow row = dataRowView.Row;
                        gridColumn = new GridColumn();

                        string fieldName = dataRowView["Field"].ToString();
                        isnull = Convert.ToBoolean(dataRowView["IsNull"]);
                        isreadonly = Convert.ToBoolean(dataRowView["ReadOnly"]);

                        if (this.IsBandedGridView == true)
                        {
                            gridColumn = (GridColumn)(new BandedGridColumn());
                        }

                        gridColumn.OptionsEditForm.VisibleIndex = Convert.ToInt32(dataRowView["ColIndex"]);
                        gridColumn.Name = fieldName;
                        gridColumn.FieldName = fieldName;
                        gridColumn.Fixed = XmlRow.FixedConverter(dataRowView["Fixed"].ToString());
                        gridColumn.Visible = Convert.ToBoolean(dataRowView["Visible"]);
                        gridColumn.Width = Convert.ToInt32(dataRowView["Width"]);
                        gridColumn.OptionsColumn.ReadOnly = Convert.ToBoolean(dataRowView["ReadOnly"]);
                        gridColumn.OptionsColumn.AllowEdit = Convert.ToBoolean(dataRowView["Enabled"]);
                        gridColumn.Caption = dataRowView["DesigeCaption"].ToString();
                        gridColumn.AppearanceHeader.Options.UseTextOptions = true;
                        gridColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gridColumn.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gridColumn.DisplayFormat.FormatString = row["KzxFormatString"] == DBNull.Value ? string.Empty : row["KzxFormatString"].ToString();
                        gridColumn.DisplayFormat.FormatType = row["KzxFormatType"] == DBNull.Value ? DevExpress.Utils.FormatType.None : XmlRow.KzxFormatTypeConverter(row["KzxFormatType"].ToString());

                        gridColumn.ToolTip = GetLanguage(dataRowView["ToolTipMessageCode"] == DBNull.Value ? string.Empty : dataRowView["ToolTipMessageCode"].ToString(), dataRowView["ToolTipText"] == DBNull.Value ? string.Empty : dataRowView["ToolTipText"].ToString());
                        gridColumn.OptionsColumn.AllowSort = (DefaultBoolean)Enum.Parse(typeof(DefaultBoolean), dataRowView["AllowSort"].ToString());

                        DevExpress.Data.SummaryItemType summaryitemtype = row["KzxSummaryItemType"] == DBNull.Value ? DevExpress.Data.SummaryItemType.None : XmlRow.KzxSummaryItemTypeConverter(row["KzxSummaryItemType"].ToString());

                        SetBackColor(gridColumn, isnull, isreadonly);

                        //单据网格窗体汇总行要做权限控制，改为框架代码实现 HHL 20160809
                        //GridColumnSummaryItem item = new DevExpress.XtraGrid.GridColumnSummaryItem(summaryitemtype);
                        //item.FieldName = gridColumn.FieldName;
                        //gridColumn.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { item });

                        #region -- 调整逻辑，优化性能 modify by huangyq 20170620 --
                        {
                            string columnTypeString = dataRowView["ColumnType"].ToString();
                            ColumnType columnType = XmlRow.KzxColumnTypeConverter(columnTypeString);
                            switch (columnType)
                            {
                                case ColumnType.Text:
                                    gridColumn.ColumnEdit = this.CreateRepositoryItemTextEdit(row);
                                    break;
                                case ColumnType.Date:
                                    gridColumn.ColumnEdit = this.CreateRepositoryItemDateEdit(row);
                                    break;
                                case ColumnType.Time:
                                    gridColumn.ColumnEdit = this.CreateRepositoryItemTimeEdit(row);
                                    break;
                                case ColumnType.ComboBox:
                                    gridColumn.ColumnEdit = this.CreateRepositoryItemGridLookUpEdit(row);
                                    ((RepositoryItemGridLookUpEdit)gridColumn.ColumnEdit).DisplayMember = dataRowView["DisplayMemberPath"].ToString();
                                    ((RepositoryItemGridLookUpEdit)gridColumn.ColumnEdit).ValueMember = dataRowView["SelectedValuePath"].ToString();
                                    break;
                                case ColumnType.CheckBox:
                                    gridColumn.ColumnEdit = this.CreateRepositoryItemCheckEdit(row);
                                    break;
                                case ColumnType.RadioBox:
                                    gridColumn.ColumnEdit = this.CreateRepositoryItemRadioGroup(row);
                                    break;
                                case ColumnType.PictureBox:
                                    gridColumn.ColumnEdit = this.CreateRepositoryItemPictureEdit(row);
                                    break;
                                case ColumnType.HyperLink:
                                    gridColumn.ColumnEdit = this.CreateRepositoryItemHyperLinkEdit(row);
                                    break;
                                case ColumnType.ButtonEdit:
                                    gridColumn.ColumnEdit = this.CreateRepositoryItemButtonEdit(row);
                                    break;
                                case ColumnType.NoSourceComboBox:
                                    gridColumn.ColumnEdit = this.CreateRepositoryItemNoSourceComboBox(row);
                                    break;
                                case ColumnType.MemoEdit:
                                    gridColumn.ColumnEdit = this.CreateRepositoryItemMemoEdit(row);
                                    break;
                                case ColumnType.CalcEdit:
                                    gridColumn.ColumnEdit = this.CreateRepositoryItemCalcEdit(row);
                                    break;
                                default:
                                    gridColumn.ColumnEdit = this.CreateRepositoryItemTextEdit(row);
                                    break;
                            }
                        }

                        #endregion

                        gridView.Columns.Add(gridColumn);
                        string sCaptionName = string.Format("{0} ({1})", gridColumn.Caption, gridColumn.FieldName);
                        field.Add(sCaptionName);
                    }
                }
                if (this.DesignMode == false)
                {
                    SetLanguage();
                }
                try
                {
                    //this.ForceInitialize();
                    string XmlFileName = Application.StartupPath + @"\Guid\" + _ColorSet + this.Name + ".xml";
                    if (File.Exists(XmlFileName) == true)
                    {
                        this.MainView.RestoreLayoutFromXml(XmlFileName);
                    }
                }
                catch (Exception ex)
                {
                    string aa = ex.Message;
                }
            }
        }

        /// <summary>
        /// 设置背景色
        /// </summary>
        private void SetBackColor(GridColumn column, bool isNull, bool isreadonly)
        {
            if (isreadonly == true)
            {
                column.AppearanceCell.BackColor = Color.FromArgb(242, 242, 243);
            }
            else
            {
                if (isNull.Equals(true) == false)
                {
                    column.AppearanceCell.BackColor = Color.Yellow;
                }
                else
                {
                    column.AppearanceCell.BackColor = Color.White;
                }
            }
        }

        #region 设计器专用

        /// <summary>
        ///  生成文本列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public GridColumn CreateRepositoryItemTextEditByDesign(GridView view, DataRow row)
        {
            GridColumn column = null;
            DevExpress.XtraEditors.Repository.RepositoryItemTextEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            //t.MaxLength = Convert.ToInt32(row["MaxLength"]);
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            if (string.IsNullOrWhiteSpace(row["PasswordChar"].ToString()) == true)
            {
                t.PasswordChar = '\0';
            }
            else
            {
                char c = '\0';
                if (true == char.TryParse(row["PasswordChar"].ToString().Substring(0, 1), out c))
                {
                    t.PasswordChar = c;
                }
                else
                {
                    t.PasswordChar = '*';
                }
            }
            column = new GridColumn();
            column.ColumnEdit = t;
            column.FieldName = row["sField"].ToString();
            return column;
        }

        /// <summary>
        ///  生成备注列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private GridColumn CreateRepositoryItemMemoEditByDesign(DataRow row)
        {
            GridColumn column = null;
            DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            column = new GridColumn();
            column.ColumnEdit = t;
            column.FieldName = row["sField"].ToString();
            return column;
        }

        /// <summary>
        ///  生成日期列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit CreateRepositoryItemDateEditByDesign(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemDateEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            return t;
        }

        /// <summary>
        ///  生成时间列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit CreateRepositoryItemTimeEditByDesign(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            return t;
        }

        /// <summary>
        ///  生成下拉框列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit CreateRepositoryItemGridLookUpEditByDesign(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            t.QueryPopUp += new CancelEventHandler(RepositoryItemQueryPopUp);
            t.Popup += new EventHandler(RepositoryItemPopup);
            t.Closed += new ClosedEventHandler(RepositoryItemClosed);
            t.CloseUp += new CloseUpEventHandler(RepositoryItemCloseUp);
            GridView view = new GridView();
            t.View = view;
            return t;
        }

        /// <summary>
        ///  生成复选框列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit CreateRepositoryItemCheckEditByDesign(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            return t;
        }

        /// <summary>
        ///  生成单选框列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup CreateRepositoryItemRadioGroupByDesign(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup t = new DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            return t;
        }

        /// <summary>
        ///  生成图片框列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit CreateRepositoryItemPictureEditByDesign(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            return t;
        }

        /// <summary>
        ///  生成超链接列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit CreateRepositoryItemHyperLinkEditByDesign(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            return t;
        }

        /// <summary>
        ///  生成无数据源下拉框列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox CreateRepositoryItemNoSourceComboBoxByDesign(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemComboBox t = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            t.ButtonClick += new ButtonPressedEventHandler(RepositoryItemButtonClick);
            return t;
        }

        #endregion


        /// <summary>
        ///  生成文本列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit CreateRepositoryItemTextEdit(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemTextEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            t.MaxLength = Convert.ToInt32(row["MaxLength"]);
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryTextEditItemDoubleClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            t.Tag = true;
            t.MouseUp += new MouseEventHandler(RepositoryItemMouseUp);
            t.Enter += new EventHandler(RepositoryItemEnter);

            if (string.IsNullOrWhiteSpace(row["PasswordChar"].ToString()) == true)
            {
                t.PasswordChar = '\0';
                t.AllowMouseWheel = row["KzxAllowMouseWheel"] == DBNull.Value ? false : Convert.ToBoolean(row["KzxAllowMouseWheel"]);
                t.Mask.UseMaskAsDisplayFormat = false;
                t.Mask.MaskType = row["KzxMaskType"] == DBNull.Value ? MaskType.None : XmlRow.KzxMaskTypeConverter(row["KzxMaskType"].ToString());
                t.Mask.EditMask = row["KzxEditMask"] == DBNull.Value ? string.Empty : row["KzxEditMask"].ToString();
            }
            else
            {
                char c = '\0';
                if (true == char.TryParse(row["PasswordChar"].ToString().Substring(0, 1), out c))
                {
                    t.PasswordChar = c;
                }
                else
                {
                    t.PasswordChar = '*';
                }
            }
            return t;
        }

        /// <summary>
        /// 生成计算器列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit CreateRepositoryItemCalcEdit(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);

            return t;
        }

        /// <summary>
        ///  生成备注列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit CreateRepositoryItemMemoEdit(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryMemoEditItemDoubleClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            t.KeyDown += new KeyEventHandler(RepositoryItemKeyDown);
            return t;
        }


        /// <summary>
        ///  生成日期列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit CreateRepositoryItemDateEdit(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemDateEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            t.AllowMouseWheel = row["KzxAllowMouseWheel"] == DBNull.Value ? false : Convert.ToBoolean(row["KzxAllowMouseWheel"]);
            t.Mask.MaskType = row["KzxMaskType"] == DBNull.Value ? MaskType.None : XmlRow.KzxMaskTypeConverter(row["KzxMaskType"].ToString());
            t.Mask.EditMask = row["KzxEditMask"] == DBNull.Value ? string.Empty : row["KzxEditMask"].ToString();
            t.Mask.UseMaskAsDisplayFormat = false;
            t.VistaEditTime = DefaultBoolean.True;
            return t;
        }

        /// <summary>
        ///  生成时间列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit CreateRepositoryItemTimeEdit(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            t.AllowMouseWheel = row["KzxAllowMouseWheel"] == DBNull.Value || string.IsNullOrEmpty(row["KzxAllowMouseWheel"].ToString()) ? false : Convert.ToBoolean(row["KzxAllowMouseWheel"]);
            t.Mask.MaskType = row["KzxMaskType"] == DBNull.Value || string.IsNullOrEmpty(row["KzxMaskType"].ToString()) ? MaskType.DateTime : XmlRow.KzxMaskTypeConverter(row["KzxMaskType"].ToString());
            t.Mask.EditMask = row["KzxEditMask"] == DBNull.Value || string.IsNullOrEmpty(row["KzxEditMask"].ToString()) ? "T" : row["KzxEditMask"].ToString();
            t.Mask.UseMaskAsDisplayFormat = false;
            return t;
        }

        /// <summary>
        ///  生成下拉框列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit CreateRepositoryItemGridLookUpEdit(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            t.QueryPopUp += new CancelEventHandler(RepositoryItemQueryPopUp);
            t.Popup += new EventHandler(RepositoryItemPopup);
            t.Closed += new ClosedEventHandler(RepositoryItemClosed);
            t.CloseUp += new CloseUpEventHandler(RepositoryItemCloseUp);
            GridView view = new GridView();
            t.View = view;
            t.NullText = string.Empty;
            t.View.OptionsBehavior.AutoPopulateColumns = true;

            t.AllowMouseWheel = row["KzxAllowMouseWheel"] == DBNull.Value ? false : Convert.ToBoolean(row["KzxAllowMouseWheel"]);
            t.Mask.MaskType = row["KzxMaskType"] == DBNull.Value ? MaskType.None : XmlRow.KzxMaskTypeConverter(row["KzxMaskType"].ToString());
            t.Mask.EditMask = row["KzxEditMask"] == DBNull.Value ? string.Empty : row["KzxEditMask"].ToString();
            t.Mask.UseMaskAsDisplayFormat = false;
            t.TextEditStyle = TextEditStyles.Standard;
            return t;
        }

        /// <summary>
        ///  生成带搜索功能的下拉框列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit CreateRepositoryItemSearchLookUpEdit(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            t.QueryPopUp += new CancelEventHandler(RepositoryItemQueryPopUp);
            t.Popup += new EventHandler(RepositoryItemPopup);
            t.Closed += new ClosedEventHandler(RepositoryItemClosed);
            t.CloseUp += new CloseUpEventHandler(RepositoryItemCloseUp);
            GridView view = new GridView();
            t.View = view;
            t.NullText = string.Empty;
            t.View.OptionsBehavior.AutoPopulateColumns = true;

            t.AllowMouseWheel = row["KzxAllowMouseWheel"] == DBNull.Value ? false : Convert.ToBoolean(row["KzxAllowMouseWheel"]);
            t.Mask.MaskType = row["KzxMaskType"] == DBNull.Value ? MaskType.None : XmlRow.KzxMaskTypeConverter(row["KzxMaskType"].ToString());
            t.Mask.EditMask = row["KzxEditMask"] == DBNull.Value ? string.Empty : row["KzxEditMask"].ToString();
            t.Mask.UseMaskAsDisplayFormat = false;
            t.TextEditStyle = TextEditStyles.Standard;
            return t;
        }

        /// <summary>
        ///  生成按钮下拉框列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit CreateRepositoryItemButtonEdit(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.ButtonClick += new ButtonPressedEventHandler(RepositoryItemButtonClick);

            t.AllowMouseWheel = row["KzxAllowMouseWheel"] == DBNull.Value ? false : Convert.ToBoolean(row["KzxAllowMouseWheel"]);
            t.Mask.MaskType = row["KzxMaskType"] == DBNull.Value ? MaskType.None : XmlRow.KzxMaskTypeConverter(row["KzxMaskType"].ToString());
            t.Mask.EditMask = row["KzxEditMask"] == DBNull.Value ? string.Empty : row["KzxEditMask"].ToString();
            t.Mask.UseMaskAsDisplayFormat = false;
            return t;
        }

        /// <summary>
        ///  生成复选框列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit CreateRepositoryItemCheckEdit(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);

            t.AllowMouseWheel = row["KzxAllowMouseWheel"] == DBNull.Value ? false : Convert.ToBoolean(row["KzxAllowMouseWheel"]);
            return t;
        }

        /// <summary>
        ///  生成单选框列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup CreateRepositoryItemRadioGroup(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup t = new DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);

            t.AllowMouseWheel = row["KzxAllowMouseWheel"] == DBNull.Value ? false : Convert.ToBoolean(row["KzxAllowMouseWheel"]);
            return t;
        }

        /// <summary>
        ///  生成图片框列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit CreateRepositoryItemPictureEdit(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);

            t.AllowMouseWheel = row["KzxAllowMouseWheel"] == DBNull.Value ? false : Convert.ToBoolean(row["KzxAllowMouseWheel"]);
            return t;
        }

        /// <summary>
        ///  生成超链接列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit CreateRepositoryItemHyperLinkEdit(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);

            t.AllowMouseWheel = row["KzxAllowMouseWheel"] == DBNull.Value ? false : Convert.ToBoolean(row["KzxAllowMouseWheel"]);
            t.Mask.MaskType = row["KzxMaskType"] == DBNull.Value ? MaskType.None : XmlRow.KzxMaskTypeConverter(row["KzxMaskType"].ToString());
            t.Mask.EditMask = row["KzxEditMask"] == DBNull.Value ? string.Empty : row["KzxEditMask"].ToString();
            t.Mask.UseMaskAsDisplayFormat = false;
            return t;
        }

        /// <summary>
        ///  生成无数据源下拉框列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox CreateRepositoryItemNoSourceComboBox(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemComboBox t = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            t.ButtonClick += new ButtonPressedEventHandler(RepositoryItemButtonClick);
            return t;
        }

        #endregion

        #region 事件处理

        protected virtual void UserClick(object sender, EventArgs e)
        {
            RaiseEvent(sender, "Click", e);
        }

        protected virtual void UserDoubleClick(object sender, EventArgs e)
        {
            RaiseEvent(sender, "DoubleClick", e);
        }

        protected virtual void UserEditValueChanged(object sender, EventArgs e)
        {
            RaiseEvent(sender, "EditValueChanged", e);
        }

        protected virtual void UserEditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            RaiseEvent(sender, "EditValueChanging", e);
        }

        protected virtual void UserKeyDown(object sender, KeyEventArgs e)
        {
            RaiseEvent(sender, "KeyDown", e);
        }

        protected virtual void UserKeyUp(object sender, KeyEventArgs e)
        {
            RaiseEvent(sender, "KeyUp", e);
        }

        protected virtual void UserKeyPress(object sender, KeyPressEventArgs e)
        {
            RaiseEvent(sender, "KeyPress", e);
        }

        protected virtual void UserGotFocus(object sender, EventArgs e)
        {
            RaiseEvent(sender, "GotFocus", e);
        }

        protected virtual void UserLostFocus(object sender, EventArgs e)
        {
            RaiseEvent(sender, "LostFocus", e);
        }

        protected virtual void UserEnter(object sender, EventArgs e)
        {
            RaiseEvent(sender, "Enter", e);
        }

        protected virtual void UserLeave(object sender, EventArgs e)
        {
            RaiseEvent(sender, "Leave", e);
        }

        protected virtual void UserTextChanged(object sender, EventArgs e)
        {
            RaiseEvent(sender, "TextChanged", e);
        }

        protected virtual void UserValidating(object sender, CancelEventArgs e)
        {
            RaiseEvent(sender, "Validating", e);
        }

        protected virtual void UserVisibleChanged(object sender, EventArgs e)
        {
            RaiseEvent(sender, "VisibleChanged", e);
        }

        protected virtual void UserButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            RaiseEvent(sender, "ButtonClick", e);
        }

        protected virtual void UserClosed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            RaiseEvent(sender, "Closed", e);
        }

        protected virtual void UserCloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            RaiseEvent(sender, "CloseUp", e);
        }

        protected virtual void UserCheckedChanged(object sender, EventArgs e)
        {
            RaiseEvent(sender, "CheckedChanged", e);
        }

        protected virtual void UserCheckStateChanged(object sender, EventArgs e)
        {
            RaiseEvent(sender, "CheckStateChanged", e);
        }

        protected virtual void UserItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            RaiseEvent(sender, "ItemCheck", e);
        }

        protected virtual void UserItemChecking(object sender, DevExpress.XtraEditors.Controls.ItemCheckingEventArgs e)
        {
            RaiseEvent(sender, "ItemChecking", e);
        }

        protected virtual void UserSelectedIndexChanged(object sender, EventArgs e)
        {
            RaiseEvent(sender, "SelectedIndexChanged", e);
        }

        protected virtual void UserSelectedValueChanged(object sender, EventArgs e)
        {
            RaiseEvent(sender, "SelectedValueChanged", e);
        }

        protected virtual void UserImageChanged(object sender, EventArgs e)
        {
            RaiseEvent(sender, "ImageChanged", e);
        }

        protected virtual void UserDateTimeChanged(object sender, EventArgs e)
        {
            RaiseEvent(sender, "DateTimeChanged", e);
        }

        protected virtual void UserCloseButtonClick(object sender, EventArgs e)
        {
            RaiseEvent(sender, "CloseButtonClick", e);
        }

        protected virtual void UserSelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            RaiseEvent(sender, "SelectedPageChanged", e);
        }

        protected virtual void UserSelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {
            RaiseEvent(sender, "SelectedPageChanging", e);
        }

        protected virtual void UserEditorKeyDown(object sender, KeyEventArgs e)
        {
            RaiseEvent(sender, "EditorKeyDown", e);
        }

        protected virtual void UserEditorKeyPress(object sender, KeyPressEventArgs e)
        {
            RaiseEvent(sender, "EditorKeyPress", e);
        }

        protected virtual void UserCellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            RaiseEvent(sender, "CellValueChanged", e);
        }

        protected virtual void UserCellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            RaiseEvent(sender, "CellValueChanging", e);
        }

        protected virtual void UserColumnChanged(object sender, EventArgs e)
        {
            RaiseEvent(sender, "ColumnChanged", e);
        }

        protected virtual void UserAfterFocusNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            RaiseEvent(sender, "AfterFocusNode", e);
        }

        protected virtual void UserAfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            RaiseEvent(sender, "AfterCheckNode", e);
        }

        protected virtual void UserShowingEditor(object sender, CancelEventArgs e)
        {
            RaiseEvent(sender, "ShowingEditor", e);
        }

        private void UserShownEditor(object sender, EventArgs e)
        {
            this.RaiseEvent(sender, "ShownEditor", e);
        }

        #endregion

        /// <summary>
        /// 控件被加载后调用的方法
        /// 此方法在控件还原后被窗口调用
        /// </summary>
        public virtual void KzxControlLoaded()
        {
            //加载后事件
            RaiseEvent(this, "KzxControlLoaded", new EventArgs());
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="valueControl">控件</param>
        /// <param name="eventInfoTable">事件信息表</param>
        public virtual void BindingEvent(Control valueControl, DataTable eventInfoTable)
        {
            //以下为表的格式
            //dt.Columns.Add("uGuid1", typeof(string));    //Sys_FrmXml的uGuid1
            //dt.Columns.Add("uGuid2", typeof(string));    //主键
            //dt.Columns.Add("sFormName", typeof(string));    //窗口名称
            //dt.Columns.Add("sKey", typeof(string));      //控件的Key属性
            //dt.Columns.Add("sEventName", typeof(string));    //事件名称
            //dt.Columns.Add("sFileName", typeof(string)); //插件的dll文件名
            //dt.Columns.Add("sMark", typeof(string));    //备注

            EventInfo ei = null;
            Type eventtype = null;
            Delegate handler = null;
            MethodInfo mi = null;
            string eventname = string.Empty;
            DataRow[] rows = null;

            if (this.PluginInfoTable == null || valueControl == null)
            {
                return;
            }
            rows = eventInfoTable.Select("sKey='" + this.Key + "'");
            if (valueControl != null)
            {
                for (int i = 0; i < rows.Length; i++)
                {
                    eventname = rows[i]["sEventName"] == DBNull.Value ? string.Empty : rows[i]["sEventName"].ToString();
                    if (string.IsNullOrWhiteSpace(eventname) == true)
                    {
                        continue;
                    }
                    ei = valueControl.GetType().GetEvent(eventname, BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                    if (ei != null)
                    {
                        eventtype = ei.EventHandlerType;
                        if (eventtype != null)
                        {
                            mi = this.GetType().GetMethod("User" + eventname, BindingFlags.CreateInstance | BindingFlags.Default | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                            if (mi != null)
                            {
                                try
                                {
                                    handler = Delegate.CreateDelegate(eventtype, this, mi);
                                    if (handler != null)
                                    {
                                        ei.RemoveEventHandler(valueControl, handler);
                                        ei.AddEventHandler(valueControl, handler);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="eventInfoTable">事件信息表</param>
        public virtual void BindingEvent(DataTable eventInfoTable)
        {
            BindingEvent(this, eventInfoTable);
        }

        private void InitializeComponent()
        {

            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this;
            this.gridView1.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gridView1.Name = "gridView1";
            this.gridView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            // 
            // KzxGridControl
            // 
            this.MainView = this.gridView1;
            this.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 显示和更新明细打备注
        /// </summary>
        /// <param name="pAllowEdit">单据是否允许编辑</param>
        /// <param name="pFrmName">当前模块编号</param>
        /// <param name="pDetailView">当前网格</param>
        private void ShowUpdateDetailedRemark(bool pAllowEdit, string pFrmName, GridView pDetailView, string pFieldName, Int32 maxLength = 0)
        { 

        }
    }

    #region 列表报表网格状态设置回调 HHL modify 20170809
    public class CheckedFilterParam
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string ColumnCode { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 父级字段名
        /// </summary>
        public string ColumnParentCode { get; set; }

        /// <summary>
        /// 是否勾选了“过滤”
        /// </summary>
        public bool IsFilter { get; set; }

        /// <summary>
        /// 是否勾选了“关键字”
        /// </summary>
        public bool IsSearchKeyField { get; set; }
    }

    public class ThrowCheckFilterParamEventArgs : EventArgs
    {
        public List<CheckedFilterParam> FilterParamList { get; set; }

        public ThrowCheckFilterParamEventArgs(List<CheckedFilterParam> filterParamList)
        {
            FilterParamList = filterParamList;
        }
    }

    public delegate void DelegaeOnThrowCheckFilterParam(ThrowCheckFilterParamEventArgs e);
    #endregion

    /// <summary>
    /// 列类型
    /// </summary>
    public enum ColumnType
    {
        /// <summary>
        /// 文本
        /// </summary>
        Text = 0,
        /// <summary>
        /// 日期
        /// </summary>
        Date = 1,
        /// <summary>
        /// 时间
        /// </summary>
        Time = 2,
        /// <summary>
        /// 下拉框
        /// </summary>
        ComboBox = 3,
        /// <summary>
        /// 复选
        /// </summary>
        CheckBox = 4,
        /// <summary>
        /// 单选
        /// </summary>
        RadioBox = 5,
        /// <summary>
        /// 图片
        /// </summary>
        PictureBox = 6,
        /// <summary>
        /// 超链接
        /// </summary>
        HyperLink = 7,
        /// <summary>
        /// 按钮下拉框
        /// </summary>
        ButtonEdit = 8,
        /// <summary>
        /// 无数据下拉框(ComboBoxEdit控件)
        /// </summary>
        NoSourceComboBox = 9,
        /// <summary>
        /// 备注列
        /// </summary>
        MemoEdit = 10,
        /// <summary>
        /// 计算编辑框
        /// </summary>
        CalcEdit = 11,
    }

    /// <summary>
    /// 设计器辅助类
    /// </summary>
    public class XmlRow : INotifyPropertyChanged
    {
        public XmlRow()
        {

        }

        private DataRowView _RowView = null;

        public DataRowView RowView
        {
            get
            {
                return this._RowView;
            }
            set
            {
                this._RowView = value;
            }
        }

        public static ColumnType KzxColumnTypeConverter(string s)
        {
            if (s.Equals("Text", StringComparison.OrdinalIgnoreCase) == true)
            {
                return ColumnType.Text;
            }
            else if (s.Equals("Date", StringComparison.OrdinalIgnoreCase) == true)
            {
                return ColumnType.Date;
            }
            else if (s.Equals("Time", StringComparison.OrdinalIgnoreCase) == true)
            {
                return ColumnType.Time;
            }
            else if (s.Equals("ComboBox", StringComparison.OrdinalIgnoreCase) == true)
            {
                return ColumnType.ComboBox;
            }
            else if (s.Equals("CheckBox", StringComparison.OrdinalIgnoreCase) == true)
            {
                return ColumnType.CheckBox;
            }
            else if (s.Equals("RadioBox", StringComparison.OrdinalIgnoreCase) == true)
            {
                return ColumnType.RadioBox;
            }
            else if (s.Equals("PictureBox", StringComparison.OrdinalIgnoreCase) == true)
            {
                return ColumnType.PictureBox;
            }
            else if (s.Equals("HyperLink", StringComparison.OrdinalIgnoreCase) == true)
            {
                return ColumnType.HyperLink;
            }
            else if (s.Equals("ButtonEdit", StringComparison.OrdinalIgnoreCase) == true)
            {
                return ColumnType.ButtonEdit;
            }
            else if (s.Equals("NoSourceComboBox", StringComparison.OrdinalIgnoreCase) == true)
            {
                return ColumnType.NoSourceComboBox;
            }
            else if (s.Equals("MemoEdit", StringComparison.OrdinalIgnoreCase) == true)
            {
                return ColumnType.MemoEdit;
            }
            else if (s.Equals("CalcEdit", StringComparison.OrdinalIgnoreCase) == true)
            {
                return ColumnType.CalcEdit;
            }
            else
            {
                return ColumnType.Text;
            }
        }

        public static DevExpress.XtraGrid.Columns.FixedStyle FixedConverter(string s)
        {
            if (s.Equals("None", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.XtraGrid.Columns.FixedStyle.None;
            }
            else if (s.Equals("Left", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.XtraGrid.Columns.FixedStyle.Left;
            }
            else if (s.Equals("Right", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.XtraGrid.Columns.FixedStyle.Right;
            }
            else
            {
                return DevExpress.XtraGrid.Columns.FixedStyle.None;
            }
        }

        public static DevExpress.XtraTreeList.Columns.FixedStyle TreeListFixedConverter(string s)
        {
            if (s.Equals("None", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.XtraTreeList.Columns.FixedStyle.None;
            }
            else if (s.Equals("Left", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.XtraTreeList.Columns.FixedStyle.Left;
            }
            else if (s.Equals("Right", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.XtraTreeList.Columns.FixedStyle.Right;
            }
            else
            {
                return DevExpress.XtraTreeList.Columns.FixedStyle.None;
            }
        }

        public static DevExpress.Utils.FormatType KzxFormatTypeConverter(string s)
        {
            if (s.Equals("None", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.Utils.FormatType.None;
            }
            else if (s.Equals("Custom", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.Utils.FormatType.Custom;
            }
            else if (s.Equals("DateTime", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.Utils.FormatType.DateTime;
            }
            else if (s.Equals("Numeric", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.Utils.FormatType.Numeric;
            }
            else
            {
                return DevExpress.Utils.FormatType.None;
            }
        }

        public static MaskType KzxMaskTypeConverter(string s)
        {
            if (s.Equals("None", StringComparison.OrdinalIgnoreCase) == true)
            {
                return MaskType.None;
            }
            else if (s.Equals("Custom", StringComparison.OrdinalIgnoreCase) == true)
            {
                return MaskType.Custom;
            }
            else if (s.Equals("DateTime", StringComparison.OrdinalIgnoreCase) == true)
            {
                return MaskType.DateTime;
            }
            else if (s.Equals("DateTimeAdvancingCaret", StringComparison.OrdinalIgnoreCase) == true)
            {
                return MaskType.DateTimeAdvancingCaret;
            }
            else if (s.Equals("Numeric", StringComparison.OrdinalIgnoreCase) == true)
            {
                return MaskType.Numeric;
            }
            else if (s.Equals("RegEx", StringComparison.OrdinalIgnoreCase) == true)
            {
                return MaskType.RegEx;
            }
            else if (s.Equals("Regular", StringComparison.OrdinalIgnoreCase) == true)
            {
                return MaskType.Regular;
            }
            else if (s.Equals("Simple", StringComparison.OrdinalIgnoreCase) == true)
            {
                return MaskType.Simple;
            }
            else
            {
                return MaskType.None;
            }
        }

        public static DevExpress.Data.SummaryItemType KzxSummaryItemTypeConverter(string s)
        {
            if (s.Equals("None", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.Data.SummaryItemType.None;
            }
            else if (s.Equals("Sum", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.Data.SummaryItemType.Sum;
            }
            else if (s.Equals("Average", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.Data.SummaryItemType.Average;
            }
            else if (s.Equals("Count", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.Data.SummaryItemType.Count;
            }
            else if (s.Equals("Max", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.Data.SummaryItemType.Max;
            }
            else if (s.Equals("Min", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.Data.SummaryItemType.Min;
            }
            else if (s.Equals("Custom", StringComparison.OrdinalIgnoreCase) == true)
            {
                return DevExpress.Data.SummaryItemType.Custom;
            }
            else
            {
                return DevExpress.Data.SummaryItemType.None;
            }
        }

        public static KzxDataType KzxDataTypeeConverter(string s)
        {
            if (s.Equals("Str", StringComparison.OrdinalIgnoreCase) == true)
            {
                return KzxDataType.Str;
            }
            else if (s.Equals("Int", StringComparison.OrdinalIgnoreCase) == true)
            {
                return KzxDataType.Int;
            }
            else if (s.Equals("Decimal", StringComparison.OrdinalIgnoreCase) == true)
            {
                return KzxDataType.Decimal;
            }
            else if (s.Equals("Double", StringComparison.OrdinalIgnoreCase) == true)
            {
                return KzxDataType.Double;
            }
            else if (s.Equals("Date", StringComparison.OrdinalIgnoreCase) == true)
            {
                return KzxDataType.Date;
            }
            else
            {
                return KzxDataType.Str;
            }
        }

        public static XmlRow Converter(DataRowView row)
        {
            XmlRow xmlrow = null;
            xmlrow = new XmlRow();
            xmlrow.RowView = row;
            xmlrow.ColIndex = Convert.ToInt32(row["ColIndex"]);
            xmlrow.Key = row["Key"].ToString();
            xmlrow.Field = row["Field"].ToString();
            xmlrow.MessageCode = row["MessageCode"].ToString();
            xmlrow.DesigeCaption = row["DesigeCaption"].ToString();
            xmlrow.ColumnType = XmlRow.KzxColumnTypeConverter(row["ColumnType"].ToString());
            xmlrow.SourceTable = row["SourceTable"].ToString();
            xmlrow.DisplayMemberPath = row["DisplayMemberPath"].ToString();
            xmlrow.SelectedValuePath = row["SelectedValuePath"].ToString();
            xmlrow.MaxLength = Convert.ToInt32(row["MaxLength"]);
            xmlrow.PasswordChar = row["PasswordChar"].ToString();
            xmlrow.ValidateGroup = row["ValidateGroup"].ToString();
            xmlrow.Fixed = XmlRow.FixedConverter(row["Fixed"].ToString());
            xmlrow.ReadOnly = row["ReadOnly"] == DBNull.Value ? false : Convert.ToBoolean(row["ReadOnly"]);
            xmlrow.IsNull = row["IsNull"] == DBNull.Value ? true : Convert.ToBoolean(row["IsNull"]);
            xmlrow.IsUnique = row["IsUnique"] == DBNull.Value ? false : Convert.ToBoolean(row["IsUnique"]);
            xmlrow.Enabled = row["Enabled"] == DBNull.Value ? true : Convert.ToBoolean(row["Enabled"]);
            xmlrow.Visible = row["Visible"] == DBNull.Value ? true : Convert.ToBoolean(row["Visible"]);
            xmlrow.Width = Convert.ToInt32(row["Width"]);
            xmlrow.FieldCaption = row["FieldCaption"].ToString();
            xmlrow.ParentField = row["ParentField"].ToString();
            xmlrow.ValueDependencyField = row["ValueDependencyField"].ToString();
            xmlrow.FilterString = row["FilterString"].ToString();
            xmlrow.DllName = row["DllName"].ToString();
            xmlrow.KzxMaskType = XmlRow.KzxMaskTypeConverter(row["KzxMaskType"].ToString());
            xmlrow.KzxEditMask = row["KzxEditMask"].ToString();
            xmlrow.KzxAllowMouseWheel = row["KzxAllowMouseWheel"] == DBNull.Value ? false : Convert.ToBoolean(row["KzxAllowMouseWheel"]);
            xmlrow.KzxSummaryItemType = XmlRow.KzxSummaryItemTypeConverter(row["KzxSummaryItemType"] == DBNull.Value ? "None" : row["KzxSummaryItemType"].ToString());
            xmlrow.DefaultValue = row["DefaultValue"].ToString();
            xmlrow.KzxFormatString = row["KzxFormatString"].ToString();
            xmlrow.KzxFormatType = XmlRow.KzxFormatTypeConverter(row["KzxFormatType"] == DBNull.Value ? "None" : row["KzxFormatType"].ToString());
            xmlrow.AllowValueRange = row["AllowValueRange"] == DBNull.Value ? false : Convert.ToBoolean(row["AllowValueRange"]);
            xmlrow.DataType = XmlRow.KzxDataTypeeConverter(row["DataType"].ToString());
            xmlrow.MaxValue = row["MaxValue"].ToString();
            xmlrow.MinValue = row["MinValue"].ToString();
            xmlrow.RegexString = row["RegexString"].ToString();
            xmlrow.ToolTipMessageCode = row["ToolTipMessageCode"].ToString();
            xmlrow.ToolTipText = row["ToolTipText"].ToString();
            xmlrow.AllowEdit = row["AllowEdit"] == DBNull.Value ? true : Convert.ToBoolean(row["AllowEdit"]);
            xmlrow.AllowSort = row["AllowSort"] == DBNull.Value ? true : Convert.ToBoolean(row["AllowSort"]);
            return xmlrow;
        }

        private bool _AllowEdit = true;
        /// <summary>
        /// 被引用后允许修改
        /// true允许,false不允许
        /// </summary>
        [Category("验证"), Description("AllowEdit,被引用后允许修改,true允许,false不允许"), Browsable(true)]
        [McDisplayName("AllowEdit")]
        public virtual bool AllowEdit
        {
            get
            {
                return this._AllowEdit;
            }
            set
            {
                this._AllowEdit = value;
            }
        }

        private int _ColIndex = 0;
        [Category("自定义"), Description("ColIndex,列下标"), Browsable(false)]
        public int ColIndex
        {
            get
            {
                return this._ColIndex;
            }
            set
            {
                this._ColIndex = value;
                OnPropertyChanged("ColIndex");
            }
        }

        private string _Key = string.Empty;
        /// <summary>
        /// 控件的唯一标识
        /// </summary>
        [Category("数据"), Description("Key,控件的唯一标识"), Browsable(true)]
        [McDisplayName("Key")]
        public virtual string Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
                OnPropertyChanged("Key");
            }
        }

        private string _Field = string.Empty;
        /// <summary>
        /// 数据库的字段名称
        /// </summary>
        [Category("数据"), Description("Field,数据库的字段名称"), Browsable(true)]
        [McDisplayName("Field")]
        public virtual string Field
        {
            get
            {
                return this._Field.Trim();
            }
            set
            {
                this._Field = value;
                OnPropertyChanged("Field");
            }
        }

        private string _MessageCode = "0";
        /// <summary>
        /// 多语言环境下显示文本的对应标识
        /// </summary>
        [Category("多语言"), Description("MessageCode,多语言环境下显示文本的对应标识"), Browsable(true)]
        [McDisplayName("MessageCode")]
        public virtual string MessageCode
        {
            get
            {
                return this._MessageCode.Trim();
            }
            set
            {
                this._MessageCode = value;
                OnPropertyChanged("MessageCode");
            }
        }

        private string _DesigeCaption = "显示标题";
        /// <summary>
        /// 没有多语言的情况下的默认显示标题
        /// </summary>
        [Category("多语言"), Description("DesigeCaption,没有多语言的情况下的默认显示标题"), Browsable(true)]
        [McDisplayName("DesigeCaption")]
        public virtual string DesigeCaption
        {
            get
            {
                return this._DesigeCaption;
            }
            set
            {
                this._DesigeCaption = value;
                OnPropertyChanged("DesigeCaption");
            }
        }

        private ColumnType _ColumnType = ColumnType.Text;
        /// <summary>
        /// 值范围验证中的数据类型
        /// </summary>
        [Category("外观"), Description("ColumnType,列类型"), Browsable(true)]
        [McDisplayName("ColumnType")]
        public virtual ColumnType ColumnType
        {
            get
            {
                return this._ColumnType;
            }
            set
            {
                this._ColumnType = value;
                OnPropertyChanged("ColumnType");
            }
        }

        private string _SourceTable = string.Empty;
        /// <summary>
        /// 源表名称
        /// </summary>
        [Category("下拉数据"), Description("SourceTable,源表名称"), Browsable(true), Editor(typeof(TextUiTypEdit), typeof(UITypeEditor))]
        [McDisplayName("SourceTable")]
        public virtual string SourceTable
        {
            get
            {
                return this._SourceTable.Trim();
            }
            set
            {
                this._SourceTable = value;
                OnPropertyChanged("SourceTable");
            }
        }

        private string _SelectedValuePath = string.Empty;
        /// <summary>
        /// 源表实际值字段
        /// </summary>
        [Category("下拉数据"), Description("SelectedValuePath,源表实际值字段"), Browsable(true)]
        [McDisplayName("SelectedValuePath")]
        public virtual string SelectedValuePath
        {
            get
            {
                return this._SelectedValuePath.Trim();
            }
            set
            {
                this._SelectedValuePath = value;
                OnPropertyChanged("SelectedValuePath");
            }
        }

        private string _DisplayMemberPath = string.Empty;
        /// <summary>
        /// 源表显示值字段
        /// </summary>
        [Category("下拉数据"), Description("DisplayMemberPath,源表显示值字段"), Browsable(true)]
        [McDisplayName("DisplayMemberPath")]
        public virtual string DisplayMemberPath
        {
            get
            {
                return this._DisplayMemberPath.Trim();
            }
            set
            {
                this._DisplayMemberPath = value;
                OnPropertyChanged("DisplayMemberPath");
            }
        }

        private int _MaxLength = 0;
        /// <summary>
        /// 可录入的最大长度
        /// </summary>
        [Category("验证"), Description("MaxLength,可录入的最大长度"), Browsable(true)]
        [McDisplayName("MaxLength")]
        public virtual int MaxLength
        {
            get
            {
                return this._MaxLength;
            }
            set
            {
                this._MaxLength = value;
                OnPropertyChanged("MaxLength");
            }
        }

        private bool _ReadOnly = false;
        /// <summary>
        /// 只读性
        /// </summary>
        [Category("验证"), Description("ReadOnly,只读性"), Browsable(true)]
        [McDisplayName("ReadOnly")]
        public virtual bool ReadOnly
        {
            get
            {
                return this._ReadOnly;
            }
            set
            {
                this._ReadOnly = value;
                OnPropertyChanged("ReadOnly");
            }
        }

        private bool _IsNull = true;
        /// <summary>
        /// 可空性
        /// </summary>
        [Category("验证"), Description("IsNull,可空性"), Browsable(true)]
        [McDisplayName("IsNull")]
        public virtual bool IsNull
        {
            get
            {
                return this._IsNull;
            }
            set
            {
                this._IsNull = value;
                OnPropertyChanged("IsNull");
            }
        }

        private bool _IsUnique = false;
        /// <summary>
        /// 唯一性
        /// </summary>
        [Category("验证"), Description("IsUnique,唯一性"), Browsable(true)]
        [McDisplayName("IsUnique")]
        public virtual bool IsUnique
        {
            get
            {
                return this._IsUnique;
            }
            set
            {
                this._IsUnique = value;
                OnPropertyChanged("IsUnique");
            }
        }

        private bool _AllowValueRange = false;
        /// <summary>
        /// 是否启用值范围验证
        /// </summary>
        [Category("验证"), Description("AllowValueRange,是否启用值范围验证"), Browsable(true)]
        [McDisplayName("AllowValueRange")]
        public virtual bool AllowValueRange
        {
            get
            {
                return this._AllowValueRange;
            }
            set
            {
                this._AllowValueRange = value;
            }
        }

        private KzxDataType _DataType = KzxDataType.Str;
        /// <summary>
        /// 值范围验证中的数据类型
        /// </summary>
        [Category("验证"), Description("DataType,值范围验证中的数据类型"), Browsable(true)]
        [McDisplayName("DataType")]
        public virtual KzxDataType DataType
        {
            get
            {
                return this._DataType;
            }
            set
            {
                this._DataType = value;
            }
        }

        private string _MaxValue = string.Empty;
        /// <summary>
        /// 值范围验证中最大值
        /// </summary>
        [Category("验证"), Description("MaxValue,值范围验证中最大值"), Browsable(true)]
        [McDisplayName("MaxValue")]
        public virtual string MaxValue
        {
            get
            {
                return this._MaxValue;
            }
            set
            {
                this._MaxValue = value;
            }
        }

        private string _MinValue = string.Empty;
        /// <summary>
        /// 值范围验证中最小值
        /// </summary>
        [Category("验证"), Description("MinValue,值范围验证中最大值"), Browsable(true)]
        [McDisplayName("MinValue")]
        public virtual string MinValue
        {
            get
            {
                return this._MinValue;
            }
            set
            {
                this._MinValue = value;
            }
        }

        private string _RegexString = string.Empty;
        /// <summary>
        /// 正则表达式验证
        /// </summary>
        [Category("验证"), Description("RegexString,正则表达式验证"), Browsable(true)]
        [McDisplayName("RegexString")]
        public virtual string RegexString
        {
            get
            {
                return this._RegexString;
            }
            set
            {
                this._RegexString = value;
            }
        }

        private string _ValidateGroup = string.Empty;
        /// <summary>
        /// 唯一性验证组别
        /// </summary>
        [Category("验证"), Description("ValidateGroup,唯一性验证组别"), Browsable(false)]
        [McDisplayName("ValidateGroup")]
        public virtual string ValidateGroup
        {
            get
            {
                return this._ValidateGroup;
            }
            set
            {
                this._ValidateGroup = value;
                OnPropertyChanged("ValidateGroup");
            }
        }

        private bool _Enabled = false;
        /// <summary>
        /// 可用性
        /// </summary>
        [Category("外观"), Description("Enabled,可用性"), Browsable(true)]
        [McDisplayName("Enabled")]
        public virtual bool Enabled
        {
            get
            {
                return this._Enabled;
            }
            set
            {
                this._Enabled = value;
                OnPropertyChanged("Enabled");
            }
        }

        private bool _AllowSort = true;
        /// <summary>
        /// 可用性
        /// </summary>
        [Category("外观"), Description("AllowSort,点列头排序"), Browsable(true)]
        [McDisplayName("AllowSort")]
        public virtual bool AllowSort
        {
            get
            {
                return this._AllowSort;
            }
            set
            {
                this._AllowSort = value;
                OnPropertyChanged("AllowSort");
            }
        }


        private bool _Visible = false;
        /// <summary>
        /// 可见性
        /// </summary>
        [Category("外观"), Description("Visible,可见性"), Browsable(true)]
        [McDisplayName("Visible")]
        public virtual bool Visible
        {
            get
            {
                return this._Visible;
            }
            set
            {
                this._Visible = value;
                OnPropertyChanged("Visible");
            }
        }

        private string _PasswordChar = string.Empty;
        /// <summary>
        /// 密码显示字符
        /// </summary>
        [Category("密码"), Description("PasswordChar,密码显示字符"), Browsable(true)]
        [McDisplayName("PasswordChar")]
        public virtual string PasswordChar
        {
            get
            {
                return this._PasswordChar;
            }
            set
            {
                this._PasswordChar = value;
                OnPropertyChanged("PasswordChar");
            }
        }

        private DevExpress.XtraGrid.Columns.FixedStyle _Fixed = DevExpress.XtraGrid.Columns.FixedStyle.None;
        /// <summary>
        /// 列冻结
        /// </summary>
        [Category("外观"), Description("Fixed,列冻结"), Browsable(true)]
        [McDisplayName("Fixed")]
        public virtual DevExpress.XtraGrid.Columns.FixedStyle Fixed
        {
            get
            {
                return this._Fixed;
            }
            set
            {
                this._Fixed = value;
                OnPropertyChanged("Fixed");
            }
        }

        private string _FieldCaption = string.Empty;
        /// <summary>
        /// 下拉框列的显示标题
        /// </summary>
        [Category("下拉数据"), Description("FieldCaption,下拉框列的显示标题,格式:列名=语言标识|1或0,其中1表示列显示,0表示列不显示,如：code=MSG0001|0,name=MSG0002|1"), Browsable(true), Editor(typeof(TextUiTypEdit), typeof(UITypeEditor))]
        [McDisplayName("FieldCaption")]
        public virtual string FieldCaption
        {
            get
            {
                return this._FieldCaption.Trim();
            }
            set
            {
                this._FieldCaption = value;
                OnPropertyChanged("FieldCaption");
            }
        }

        private string _ParentField = string.Empty;
        /// <summary>
        /// 上级携带字段
        /// </summary>
        [Category("下拉数据"), Description("ParentField,对应Field中的值,表示当前字段的从ParentField表示字段的下拉数据源中获取的,实现字段间的值携带"), Browsable(true)]
        [McDisplayName("ParentField")]
        public virtual string ParentField
        {
            get
            {
                return this._ParentField.Trim();
            }
            set
            {
                this._ParentField = value;
                OnPropertyChanged("ParentField");
            }
        }

        private string _ValueDependencyField = string.Empty;
        /// <summary>
        /// 字段值来源字段
        /// </summary>
        [Category("下拉数据"), Description("ValueDependencyField,格式:网格Field=下拉数据源的Field,表示网格中的Field对应的列的值来自于下拉数据源中的Field对应列,多个表达式可以用逗号分隔.如c=code,n=name"), Browsable(true), Editor(typeof(TextUiTypEdit), typeof(UITypeEditor))]
        [McDisplayName("ValueDependencyField")]
        public virtual string ValueDependencyField
        {
            get
            {
                return this._ValueDependencyField.Trim();
            }
            set
            {
                this._ValueDependencyField = value;
                OnPropertyChanged("ValueDependencyField");
            }
        }

        private int _Width = 80;
        /// <summary>
        /// 列宽
        /// </summary>
        [Category("外观"), Description("Width,列宽"), Browsable(true)]
        [McDisplayName("Width")]
        public virtual int Width
        {
            get
            {
                return this._Width;
            }
            set
            {
                this._Width = value;
                OnPropertyChanged("Width");
            }
        }

        private string _FilterString = string.Empty;
        /// <summary>
        /// 下拉数据源的过滤表达式
        /// </summary>
        [Category("下拉数据"), Description("FilterString,下拉数据源的过滤表达式,如:下拉数据源中的列=单据的表|表中的字段名.如：code=mastertable|code,name=detailtable|name表示当前的下拉框的过滤条件为code=主表mastertable中的code字段, name=明细表中当前行的name"), Browsable(true), Editor(typeof(TextUiTypEdit), typeof(UITypeEditor))]
        [McDisplayName("FilterString")]
        public virtual string FilterString
        {
            get
            {
                return this._FilterString.Trim();
            }
            set
            {
                this._FilterString = value;
                OnPropertyChanged("FilterString");
            }
        }

        private string _DllName = string.Empty;
        /// <summary>
        /// 自定义弹出窗口的配置关键字
        /// </summary>
        [Category("弹出窗口"), Description("DllName,自定义弹出窗口的配置关键字,配置后系统将采用自定义弹出窗口处理数据"), Browsable(true), Editor(typeof(TextUiTypEdit), typeof(UITypeEditor))]
        [McDisplayName("DllName")]
        public virtual string DllName
        {
            get
            {
                return this._DllName.Trim();
            }
            set
            {
                this._DllName = value;
                OnPropertyChanged("DllName");
            }
        }

        private bool _KzxAllowMouseWheel = false;
        /// <summary>
        /// 允许鼠标滚动轮滚动时修改数据
        /// </summary>
        [Category("外观"), Description("KzxAllowMouseWheel,允许鼠标滚动轮滚动时修改数据"), Browsable(true)]
        [McDisplayName("KzxAllowMouseWheel")]
        public virtual bool KzxAllowMouseWheel
        {
            get
            {
                return this._KzxAllowMouseWheel;
            }
            set
            {
                this._KzxAllowMouseWheel = value;
                OnPropertyChanged("KzxAllowMouseWheel");
            }
        }

        private string _KzxEditMask = null;
        /// <summary>
        /// 格式掩码
        /// </summary>
        [Category("数据格式"), Description("KzxEditMask,掩码格式"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [McDisplayName("KzxEditMask")]
        public virtual string KzxEditMask
        {
            get
            {
                return this._KzxEditMask;
            }
            set
            {
                this._KzxEditMask = value;
                OnPropertyChanged("KzxEditMask");
            }
        }

        private MaskType _KzxMaskType = MaskType.None;
        /// <summary>
        /// 掩码验证类型
        /// </summary>
        [Category("数据格式"), Description("KzxMaskType,掩码验证类型"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [McDisplayName("KzxMaskType")]
        public virtual MaskType KzxMaskType
        {
            get
            {
                return this._KzxMaskType;
            }
            set
            {
                this._KzxMaskType = value;
                OnPropertyChanged("KzxMaskType");
            }
        }

        private DevExpress.Data.SummaryItemType _KzxSummaryItemType = DevExpress.Data.SummaryItemType.None;
        /// <summary>
        /// 合计类型
        /// </summary>
        [Category("数据"), Description("KzxSummaryItemType,合计类型"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [McDisplayName("KzxSummaryItemType")]
        public virtual DevExpress.Data.SummaryItemType KzxSummaryItemType
        {
            get
            {
                return this._KzxSummaryItemType;
            }
            set
            {
                this._KzxSummaryItemType = value;
                OnPropertyChanged("KzxSummaryItemType");
            }
        }

        private string _DefaultValue = string.Empty;
        /// <summary>
        /// 默认值
        /// </summary>
        [Category("数据"), Description("DefaultValue,控件的默认值"), Browsable(true)]
        [McDisplayName("DefaultValue")]
        public virtual String DefaultValue
        {
            get
            {
                return this._DefaultValue;
            }
            set
            {
                this._DefaultValue = value;
                OnPropertyChanged("DefaultValue");
            }
        }

        private string _KzxFormatString = string.Empty;
        /// <summary>
        /// 显示格式模式字符串(不验证录入)
        /// </summary>
        [Category("数据格式"), Description("KzxFormatString,显示模式字符串(不验证录入数据)"), Browsable(true)]
        [McDisplayName("KzxFormatString")]
        public virtual string KzxFormatString
        {
            get
            {
                return this._KzxFormatString;
            }
            set
            {
                this._KzxFormatString = value;
                OnPropertyChanged("KzxFormatString");
            }
        }

        private DevExpress.Utils.FormatType _KzxFormatType = DevExpress.Utils.FormatType.None;
        /// <summary>
        /// 显示格式模式类型(不验证录入数据)
        /// </summary>
        [Category("数据格式"), Description("KzxFormatType,显示格式模式类型(不验证录入数据)"), Browsable(true)]
        [McDisplayName("KzxFormatType")]
        public virtual DevExpress.Utils.FormatType KzxFormatType
        {
            get
            {
                return this._KzxFormatType;
            }
            set
            {
                this._KzxFormatType = value;
                OnPropertyChanged("KzxFormatType");
            }
        }

        private string _ToolTipText = string.Empty;
        /// <summary>
        /// 提示信息
        /// </summary>
        [Category("汽泡提示"), Description("ToolTipText,提示信息"), Browsable(true)]
        [McDisplayName("ToolTipText")]
        public virtual string ToolTipText
        {
            get
            {
                return this._ToolTipText;
            }
            set
            {
                this._ToolTipText = value;
            }
        }


        private string _ToolTipMessageCode = string.Empty;
        /// <summary>
        /// 提示多语言标识
        /// </summary>
        [Category("汽泡提示"), Description("ToolTipMessageCode,提示信息多语言标识"), Browsable(true)]
        [McDisplayName("ToolTipMessageCode")]
        public virtual string ToolTipMessageCode
        {
            get
            {
                return this._ToolTipMessageCode;
            }
            set
            {
                this._ToolTipMessageCode = value;
            }
        }


        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class KzxBandedGridView
    {
        private KzxGridControl _KzxGridControl = null;

        public KzxGridControl KzxGdControl
        {
            get
            {
                return this._KzxGridControl;
            }
            set
            {
                this._KzxGridControl = value;
            }
        }

        private string _SplitStr = "|";
        /// <summary>
        /// 列头的分隔符,默认为"|"
        /// </summary>
        public string SplitStr
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this._SplitStr) == true)
                {
                    this._SplitStr = "|";
                }
                return this._SplitStr;
            }
            set
            {
                this._SplitStr = value;
            }
        }

        private BandedGridView _View = new BandedGridView();
        /// <summary>
        /// 视图对象
        /// </summary>
        public BandedGridView View
        {
            get
            {
                return this._View;
            }
            set
            {
                this._View = value;
            }
        }

        private Dictionary<string, string> _Data = null;
        /// <summary>
        /// 列字典
        /// </summary>
        public Dictionary<string, string> Data
        {
            get
            {
                return this._Data;
            }
            set
            {
                this._Data = value;
            }
        }

        /// <summary>
        /// 构造视图
        /// </summary>
        /// <returns>BandedGridView对象</returns>
        public BandedGridView CreateBandedGridView()
        {

            BandedGridColumn column = null;
            GridBand band = null;
            Dictionary<string, BandedGridColumn> columndictionary = new Dictionary<string, BandedGridColumn>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, GridBand> banddictionary = new Dictionary<string, GridBand>(StringComparer.OrdinalIgnoreCase);
            string[] headers = null;
            KeyValuePair<string, string> kvp;
            IEnumerator enumrator;
            bool visible = true;
            bool bandvisible = true;

            if (this.Data != null)
            {
                enumrator = this.Data.GetEnumerator();
                while (enumrator.MoveNext())
                {
                    kvp = (KeyValuePair<string, string>)enumrator.Current;
                    headers = kvp.Value.Split(new string[] { this.SplitStr }, StringSplitOptions.RemoveEmptyEntries);
                    for (int k = 0; k < headers.Length; k++)
                    {
                        if (banddictionary.ContainsKey(headers[k]) == true)
                        {
                            band = banddictionary[headers[k]];
                        }
                        else
                        {
                            band = new GridBand();
                            band.Caption = headers[k];
                            band.AppearanceHeader.Options.UseTextOptions = true;
                            band.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            band.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                            if (k == 0)
                            {
                                banddictionary.Add(headers[k], band);
                                this._View.Bands.Add(band);
                            }
                            else
                            {
                                if (k < headers.Length - 1)
                                {
                                    banddictionary[headers[k - 1]].Children.Add(band);
                                    banddictionary.Add(headers[k], band);
                                }
                            }
                        }

                        if (k == headers.Length - 1)
                        {
                            column = this.View.Columns[kvp.Key];
                            visible = column.Visible;
                            columndictionary.Add(kvp.Value, column);
                            band = banddictionary[headers[k - 1 > 0 ? k - 1 : 0]];
                            band.Columns.Add(column);
                            for (int kk = 0; kk < band.Columns.Count; kk++)
                            {
                                visible = visible | band.Columns[kk].Visible;
                            }
                            band.Visible = visible;
                            SetBandVisible(band);
                        }
                    }
                }
            }

            return this._View;
        }

        private void SetBandVisible(GridBand band)
        {
            GridBand p = null;
            bool visible = band.Visible;

            if (band.ParentBand != null)
            {
                p = band.ParentBand;
                for (int i = 0; i < p.Children.Count; i++)
                {
                    visible = visible | p.Children[i].Visible;
                }
                p.Visible = visible;
                SetBandVisible(p);
            }
        }

        /// <summary>
        /// 构造视图
        /// </summary>
        /// <param name="dt">列字典</param>
        /// <returns>BandedGridView对象</returns>
        public BandedGridView CreateBandedGridView(Dictionary<string, string> dt)
        {
            this._Data = dt;
            return this.CreateBandedGridView();
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="readOnly">只读</param>
        /// <param name="view">网格</param>
        public KzxBandedGridView(Boolean readOnly, BandedGridView view, KzxGridControl g)
        {
            this._KzxGridControl = g;
            this._View = view;
            this._View.OptionsBehavior.AutoPopulateColumns = false;
            this._View.OptionsBehavior.ReadOnly = readOnly;
        }

    }

    public class KzxDragEventArgs : EventArgs
    {

        private List<DataRow> _SelectedRowList = new List<DataRow>();
        /// <summary>
        /// 选中的DataRow列表
        /// </summary>
        public List<DataRow> SelectedRowList
        {
            get
            {
                return this._SelectedRowList;
            }
            set
            {
                this._SelectedRowList = value;
            }
        }

        private int _TargetRowHandle = -1;
        /// <summary>
        /// 拖到的目标行的下标
        /// </summary>
        public int TargetRowHandle
        {
            get
            {
                return this._TargetRowHandle;
            }
            set
            {
                this._TargetRowHandle = value;
            }
        }

        public DragEventArgs _DragEventArgs = null;
        /// <summary>
        /// 拖放的参数
        /// </summary>
        public DragEventArgs DragEventArgs
        {
            get
            {
                return this._DragEventArgs;
            }
            set
            {
                this._DragEventArgs = value;
            }
        }


        /// <summary>
        /// 构造
        /// </summary>
        public KzxDragEventArgs()
        {
        }
    }
}
