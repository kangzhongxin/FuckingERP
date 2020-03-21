using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Utils.Design;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using Kzx.UserControl.UITypeEdit; 

namespace Kzx.UserControl
{
    public partial class KzxSearchComboboxEdit : KzxBaseControl
    {
        public List<KzxLookUpColumnInfo> ColumnList = new List<KzxLookUpColumnInfo>();
        private string _Value = string.Empty;

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

        public KzxSearchComboboxEdit()
        {
            InitializeComponent();

            this.ValueControl.QueryPopUp += new CancelEventHandler(lookUpEdit_QueryPopUp);
            this.ValueControl.Closed += new ClosedEventHandler(lookUpEdit_Closed);

            this.CaptionControl.AutoSizeMode = LabelAutoSizeMode.Vertical;
            this.CaptionControl.SizeChanged += new EventHandler(SetSize);
            this.ValueControl.Enter -= new EventHandler(ValueControl_Enter);
            this.ValueControl.Enter += new EventHandler(ValueControl_Enter);

            this._SearchLookUpEditView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this._SearchLookUpEditView.Name = "gridLookUpEdit1View";
            this._SearchLookUpEditView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this._SearchLookUpEditView.OptionsView.ShowGroupPanel = false;
            this.ValueControl.Properties.PopupFormSize = new System.Drawing.Size((int)(this.ValueControl.Width * 2), (int)(this.ValueControl.Width * 1.5));

            if (this.DesignMode == true)
            {
                this.Size = new Size(284, 21);
            }
        }

