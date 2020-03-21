using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTreeList; 
using DevExpress.XtraEditors.Repository;
using System.Xml;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList.ViewInfo;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Mask;
using System.Collections;
using DevExpress.Utils;
using System.Text.RegularExpressions; 
using DevExpress.XtraTreeList.Nodes;
using System.IO;
using Kzx.UserControl.UITypeEdit;
using Kzx.Common;
using Kzx.AppCore;

namespace Kzx.UserControl
{
    /// <summary>
    /// 树列表
    /// </summary>
    public partial class KzxTreeList : TreeList, IControl
    {
        #region 字段

        public static int ColumnCount = 0;
        private ToolTip _toolTip = new ToolTip();
        private ContextMenuStrip _contextMenu = new ContextMenuStrip();

        #endregion

        #region 事件&委托

        protected delegate void UpdatePropertyDelegate(object e);
        protected delegate void UpdateDelegate(object source);
        //protected delegate void UpdatePropertyDelegate(object e);
        //protected delegate void UpdateDelegate(object source);
        //protected delegate void UpdatePropertyDelegate(object e);
        //protected delegate void UpdateDelegate(object source);

        public event KzxControlOperateEventHandler KzxControlOperate;

        public event KzxGetLanguageEventHandler KzxGetLanguage;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造
        /// </summary>
        public KzxTreeList()
        {
            this.ContextMenuStrip = _contextMenu;

            this.OptionsView.AutoWidth = false;
            this.VertScrollVisibility = ScrollVisibility.Auto;

            this.BeforeCheckNode += new CheckNodeEventHandler(TreeList_BeforeCheckNode);
            this.CellValueChanged += new CellValueChangedEventHandler(KzxCellValueChanged);
            this.CellValueChanging += new CellValueChangedEventHandler(KzxCellValueChanging);

            LayoutControl();
            ContextMenuInit();
            BindingEvent(this, PluginInfoTable);
        }

        #endregion

        #region 控件·加载

        protected override void OnLoaded()
        {
            base.OnLoaded();
        }

        #endregion

        private void LayoutControl()
        {
            if (this.DesignMode == true)
            {
                DevExpress.XtraTreeList.Columns.TreeListColumn column;
                DevExpress.XtraTreeList.TreeList tmpTreeList = this as DevExpress.XtraTreeList.TreeList;
                this.CreateColumnByColumnInfo();
                int iCnt = tmpTreeList.Columns.Count;
                for (int j = 0; j < iCnt; j++)
                {
                    column = tmpTreeList.Columns[j];

                    DataRow[] rows = this.ColumnInfoTable.Select("Field='" + column.FieldName + "'");
                    if (rows.Length > 0)
                    {
                        column.Caption = GetLanguage(rows[0]["MessageCode"].ToString(), rows[0]["DesigeCaption"].ToString());
                    }
                }
            }
        }

        #region 属性

        /// <summary>
        /// 有Load方法
        /// </summary>
        private bool _HasLoad = false;
        public bool HasLoad
        {
            get
            {
                return this._HasLoad;
            }
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

        private string toolTipMaxLengthText = string.Empty;
        /// <summary>
        /// 数据长度不能超过数据库长度提示文本 add by huangyq20170519
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
                if (string.IsNullOrWhiteSpace(this.Table) == false && string.IsNullOrWhiteSpace(this.Field) == false)
                {
                    this._Key = this.Table + "." + this.Field;
                }
                else if (string.IsNullOrWhiteSpace(this.Table) == false)
                {
                    this._Key = this.Table;
                }
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
                //this.Visible = value;
                if (this.DesignMode == true)
                {
                }
                else
                {
                    this.Visible = value;
                }
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
                return !this.OptionsBehavior.Editable;
            }
            set
            {
                this.OptionsBehavior.Editable = !value;
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

        private string _ValueDependencyField = string.Empty;
        /// <summary>
        /// 数据携带
        /// </summary>
        [Category("下拉数据"), Description("ValueDependencyField,格式:控件Field=下拉数据源的Field,表示Field对应控件的值来自于下拉数据源中的Field对应列,多个表达式可以用逗号分隔.如c=code,n=name"), Browsable(true), Editor(typeof(TextUiTypEdit), typeof(UITypeEditor))]
        [McDisplayName("ValueDependencyField")]
        public virtual string ValueDependencyField
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
            set
            {
                this._ObjectList = value;
            }
        }

        private DataTable _PluginInfoTable = KzxBaseControl.CreatePluginDataTable();
        /// <summary>
        /// 插件表
        /// </summary>
        [Category("自定义"), Description("PluginInfoTable,插件表"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        /// <summary>
        /// 指定父表
        /// </summary>
        private string _ParentTable;
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

        private string _Columns = string.Empty;
        /// <summary>
        /// 列集合
        /// </summary>
        [Category("列"), Description("Columns,列集合"), Browsable(true), Editor(typeof(KzxGridControlColumnsUiTypeEdit), typeof(UITypeEditor))]
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
                if (this.DesignMode == true)
                {
                    CreateColumnByColumnInfo();
                }
            }
        }

