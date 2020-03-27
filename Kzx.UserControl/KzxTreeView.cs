using Kzx.UserControl.UITypeEdit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms; 

namespace Kzx.UserControl
{
    /// <summary>
    /// 微软的树控件
    /// </summary>
    public partial class KzxTreeView : TreeView, IControl
    {
        private ToolTip _ToolTip = new ToolTip();
        protected delegate void UpdateDelegate(object source);

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

        /// <summary>
        /// 构造
        /// </summary>
        public KzxTreeView()
        {
            this.BeforeCheck += new TreeViewCancelEventHandler(Before_Check);
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
                return !this.LabelEdit;
            }
            set
            {
                this.LabelEdit = !value;
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

        private string _ParentTable;
        /// <summary>
        /// 指定父表
        /// </summary>
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

        private string _ParentFieldName = string.Empty;
        /// <summary>
        /// 父级结点标识字段
        /// </summary>
        [Category("数据"), Description("ParentFieldName,父级结点标识字段"), Browsable(true)]
        [McDisplayName("ParentFieldName")]
        public virtual string ParentFieldName
        {
            get
            {
                return this._ParentFieldName;
            }
            set
            {
                this._ParentFieldName = value;
            }
        }

        private string _KeyFieldName = string.Empty;
        /// <summary>
        /// 本级结点标识字段
        /// </summary>
        [Category("数据"), Description("KeyFieldName,本级结点标识字段"), Browsable(true)]
        [McDisplayName("KeyFieldName")]
        public virtual string KeyFieldName
        {
            get
            {
                return this._KeyFieldName;
            }
            set
            {
                this._KeyFieldName = value;
            }
        }

        public int SetDataBinding(object binding)
        {
            this.DataBindings.Add("Tag", binding, this.Field);
            return 1;
        }

        public int SetSourceTableBinding(object binding, string displayMember, string valueMember)
        {
            TreeViewHelper helper = new TreeViewHelper(this);
            //if (string.IsNullOrWhiteSpace(displayMember) == false && displayMember.Equals(this.DisplayMemberPath, StringComparison.OrdinalIgnoreCase) == false)
            //{
            //    this.DisplayMemberPath = displayMember;
            //}
            //if (string.IsNullOrWhiteSpace(valueMember) == false && valueMember.Equals(this.SelectedValuePath, StringComparison.OrdinalIgnoreCase) == false)
            //{
            //    this.SelectedValuePath = valueMember;
            //}
            helper.LoadData(binding);
            //return 1;
            //UpdateDelegate d = OnSetDataBinding;
            //this.BeginInvoke(d, new object[] { binding });
            return 1;
        }

        private void OnSetDataBinding(object binding)
        {
            TreeViewHelper helper = new TreeViewHelper(this);
            helper.LoadData(binding);
        }


        public object GetValue()
        {
            if (this.SelectedNode == null)
            {
                return null;
            }
            return this.SelectedNode.Text;
        }

        public int SetValue(object value)
        {
            if (this.SelectedNode == null)
            {
                return 0;
            }
            this.SelectedNode.Text = value == null ? string.Empty : value.ToString();
            return 1;
        }

        /// <summary>
        /// 是否触发验证事件
        /// </summary>
        /// <param name="e">true触发，false不触发</param>
        public void SetCausesValidation(bool e)
        {
            this.CausesValidation = e;
        }

        public int Validation(object sender)
        {
            return 1;
        }

        public void SetDefaultValue()
        {
            SetValue(this.DefaultValue);
        }

        /// <summary>
        /// 清除所有的错误信息
        /// </summary>
        public virtual void ClearErrors()
        {
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
            args.FieldName = this.Field;
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

        protected virtual void UserBeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            RaiseEvent(sender, "BeforeCheck", e);
        }

        protected virtual void UserAfterSelect(object sender, TreeViewEventArgs e)
        {
            RaiseEvent(sender, "AfterSelect", e);
        }

        protected virtual void UserAfterCheck(object sender, TreeViewEventArgs e)
        {
            RaiseEvent(sender, "AfterCheck", e);
        }

        protected virtual void UserAfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            RaiseEvent(sender, "AfterLabelEdit", e);
        }

        protected virtual void UserBeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            RaiseEvent(sender, "BeforeLabelEdit", e);
        }