        private int _ItemIndex = -1;
        /// <summary>
        /// 选中项的下标
        /// </summary>
        [Category("自定义"), Description("ItemIndex,选中项的下标"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [McDisplayName("ItemIndex")]
        public int ItemIndex
        {
            get
            {
                return this._ItemIndex;
            }
            protected set
            {
                this._ItemIndex = value;
            }
        }

        private DataRow _CurrentItem = null;
        /// <summary>
        /// 选中项的DataRow对象
        /// </summary>
        [Category("自定义"), Description("CurrentItem,选中项的DataRow对象"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [McDisplayName("CurrentItem")]
        public DataRow CurrentItem
        {
            get
            {
                return this._CurrentItem;
            }
            protected set
            {
                this._CurrentItem = value;
            }
        }


        private DataSet _BillDataSet = new DataSet();
        /// <summary>
        /// 单据的数据源
        /// </summary>
        [Category("自定义"), Description("BillDataSet,单据的数据源"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [McDisplayName("BillDataSet")]
        public DataSet BillDataSet
        {
            get
            {
                return this._BillDataSet;
            }
            set
            {
                this._BillDataSet = value;
            }
        }

        /// <summary>
        /// 没有多语言的情况下的默认显示标题
        /// </summary>
        [Category("多语言"), Description("DesigeCaption,没有多语言的情况下的默认显示标题"), Browsable(true)]
        [McDisplayName("DesigeCaption")]
        public override string DesigeCaption
        {
            get
            {
                return this.CaptionControl.Text.Trim();
            }
            set
            {
                this.CaptionControl.Text = value;
            }
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
                return this._IsNull;
            }
            set
            {
                this._IsNull = value;
                SetBackColor();
            }
        }

        /// <summary>
        /// 源表实际值字段
        /// </summary>
        [Category("下拉数据"), Description("SelectedValuePath,源表实际值字段"), Browsable(true)]
        [McDisplayName("SelectedValuePath")]
        public override string SelectedValuePath
        {
            get
            {
                return this.ValueControl.Properties.ValueMember;
            }
            set
            {
                this.ValueControl.Properties.ValueMember = value;
            }
        }

        /// <summary>
        /// 源表显示值字段
        /// </summary>
        [Category("下拉数据"), Description("DisplayMemberPath,源表显示值字段"), Browsable(true)]
        [McDisplayName("DisplayMemberPath")]
        public override string DisplayMemberPath
        {
            get
            {
                return this.ValueControl.Properties.DisplayMember;
            }
            set
            {
                this.ValueControl.Properties.DisplayMember = value;
            }
        }

        private string _Columns = string.Empty;
        /// <summary>
        /// 列集合
        /// </summary>
        [Category("下拉数据"), Description("Columns,列集合"), Browsable(true), Editor(typeof(KzxLookUpEditUiTypeEdit), typeof(UITypeEditor))]
        [McDisplayName("Columns")]
        public string Columns
        {
            get
            {
                return this._Columns;
            }
            set
            {
                GridColumn column = null;
                XmlNode node = null;
                XmlDocument doc = new XmlDocument();
                KzxLookUpColumnInfo info = null;

                this._Columns = value;

                if (string.IsNullOrWhiteSpace(this._Columns) == true)
                {
                    return;
                }
                doc.LoadXml(this._Columns);
                this.ColumnList.Clear();
                for (int i = 0; i < doc.DocumentElement.ChildNodes.Count; i++)
                {
                    column = new GridColumn();
                    node = doc.DocumentElement.ChildNodes[i];

                    info = KzxLookUpEdit.ReadObject(node);
                    column.Caption = info.Caption;
                    column.FieldName = info.FieldName;
                    column.Width = info.Width;
                    column.Visible = info.Visible;
                    info.Tag = column;
                    this.ColumnList.Add(info);
                    column.Caption = GetLanguage(info.MessageCode, info.Caption);
                    this._SearchLookUpEditView.Columns.Add(column);
                }
                doc = null;
            }
        }

        /// <summary>
        /// Null值的显示信息
        /// </summary>
        [Category("数据"), Description("NullText,Null值的显示信息"), Browsable(true)]
        [McDisplayName("NullText")]
        public string NullText
        {
            get
            {
                return this.ValueControl.Properties.NullText;
            }
            set
            {
                this.ValueControl.Properties.NullText = value;
            }
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
                return this.ValueControl.Properties.ReadOnly;
            }
            set
            {
                this.ValueControl.Properties.ReadOnly = value;
                SetBackColor();
            }
        }

        private TextEditStyles _TextEditStyle = TextEditStyles.DisableTextEditor;
        /// <summary>
        /// 文本框的编辑样式
        /// </summary>
        [Category("外观"), Description("TextEditStyle,文本框的编辑样式"), Browsable(true)]
        [McDisplayName("TextEditStyle")]
        public TextEditStyles TextEditStyle
        {
            get
            {
                return this._TextEditStyle;
            }
            set
            {
                this._TextEditStyle = value;
                //this.ValueControl.Properties.TextEditStyle = value;
            }
        }

        /// <summary>
        /// 标题控件
        /// </summary>
        public LabelControl CaptionLabel
        {
            get
            {
                return this.CaptionControl;
            }
        }

        /// <summary>
        /// 下拉框控件
        /// </summary>
        public SearchLookUpEdit ContentControl
        {
            get
            {
                return this.ValueControl;
            }
        }

        private string _ValueDependencyField = string.Empty;
        /// <summary>
        /// 数据携带
        /// </summary>
        [Category("下拉数据"), Description("ValueDependencyField,格式:控件标识Key=下拉数据源的Field,表示Key对应控件的值来自于下拉数据源中的Field对应列,多个表达式可以用逗号分隔.如master.sCode=code,master.sName=name"), Browsable(true), Editor(typeof(TextUiTypEdit), typeof(UITypeEditor))]
        [McDisplayName("ValueDependencyField")]
        public override string ValueDependencyField
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

        /// <summary>
        /// 提示信息
        /// </summary>
        [Category("汽泡提示"), Description("ToolTipText,提示信息"), Browsable(true)]
        [McDisplayName("ToolTipText")]
        public override string ToolTipText
        {
            get
            {
                return (ValueControl == null) == true ? string.Empty : ValueControl.ToolTip;
            }
            set
            {
                if (ValueControl != null)
                {
                    ValueControl.ToolTip = value;
                }
                if (CaptionControl != null)
                {
                    CaptionControl.ToolTip = value;
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
            get
            {
                return this.CaptionControl.ForeColor;
            }
            set
            {
                this._LabelForeColor = value;
                this.CaptionControl.ForeColor = value;
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
            get
            {
                return this.ValueControl.ForeColor;
            }
            set
            {
                this._TextForeColor = value;
                this.ValueControl.ForeColor = value;
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
            get
            {
                return this.ValueControl.BackColor;
            }
            set
            {
                this._TextBackColor = value;
                this.ValueControl.BackColor = value;
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
            get
            {
                return this._ToolTipMessageCode;
            }
            set
            {
                this._ToolTipMessageCode = value;
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
            }
        }

        /// <summary>
        /// 可录入的最大长度
        /// </summary>
        [Category("验证"), Description("MaxLength,可录入的最大长度"), Browsable(true)]
        [McDisplayName("MaxLength")]
        public override int MaxLength
        {
            get
            {
                return this.ValueControl.Properties.MaxLength;
            }
            set
            {
                this.ValueControl.Properties.MaxLength = value;
            }
        }

        private object _ResourceDataSource = null;
        /// <summary>
        /// 下拉数据源
        /// </summary>
        [Category("自定义"), Description("ResourceDataSource,下拉数据源"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [McDisplayName("ResourceDataSource")]
        public object ResourceDataSource
        {
            get
            {
                return this._ResourceDataSource;
            }
        }

        /// <summary>
        /// 取控件的值
        /// </summary>
        /// <return>Object</return>
        public override object GetValue()
        {
            DataRowView rowview = null;
            BindingSource bs = null;
            object v = null;

            v = this.ValueControl.EditValue == null || this.ValueControl.EditValue == DBNull.Value ? string.Empty : this.ValueControl.EditValue.ToString();
            return v;
        }

        /// <summary>
        /// 设置数据绑定
        /// </summary>
        /// <param name="binding">数据绑定对象</param>
        /// <return>int</return>
        public override int SetDataBinding(object binding)
        {
            this.BindingObject = this.ValueControl.DataBindings.Add("EditValue", binding, this.Field, true, DataSourceUpdateMode.OnValidation, string.Empty, this.FormatString);
            SetColumnDisplayFormat();
            if (binding is BindingSource)
            {
                int maxlength = 0;
                if (((BindingSource)binding).DataSource is DataView)
                {
                    if (((DataView)(((BindingSource)binding).DataSource)).Table.Columns[this.Field].DataType == typeof(string))
                    {
                        maxlength = ((DataView)(((BindingSource)binding).DataSource)).Table.Columns[this.Field].MaxLength;
                        if (maxlength >= 0)
                        {
                            this.MaxLength = maxlength;
                        }
                    }
                }
                else if (((BindingSource)binding).DataSource is DataTable)
                {
                    if (((DataTable)(((BindingSource)binding).DataSource)).Columns[this.Field].DataType == typeof(string))
                    {
                        maxlength = ((DataTable)(((BindingSource)binding).DataSource)).Columns[this.Field].MaxLength;
                        if (maxlength >= 0)
                        {
                            this.MaxLength = maxlength;
                        }
                    }
                }
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
            this.DisplayMemberPath = displayMember;
            this.SelectedValuePath = valueMember;
            this.ValueControl.Properties.DataSource = binding;
            this._ResourceDataSource = binding;
            return 1;
        }

        /// <summary>
        /// 设置控件的值
        /// </summary>
        /// <param name="value">控件的值</param>
        /// <return>int</return>
        public override int SetValue(object value)
        {
            this.ValueControl.EditValue = value == null || value == DBNull.Value ? string.Empty : value;
            //if (this.BindingObject != null)
            //{
            //    this.BindingObject.WriteValue();
            //}
            //if (this.BindingObject != null)
            //{
            //    if (this.BindingObject.IsBinding == true)
            //    {
            //        DataRowView rowview = this.BindingObject.BindingManagerBase.Current as DataRowView;
            //        if (rowview != null)
            //        {
            //            if (rowview.DataView.AllowEdit == true)
            //            {
            //                this.ValueControl.EditValue = value == null || value == DBNull.Value ? string.Empty : value;
            //                rowview[this.Field] = value == null ? DBNull.Value : value;
            //            }
            //        }
            //        else
            //        {
            //            this.ValueControl.EditValue = value == null || value == DBNull.Value ? string.Empty : value;
            //        }
            //    }
            //    else
            //    {
            //        this.ValueControl.EditValue = value == null || value == DBNull.Value ? string.Empty : value;
            //    }
            //}
            //else
            //{
            //    this.ValueControl.EditValue = value == null || value == DBNull.Value ? string.Empty : value;
            //}
            return 1;
        }

        private void lookUpEdit_QueryPopUp(object sender, CancelEventArgs e)
        {

        }

        private void lookUpEdit_Closed(object sender, ClosedEventArgs e)
        {
            BindingSource ss = this.ValueControl.Properties.DataSource as BindingSource;
            if (ss != null)
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
            this.ValueControl.EditValue = this.DefaultValue;
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
            int rowindex = 0;
            string[] fieldarray = null;
            string[] expressionarray = null;
            BindingSource source = this.ValueControl.Properties.DataSource as BindingSource;
            if (source == null)
            {
                source = new BindingSource();
                source.DataSource = this.ValueControl.Properties.DataSource;
            }

            base.RaiseEvent(sender, "EditValueChanged", e);

            //数据携带
            #region 数据携带

            fieldarray = (this.ValueDependencyField == null ? string.Empty : this.ValueDependencyField).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (fieldarray.Length > 0)
            {
                for (int i = 0; i < fieldarray.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(fieldarray[i]) == false)
                    {
                        if (this.ValueControl.EditValue == null || this.ValueControl.EditValue == DBNull.Value || string.IsNullOrWhiteSpace(this.ValueControl.EditValue.ToString()) == true)
                        {
                            rowindex = -1;
                        }
                        else
                        {
                            try
                            {
                                rowindex = source.Find(this.SelectedValuePath, this.ValueControl.EditValue);
                            }
                            catch
                            {
                                rowindex = -1;
                            }
                        }
                        expressionarray = fieldarray[i].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        if (expressionarray.Length == 2)
                        {
                            if (string.IsNullOrWhiteSpace(expressionarray[0]) == false && string.IsNullOrWhiteSpace(expressionarray[1]) == false)
                            {
                                for (int h = 0; h < this.ObjectList.Count; h++)
                                {
                                    if (this.ObjectList[h] is IControl)
                                    {
                                        if (((IControl)this.ObjectList[h]).Key.Equals(expressionarray[0], StringComparison.OrdinalIgnoreCase) == true)
                                        {
                                            if (rowindex >= 0)
                                            {
                                                PropertyInfo pi = source[rowindex].GetType().GetProperty("Item", new Type[] { typeof(string) });
                                                if (pi != null)
                                                {
                                                    ((IControl)this.ObjectList[h]).SetValue(pi.GetValue(source[rowindex], new object[] { expressionarray[1] }));
                                                    //if (this.BindingObject != null && string.IsNullOrWhiteSpace(((IControl)this.ObjectList[h]).Field) == false)
                                                    //{
                                                    //    if (this.BindingObject.DataSource is BindingSource)
                                                    //    {
                                                    //        if (((BindingSource)this.BindingObject.DataSource).Current != null && (((BindingSource)this.BindingObject.DataSource).AllowEdit == true || ((BindingSource)this.BindingObject.DataSource).AllowNew == true))
                                                    //        {
                                                    //            DataRowView rowview = ((BindingSource)this.BindingObject.DataSource).Current as DataRowView;
                                                    //            if (rowview != null)
                                                    //            {
                                                    //                rowview[((IControl)this.ObjectList[h]).Field] = pi.GetValue(source[rowindex], new object[] { expressionarray[1] });
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //}
                                                }
                                                else
                                                {
                                                    ((IControl)this.ObjectList[h]).SetValue(DBNull.Value);
                                                    //if (this.BindingObject != null && string.IsNullOrWhiteSpace(((IControl)this.ObjectList[h]).Field) == false)
                                                    //{
                                                    //    if (this.BindingObject.DataSource is BindingSource)
                                                    //    {
                                                    //        if (((BindingSource)this.BindingObject.DataSource).Current != null && (((BindingSource)this.BindingObject.DataSource).AllowEdit == true || ((BindingSource)this.BindingObject.DataSource).AllowNew == true))
                                                    //        {
                                                    //            DataRowView rowview = ((BindingSource)this.BindingObject.DataSource).Current as DataRowView;
                                                    //            if (rowview != null)
                                                    //            {
                                                    //                rowview[((IControl)this.ObjectList[h]).Field] = DBNull.Value;
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //}
                                                }
                                            }
                                            else
                                            {
                                                ((IControl)this.ObjectList[h]).SetValue(DBNull.Value);
                                                //if (this.BindingObject != null && string.IsNullOrWhiteSpace(((IControl)this.ObjectList[h]).Field) == false)
                                                //{
                                                //    if (this.BindingObject.DataSource is BindingSource)
                                                //    {
                                                //        if (((BindingSource)this.BindingObject.DataSource).Current != null && (((BindingSource)this.BindingObject.DataSource).AllowEdit == true || ((BindingSource)this.BindingObject.DataSource).AllowNew == true))
                                                //        {
                                                //            DataRowView rowview = ((BindingSource)this.BindingObject.DataSource).Current as DataRowView;
                                                //            if (rowview != null)
                                                //            {
                                                //                rowview[((IControl)this.ObjectList[h]).Field] = DBNull.Value;
                                                //            }
                                                //        }
                                                //    }
                                                //}
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
        }


#if EVENT
        private void UserEditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            base.RaiseEvent(sender, "EditValueChanging", e);
        }
#endif

        private Int32 _CaptionLabelWidth = 75;
        /// <summary>
        /// 显示标题宽度
        /// </summary>
        [Category("外观"), Description("CaptionLabelWidth,显示标题宽度"), Browsable(true)]
        [McDisplayName("CaptionLabelWidth")]
        public Int32 CaptionLabelWidth
        {
            get
            {
                return this.CaptionControl.Width;
            }
            set
            {
                this._CaptionLabelWidth = value;
                this.CaptionControl.Width = value;
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
                return this.ContentControl.Properties.DisplayFormat.FormatString;
            }
            set
            {
                this._KzxFormatString = value;
                this.ContentControl.Properties.DisplayFormat.FormatString = value;
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
                return this.ContentControl.Properties.DisplayFormat.FormatType;
            }
            set
            {
                this._KzxFormatType = value;
                this.ContentControl.Properties.DisplayFormat.FormatType = value;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.ValueControl != null && this.CaptionControl != null)
            {
                if (this._CaptionLabelWidth <= 0 || string.IsNullOrWhiteSpace(this.CaptionControl.Text) == true)
                {
                    this.ValueControl.Location = new Point(0);
                    this.ValueControl.Dock = DockStyle.Fill;
                    //this.ValueControl.Width = this.Width;
                    //this.ValueControl.Top = 0;
                    //this.ValueControl.Height = this.Height;
                }
                else
                {
                    this.ValueControl.Dock = DockStyle.None;
                    this.ValueControl.Location = new Point(this._CaptionLabelWidth + this.CaptionControl.Left + KzxBaseControl.Space, 0);
                    this.ValueControl.Width = this.Width - this.ValueControl.Location.X;
                    this.ValueControl.Top = 0;
                    this.ValueControl.Height = this.Height;
                }
            }
        }

        protected override void SetSize(object sender, EventArgs e)
        {
            if (this.ValueControl != null && this.CaptionControl != null)
            {
                if (this._CaptionLabelWidth != this.CaptionControl.Width)
                {
                    this.CaptionControl.Width = this._CaptionLabelWidth;
                }
                if (this._CaptionLabelWidth <= 0 || string.IsNullOrWhiteSpace(this.CaptionControl.Text) == true)
                {
                    this.ValueControl.Location = new Point(0);
                    this.ValueControl.Dock = DockStyle.Fill;
                    //this.ValueControl.Width = this.Width;
                    //this.ValueControl.Top = 0;
                    //this.ValueControl.Height = this.Height;
                }
                else
                {
                    this.ValueControl.Dock = DockStyle.None;
                    this.ValueControl.Location = new Point(this._CaptionLabelWidth + this.CaptionControl.Left + KzxBaseControl.Space, 0);
                    this.ValueControl.Width = this.Width - this.ValueControl.Location.X;
                    this.ValueControl.Top = 0;
                    this.ValueControl.Height = this.Height;
                }
            }
        }

        public static KzxLookUpColumnInfo ReadObject(XmlNode node)
        {
            KzxLookUpColumnInfo info = new KzxLookUpColumnInfo();
            EnumTypeConverter EnumTypeConverter = new EnumTypeConverter(typeof(HorzAlignment));
            DefaultBooleanConverter DefaultBooleanConverter = new DefaultBooleanConverter();
            for (int i = 0; i < node.Attributes.Count; i++)
            {
                if (node.Attributes[i].Name.Equals("Alignment", StringComparison.OrdinalIgnoreCase) == true)
                {
                    info.Alignment = (HorzAlignment)EnumTypeConverter.ConvertFrom(node.Attributes[i].Value);
                }
                if (node.Attributes[i].Name.Equals("AllowSort", StringComparison.OrdinalIgnoreCase) == true)
                {
                    info.AllowSort = (DefaultBoolean)DefaultBooleanConverter.ConvertFrom(node.Attributes[i].Value);
                }
                if (node.Attributes[i].Name.Equals("Caption", StringComparison.OrdinalIgnoreCase) == true)
                {
                    info.Caption = node.Attributes[i].Value;
                }
                if (node.Attributes[i].Name.Equals("FieldName", StringComparison.OrdinalIgnoreCase) == true)
                {
                    info.FieldName = node.Attributes[i].Value;
                }
                if (node.Attributes[i].Name.Equals("FormatString", StringComparison.OrdinalIgnoreCase) == true)
                {
                    info.FormatString = node.Attributes[i].Value;
                }
                if (node.Attributes[i].Name.Equals("FormatType", StringComparison.OrdinalIgnoreCase) == true)
                {
                    EnumTypeConverter = new DevExpress.Utils.Design.EnumTypeConverter(typeof(FormatType));
                    info.FormatType = (FormatType)EnumTypeConverter.ConvertFrom(node.Attributes[i].Value);
                }
                if (node.Attributes[i].Name.Equals("SortOrder", StringComparison.OrdinalIgnoreCase) == true)
                {
                    EnumTypeConverter = new DevExpress.Utils.Design.EnumTypeConverter(typeof(ColumnSortOrder));
                    info.SortOrder = (ColumnSortOrder)EnumTypeConverter.ConvertFrom(node.Attributes[i].Value);
                }
                if (node.Attributes[i].Name.Equals("Visible", StringComparison.OrdinalIgnoreCase) == true)
                {
                    info.Visible = Convert.ToBoolean(node.Attributes[i].Value);
                }
                if (node.Attributes[i].Name.Equals("Width", StringComparison.OrdinalIgnoreCase) == true)
                {
                    info.Width = Convert.ToInt32(node.Attributes[i].Value);
                }
                if (node.Attributes[i].Name.Equals("MessageCode", StringComparison.OrdinalIgnoreCase) == true)
                {
                    info.MessageCode = node.Attributes[i].Value;
                }
            }
            return info;
        }

        public static string WriteObject(KzxLookUpColumnInfo e)
        {
            StringBuilder xmlsb = new StringBuilder();
            EnumTypeConverter EnumTypeConverter = new EnumTypeConverter(typeof(HorzAlignment));
            DefaultBooleanConverter DefaultBooleanConverter = new DefaultBooleanConverter();

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
            int min = Int32.MaxValue;
            int max = Int32.MinValue;
            int count = 0;

            for (int i = 0; i < cc.Count; i++)
            {
                string name = cc[i].Caption;
                if (name.StartsWith(type))
                {
                    try
                    {
                        count++;
                        int value = Int32.Parse(name.Substring(type.Length));

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
                int j = min - 1;
                return type + j.ToString();
            }
            else
            {
                int j = max + 1;
                return type + j.ToString();
            }
        }

        private void KzxLookUpEdit_Load(object sender, EventArgs e)
        {
            LayoutControl();
            //UpdateDelegate d = LayoutControl;
            //this.BeginInvoke(d);
        }

        /// <summary>
        /// 根据列的设计信息创建列
        /// </summary>
        public void SetColumnDisplayFormat()
        {
            DataTable dt = null;
            BindingSource bs = null;
            DataView view = null;

            if (string.IsNullOrWhiteSpace(this._KzxFormatString) == true)
            {
                if (this.BindingObject != null)
                {
                    if (this.BindingObject.DataSource != null)
                    {
                        bs = this.BindingObject.DataSource as BindingSource;
                        if (bs != null)
                        {
                            if (bs.List != null)
                            {
                                view = bs.List as DataView;
                                if (view != null)
                                {
                                    dt = view.Table;
                                    if (dt != null)
                                    {
                                        if (dt.Columns.Contains(this.Field) == true)
                                        {
                                            if (dt.Columns[this.Field].DataType == typeof(int) ||
                                                dt.Columns[this.Field].DataType == typeof(Int32) ||
                                                dt.Columns[this.Field].DataType == typeof(Int16) ||
                                                dt.Columns[this.Field].DataType == typeof(Int64) ||
                                                dt.Columns[this.Field].DataType == typeof(Decimal) ||
                                                dt.Columns[this.Field].DataType == typeof(double) ||
                                                dt.Columns[this.Field].DataType == typeof(float) ||
                                                dt.Columns[this.Field].DataType == typeof(long) ||
                                                dt.Columns[this.Field].DataType == typeof(Single))
                                            {
                                                if (this.ContentControl.Properties.DisplayFormat.FormatType == DevExpress.Utils.FormatType.None)
                                                {
                                                    this.ContentControl.Properties.Appearance.Options.UseTextOptions = true;
                                                    this.ContentControl.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                                                    this.ContentControl.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                                    this.ContentControl.Properties.DisplayFormat.FormatString = "#0.######";
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
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="eventInfoTable">事件信息表</param>
        public override void BindingEvent(DataTable eventInfoTable)
        {
            BindingEvent(this.ValueControl, eventInfoTable);
        }

        private void LayoutControl()
        {
            this.NullText = string.Empty;

            this.CaptionControl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ValueControl.CausesValidation = false;
            this.ValueControl.Properties.TextEditStyle = _TextEditStyle;
            this.ValueControl.Properties.AutoHeight = false;
            this.CaptionControl.Width = this._CaptionLabelWidth;
            this.ValueControl.Properties.DisplayFormat.FormatType = this._KzxFormatType;
            this.ValueControl.Properties.DisplayFormat.FormatString = this._KzxFormatString;
            SetColumnDisplayFormat();
            BindingEvent(this.ValueControl, PluginInfoTable);
            //if (this.ValueControl.Properties.TextEditStyle == TextEditStyles.Standard)
            //{
            this.ValueControl.Validating += new CancelEventHandler(DataValidating);
            //}
            this.ValueControl.EditValueChanging -= new ChangingEventHandler(UserEditValueChanging);
            this.ValueControl.EditValueChanging += new ChangingEventHandler(UserEditValueChanging);
            this.ValueControl.EditValueChanged -= new EventHandler(UserEditValueChanged);
            this.ValueControl.EditValueChanged += new EventHandler(UserEditValueChanged);

            SetBackColor();
            if (this.DesignMode == true)
            {
                this.DesigeCaption = GetLanguage(this.MessageCode, this.DesigeCaption);
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

        /// <summary>
        /// 是否触发验证事件
        /// </summary>
        /// <param name="e">true触发，false不触发</param>
        public override void SetCausesValidation(bool e)
        {
            this.ValueControl.CausesValidation = e;
        }

        private void ValueControl_Enter(object sender, EventArgs e)
        {
            this._Value = this.ValueControl.EditValue == null || this.ValueControl.EditValue == DBNull.Value ? string.Empty : this.ValueControl.EditValue.ToString();
            this.ValueControl.CausesValidation = true;
        }

        protected override void UserLeave(object sender, EventArgs e)
        {
            RaiseEvent(sender, "Leave", e);
            if (this._Value.Equals((this.ValueControl.EditValue == null || this.ValueControl.EditValue == DBNull.Value ? string.Empty : this.ValueControl.EditValue.ToString())) == false)
            {
                RaiseEvent(sender, "KzxValueChanged", e);
                this._Value = this.ValueControl.EditValue == null || this.ValueControl.EditValue == DBNull.Value ? string.Empty : this.ValueControl.EditValue.ToString();
            }
        }

        private void searchLookUpEdit1View_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            GridView grid = (GridView)sender;
            if (e.RowHandle < 0)
            {
                this._ItemIndex = -1;
                this._CurrentItem = null;
            }
            else
            {
                this._CurrentItem = grid.GetRow(e.RowHandle) as DataRow;
                this._ItemIndex = e.RowHandle;
            }
        }


        /// <summary>
        /// 设置背景色
        /// </summary>
        private void SetBackColor()
        {
            if (this.ValueControl.Properties.ReadOnly == true)
            {
                this.ValueControl.BackColor = Color.FromArgb(242, 242, 243);
            }
            else
            {
                if (this._IsNull.Equals(true) == false)
                {
                    this.ValueControl.BackColor = Color.Yellow;
                }
                else
                {
                    this.ValueControl.BackColor = this._TextBackColor;
                }
            }
        }
    }
}
