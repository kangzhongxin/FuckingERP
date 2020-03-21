  

using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Utils.Design;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Kzx.UserControl;
using Kzx.UserControl.UITypeEdit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml; 

namespace Kzx.UserControl
{
    public partial class KzxLookUpEdit : KzxBaseControl
    {
        private string _Value = string.Empty;

        public KzxLookUpEdit()
        {
            InitializeComponent();

            CaptionControl.AutoSizeMode = LabelAutoSizeMode.Vertical;
            CaptionControl.SizeChanged += SetSize;
            ValueControl.MouseWheel += KzxLookUpEdit_MouseWheel;
            ValueControl.KeyDown += ValueControl_KeyDown;
            ValueControl.Enter -= ValueControl_Enter;
            ValueControl.Enter += ValueControl_Enter;
            if (DesignMode)
            {
                Size = new Size(284, 21);
            }
        }

        protected override void DataValidating(object sender, CancelEventArgs e)
        {
            if (Validation(sender) < 1)
            {
                IsValidation = false;
                ValueControl.CancelPopup();
                ValueControl.ClosePopup();
                e.Cancel = false;  //焦点不能离开
            }
            else
            {
                IsValidation = true;
                e.Cancel = false;
            }
        }

        /// <summary>
        /// 设置背景色
        /// </summary>
        private void SetBackColor()
        {
            if (ValueControl.Properties.ReadOnly)
            {
                ValueControl.BackColor = Color.FromArgb(242, 242, 243);
            }
            else
            {
                if (_IsNull.Equals(true) == false)
                {
                    ValueControl.BackColor = Color.Yellow;
                }
                else
                {
                    _TextBackColor = Color.White;
                    ValueControl.BackColor = _TextBackColor;
                }
            }
        }

        #region 多语言设置

        private static MethodInfo _methodInfo;

