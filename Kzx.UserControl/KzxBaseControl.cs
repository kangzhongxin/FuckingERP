using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using Kzx.UserControls.UITypeEdit;
using System.Drawing.Design;
using System.Text.RegularExpressions;
using DevExpress.XtraEditors.Controls;

namespace Kzx.UserControl
{
    public partial class KzxBaseControl : System.Windows.Forms.UserControl,IControl
    {
        protected delegate void UpdateDelegate();

        protected delegate void UpdatePropertyDelegate(object e);

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
        /// 空格
        /// </summary>
        public const int Space = 4;

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
        protected Binding BindingObject = null;

        public KzxBaseControl()
        {
            InitializeComponent();
        }

        event KzxControlOperateEventHandler IControl.KzxControlOperate
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event KzxGetLanguageEventHandler IControl.KzxGetLanguage
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
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
                if (string.IsNullOrWhiteSpace(this._Key) == true)
                {
                    if (string.IsNullOrWhiteSpace(this.Table) == false && string.IsNullOrWhiteSpace(this.Field) == false)
                    {
                        this._Key = this.Table + "." + this.Field;
                    }
                    else if (string.IsNullOrWhiteSpace(this.Table) == false)
                    {
                        this._Key = this.Table;
                    }
                } 
                if (string.IsNullOrEmpty(this._Key)) return this.Name; 

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
                //this.Enabled = value;
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
                    if (value == false)
                    {
                        this.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        this.BorderStyle = BorderStyle.None;
                    }
                }
                else
                {
                    //this.Visible = value;
                }
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
        [Category("数据"), Description("Field,数据库的字段名称或者存储过程参数"), Browsable(true)]
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
                return this._ReadOnly;
            }
            set
            {
                this._ReadOnly = value;
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

        private string _ValueDependencyField = string.Empty;
        /// <summary>
        /// 数据携带
        /// </summary>
        [Category("下拉数据"), Description("ValueDependencyField,格式:控件标识Key=下拉数据源的Field,表示Key对应控件的值来自于下拉数据源中的Field对应列,多个表达式可以用逗号分隔.如master.sCode=code,master.sName=name"), Browsable(true), Editor(typeof(TextUiTypEdit), typeof(UITypeEditor))]
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

        private string _EventList = string.Empty;
        /// <summary>
        /// 事件列表
        /// </summary>
        [Category("自定义"), Description("EventList,事件列表"), Browsable(true), Editor(typeof(UITypeEdit.ControlEventInfoUiTypeEdit), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [McDisplayName("EventList")]
        public virtual string EventList
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

        private string toolTipMaxLengthText = string.Empty;
        /// <summary>
        /// 数据长度不能超过数据库长度提示文本 
        /// </summary>
        public virtual string ToolTipMaxLengthText
        {
            get
            {
                return this.toolTipMaxLengthText;
            }
            set
            {
                this.toolTipMaxLengthText = value;
            }
        }

        private  BorderStyles _BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        /// <summary>
        /// 边框显示格式
        /// </summary>
        [Category("数据格式"), Description("KzxBorderStyle,边框显示格式"), Browsable(true)]
        [McDisplayName("KzxBorderStyle")]
        public virtual DevExpress.XtraEditors.Controls.BorderStyles KzxBorderStyle
        {
            get
            {
                return this._BorderStyle;
            }
            set
            {
                this._BorderStyle = value;
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
        /// 设置数据绑定
        /// </summary>
        /// <param name="binding">数据绑定对象</param>
        /// <return>int</return>
        public virtual int SetDataBinding(object binding)
        {
            return 1;
        }

        /// <summary>
        /// 设置下拉框的数据源
        /// </summary>
        /// <param name="binding">下拉框的数据源</param>
        /// <param name="displayMember">显示值字段名</param>
        /// <param name="valueMember">实际值字段名</param>
        /// <returns>int</returns>
        public virtual int SetSourceTableBinding(object binding, string displayMember, string valueMember)
        {
            return 1;
        }

        /// <summary>
        /// 是否触发验证事件
        /// </summary>
        /// <param name="e">true触发，false不触发</param>
        public virtual void SetCausesValidation(bool e)
        {

        }

        /// <summary>
        /// 取控件的值
        /// </summary>
        /// <return>Object</return>
        public virtual object GetValue()
        {
            return null;
        }

        /// <summary>
        /// 设置控件的值
        /// </summary>
        /// <param name="value">控件的值</param>
        /// <return>int</return>
        public virtual int SetValue(object value)
        {
            return 1;
        }

        /// <summary>
        /// 还原默认值
        /// </summary>
        /// <return>void</return>
        public virtual void SetDefaultValue()
        {

        }

        /// <summary>
        /// 合法性验证
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <returns>1,验证通过,其他验证失败</returns>
        public virtual int Validation(object sender)
        {
            int ret = 1;

            if (this.IsNull == false && this.ReadOnly == false && this.DesigeEnabled == true && this.DesigeVisible == true)
            {
                //非空验证
                if (string.IsNullOrWhiteSpace(this.GetValue() == null || this.GetValue() == DBNull.Value ? string.Empty : this.GetValue().ToString()) == true)
                { 
                    SetErrorText(sender, GetLanguage("SYS001185", "数据不能为空"));
                    ret = 0;
                    return ret;
                }
                else
                {
                    SetErrorText(sender, string.Empty);
                }
            }
            if (this.AllowValueRange == true && this.ReadOnly == false && this.DesigeEnabled == true && this.DesigeVisible == true)
            {
                //启用了范围验证
                switch (this.DataType)
                {
                    case KzxDataType.Str:
                        ret = ValidationForString(sender);
                        break;
                    case KzxDataType.Int:
                        ret = ValidationForInt(sender);
                        break;
                    case KzxDataType.Double:
                        ret = ValidationForDouble(sender);
                        break;
                    case KzxDataType.Decimal:
                        ret = ValidationForDecimal(sender);
                        break;
                    case KzxDataType.Date:
                        ret = ValidationForDate(sender);
                        break;
                    default:
                        ret = ValidationForString(sender);
                        break;
                }
                if (ret < 1)
                {
                    return ret;
                }
            }
            if (string.IsNullOrWhiteSpace(this.RegexString) == false)
            {
                //正则表达式验证
                ret = ValidationForRegex(sender);
                if (ret < 1)
                {
                    return ret;
                }
            }

            return 1;
        }

        #region 验证方法

        /// <summary>
        /// 字符串范围验证
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <returns>1通过，其他验证失败</returns>
        protected virtual int ValidationForString(object sender)
        {
            int ret = 1;
            string value = this.GetValue() == null || this.GetValue() == DBNull.Value ? string.Empty : this.GetValue().ToString();

            SetErrorText(sender, string.Empty);
            if (string.IsNullOrWhiteSpace(value) == true)
            {
                //空值不验证
                return ret;
            }
            if (string.IsNullOrWhiteSpace(this.MinValue) == false)
            {
                if (string.Compare(value, this.MinValue) < 0)
                {
                    ret = 0;
                    SetErrorText(sender, GetLanguage("SYS001186", "数据太小"));
                }
            }
            if (string.IsNullOrWhiteSpace(this.MaxValue) == false)
            {
                if (string.Compare(value, this.MaxValue) > 0)
                {
                    ret = 0;
                    SetErrorText(sender, GetLanguage("SYS001187", "数据太大"));
                }
            }
            return ret;
        }

        /// <summary>
        /// 整数范围验证
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <returns>1通过，其他验证失败</returns>
        protected virtual int ValidationForInt(object sender)
        {
            int ret = 1;
            int min = 0;
            int max = 0;
            int value = 0;

            SetErrorText(sender, string.Empty);
            if (string.IsNullOrWhiteSpace(this.GetValue() == null || this.GetValue() == DBNull.Value ? string.Empty : this.GetValue().ToString()) == true)
            {
                ret = 1;
                return ret;
            }

            if (int.TryParse((this.GetValue() == null || this.GetValue() == DBNull.Value ? "0" : this.GetValue().ToString()), out value) == true)
            {
                if (int.TryParse(this.MinValue, out min) == true)
                {
                    if (value < min)
                    {
                        ret = 0;
                        SetErrorText(sender, GetLanguage("SYS001186", "数据太小"));
                        return ret;
                    }
                }
                if (int.TryParse(this.MaxValue, out max) == true)
                {
                    if (value > max)
                    {
                        ret = 0;
                        SetErrorText(sender, GetLanguage("SYS001187", "数据太大"));
                        return ret;
                    }
                }
            }
            else
            {
                SetErrorText(sender, GetLanguage("SYS001188", "请录入整数"));
                ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// 实数范围验证
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <returns>1通过，其他验证失败</returns>
        protected virtual int ValidationForDouble(object sender)
        {
            int ret = 1;
            Double min = 0;
            Double max = 0;
            Double value = 0;

            SetErrorText(sender, string.Empty);
            if (string.IsNullOrWhiteSpace(this.GetValue() == null || this.GetValue() == DBNull.Value ? string.Empty : this.GetValue().ToString()) == true)
            {
                ret = 1;
                return ret;
            }

            if (Double.TryParse((this.GetValue() == null || this.GetValue() == DBNull.Value ? "0" : this.GetValue().ToString()), out value) == true)
            {
                if (Double.TryParse(this.MinValue, out min) == true)
                {
                    if (value < min)
                    {
                        ret = 0;
                        SetErrorText(sender, GetLanguage("SYS001186", "数据太小"));
                        return ret;
                    }
                }
                if (Double.TryParse(this.MaxValue, out max) == true)
                {
                    if (value > max)
                    {
                        ret = 0;
                        SetErrorText(sender, GetLanguage("SYS001187", "数据太大"));
                        return ret;
                    }
                }
            }
            else
            {
                SetErrorText(sender, GetLanguage("SYS001189", "请录入数字"));
                ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// DECIMAL范围验证
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <returns>1通过，其他验证失败</returns>
        protected virtual int ValidationForDecimal(object sender)
        {
            int ret = 1;
            decimal min = 0;
            decimal max = 0;
            decimal value = 0;

            SetErrorText(sender, string.Empty);
            if (string.IsNullOrWhiteSpace(this.GetValue() == null || this.GetValue() == DBNull.Value ? string.Empty : this.GetValue().ToString()) == true)
            {
                ret = 1;
                return ret;
            }
            if (decimal.TryParse((this.GetValue() == null || this.GetValue() == DBNull.Value ? "0" : this.GetValue().ToString()), out value) == true)
            {
                if (decimal.TryParse(this.MinValue, out min) == true)
                {
                    if (value < min)
                    {
                        ret = 0;
                        SetErrorText(sender, GetLanguage("SYS001186", "数据太小"));
                        return ret;
                    }
                }
                if (decimal.TryParse(this.MaxValue, out max) == true)
                {
                    if (value > max)
                    {
                        ret = 0;
                        SetErrorText(sender, GetLanguage("SYS001187", "数据太大"));
                        return ret;
                    }
                }
            }
            else
            {
                SetErrorText(sender, GetLanguage("SYS001189", "请录入数字"));
                ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// 日期范围验证
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <returns>1通过，其他验证失败</returns>
        protected virtual int ValidationForDate(object sender)
        {
            int ret = 1;
            DateTime min = new DateTime(1900, 1, 1);
            DateTime max = new DateTime(1900, 1, 1);
            DateTime value = new DateTime(1900, 1, 1);

            SetErrorText(sender, string.Empty);
            if (string.IsNullOrWhiteSpace(this.GetValue() == null || this.GetValue() == DBNull.Value ? string.Empty : this.GetValue().ToString()) == true)
            {
                ret = 1;
                return ret;
            }
            if (DateTime.TryParse((this.GetValue() == null || this.GetValue() == DBNull.Value ? (new DateTime(1900, 1, 1)).ToString() : this.GetValue().ToString()), out value) == true)
            {
                if (DateTime.TryParse(this.MinValue, out min) == true)
                {
                    if (DateTime.Compare(value, min) < 0)
                    {
                        SetErrorText(sender, GetLanguage("SYS001186", "数据太小"));
                        ret = 0;
                    }
                }
                if (DateTime.TryParse(this.MaxValue, out max) == true)
                {
                    if (DateTime.Compare(value, max) > 0)
                    {
                        SetErrorText(sender, GetLanguage("SYS001187", "数据太大"));
                        ret = 0;
                    }
                }
            }
            else
            {
                SetErrorText(sender, GetLanguage("SYS001190", "请录入日期"));
                ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// 正则表达式验证       
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <returns>1通过，其他验证失败</returns>
        protected virtual int ValidationForRegex(object sender)
        {
            int ret = 1;
            string value = this.GetValue() == null || this.GetValue() == DBNull.Value ? string.Empty : this.GetValue().ToString();

            SetErrorText(sender, string.Empty);
            if (string.IsNullOrWhiteSpace(this.RegexString) == false && string.IsNullOrWhiteSpace(value) == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(value, this.RegexString).Equals(false))
                {
                    ret = 0;
                    SetErrorText(sender, GetLanguage("SYS001191", "数据不合法"));
                    return ret;
                }
            }
            return ret;
        }

        #endregion

        /// <summary>
        /// 清空错误信息
        /// </summary>
        private void ClearErrorText()
        {
            string fieldname = string.Empty;
            object sender = null;
            PropertyInfo pi = null;
            DataRowView rowview = null;

            pi = this.GetType().GetProperty("ContentControl");
            if (pi != null)
            {
                sender = pi.GetValue(this, null);
                if (sender != null)
                {
                    pi = sender.GetType().GetProperty("ErrorText");
                    if (pi != null)
                    {
                        pi.SetValue(sender, string.Empty, null);
                    }
                }
            }
            return; 
        }

        public virtual void SetErrorText(object sender, string errorText)
        {
            string fieldname = string.Empty;
            PropertyInfo pi = null;
            pi = sender.GetType().GetProperty("ErrorText");
            if (pi != null)
            {
                pi.SetValue(sender, errorText, null);
            }
            return; 
        }

        /// <summary>
        /// 清除所有的错误信息
        /// </summary>
        public virtual void ClearErrors()
        {
            ClearErrorText();
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="valueControl">控件</param>
        /// <param name="eventInfoTable">事件信息表</param>
        public virtual void BindingEvent(Control valueControl, DataTable eventInfoTable)
        {  
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
                    if (eventname.Equals("KzxValueChanged", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        eventname = "Leave";
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

        /// <summary>
        /// 控件事件
        /// </summary>
        public event KzxControlOperateEventHandler KzxControlOperate;

        /// <summary>
        /// 获取多语言文本事件
        /// </summary>
        public event KzxGetLanguageEventHandler KzxGetLanguage;

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
            RaiseEvent(sender, "KzxValueChanged", e);
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

        //

        protected virtual void DataValidating(object sender, CancelEventArgs e)
        {
            if (this.Validation(sender) < 1)
            {
                this.IsValidation = false;
                e.Cancel = false;  //焦点不能离开
            }
            else
            {
                this.IsValidation = true;
                e.Cancel = false;
            }
        }

        #endregion

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
            args.FieldName = this.Field;
            args.TableName = this.Table;
            args.Key = this.Key;
            if (this.KzxControlOperate != null)
            {
                this.KzxControlOperate(this, args);
                e = args.SystemEventArgs;
            }
        }


        private static MethodInfo _methodInfo = null;

        /// <summary>
        /// 获取多语言文本
        /// </summary>
        /// <param name="messageCode">语言文本标识</param>
        /// <param name="defaultMessage">默认的文本</param>
        /// <returns>取到的文本</returns>
        protected virtual string GetLanguage(string messageCode, string defaultMessage)
        {
            string text = string.Empty;

            try
            {
                text = defaultMessage;
                string filepath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "KzxCommon.dll");
                Assembly assembly = null;
                object obj = null;

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
            catch (Exception ex)
            {

            }
            return string.IsNullOrWhiteSpace(text) == true ? defaultMessage : text;
        }

        protected virtual void OnKzxBaseControlLoad()
        {
            object obj = null;
            PropertyInfo pi = null;
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i].Name.Equals("ValueControl", StringComparison.OrdinalIgnoreCase) == true)
                {
                    pi = this.Controls[i].GetType().GetProperty("ErrorIconAlignment");
                    if (pi != null)
                    {
                        pi.SetValue(this.Controls[i], ErrorIconAlignment.TopRight, null);
                    }
                }
            }
        }

        protected override void OnControlAdded(System.Windows.Forms.ControlEventArgs e)
        {
            base.OnControlAdded(e);
            OnKzxBaseControlLoad();
            SetAppearance();
        }

        protected virtual void SetSize(object sender, EventArgs e)
        {

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

        protected virtual void SetAppearance()
        {
            if (this.DesignMode == true)
            {
                if (this.DesigeEnabled == false)
                {
                    this.Enabled = false;
                }
                else
                {
                    this.Enabled = true;
                }
                if (this.DesigeVisible == false)
                {
                    this.BorderStyle = BorderStyle.Fixed3D;
                }
                else
                {
                    this.BorderStyle = BorderStyle.None;
                }
            }
            else
            {
                this.Enabled = this.DesigeEnabled;
                this.Visible = this.DesigeVisible;
            }
        }

        /// <summary>
        /// 创建一个插件信息表
        /// </summary>
        /// <returns></returns>
        public static DataTable CreatePluginDataTable()
        {
            DataTable dt = new DataTable("plugins");
            dt.Columns.Add("uGuid1", typeof(string));    //Sys_FrmXml的uGuid1
            dt.Columns.Add("uGuid2", typeof(string));    //主键
            dt.Columns.Add("sFormName", typeof(string));    //窗口名称
            dt.Columns.Add("sKey", typeof(string));      //控件的Key属性
            dt.Columns.Add("sEventName", typeof(string));    //事件名称
            dt.Columns.Add("sFileName", typeof(string)); //插件的dll文件名
            dt.Columns.Add("sMark", typeof(string));    //备注
            return dt;
        } 
        
    }
}