        /// <summary>
        /// Dev控件集合
        /// </summary>
        [Category("自定义"), Description("Columns,列集合"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TreeListColumnCollection DevColumns
        {
            get
            {
                return base.Columns;
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

        #endregion

        #region 布局属性

        #region 列信息表

        private DataTable _ColumnInfoTable = null;
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

        #endregion

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

        /// <summary>
        /// 提示信息
        /// </summary>
        [Category("汽泡提示"), Description("ToolTipText,提示信息"), Browsable(true)]
        [McDisplayName("ToolTipText")]
        public virtual string ToolTipText
        {
            get
            {
                return this._toolTip.GetToolTip(this);
            }
            set
            {
                this._toolTip.SetToolTip(this, value);
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
        /// 父级结点标识字段
        /// </summary>
        [Category("数据"), Description("ParentFieldName,父级结点标识字段"), Browsable(true)]
        [McDisplayName("ParentFieldName")]
        public new string ParentFieldName
        {
            get
            {
                return base.ParentFieldName;
            }
            set
            {
                base.ParentFieldName = value;
            }
        }

        /// <summary>
        /// 本级结点标识字段
        /// </summary>
        [Category("数据"), Description("KeyFieldName,本级结点标识字段"), Browsable(true)]
        [McDisplayName("KeyFieldName")]
        public new string KeyFieldName
        {
            get
            {
                return base.KeyFieldName;
            }
            set
            {
                base.KeyFieldName = value;
            }
        }

        private void KzxCellValueChanged(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
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
        }

        private void KzxCellValueChanging(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            RaiseEvent(sender, "CellValueChanging", e);
        }

        /// <summary>
        /// 列的值的携带
        /// </summary>
        private void SetReferenceFieldValue(DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            DataRow[] rows = null;
            string[] fieldarray = null;
            string[] expressionarray = null;
            int rowindex = 0;

            if (this.ColumnInfoTable == null)
            {
                this.ColumnInfoTable = KzxGridControl.SerializeColumns(this.Columns);
            }
            rows = this.ColumnInfoTable.Select("Field='" + e.Column.FieldName + "'", "Field ASC", DataViewRowState.CurrentRows);
            if (rows.Length > 0)
            {
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

                                                    this.OnSetValue(this.FocusedNode, expressionarray[0], pi.GetValue(this.ColumnDataSourceDictionary[e.Column.FieldName][rowindex], new object[] { expressionarray[1] }));
                                                }
                                                else
                                                {

                                                    this.OnSetValue(this.FocusedNode, expressionarray[0], null);
                                                }
                                            }
                                            else
                                            {
                                                this.OnSetValue(this.FocusedNode, expressionarray[0], null);
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


        /// <summary>
        /// 设置数据绑定
        /// </summary>
        /// <param name="binding">数据绑定对象</param>
        /// <return>int</return>
        public int SetDataBinding(object binding)
        {
            PropertyInfo pi = null;
            DataTable table = null;
            TreeListColumn column = null;

            this.DataSource = binding;
            if (binding is BindingSource)
            {
                int maxlength = 0;
                if (((BindingSource)binding).DataSource is DataView)
                {
                    table = ((DataView)(((BindingSource)binding).DataSource)).Table;
                    for (int i = 0; i < base.Columns.Count; i++)
                    {
                        column = base.Columns[i];
                        if (table.Columns.Contains(column.FieldName) == true)
                        {
                            if (table.Columns[column.FieldName].DataType == typeof(string))
                            {
                                maxlength = table.Columns[column.FieldName].MaxLength;
                                if (maxlength >= 0)
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
                else if (((BindingSource)binding).DataSource is DataTable)
                {
                    table = ((BindingSource)binding).DataSource as DataTable;
                    for (int i = 0; i < base.Columns.Count; i++)
                    {
                        column = base.Columns[i];
                        if (table.Columns[column.FieldName].DataType == typeof(string))
                        {
                            maxlength = table.Columns[column.FieldName].MaxLength;
                            if (maxlength >= 0)
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
            //设置网格中的数据值显示格式与列标题居中,数字类型数据居右
            this.SetColumnDisplayFormat();
            return 1;
        }

        public int SetSourceTableBinding(object binding, string displayMember, string valueMember)
        {
            //this.DisplayMemberPath = displayMember;
            //this.SelectedValuePath = valueMember;
            this.DataSource = binding;
            //UpdateDelegate d = OnSetDataBinding;
            //this.BeginInvoke(d, new object[] { binding });


            //设置网格中的数据值显示格式与列标题居中,数字类型数据居右
            this.SetColumnDisplayFormat();
            return 1;
        }

        private void OnSetDataBinding(object binding)
        {
            this.DataSource = binding;
        }

        /// <summary>
        /// 是否触发验证事件
        /// </summary>
        /// <param name="e">true触发，false不触发</param>
        public void SetCausesValidation(bool e)
        {
            this.CausesValidation = e;
        }

        public object GetValue()
        {
            if (this.FocusedNode == null || this.FocusedColumn == null)
            {
                return null;
            }
            return this.FocusedNode.GetValue(this.FocusedColumn);
        }

        public int SetValue(object value)
        {
            if (this.FocusedNode == null || this.FocusedColumn == null)
            {
                return 0;
            }
            this.FocusedNode.SetValue(this.FocusedColumn, value);
            return 1;
        }

        public int Validation(object sender)
        {
            return 1;
        }

        public void SetDefaultValue()
        {
            if (this.FocusedNode == null || this.FocusedColumn == null)
            {
            }
            else
            {
                this.FocusedNode.SetValue(this.FocusedColumn, this.DefaultValue);
            }
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

        /// <summary>
        /// 清除所有的错误信息
        /// </summary>
        public virtual void ClearErrors()
        {
        }

        /// <summary>
        /// 设置列的下拉数源
        /// </summary>
        /// <param name="field">列名</param>
        /// <param name="source">数据源</param>
        public void SetColumnDataSource(string field, BindingSource source)
        {
            PropertyInfo pi = null;
            TreeListColumn column = null;
            GridColumn columnedit = null;
            GridView view = null;
            DataView dataview = null;
            DataTable table = null;
            DataRow[] rows = null;

            if (this.ViewInfo == null)
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

            column = ((TreeList)this).Columns.ColumnByFieldName(field);
            if (column != null)
            {
                if (column.ColumnEdit != null)
                {
                    pi = column.ColumnEdit.GetType().GetProperty("DataSource");
                    if (pi != null)
                    {
                        pi.SetValue(column.ColumnEdit, source, null);
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

        #region 事件处理

        /// <summary>
        /// 触发控件事件
        /// </summary>
        /// <param name="sender">事件发起者</param>
        /// <param name="eventName">事件名称</param>
        /// <param name="e">事件参数</param>
        protected virtual void RaiseEvent(object sender, string eventName, object e)
        {
            ControlEventArgs args = new ControlEventArgs();
            args.CurrentControl = sender;
            args.EventId = eventName;
            args.SystemEventArgs = e;
            if (this.FocusedColumn == null)
            {
                args.FieldName = string.Empty;
            }
            else
            {
                args.FieldName = this.FocusedColumn.FieldName == null ? string.Empty : this.FocusedColumn.FieldName;
            }
            args.TableName = this.Table;
            args.Key = this.Key;
            if (this.KzxControlOperate != null)
            {
                this.KzxControlOperate(this, args);
                e = args.SystemEventArgs;
            }
        }

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

        protected virtual void UserCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            RaiseEvent(sender, "CellValueChanged", e);
        }

        protected virtual void UserCellValueChanging(object sender, CellValueChangedEventArgs e)
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

        protected virtual void UserFocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            RaiseEvent(sender, "FocusedNodeChanged", e);
        }

        private void UserShownEditor(object sender, EventArgs e)
        {
            this.RaiseEvent(sender, "ShownEditor", e);
        }

        private void UserShowingEditor(object sender, CancelEventArgs e)
        {
            this.RaiseEvent(sender, "ShowingEditor", e);
        }

        #endregion

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

            SetLayout();

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

                    //if (rows[i]["sEventName"].ToString().Equals("Click", StringComparison.OrdinalIgnoreCase) == true)
                    //{
                    //    valuecontrol.Click -= new EventHandler(UserClick);
                    //    valuecontrol.Click += new EventHandler(UserClick);
                    //}
                    //else if (rows[i]["sEventName"].ToString().Equals("DoubleClick", StringComparison.OrdinalIgnoreCase) == true)
                    //{
                    //    valuecontrol.DoubleClick -= new EventHandler(UserDoubleClick);
                    //    valuecontrol.DoubleClick += new EventHandler(UserDoubleClick);
                    //}
                    //else if (rows[i]["sEventName"].ToString().Equals("EditValueChanged", StringComparison.OrdinalIgnoreCase) == true)
                    //{

                    //}

                    //else if (rows[i]["sEventName"].ToString().Equals("KeyDown", StringComparison.OrdinalIgnoreCase) == true)
                    //{
                    //    valuecontrol.KeyDown -= new KeyEventHandler(UserKeyDown);
                    //    valuecontrol.KeyDown += new KeyEventHandler(UserKeyDown);
                    //}
                    //else if (rows[i]["sEventName"].ToString().Equals("KeyPress", StringComparison.OrdinalIgnoreCase) == true)
                    //{
                    //    valuecontrol.KeyPress -= new KeyPressEventHandler(UserKeyPress);
                    //    valuecontrol.KeyPress += new KeyPressEventHandler(UserKeyPress);
                    //}
                    //else if (rows[i]["sEventName"].ToString().Equals("KeyUp", StringComparison.OrdinalIgnoreCase) == true)
                    //{
                    //    valuecontrol.KeyUp -= new KeyEventHandler(UserKeyUp);
                    //    valuecontrol.KeyUp += new KeyEventHandler(UserKeyUp);
                    //}


                    //else if (rows[i]["sEventName"].ToString().Equals("GotFocus", StringComparison.OrdinalIgnoreCase) == true)
                    //{
                    //    valuecontrol.GotFocus -= new EventHandler(UserGotFocus);
                    //    valuecontrol.GotFocus += new EventHandler(UserGotFocus);
                    //}
                    //else if (rows[i]["sEventName"].ToString().Equals("LostFocus", StringComparison.OrdinalIgnoreCase) == true)
                    //{
                    //    valuecontrol.LostFocus -= new EventHandler(UserLostFocus);
                    //    valuecontrol.LostFocus += new EventHandler(UserLostFocus);
                    //}
                    //else if (rows[i]["sEventName"].ToString().Equals("Validating", StringComparison.OrdinalIgnoreCase) == true)
                    //{
                    //    valuecontrol.Validating -= new CancelEventHandler(UserValidating);
                    //    valuecontrol.Validating += new CancelEventHandler(UserValidating);
                    //}
                    //else
                    //{

                    //}
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

        /// <summary>
        /// 控件被加载后调用的方法
        /// 此方法在控件还原后被窗口调用
        /// </summary>
        public virtual void KzxControlLoaded()
        {
            //加载后事件
            RaiseEvent(this, "KzxControlLoaded", new EventArgs());
        }

        #region 扩展方法

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
                                column.Caption = GetLanguage(values[0], fieldvalues[0]);
                                column.Visible = values[1] == "1" || string.IsNullOrWhiteSpace(values[1]) == true ? true : false;
                            }
                            else
                            {
                                column.Caption = GetLanguage(fieldvalues[1], fieldvalues[0]);
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
            TreeListColumn gridColumn = null;

            this.ColumnInfoTable = KzxGridControl.SerializeColumns(this._Columns);
            if (this.ColumnInfoTable != null && this.ColumnInfoTable.Rows.Count > 0)
            {
                foreach (DataRowView dataRowView in this.ColumnInfoTable.DefaultView)
                {
                    if (ColumnType.ComboBox == XmlRow.KzxColumnTypeConverter(dataRowView["ColumnType"].ToString()))
                    {
                        gridColumn = base.Columns.ColumnByFieldName(dataRowView["Field"].ToString());
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

        /// <summary>
        /// 设置引用后允许修改属性
        /// </summary>
        /// <param name="e">true允许修改,false不允许修改</param>
        public void SetAllowEditForReference(bool e)
        {
            DevExpress.XtraTreeList.Columns.TreeListColumn column = null;
            foreach (DataRowView dataRowView in this.ColumnInfoTable.DefaultView)
            {
                column = base.Columns.ColumnByName(dataRowView["Field"].ToString());
                if (Convert.ToBoolean(dataRowView["AllowEdit"]) == false)
                {
                    column.OptionsColumn.ReadOnly = e;
                }
            }
        }

        /// <summary>
        /// 根据列的设计信息创建列
        /// </summary>
        public void CreateColumnByColumnInfo()
        {
            List<DevExpress.XtraEditors.Repository.RepositoryItem> itemlist = new List<RepositoryItem>();
            List<DevExpress.XtraTreeList.Columns.TreeListColumn> columnlist = new List<DevExpress.XtraTreeList.Columns.TreeListColumn>();
            this.ColumnInfoTable = KzxGridControl.SerializeColumns(this._Columns);
            this.RefreshColumnInfoByModuleSetting();
            //梁森 DLL读出来的栏位属性,应该根据数据库来去(bug8319:上海大洪印务 硬包C#8.1.1 生产施工单部件表字段在模块中设置显示,,在单据上不会显示出来)
            //this.UpdateColumnByModuleSetting(this.ColumnInfoTable);

            if (this.ColumnInfoTable != null && this.ColumnInfoTable.Rows.Count > 0)
            {
                base.Columns.Clear();
                this.ColumnInfoTable.DefaultView.Sort = " ColIndex ASC";
                foreach (DataRowView dataRowView in this.ColumnInfoTable.DefaultView)
                {
                    DataRow row = dataRowView.Row;
                    DevExpress.XtraTreeList.Columns.TreeListColumn gridColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
                    gridColumn.VisibleIndex = Convert.ToInt32(dataRowView["ColIndex"]);
                    gridColumn.Name = dataRowView["Field"].ToString();
                    gridColumn.FieldName = dataRowView["Field"].ToString();
                    gridColumn.Fixed = XmlRow.TreeListFixedConverter(dataRowView["Fixed"].ToString());
                    gridColumn.Visible = Convert.ToBoolean(dataRowView["Visible"]);
                    gridColumn.Width = Convert.ToInt32(dataRowView["Width"]);
                    gridColumn.OptionsColumn.ReadOnly = Convert.ToBoolean(dataRowView["ReadOnly"]);
                    gridColumn.OptionsColumn.AllowEdit = Convert.ToBoolean(dataRowView["Enabled"]);
                    gridColumn.Caption = dataRowView["DesigeCaption"].ToString();
                    if (ColumnType.Text == XmlRow.KzxColumnTypeConverter(dataRowView["ColumnType"].ToString()))
                    {
                        gridColumn.ColumnEdit = this.CreateRepositoryItemTextEdit(row);
                    }
                    else if (ColumnType.Date == XmlRow.KzxColumnTypeConverter(dataRowView["ColumnType"].ToString()))
                    {
                        gridColumn.ColumnEdit = this.CreateRepositoryItemDateEdit(row);
                    }
                    else if (ColumnType.Time == XmlRow.KzxColumnTypeConverter(dataRowView["ColumnType"].ToString()))
                    {
                        gridColumn.ColumnEdit = this.CreateRepositoryItemTimeEdit(row);
                    }
                    else if (ColumnType.ComboBox == XmlRow.KzxColumnTypeConverter(dataRowView["ColumnType"].ToString()))
                    {
                        gridColumn.ColumnEdit = this.CreateRepositoryItemGridLookUpEdit(row);
                        ((RepositoryItemGridLookUpEdit)gridColumn.ColumnEdit).DisplayMember = dataRowView["DisplayMemberPath"].ToString();
                        ((RepositoryItemGridLookUpEdit)gridColumn.ColumnEdit).ValueMember = dataRowView["SelectedValuePath"].ToString();
                        if (dataRowView["FieldCaption"] != DBNull.Value)
                        {
                            if (string.IsNullOrWhiteSpace(dataRowView["FieldCaption"].ToString()) == false)
                            {
                                SetLanguage((RepositoryItemGridLookUpEdit)gridColumn.ColumnEdit, dataRowView["FieldCaption"].ToString());
                            }
                        }
                    }
                    else if (ColumnType.CheckBox == XmlRow.KzxColumnTypeConverter(dataRowView["ColumnType"].ToString()))
                    {
                        gridColumn.ColumnEdit = this.CreateRepositoryItemCheckEdit(row);
                    }
                    else if (ColumnType.RadioBox == XmlRow.KzxColumnTypeConverter(dataRowView["ColumnType"].ToString()))
                    {
                        gridColumn.ColumnEdit = this.CreateRepositoryItemRadioGroup(row);
                    }
                    else if (ColumnType.PictureBox == XmlRow.KzxColumnTypeConverter(dataRowView["ColumnType"].ToString()))
                    {
                        gridColumn.ColumnEdit = this.CreateRepositoryItemPictureEdit(row);
                    }
                    else if (ColumnType.HyperLink == XmlRow.KzxColumnTypeConverter(dataRowView["ColumnType"].ToString()))
                    {
                        gridColumn.ColumnEdit = this.CreateRepositoryItemHyperLinkEdit(row);
                    }
                    else if (ColumnType.ButtonEdit == XmlRow.KzxColumnTypeConverter(dataRowView["ColumnType"].ToString()))
                    {
                        gridColumn.ColumnEdit = this.CreateRepositoryItemButtonEdit(row);
                    }
                    else if (ColumnType.NoSourceComboBox == XmlRow.KzxColumnTypeConverter(dataRowView["ColumnType"].ToString()))
                    {
                        gridColumn.ColumnEdit = this.CreateRepositoryItemNoSourceComboBox(row);
                    }
                    else if (ColumnType.MemoEdit == XmlRow.KzxColumnTypeConverter(dataRowView["ColumnType"].ToString()))
                    {
                        gridColumn.ColumnEdit = this.CreateRepositoryItemMemoEdit(row);
                    }
                    else if (ColumnType.CalcEdit == XmlRow.KzxColumnTypeConverter(dataRowView["ColumnType"].ToString()))
                    {
                        gridColumn.ColumnEdit = this.CreateRepositoryItemCalcEdit(row);
                    }
                    else
                    {
                        gridColumn.ColumnEdit = this.CreateRepositoryItemTextEdit(row);
                    }

                    columnlist.Add(gridColumn);
                    itemlist.Add(gridColumn.ColumnEdit);
                }
                base.Columns.AddRange(columnlist.ToArray());
                //base.RepositoryItems.AddRange(itemlist.ToArray());
            }

            //从本地自定义配置中还原
            RestoreGridLayoutFromXml();
        }

        /// <summary>
        /// 根据列的设计信息创建列
        /// </summary>
        public void SetColumnDisplayFormat()
        {
            DataTable dt = null;

            DevExpress.XtraTreeList.Columns.TreeListColumn column;
            DevExpress.XtraTreeList.TreeList tmpTreeList = this as DevExpress.XtraTreeList.TreeList;
            int iCnt = tmpTreeList.Columns.Count;

            if (this.DataSource is BindingSource)
            {
                dt = ((this.DataSource as BindingSource).List as DataView).Table;
            }
            for (int j = 0; j < iCnt; j++)
            {
                column = tmpTreeList.Columns[j];
                if (dt != null)
                {
                    if (dt.Columns.Contains(column.FieldName) == true)
                    {
                        if (dt.Columns[column.FieldName].DataType == typeof(int) ||
                            dt.Columns[column.FieldName].DataType == typeof(Int32) ||
                            dt.Columns[column.FieldName].DataType == typeof(Int16) ||
                            dt.Columns[column.FieldName].DataType == typeof(Int64) ||
                            dt.Columns[column.FieldName].DataType == typeof(Decimal) ||
                            dt.Columns[column.FieldName].DataType == typeof(double) ||
                            dt.Columns[column.FieldName].DataType == typeof(float) ||
                            dt.Columns[column.FieldName].DataType == typeof(long) ||
                            dt.Columns[column.FieldName].DataType == typeof(Single))
                        {
                            if (column.Format.FormatType == DevExpress.Utils.FormatType.None)
                            {
                                //数字全部靠右显示
                                column.AppearanceCell.Options.UseTextOptions = true;
                                column.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
                                column.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
                                column.Format.FormatString = "#0.######";
                            }
                        }
                    }
                }
                //标题居中，lfx，20170412
                column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }
        }

        public void SetColumnDisplayFormat(DataTable tabledetail, int qtyscale, int pricescale, int moneyscale, bool rightAmount)
        {
            DataTable dt = null;
            int scale = 3;
            int type = -1;
            Object obj = null;
            PropertyInfo pi = null;

            DevExpress.XtraTreeList.Columns.TreeListColumn column;
            DevExpress.XtraTreeList.TreeList tmpTreeList = this as DevExpress.XtraTreeList.TreeList;
            int iCnt = tmpTreeList.Columns.Count;

            if (this.DataSource is BindingSource)
            {
                dt = ((this.DataSource as BindingSource).List as DataView).Table;
            }
            for (int j = 0; j < iCnt; j++)
            {
                scale = 3;
                type = -1;
                column = tmpTreeList.Columns[j];
                if (dt != null && column != null)
                {
                    if (dt.Columns.Contains(column.FieldName) == true)
                    {
                        if (
                            //dt.Columns[column.FieldName].DataType == typeof(int) ||
                            //dt.Columns[column.FieldName].DataType == typeof(Int32) ||
                            //dt.Columns[column.FieldName].DataType == typeof(Int16) ||
                            //dt.Columns[column.FieldName].DataType == typeof(Int64) ||
                            dt.Columns[column.FieldName].DataType == typeof(Decimal) ||
                            dt.Columns[column.FieldName].DataType == typeof(double) ||
                            dt.Columns[column.FieldName].DataType == typeof(float) ||
                            dt.Columns[column.FieldName].DataType == typeof(long) ||
                            dt.Columns[column.FieldName].DataType == typeof(Single))
                        {
                            scale = 0;
                            DataRow[] rows = tabledetail.Select("sTableName='" + this.Table + "' and sField='" + column.FieldName + "' and (sContentType='0' or sContentType='1' or sContentType='2')", string.Empty);
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
                            if (column.ColumnEdit != null)
                            {
                                if (type == 1 || type == 2)
                                {
                                    if (rightAmount == true)
                                    {
                                        pi = column.ColumnEdit.GetType().GetProperty("PasswordChar");
                                        if (pi != null)
                                        {
                                            pi.SetValue(column.ColumnEdit, '\0', null);
                                        }
                                    }
                                    else
                                    {
                                        pi = column.ColumnEdit.GetType().GetProperty("PasswordChar");
                                        if (pi != null)
                                        {
                                            pi.SetValue(column.ColumnEdit, '*', null);
                                        }
                                    }
                                }
                            }
                            //数字全部靠右显示
                            if (scale > 0)
                            {
                                column.AppearanceCell.Options.UseTextOptions = true;
                                column.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
                                column.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
                                column.Format.FormatString = "0." + new string('#', scale);
                            }
                            else
                            {
                                column.AppearanceCell.Options.UseTextOptions = true;
                                column.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
                                column.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
                                column.Format.FormatString = "0.######";
                            }
                        }
                    }
                }
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
        ///  生成文本列
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit CreateRepositoryItemTextEdit(DataRow row)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemTextEdit t = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            t.MaxLength = Convert.ToInt32(row["MaxLength"]);
            t.Click += new EventHandler(RepositoryItemClick);
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);
            t.Tag = true;
            t.MouseUp += new MouseEventHandler(RepositoryItemMouseUp);
            t.Enter += new EventHandler(RepositoryItemEnter);


            if (string.IsNullOrWhiteSpace(row["PasswordChar"].ToString()) == true)
            {
                t.PasswordChar = '\0';
                t.AllowMouseWheel = row["KzxAllowMouseWheel"] == DBNull.Value ? false : Convert.ToBoolean(row["KzxAllowMouseWheel"]);
                t.Mask.MaskType = row["KzxMaskType"] == DBNull.Value ? MaskType.None : XmlRow.KzxMaskTypeConverter(row["KzxMaskType"].ToString());
                t.Mask.EditMask = row["KzxEditMask"] == DBNull.Value ? string.Empty : row["KzxEditMask"].ToString();
                t.Mask.UseMaskAsDisplayFormat = false;
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
            t.DoubleClick += new EventHandler(RepositoryItemDoubleClick);

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
            t.AllowMouseWheel = row["KzxAllowMouseWheel"] == DBNull.Value ? false : Convert.ToBoolean(row["KzxAllowMouseWheel"]);
            t.Mask.MaskType = row["KzxMaskType"] == DBNull.Value ? MaskType.None : XmlRow.KzxMaskTypeConverter(row["KzxMaskType"].ToString());
            t.Mask.EditMask = row["KzxEditMask"] == DBNull.Value ? string.Empty : row["KzxEditMask"].ToString();
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
            t.View.OptionsBehavior.AutoPopulateColumns = true;
            t.NullText = string.Empty;

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
            this.RaiseEvent(sender, "EditorKeyUp", e);
        }

        private void RepositoryItemClick(object sender, EventArgs e)
        {
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

        private void RepositoryMemoEditItemDoubleClick(object sender, EventArgs e)
        {
            
        }

        private void RepositoryItemKeyDown(object sender, KeyEventArgs e)
        {
            BindingSource bs = null;
            string currenttext = string.Empty;
            PropertyInfo pi = null;

            e.Handled = true;
            if (this.ColumnDataSourceDictionary.Keys.Contains(this.FocusedColumn.FieldName) == true)
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
            TreeListViewInfo view = (this.ViewInfo as TreeListViewInfo);

            if (view != null)
            {
                if (this.ColumnDataSourceDictionary.ContainsKey(this.FocusedColumn.FieldName) == true)
                {
                    rows = this.ColumnInfoTable.Select("Field='" + this.FocusedColumn.FieldName + "'", " ColIndex ASC ", DataViewRowState.CurrentRows);
                    if (rows.Length > 0)
                    {
                        filterstring = rows[0]["FilterString"] == DBNull.Value || rows[0]["FilterString"] == null ? string.Empty : rows[0]["FilterString"].ToString();
                        if (string.IsNullOrWhiteSpace(filterstring) == false)
                        {
                            expressionarray = filterstring.Split(new char[] { ',' });
                            for (int i = 0; i < expressionarray.Length; i++)
                            {
                                sb.Clear();
                                if (string.IsNullOrWhiteSpace(expressionarray[i]) == false)
                                {
                                    filterarray = expressionarray[i].Split(new string[] { "=", ">", "<", ">=", "<=" }, StringSplitOptions.None);
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
                                            else if (s.StartsWith(">") == true)
                                            {
                                                sb.Append(">");
                                            }
                                            else if (s.StartsWith("<") == true)
                                            {
                                                sb.Append("<");
                                            }
                                            else if (s.StartsWith(">=") == true)
                                            {
                                                sb.Append(">=");
                                            }
                                            else if (s.StartsWith("<=") == true)
                                            {
                                                sb.Append("<=");
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
                                                sb.Clear();
                                                continue;
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
                                this.ColumnDataSourceDictionary[this.FocusedColumn.FieldName].Filter = expressionsb.ToString();
                            }
                            else
                            {
                                this.ColumnDataSourceDictionary[this.FocusedColumn.FieldName].RemoveFilter();
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
        }

        private void RepositoryItemCloseUp(object sender, CloseUpEventArgs e)
        {
            this.RaiseEvent(sender, "CloseUp", e);
        }

        private void RepositoryItemClosed(object sender, ClosedEventArgs e)
        {
            if (this.ColumnDataSourceDictionary.ContainsKey(this.FocusedColumn.FieldName) == true)
            {
                this.ColumnDataSourceDictionary[this.FocusedColumn.FieldName].RemoveFilter();
            }
            this.RaiseEvent(sender, "Closed", e);
        }

        #endregion

        private void TreeList_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
        {
            e.Node.TreeList.SetFocusedNode(e.Node);
        }

        #region 右键菜单

        private void ContextMenuInit()
        {
            _contextMenu.Items.Clear();

            var gridSetItem = new ToolStripMenuItem();
            gridSetItem.Text = GetLanguage("MSG000686", "网格状态设置");
            gridSetItem.Click += gridSetItem_Click;
            _contextMenu.Items.Add(gridSetItem);

            var gridSetClearItem = new ToolStripMenuItem();
            gridSetClearItem.Text = GetLanguage("MSG006992", "清除网格状态");
            gridSetClearItem.Click += gridSetClearItem_Click;
            _contextMenu.Items.Add(gridSetClearItem);

            var selectAllItem = new ToolStripMenuItem();
            selectAllItem.Text = GetLanguage("MSG000588", "全选");
            selectAllItem.Click += selectAllItem_Click;
            _contextMenu.Items.Add(selectAllItem);

            var cancelSelectAllItem = new ToolStripMenuItem();
            cancelSelectAllItem.Text = GetLanguage("MSG000589", "全消");
            cancelSelectAllItem.Click += cancelSelectAllItem_Click;
            _contextMenu.Items.Add(cancelSelectAllItem);
        }

        void cancelSelectAllItem_Click(object sender, EventArgs e)
        {
            SetAllCheckStatus(false);
        }

        void selectAllItem_Click(object sender, EventArgs e)
        {
            SetAllCheckStatus(true);
        }

        void gridSetClearItem_Click(object sender, EventArgs e)
        {
            GridStatusSetClear();
        }

        void gridSetItem_Click(object sender, EventArgs e)
        {
            GridStatusSet();
        }

        #endregion

        #region 网格状态设置

        /// <summary>
        /// 网格状态设置
        /// </summary>
        private void GridStatusSet()
        {
            if (this.ColumnInfoTable == null) return;
            if (string.IsNullOrWhiteSpace(SysVar.FCurryFormInfo.sFormName)) return;

            var dataSourceTable = GetDataTableFromDataSource();
            if (dataSourceTable == null || dataSourceTable.Columns.Count == 0) return;

            Status frm = new Status(this.Table, this.Name, this.Name);
            if (string.Equals(SysVar.FCurryFormInfo.sFormType, "qry", StringComparison.OrdinalIgnoreCase))
            {
                frm.IsDisplaySearchKeyColumn = false;
            }

            StatusSet.dt.Clear();
            StatusSet.dt.Columns.Clear();
            StatusSet.dt.Columns.Add("bSelect", Type.GetType("System.Boolean"));
            StatusSet.dt.Columns.Add("bSum", Type.GetType("System.Boolean"));
            StatusSet.dt.Columns.Add("sCaption", Type.GetType("System.String"));
            StatusSet.dt.Columns.Add("sField", Type.GetType("System.String"));
            StatusSet.dt.Columns.Add("Sumtype", Type.GetType("System.String"));
            StatusSet.dt.Columns.Add("StringFormat", Type.GetType("System.String"));
            StatusSet.dt.Columns.Add("bFilter", Type.GetType("System.Boolean"));
            StatusSet.dt.Columns.Add("sParent", Type.GetType("System.String"));
            StatusSet.dt.Columns.Add("bSearchKeyField", Type.GetType("System.Boolean"));
            StatusSet.dt.Columns.Add("sFieldDesc", Type.GetType("System.String"));

            //表格状态配置配置
            var filterConfigSection = string.Format("{0}_KzxTreeList_{1}", SysVar.FCurryFormInfo.sFormName, this.Name);

            //读取表格列格式
            var formatFilePath = Application.StartupPath + @"\Guid\StringFormat.ini";
            var formatIniFile = new IniFileCore(formatFilePath);
            var formatSection = string.Format("{0}_KzxTreeList_{1}", SysVar.FCurryFormInfo.sFormName, this.Name);
            var formatString = formatIniFile.Read(formatSection, "Format");
            var formatDic = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(formatString))
            {
                var formatFieldInfo = formatString.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var fileInfo in formatFieldInfo)
                {
                    if (string.IsNullOrWhiteSpace(fileInfo))
                        continue;

                    var formatInfo = fileInfo.Split('=');
                    if (formatInfo.Length != 2) continue;

                    formatDic[formatInfo[0].Trim()] = formatInfo[1];
                }
            }

            //初始化表格设置信息
            foreach (TreeListColumn col in base.Columns)
            {
                if (string.IsNullOrWhiteSpace(col.FieldName)
                    || string.IsNullOrWhiteSpace(col.Caption)
                    || col.Caption.Equals(col.FieldName))
                    continue;

                var rows = ColumnInfoTable.Select(string.Format("Field='{0}'", col.FieldName));
                if (rows.Length == 0) continue;

                //模块设置不可见
                var visibleValue = rows[0]["Visible"];
                if (visibleValue == null || visibleValue == DBNull.Value || !(bool)visibleValue)
                    continue;

                var newRow = StatusSet.dt.NewRow();
                var caption = GetLanguage(rows[0]["MessageCode"].ToString(), rows[0]["DesigeCaption"].ToString());

                newRow["sCaption"] = string.Format("{0}({1})", caption, col.FieldName);
                newRow["sField"] = col.FieldName;
                newRow["sFieldDesc"] = caption;

                if (col.Visible)
                    newRow["bSelect"] = true;

                if (col.SummaryFooter != SummaryItemType.None && !string.IsNullOrWhiteSpace(col.SummaryFooterStrFormat))
                    newRow["bSum"] = true;

                if (formatDic.ContainsKey(col.FieldName))
                    newRow["StringFormat"] = formatDic[col.FieldName];

                switch (col.SummaryFooter)
                {
                    case SummaryItemType.Sum: newRow["Sumtype"] = "合计"; break;
                    case SummaryItemType.Average: newRow["Sumtype"] = "平均值"; break;
                    case SummaryItemType.Max: newRow["Sumtype"] = "最大值"; break;
                    case SummaryItemType.Min: newRow["Sumtype"] = "最小值"; break;
                    case SummaryItemType.Count: newRow["Sumtype"] = "数据总数量"; break;
                    case SummaryItemType.None: newRow["Sumtype"] = ""; break;
                    default: break;
                }

                StatusSet.dt.Rows.Add(newRow);
            }

            //打开网格设置窗体
            using (var statusFrm = new Status(this.Table, filterConfigSection))
            {
                statusFrm.IsDisplaySearchKeyColumn = false;

                if (statusFrm.ShowDialog() == DialogResult.OK)
                {
                    foreach (TreeListColumn col in base.Columns)
                    {
                        var statusRows = StatusSet.dt.Select(string.Format("sField='{0}'", col.FieldName));
                        if (statusRows.Length == 0) continue;

                        var statusRow = statusRows[0];
                        var formatStr = statusRow["StringFormat"].ToString();

                        col.Visible = statusRow["bSelect"].ToString().ToBoolExt();

                        switch (statusRow["Sumtype"].ToString())
                        {
                            case "总计": col.SummaryFooter = SummaryItemType.Sum; break;
                            case "平均值": col.SummaryFooter = SummaryItemType.Average; break;
                            case "最大值": col.SummaryFooter = SummaryItemType.Max; break;
                            case "最小值": col.SummaryFooter = SummaryItemType.Min; break;
                            case "数据总数量": col.SummaryFooter = SummaryItemType.Count; break;
                            default: col.SummaryFooter = SummaryItemType.None; break;
                        }

                        if (col.SummaryFooter != SummaryItemType.None)
                        {
                            if (!string.IsNullOrWhiteSpace(formatStr))
                                col.SummaryFooterStrFormat = formatStr;
                            else
                                col.SummaryFooterStrFormat = "{0}";
                        }
                    }

                    SaveGridLayoutToXml();
                }
            }
        }

        private void GridStatusSetClear()
        {
            var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Guid", string.Format("{0}_KzxTreeList_{1}.xml", SysVar.FCurryFormInfo.sFormName, this.Name));

            if (File.Exists(xmlPath))
                File.Delete(xmlPath);

            foreach (TreeListColumn col in base.Columns)
            {
                var moduleSettingRows = _ColumnInfoTable.Select(string.Format("Field='{0}'", col.FieldName));
                if (moduleSettingRows.Length == 0) continue;

                var settingRow = moduleSettingRows[0];

                col.VisibleIndex = Convert.ToInt32(settingRow["ColIndex"]);
                col.Visible = Convert.ToBoolean(settingRow["Visible"]);
                col.Width = Convert.ToInt32(settingRow["Width"]);
            }
        }

        /// <summary>
        /// 保存网格状态到XML
        /// </summary>
        private void SaveGridLayoutToXml()
        {
            var saveFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Guid");
            var savePath = Path.Combine(saveFolder, string.Format("{0}_KzxTreeList_{1}.xml", SysVar.FCurryFormInfo.sFormName, this.Name));
            var xml = new StringBuilder();
            int colIndex = 0;

            xml.AppendLine("<XtraSerializer version=\"1.0\" application=\"KzxTreeList\">");
            xml.AppendLine("  <columns>");

            foreach (TreeListColumn col in base.Columns)
            {
                if (string.IsNullOrWhiteSpace(col.FieldName))
                    continue;

                xml.AppendFormat("    <column name=\"{0}\">", col.Name).AppendLine();
                xml.AppendFormat("      <visible>{0}</visible>", col.Visible).AppendLine();
                xml.AppendFormat("      <visibleIndex>{0}</visibleIndex>", col.VisibleIndex).AppendLine();
                xml.AppendFormat("      <width>{0}</width>", col.Width).AppendLine();
                xml.AppendLine("    </column>");
                colIndex += 1;
            }

            xml.AppendLine("  </columns>");
            xml.Append("</XtraSerializer>");

            if (Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);

            using (var sr = new StreamWriter(savePath, false, Encoding.UTF8))
            {
                sr.Write(xml);
                sr.Flush();
                sr.Close();
            }
        }

        private void RestoreGridLayoutFromXml()
        {
            var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Guid", string.Format("{0}_KzxTreeList_{1}.xml", SysVar.FCurryFormInfo.sFormName, this.Name));

            if (!File.Exists(xmlPath))
                return;

            try
            {
                var doc = new XmlDocument();
                doc.Load(xmlPath);

                var colCount = base.Columns.Count;
                foreach (TreeListColumn col in base.Columns)
                {
                    if (string.IsNullOrWhiteSpace(col.FieldName))
                        continue;

                    var node = doc.SelectSingleNode(string.Format("/XtraSerializer/columns/column[@name='{0}']", col.FieldName));
                    if (node == null) continue;

                    var moduleSettingRows = _ColumnInfoTable.Select(string.Format("Field='{0}'", col.FieldName));
                    var visibleNode = node.SelectSingleNode("visible");
                    var visibleIndexNode = node.SelectSingleNode("visibleIndex");
                    var widthNode = node.SelectSingleNode("width");

                    if (visibleNode != null && !string.IsNullOrWhiteSpace(visibleNode.InnerText))
                    {
                        if (moduleSettingRows.Length != 0 && Convert.ToBoolean(moduleSettingRows[0]["Visible"]))
                            col.Visible = string.Equals("True", visibleNode.InnerText);
                    }

                    if (visibleIndexNode != null && !string.IsNullOrWhiteSpace(visibleIndexNode.InnerText))
                    {
                        var index = visibleIndexNode.InnerText.ToIntExt(-9);
                        if (index != -9)
                        {
                            if (index > colCount - 1)
                                col.VisibleIndex = colCount - 1;
                            else
                                col.VisibleIndex = index;
                        }
                    }

                    if (col.Visible && widthNode != null && !string.IsNullOrWhiteSpace(widthNode.InnerText))
                    {
                        var width = widthNode.InnerText.ToIntExt(-9);
                        if (width > 0)
                        {
                            col.Width = width;
                        }
                    }
                }
            }
            catch (Exception)
            {
                //不需要做任何事情
            }
        }

        #endregion

        #region 选择

        private void SetAllCheckStatus(bool isCheck)
        {
            var checkColumnName = string.Empty;
            var dataSourceTable = GetDataTableFromDataSource();

            if (dataSourceTable == null)
                return;

            if (!dataSourceTable.DefaultView.AllowEdit
                || !dataSourceTable.DefaultView.AllowDelete
                || !dataSourceTable.DefaultView.AllowNew)
                return;

            if (dataSourceTable.Columns.Contains("bChoose"))
                checkColumnName = "bChoose";

            if (string.IsNullOrWhiteSpace(checkColumnName))
                return;

            foreach (DataRow row in dataSourceTable.Rows)
            {
                row[checkColumnName] = isCheck;
            }
        }

        #endregion

        #region 数据源·获取数据表

        /// <summary>
        /// 通过数据源获取数据表
        /// </summary>
        /// <returns></returns>
        private DataTable GetDataTableFromDataSource()
        {
            if (this.DataSource == null)
                return null;

            if (this.DataSource is BindingSource)
            {
                var bindingSource = (BindingSource)this.DataSource;
                if (bindingSource.DataSource is DataView)
                    return ((DataView)bindingSource.DataSource).Table;
                else if (bindingSource.DataSource is DataTable)
                    return (DataTable)bindingSource.DataSource;
                else
                    throw new Exception("遇到未能处理的TreeList数据源类型。");
            }
            else if (this.DataSource is DataView)
            {
                return ((DataView)this.DataSource).Table;
            }
            else if (this.DataSource is DataTable)
            {
                return (DataTable)this.DataSource;
            }
            else
            {
                throw new Exception("遇到未能处理的TreeList数据源类型。");
            }
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
            string filepath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "KzxCommon.dll");
            string text = string.Empty;
            Assembly assembly = null;
            object obj = null;

            try
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                for (int i = 0; i < assemblies.Length; i++)
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

        #endregion
    }
}