        /// <summary>
        /// 获取多语言文本
        /// </summary>
        /// <param name="messageCode">语言文本标识</param>
        /// <param name="defaultMessage">默认的文本</param>
        /// <returns>取到的文本</returns>
        protected virtual string GetLanguage(string messageCode, string defaultMessage)
        {
            var filepath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "KzxCommon.dll");
            var text = string.Empty;
            Assembly assembly = null;
            object obj = null;

            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var item in assemblies)
                {
                    if (item.GetName().Name.Equals("KzxCommon", StringComparison.OrdinalIgnoreCase))
                    {
                        assembly = item;
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
            return string.IsNullOrWhiteSpace(text) ? defaultMessage : text;
        }

        #endregion 多语言设置

        private BorderStyles _BorderStyle = BorderStyles.Default;

        /// <summary>
        /// 边框显示格式
        /// </summary>
        [Category("数据格式"), Description("KzxBorderStyle,边框显示格式"), Browsable(true)]
        [McDisplayName("KzxBorderStyle")]
        public override BorderStyles KzxBorderStyle
        {
            get => ValueControl.Properties.BorderStyle;
            set
            {
                _BorderStyle = value;
                ValueControl.Properties.BorderStyle = value;
            }
        }

        /// <summary>
        /// 单据的数据源
        /// </summary>
        [Category("自定义"), Description("BillDataSet,单据的数据源"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [McDisplayName("BillDataSet")]
        public DataSet BillDataSet { get; set; } = new DataSet();

        /// <summary>
        /// 没有多语言的情况下的默认显示标题
        /// </summary>
        [Category("多语言"), Description("DesigeCaption,没有多语言的情况下的默认显示标题"), Browsable(true)]
        [McDisplayName("DesigeCaption")]
        public override string DesigeCaption
        {
            get => CaptionControl.Text.Trim();
            set => CaptionControl.Text = value;
        }

        private bool _IsNull = true;

        /// <summary>
        /// 可空性
        /// </summary>
        [Category("验证"), Description("IsNull,可空性"), Browsable(true)]
        [McDisplayName("IsNull")]
        public override bool IsNull
        {
            get
            {
                SetBackColor();
                return _IsNull;
            }
            set
            {
                _IsNull = value;
                SetBackColor();
            }
        }

        private bool _IsMouseWheelDisable = true;

        /// <summary>
        /// 是否禁用鼠标滚轮事件，默认为True
        /// </summary>
        [Category("下拉数据"), Description("是否禁用鼠标滚轮事件，默认为True"), Browsable(true)]
        [McDisplayName("IsMouseWheelDisable")]
        public bool IsMouseWheelDisable
        {
            get => _IsMouseWheelDisable;
            set => _IsMouseWheelDisable = true;
        }

        /// <summary>
        /// 源表实际值字段
        /// </summary>
        [Category("下拉数据"), Description("SelectedValuePath,源表实际值字段"), Browsable(true)]
        [McDisplayName("SelectedValuePath")]
        public override string SelectedValuePath
        {
            get => ValueControl.Properties.ValueMember;
            set => ValueControl.Properties.ValueMember = value;
        }

        /// <summary>
        /// 源表显示值字段
        /// </summary>
        [Category("下拉数据"), Description("DisplayMemberPath,源表显示值字段"), Browsable(true)]
        [McDisplayName("DisplayMemberPath")]
        public override string DisplayMemberPath
        {
            get => ValueControl.Properties.DisplayMember;
            set => ValueControl.Properties.DisplayMember = value;
        }

        private string _Columns = string.Empty;

        /// <summary>
        /// 列集合
        /// </summary>
        [Category("自定义"), Description("Columns,列集合"), Browsable(true), Editor(typeof(KzxLookUpEditUiTypeEdit), typeof(UITypeEditor))]
        [McDisplayName("Columns")]
        public string Columns
        {
            get => _Columns;
            set
            {
                XmlNode node = null;
                var doc = new XmlDocument();
                KzxLookUpColumnInfo info = null;

                _Columns = value;

                if (string.IsNullOrWhiteSpace(_Columns))
                {
                    return;
                }
                doc.LoadXml(_Columns);
                for (var i = 0; i < doc.DocumentElement.ChildNodes.Count; i++)
                {
                    node = doc.DocumentElement.ChildNodes[i];
                    info = ReadObject(node);
                    info.Caption = GetLanguage(info.MessageCode, info.Caption);
                    ValueControl.Properties.Columns.Add(info);
                }
                doc = null;
            }
        }

        /// <summary>
        /// Null值的显示信息
        /// </summary>
        [Category("自定义"), Description("NullText,Null值的显示信息"), Browsable(true)]
        [McDisplayName("NullText")]
        public string NullText
        {
            get => ValueControl.Properties.NullText;
            set => ValueControl.Properties.NullText = value;
        }

        /// <summary>
        /// 只读性
        /// </summary>
        [Category("验证"), Description("ReadOnly,只读性"), Browsable(true)]
        [McDisplayName("ReadOnly")]
        public override bool ReadOnly
        {
            get
            {
                SetBackColor();
                return ValueControl.Properties.ReadOnly;
            }
            set
            {
                ValueControl.Properties.ReadOnly = value;
                SetBackColor();
            }
        }

        /// <summary>
        /// 文本框的编辑样式
        /// </summary>
        [Category("自定义"), Description("TextEditStyle,文本框的编辑样式"), Browsable(true)]
        [McDisplayName("TextEditStyle")]
        public TextEditStyles TextEditStyle
        {
            get => ValueControl.Properties.TextEditStyle;
            set => ValueControl.Properties.TextEditStyle = value;
        }

        /// <summary>
        /// 标题控件
        /// </summary>
        public LabelControl CaptionLabel => CaptionControl;

        /// <summary>
        /// 勾选控件
        /// </summary>
        public KzxSearchLookUpEdit ContentControl => ValueControl;

        private string _ValueDependencyField = string.Empty;

        /// <summary>
        /// 数据携带
        /// </summary>
        [Category("下拉数据"), Description("ValueDependencyField,格式:控件标识Key=下拉数据源的Field,表示Key对应控件的值来自于下拉数据源中的Field对应列,多个表达式可以用逗号分隔.如master.sCode=code,master.sName=name"), Browsable(true), Editor(typeof(Kzx.UserControl.TextUiTypEdit), typeof(UITypeEditor))]
        [McDisplayName("ValueDependencyField")]
        public override string ValueDependencyField
        {
            get => _ValueDependencyField;
            set => _ValueDependencyField = value;
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        [Category("汽泡提示"), Description("ToolTipText,提示信息"), Browsable(true)]
        [McDisplayName("ToolTipText")]
        public override string ToolTipText
        {
            get => (ValueControl == null) ? string.Empty : ValueControl.ToolTip;
            set
            {
                if (ValueControl != null)
                {
                    ValueControl.ToolTip = value;
                }
            }
        }

        private Color _LabelForeColor = Color.Black;

        /// <summary>
        /// 标签字体颜色
        /// </summary>
        [Category("外观"), Description("LabelForeColor,标签字体颜色"), Browsable(true)]
        [McDisplayName("LabelForeColor")]
        public Color LabelForeColor
        {
            get => CaptionControl.ForeColor;
            set
            {
                _LabelForeColor = value;
                CaptionControl.ForeColor = value;
            }
        }

        private Color _TextForeColor = Color.Black;

        /// <summary>
        /// 内容字体颜色
        /// </summary>
        [Category("外观"), Description("TextForeColor,内容字体颜色"), Browsable(true)]
        [McDisplayName("TextForeColor")]
        public Color TextForeColor
        {
            get => ValueControl.ForeColor;
            set
            {
                _TextForeColor = value;
                ValueControl.ForeColor = value;
            }
        }

        private Color _TextBackColor = Color.White;

        /// <summary>
        /// 内容背景颜色
        /// </summary>
        [Category("外观"), Description("TextBackColor,内容背景颜色"), Browsable(true)]
        [McDisplayName("TextBackColor")]
        public Color TextBackColor
        {
            get => ValueControl.BackColor;
            set
            {
                _TextBackColor = value;
                ValueControl.BackColor = value;
            }
        }

        private string _ToolTipMessageCode = string.Empty;

        /// <summary>
        /// 提示多语言标识
        /// </summary>
        [Category("汽泡提示"), Description("ToolTipMessageCode,提示信息多语言标识"), Browsable(true)]
        [McDisplayName("ToolTipMessageCode")]
        public override string ToolTipMessageCode
        {
            get => _ToolTipMessageCode;
            set => _ToolTipMessageCode = value;
        }

        private string _FilterString = string.Empty;

        /// <summary>
        /// 下拉数据源的过滤表达式
        /// </summary>
        [Category("下拉数据"), Description("FilterString,下拉数据源的过滤表达式,如:下拉数据源中的列=单据的表|表中的字段名.如：code=mastertable|code,name=detailtable|name表示当前的下拉框的过滤条件为code=主表mastertable中的code字段, name=明细表中当前行的name"), Browsable(true), Editor(typeof(Kzx.UserControl.TextUiTypEdit), typeof(UITypeEditor))]
        [McDisplayName("FilterString")]
        public virtual string FilterString
        {
            get => _FilterString.Trim();
            set => _FilterString = value;
        }

        /// <summary>
        /// 可录入的最大长度
        /// </summary>
        [Category("验证"), Description("MaxLength,可录入的最大长度"), Browsable(true)]
        [McDisplayName("MaxLength")]
        public override int MaxLength
        {
            get => ValueControl.Properties.MaxLength;
            set => ValueControl.Properties.MaxLength = value;
        }

        /// <summary>
        /// 显示格式模式字符串(不验证录入)
        /// </summary>
        [Category("自定义"), Description("KzxFormatString,显示模式字符串(不验证录入数据)"), Browsable(true)]
        [McDisplayName("KzxFormatString")]
        public virtual string KzxFormatString
        {
            get => ContentControl.Properties.DisplayFormat.FormatString;
            set => ContentControl.Properties.DisplayFormat.FormatString = value;
        }

        /// <summary>
        /// 显示格式模式类型(不验证录入数据)
        /// </summary>
        [Category("自定义"), Description("KzxFormatType,显示格式模式类型(不验证录入数据)"), Browsable(true)]
        [McDisplayName("KzxFormatType")]
        public virtual FormatType KzxFormatType
        {
            get => ContentControl.Properties.DisplayFormat.FormatType;
            set => ContentControl.Properties.DisplayFormat.FormatType = value;
        }

        /// <summary>
        /// 取控件的值
        /// </summary>
        /// <return>Object</return>
        public override object GetValue()
        {
            object v = null;
            v = ValueControl.EditValue == null || ValueControl.EditValue == DBNull.Value ? string.Empty : ValueControl.EditValue.ToString();
            return v;
        }

        /// <summary>
        /// 设置数据绑定
        /// </summary>
        /// <param name="binding">数据绑定对象</param>
        /// <return>int</return>
        public override int SetDataBinding(object binding)
        {
            try
            {
                //FIX: 不能在构造时指定 OnPropertyChanged，只好在初始化完成之后再改变属性，从而达成免丢失焦点且不在绑定时破环原始值 2019年5月8日10点30分 晏耀

                var bd = new Binding("EditValue", binding, Field, true, DataSourceUpdateMode.OnValidation, string.Empty, FormatString);
                ValueControl.DataBindings.Add(bd);
                BindingObject = bd;

                bd.ControlUpdateMode = ControlUpdateMode.OnPropertyChanged;

                if (binding is BindingSource bindingSource)
                {
                    int maxlength;
                    if (bindingSource.DataSource is DataView dataView)
                    {
                        if (dataView.Table.Columns[Field].DataType == typeof(string))
                        {
                            maxlength = dataView.Table.Columns[Field].MaxLength;
                            if (maxlength >= 0)
                            {
                                MaxLength = maxlength;
                            }
                        }
                    }
                    else if (bindingSource.DataSource is DataTable dataTable)
                    {
                        if (dataTable.Columns[Field].DataType == typeof(string))
                        {
                            maxlength = dataTable.Columns[Field].MaxLength;
                            if (maxlength >= 0)
                            {
                                MaxLength = maxlength;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return 1;
        }

        /// <summary>
        /// 设置下拉框的数据源
        /// </summary>
        /// <param name="binding">下拉框的数据源</param>
        /// <param name="displayMember">显示值字段名</param>
        /// <param name="valueMember">实际值字段名</param>
        /// <returns>int</returns>
        public override int SetSourceTableBinding(object binding, string displayMember, string valueMember)
        {
            DisplayMemberPath = displayMember;
            SelectedValuePath = valueMember;
            ValueControl.Properties.DataSource = binding;
            return 1;
        }

        /// <summary>
        /// 设置控件的值
        /// </summary>
        /// <param name="value">控件的值</param>
        /// <return>int</return>
        public override int SetValue(object value)
        {
            ValueControl.EditValue = value == null || value == DBNull.Value ? string.Empty : value;
            return 1;
        }

        private void lookUpEdit_QueryPopUp(object sender, CancelEventArgs e)
        {
            //下拉框过滤数据
            var filterstring = string.Empty;
            string[] expressionarray = null;
            string[] filterarray = null;
            string[] valuearray = null;
            var s = string.Empty;
            var sb = new StringBuilder();
            var expressionsb = new StringBuilder();
            var bs = new BindingSource();

            bs = ValueControl.Properties.DataSource as BindingSource;
            RaiseEvent(sender, "QueryLoadData", e);

            filterstring = FilterString;
            if (string.IsNullOrWhiteSpace(filterstring) == false)
            {
                if (filterstring.IndexOf("|") >= 0)
                {
                    expressionarray = filterstring.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (var i = 0; i < expressionarray.Length; i++)
                    {
                        sb.Clear();
                        if (string.IsNullOrWhiteSpace(expressionarray[i]) == false)
                        {
                            filterarray = expressionarray[i].Split(new[] { "=", ">=", "<=", "<>", ">", "<" }, StringSplitOptions.RemoveEmptyEntries);
                            if (filterarray.Length >= 2)
                            {
                                for (var h = 0; h < 1; h++)
                                {
                                    sb.Append("Convert(" + filterarray[0] + ",'System.String')");
                                    s = expressionarray[i].Substring(filterarray[0].Length);
                                    if (s.StartsWith("="))
                                    {
                                        sb.Append("=");
                                    }
                                    else if (s.StartsWith(">="))
                                    {
                                        sb.Append(">=");
                                    }
                                    else if (s.StartsWith("<="))
                                    {
                                        sb.Append("<=");
                                    }
                                    else if (s.StartsWith("<>"))
                                    {
                                        sb.Append("<>");
                                    }
                                    else if (s.StartsWith(">"))
                                    {
                                        sb.Append(">");
                                    }
                                    else if (s.StartsWith("<"))
                                    {
                                        sb.Append("<");
                                    }
                                    else
                                    {
                                        sb.Append("=");
                                    }
                                    valuearray = filterarray[1].Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (valuearray.Length >= 2)
                                    {
                                        foreach (var item in ObjectList)
                                        {
                                            if (!(item is IControl)) continue;

                                            if (!((IControl)item).Table.Equals(valuearray[0], StringComparison.OrdinalIgnoreCase)
                                                || !((IControl)item).Field.Equals(valuearray[1], StringComparison.OrdinalIgnoreCase)) continue;

                                            var v = ((IControl)item).GetValue();
                                            if (v == null) continue;

                                            sb.Append("'" + ((v == DBNull.Value) ? string.Empty : v.ToString()) + "'");
                                            break;
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
                        if (ValueControl.Properties.DataSource is BindingSource ss)
                        {
                            ss.Filter = expressionsb.ToString();
                        }
                    }
                    else
                    {
                        if (ValueControl.Properties.DataSource is BindingSource ss)
                        {
                            ss.RemoveFilter();
                        }
                    }
                }
                else
                {
                    if (ValueControl.Properties.DataSource is BindingSource ss)
                    {
                        ss.Filter = filterstring;
                    }
                }
            }
            RaiseEvent(sender, "QueryPopUp", e);
        }

        private void lookUpEdit_Closed(object sender, ClosedEventArgs e)
        {
            if (ValueControl.Properties.DataSource is BindingSource ss)
            {
                ss.RemoveFilter();
            }
        }

        /// <summary>
        /// 还原默认值
        /// </summary>
        /// <return>void</return>
        public override void SetDefaultValue()
        {
            ValueControl.EditValue = DefaultValue;
        }

#if EVENT
        private void UserPopup(object sender, EventArgs e)
        {
            base.RaiseEvent(sender, "Popup", e);       }

        private void UserCloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            base.RaiseEvent(sender, "CloseUp", e);
        }

        private void UserClosed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            base.RaiseEvent(sender, "Closed", e);
        }

        private void UserClick(object sender, EventArgs e)
        {
            base.RaiseEvent(sender, "Click", e);
        }

        private void UserButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            base.RaiseEvent(sender, "ButtonClick", e);
        }

        private void UserDoubleClick(object sender, EventArgs e)
        {
            base.RaiseEvent(sender, "DoubleClick", e);
        }
#endif

        protected override void UserEditValueChanged(object sender, EventArgs e)
        {
            if (!(ValueControl.Properties.DataSource is BindingSource source))
            {
                source = new BindingSource();
                source.DataSource = ValueControl.Properties.DataSource;
            }

            base.RaiseEvent(sender, "EditValueChanged", e);
            //数据携带

            #region 数据携带

            ValueDependencyField = ValueDependencyField.Replace("\t", string.Empty);
            ValueDependencyField = ValueDependencyField.Replace("\n", string.Empty);
            ValueDependencyField = ValueDependencyField.Replace("\t", string.Empty);

            var fieldarray = (ValueDependencyField == null ? string.Empty : ValueDependencyField).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (fieldarray.Length > 0)
            {
                foreach (var field in fieldarray)
                {
                    if (string.IsNullOrWhiteSpace(field)) continue;

                    int rowIndex;
                    if (ValueControl.EditValue == null || ValueControl.EditValue == DBNull.Value || string.IsNullOrWhiteSpace(ValueControl.EditValue.ToString()))
                    {
                        rowIndex = -1;
                    }
                    else
                    {
                        try
                        {
                            rowIndex = source.Find(SelectedValuePath, ValueControl.EditValue);
                        }
                        catch
                        {
                            rowIndex = -1;
                        }
                    }

                    var expressionArray = field.Trim().Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                    if (expressionArray.Length != 2) continue;
                    if (string.IsNullOrWhiteSpace(expressionArray[0])
                        || string.IsNullOrWhiteSpace(expressionArray[1])) continue;

                    foreach (var item in ObjectList)
                    {
                        if (!(item is IControl KzxControl)) continue;
                        if (!KzxControl.Key.Equals(expressionArray[0], StringComparison.OrdinalIgnoreCase)) continue;

                        if (rowIndex >= 0)
                        {
                            var pi = source[rowIndex].GetType().GetProperty("Item", new[] { typeof(string) });
                            KzxControl.SetValue(pi != null
                                ? pi.GetValue(source[rowIndex], new object[] { expressionArray[1] })
                                : null);
                        }
                        else
                        {
                            KzxControl.SetValue(null);
                        }

                        break;
                    }
                }
            }

            #endregion 数据携带
        }

#if EVENT
        private void UserEditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            base.RaiseEvent(sender, "EditValueChanging", e);
        }
#endif

        private int _CaptionLabelWidth = 75;

        /// <summary>
        /// 显示标题宽度
        /// </summary>
        [Category("外观"), Description("CaptionLabelWidth,显示标题宽度"), Browsable(true)]
        [McDisplayName("CaptionLabelWidth")]
        public int CaptionLabelWidth
        {
            get => CaptionControl.Width;
            set
            {
                _CaptionLabelWidth = value;
                CaptionControl.Width = value;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (ValueControl != null && CaptionControl != null)
            {
                if (_CaptionLabelWidth <= 0 || string.IsNullOrWhiteSpace(CaptionControl.Text))
                {
                    ValueControl.Location = new Point(0);
                    ValueControl.Width = Width;
                    ValueControl.Top = 0;
                    ValueControl.Height = Height;
                }
                else
                {
                    ValueControl.Location = new Point(_CaptionLabelWidth + CaptionControl.Left + Space, 0);
                    ValueControl.Width = Width - ValueControl.Location.X;
                    ValueControl.Top = 0;
                    ValueControl.Height = Height;
                }
            }
        }

        protected override void SetSize(object sender, EventArgs e)
        {
            if (ValueControl != null && CaptionControl != null)
            {
                if (_CaptionLabelWidth != CaptionControl.Width)
                {
                    CaptionControl.Width = _CaptionLabelWidth;
                }
                if (_CaptionLabelWidth <= 0 || string.IsNullOrWhiteSpace(CaptionControl.Text))
                {
                    ValueControl.Location = new Point(0);
                    ValueControl.Width = Width;
                    ValueControl.Top = 0;
                    ValueControl.Height = Height;
                }
                else
                {
                    ValueControl.Location = new Point(_CaptionLabelWidth + CaptionControl.Left + Space, 0);
                    ValueControl.Width = Width - ValueControl.Location.X;
                    ValueControl.Top = 0;
                    ValueControl.Height = Height;
                }
            }
        }

        public static KzxLookUpColumnInfo ReadObject(XmlNode node)
        {
            var info = new KzxLookUpColumnInfo();
            var EnumTypeConverter = new EnumTypeConverter(typeof(HorzAlignment));
            var DefaultBooleanConverter = new DefaultBooleanConverter();
            for (var i = 0; i < node.Attributes.Count; i++)
            {
                if (node.Attributes[i].Name.Equals("Alignment", StringComparison.OrdinalIgnoreCase))
                {
                    info.Alignment = (HorzAlignment)EnumTypeConverter.ConvertFrom(node.Attributes[i].Value);
                }
                if (node.Attributes[i].Name.Equals("AllowSort", StringComparison.OrdinalIgnoreCase))
                {
                    info.AllowSort = (DefaultBoolean)DefaultBooleanConverter.ConvertFrom(node.Attributes[i].Value);
                }
                if (node.Attributes[i].Name.Equals("Caption", StringComparison.OrdinalIgnoreCase))
                {
                    info.Caption = node.Attributes[i].Value;
                }
                if (node.Attributes[i].Name.Equals("FieldName", StringComparison.OrdinalIgnoreCase))
                {
                    info.FieldName = node.Attributes[i].Value;
                }
                if (node.Attributes[i].Name.Equals("FormatString", StringComparison.OrdinalIgnoreCase))
                {
                    info.FormatString = node.Attributes[i].Value;
                }
                if (node.Attributes[i].Name.Equals("FormatType", StringComparison.OrdinalIgnoreCase))
                {
                    EnumTypeConverter = new EnumTypeConverter(typeof(FormatType));
                    info.FormatType = (FormatType)EnumTypeConverter.ConvertFrom(node.Attributes[i].Value);
                }
                if (node.Attributes[i].Name.Equals("SortOrder", StringComparison.OrdinalIgnoreCase))
                {
                    EnumTypeConverter = new EnumTypeConverter(typeof(ColumnSortOrder));
                    info.SortOrder = (ColumnSortOrder)EnumTypeConverter.ConvertFrom(node.Attributes[i].Value);
                }
                if (node.Attributes[i].Name.Equals("Visible", StringComparison.OrdinalIgnoreCase))
                {
                    info.Visible = Convert.ToBoolean(node.Attributes[i].Value);
                }
                if (node.Attributes[i].Name.Equals("Width", StringComparison.OrdinalIgnoreCase))
                {
                    info.Width = Convert.ToInt32(node.Attributes[i].Value);
                }
                if (node.Attributes[i].Name.Equals("MessageCode", StringComparison.OrdinalIgnoreCase))
                {
                    info.MessageCode = node.Attributes[i].Value;
                }
            }
            return info;
        }

        public static string WriteObject(KzxLookUpColumnInfo e)
        {
            var xmlsb = new StringBuilder();
            var EnumTypeConverter = new EnumTypeConverter(typeof(HorzAlignment));
            var DefaultBooleanConverter = new DefaultBooleanConverter();

            xmlsb.Append("<" + e.Caption + " ");
            xmlsb.Append("Alignment=\"" + EnumTypeConverter.ConvertToString(e.Alignment) + "\" ");
            xmlsb.Append("AllowSort=\"" + DefaultBooleanConverter.ConvertToString(e.AllowSort) + "\" ");
            xmlsb.Append("Caption=\"" + e.Caption + "\" ");
            xmlsb.Append("FieldName=\"" + e.FieldName + "\" ");
            xmlsb.Append("FormatString=\"" + e.FormatString + "\" ");
            EnumTypeConverter = new EnumTypeConverter(typeof(FormatType));
            xmlsb.Append("FormatType=\"" + EnumTypeConverter.ConvertToString(e.FormatType) + "\" ");
            EnumTypeConverter = new EnumTypeConverter(typeof(ColumnSortOrder));
            xmlsb.Append("SortOrder=\"" + EnumTypeConverter.ConvertToString(e.SortOrder) + "\" ");
            xmlsb.Append("Visible=\"" + e.Visible.ToString() + "\" ");
            xmlsb.Append("Width=\"" + e.Width.ToString() + "\" ");
            xmlsb.Append("MessageCode=\"" + e.MessageCode.ToString() + "\" ");
            xmlsb.Append("/>");

            return xmlsb.ToString();
        }

        public static string CreateName(List<KzxLookUpColumnInfo> cc, string type)
        {
            var min = int.MaxValue;
            var max = int.MinValue;
            var count = 0;

            for (var i = 0; i < cc.Count; i++)
            {
                var name = cc[i].Caption;
                if (name.StartsWith(type))
                {
                    try
                    {
                        count++;
                        var value = int.Parse(name.Substring(type.Length));

                        if (value < min)
                            min = value;

                        if (value > max)
                            max = value;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            if (count == 0)
            {
                return type + "1";
            }
            else if (min > 1)
            {
                var j = min - 1;
                return type + j.ToString();
            }
            else
            {
                var j = max + 1;
                return type + j.ToString();
            }
        }

        private void KzxLookUpEdit_Load(object sender, EventArgs e)
        {
            LayoutControl();
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="eventInfoTable">事件信息表</param>
        public override void BindingEvent(DataTable eventInfoTable)
        {
            BindingEvent(ValueControl, eventInfoTable);
        }

        private void LayoutControl()
        {
            NullText = string.Empty;

            CaptionControl.Appearance.TextOptions.HAlignment = HorzAlignment.Far;
            ValueControl.CausesValidation = true;
            ValueControl.Properties.TextEditStyle = TextEditStyles.Standard;
            ValueControl.Properties.BorderStyle = _BorderStyle;
            ValueControl.Properties.AutoHeight = false;
            CaptionControl.Width = _CaptionLabelWidth;
            BindingEvent(ValueControl, PluginInfoTable);
            ValueControl.Validating += DataValidating;
            ValueControl.EditValueChanging -= UserEditValueChanging;
            ValueControl.EditValueChanging += UserEditValueChanging;
            ValueControl.EditValueChanged -= UserEditValueChanged;
            ValueControl.EditValueChanged += UserEditValueChanged;

            SetBackColor();
            if (DesignMode)
            {
                DesigeCaption = GetLanguage(MessageCode, DesigeCaption);
            }
#if EVENT
            this.ValueControl.Popup += new EventHandler(UserPopup);
            this.ValueControl.Closed += new ClosedEventHandler(UserClosed);
            this.ValueControl.CloseUp += new CloseUpEventHandler(UserCloseUp);
            this.ValueControl.Click += new EventHandler(UserClick);
            this.ValueControl.ButtonClick += new ButtonPressedEventHandler(UserButtonClick);
            this.ValueControl.DoubleClick += new EventHandler(UserDoubleClick);
            this.ValueControl.EditValueChanged += new EventHandler(UserEditValueChanged);
            this.ValueControl.EditValueChanging += new ChangingEventHandler(UserEditValueChanging);
#endif
        }

#if BorderStyle
        private int WM_PAINT = 0x000F;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT)
            {
                Pen pen = new Pen(SystemColors.ControlDark, 1.5f);
                using (Graphics g = this.CreateGraphics())
                {
                    g.DrawLine(pen, new Point(this.ValueControl.Left, this.Size.Height - 1), new Point(this.Size.Width, this.Size.Height - 1));
                }
            }
        }
#endif

        /// <summary>
        /// 是否触发验证事件
        /// </summary>
        /// <param name="e">true触发，false不触发</param>
        public override void SetCausesValidation(bool e)
        {
            ValueControl.CausesValidation = e;
        }

        private void ValueControl_Enter(object sender, EventArgs e)
        {
            _Value = ValueControl.EditValue == null || ValueControl.EditValue == DBNull.Value ? string.Empty : ValueControl.EditValue.ToString();
            ValueControl.CausesValidation = true;
        }

        protected override void UserLeave(object sender, EventArgs e)
        {
            RaiseEvent(sender, "Leave", e);
            if (_Value.Equals((ValueControl.EditValue == null || ValueControl.EditValue == DBNull.Value ? string.Empty : ValueControl.EditValue.ToString())) == false)
            {
                RaiseEvent(sender, "KzxValueChanged", e);
                _Value = ValueControl.EditValue == null || ValueControl.EditValue == DBNull.Value ? string.Empty : ValueControl.EditValue.ToString();
            }
        }

        private void KzxLookUpEdit_MouseWheel(object sender, MouseEventArgs e)
        {
            //禁用鼠标滚轮
            if (_IsMouseWheelDisable)
            {
                if (e is DXMouseEventArgs)
                {
                    ((DXMouseEventArgs)e).Handled = true;
                }
            }
        }

        private void ValueControl_KeyDown(object sender, KeyEventArgs e)
        {
            //禁用鼠标滚轮时同时禁用键盘上下键
            if (_IsMouseWheelDisable)
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                {
                    e.Handled = true;
                }
            }
        }
    }

    /// <summary>
    /// KzxLookUpEdit列
    /// </summary>
    public partial class KzxLookUpColumnInfo : LookUpColumnInfo
    {
        private string _MessageCode = "0";

        /// <summary>
        /// 多语言环境下显示文本的对应标识
        /// </summary>
        [Category("多语言"), Description("MessageCode,多语言环境下显示文本的对应标识"), Browsable(true)]
        [McDisplayName("MessageCode")]
        public virtual string MessageCode
        {
            get => _MessageCode;
            set => _MessageCode = value;
        }

        private object _Tag;

        [Category("数据"), Description("Tag,标志"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Tag
        {
            get => _Tag;
            set => _Tag = value;
        }

        /// <summary>
        /// 构造
        /// </summary>
        public KzxLookUpColumnInfo()
            : base()
        {
        }
    }
}