        protected virtual void UserNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            RaiseEvent(sender, "NodeMouseClick", e);
        }

        protected virtual void UserNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            RaiseEvent(sender, "NodeMouseDoubleClick", e);
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

        private void Before_Check(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.TreeView.SelectedNode = e.Node;
        }

        public virtual void SetLayout()
        {
            TableLayoutPanel panel = this.Parent as TableLayoutPanel;
            if (panel != null)
            {
                panel.SetRow(this, this._LayoutRow);
                panel.SetColumn(this, this._LayoutColumn);
                panel.SetRowSpan(this, this._LayoutRowSpan);
                panel.SetColumnSpan(this, this._LayoutColumnSpan);
            }
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
         

        public event KzxControlOperateEventHandler KzxControlOperate;

        public event KzxGetLanguageEventHandler KzxGetLanguage; 
    }

    /// <summary>
    /// 树辅助类
    /// </summary>
    internal class TreeViewHelper
    {
        private KzxTreeView _KzxTreeView;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="tv">树对象</param>
        public TreeViewHelper(KzxTreeView tv)
        {
            this._KzxTreeView = tv;
        }

        public void LoadData(object view)
        {
            if (view is BindingSource)
            {
                OnLoadData(view as BindingSource);
            }
            else if (view is DataView)
            {
                OnLoadData(view as DataView);
            }
            else if (view is DataTable)
            {
                OnLoadData(view as DataTable);
            }
        }

        private void OnLoadData(BindingSource view)
        {
            if (view.DataSource is DataView)
            {
                OnLoadData(view.DataSource as DataView);
            }
            else if (view.DataSource is DataTable)
            {
                OnLoadData(view.DataSource as DataTable);
            }
        }

        private void OnLoadData(DataView view)
        {
            OnLoadData(view.Table);
        }

        private void OnLoadData(DataTable table)
        {
            List<DataRow> list = new List<DataRow>();
            DataRow row = null;
            DataRow[] rows = null;
            TreeNode node = null;
            string pkey = string.Empty;

            this._KzxTreeView.Nodes.Clear();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                pkey = table.Rows[i][this._KzxTreeView.ParentFieldName] == DBNull.Value || table.Rows[i][this._KzxTreeView.ParentFieldName] == null ? string.Empty : table.Rows[i][this._KzxTreeView.ParentFieldName].ToString();
                rows = table.Select("Convert(ISNULL(" + this._KzxTreeView.KeyFieldName + ",''),'System.String')='" + pkey + "'", string.Empty, DataViewRowState.CurrentRows);
                if (rows.Length <= 0)
                {
                    list.Add(table.Rows[i]);
                }
            }

            //rows = table.Select("ISNULL(" + this._KzxTreeView.ParentFieldName + ",'')=''", string.Empty, DataViewRowState.CurrentRows);
            for (int i = 0; i < list.Count; i++)
            {
                row = list[i];
                node = new TreeNode();
                node.Text = row[this._KzxTreeView.DisplayMemberPath] == DBNull.Value || row[this._KzxTreeView.DisplayMemberPath] == null ? string.Empty : row[this._KzxTreeView.DisplayMemberPath].ToString();
                node.Tag = row;
                this._KzxTreeView.Nodes.Add(node);
                OnLoadData(table, node, row);
            }
        }

        private void OnLoadData(DataTable table, TreeNode parentNode, DataRow parentRow)
        {
            DataRow row = null;
            DataRow[] rows = null;
            TreeNode node = null;
            string pkey = parentRow[this._KzxTreeView.KeyFieldName] == DBNull.Value || parentRow[this._KzxTreeView.KeyFieldName] == null ? string.Empty : parentRow[this._KzxTreeView.KeyFieldName].ToString();
            if (string.IsNullOrWhiteSpace(pkey) == false)
            {
                rows = table.Select("Convert(ISNULL(" + this._KzxTreeView.ParentFieldName + ",''),'System.String')='" + pkey + "'", string.Empty, DataViewRowState.CurrentRows);
                for (int i = 0; i < rows.Length; i++)
                {
                    row = rows[i];
                    node = new TreeNode();
                    node.Text = row[this._KzxTreeView.DisplayMemberPath] == DBNull.Value || row[this._KzxTreeView.DisplayMemberPath] == null ? string.Empty : row[this._KzxTreeView.DisplayMemberPath].ToString();
                    node.Tag = row;
                    parentNode.Nodes.Add(node);
                    OnLoadData(table, node, row);
                }
            }
        }
    }
}
