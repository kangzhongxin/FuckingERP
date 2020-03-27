#define EVENT_NOT
#define BorderStyle_NOT

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraPrinting.Native;
using Kzx.UserControl.UITypeEdit;

namespace Kzx.UserControl
{
    /// <summary>
    /// 文本框验证
    /// </summary>
    [ToolboxBitmapAttribute(typeof(Bitmap), "文本框")]
    public partial class KzxTextBox : KzxBaseControl
    {
        private Boolean _IsSelectedAll = true;
        private string _Value = string.Empty;

        /// <summary>
        /// 构造
        /// </summary>
        public KzxTextBox()
        {
            InitializeComponent();
            this.CaptionControl.AutoSizeMode = LabelAutoSizeMode.Vertical;
            this.CaptionControl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ValueControl.EditValueChanged -= new EventHandler(ValueControl_TextBoxEditChanged);
            this.ValueControl.EditValueChanged += new EventHandler(ValueControl_TextBoxEditChanged);
            this.CaptionControl.SizeChanged += new EventHandler(SetSize); 
            if (this.DesignMode == true)
            {
                this.Size = new Size(284, 21);
            }
#if EVENT
            //注册事件
            //this.ValueControl.TextChanged += new EventHandler(UserTextChanged);
#endif
        }

        private void ValueControl_TextBoxEditChanged(object sender, EventArgs e)
        {
            
        }

        #region 下拉框

        private object _DataSource = null;
        /// <summary>
        /// 下拉数据源
        /// </summary>
        [Category("下拉数据"), Description("DataSource,下拉数据源"), Browsable(false)]
        public object DataSource
        {
            get
            {
                return this._DataSource;
            }
            set
            {
                this._DataSource = value;
            }
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
            this.DataSource = binding;
            return 1;
        }

        #endregion

        private DevExpress.XtraEditors.Controls.BorderStyles _BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        /// <summary>
        /// 边框显示格式
        /// </summary>
        [Category("数据格式"), Description("KzxBorderStyle,边框显示格式"), Browsable(true)]
        [McDisplayName("KzxBorderStyle")]
        public override DevExpress.XtraEditors.Controls.BorderStyles KzxBorderStyle
        {
            get
            {
                return this.ValueControl.Properties.BorderStyle;
            }
            set
            {
                this._BorderStyle = value;
                this.ValueControl.Properties.BorderStyle = value;
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
                //bug8352:ERP管理工具设计界面、ERM客户端文本控件显示样式问题
                if (value == false)
                {
                    this.ValueControl.BackColor = Color.White;
                }
            }
        }

        private int maxLength = 0;
        /// <summary>
        /// 可录入的最大长度
        /// </summary>
        [Category("验证"), Description("MaxLength,可录入的最大长度"), Browsable(true)]
        [McDisplayName("MaxLength")]
        public override int MaxLength
        {
            get
            {
                return maxLength;
                //return this.ValueControl.Properties.MaxLength;
            }
            set
            {
                maxLength = value;
                // this.ValueControl.Properties.MaxLength = value;
            }
        }

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

        private string toolTipMaxLengthText = string.Empty;
        /// <summary>
        /// 数据长度不能超过数据库长度提示文本  
        /// </summary>
        public override string ToolTipMaxLengthText
        {
            get { return toolTipMaxLengthText; }
            set { toolTipMaxLengthText = value; }
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

        /// <summary>
        /// 密码显示字符
        /// </summary>
        [Category("密码"), Description("PasswordChar,密码显示字符"), Browsable(true)]
        [McDisplayName("PasswordChar")]
        public Char PasswordChar
        {
            get
            {
                return this.ValueControl.Properties.PasswordChar;
            }
            set
            {
                this.ValueControl.Properties.PasswordChar = value;
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

        /// <summary>
        /// 格式掩码
        /// </summary>
        [Category("数据格式"), Description("KzxEditMask,掩码格式"), Browsable(true)]
        [McDisplayName("KzxEditMask")]
        public virtual string KzxEditMask
        {
            get
            {
                return this.ValueControl.Properties.Mask.EditMask;
            }
            set
            {
                this.ValueControl.Properties.Mask.EditMask = value;
            }
        }

        /// <summary>
        /// 掩码验证类型
        /// </summary>
        [Category("数据格式"), Description("KzxMaskType,掩码验证类型"), Browsable(true), Editor(typeof(KzxMaskTypeUiTypeEdit), typeof(UITypeEditor))]
        [McDisplayName("KzxMaskType")]
        public virtual string KzxMaskType
        {
            get
            {
                return this.ValueControl.Properties.Mask.MaskType.ToString();
            }
            set
            {
                this.ValueControl.Properties.Mask.MaskType = XmlRow.KzxMaskTypeConverter(value);
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
        /// 内容文本控件
        /// </summary>
        public TextEdit ContentControl
        {
            get
            {
                return this.ValueControl;
            }
        }

        /// <summary>
        /// 设置数据绑定
        /// </summary>
        /// <param name="binding">数据绑定对象</param>
        /// <return>int</return>
        public override int SetDataBinding(object binding)
        {
            Binding bd = new Binding("EditValue", binding, this.Field, true, DataSourceUpdateMode.OnValidation, string.Empty, this.FormatString);
            this.ValueControl.DataBindings.Add(bd);
            this.BindingObject = bd;

            //SetColumnDisplayFormat();

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
        /// 还原默认值
        /// </summary>
        /// <return>void</return>
        public override void SetDefaultValue()
        {
            this.ValueControl.EditValue = this.DefaultValue;
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

        public string Value
        {
            get
            {
                return this.ContentControl.Text;
            }
            set
            {
                this.ContentControl.Text = value;
            }
        }

        /// <summary>
        /// 设置控件的值
        /// </summary>
        /// <param name="value">控件的值</param>
        /// <return>int</return>
        public override int SetValue(object value)
        {
            this.ValueControl.EditValue = value == null || value == DBNull.Value ? string.Empty : value;
            return 1;
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
            //return 1;
        }

        /// <summary>
        /// 文本改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserTextChanged(object sender, EventArgs e)
        {
            base.RaiseEvent(sender, "TextChanged", e);
        }

        private void KzxTextBox_Load(object sender, EventArgs e)
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
                                        if (
                                        dt.Columns[this.Field].DataType == typeof(int) ||
                                            dt.Columns[this.Field].DataType == typeof(Int32) ||
                                            dt.Columns[this.Field].DataType == typeof(Int16) ||
                                            dt.Columns[this.Field].DataType == typeof(Int64) ||
                                            dt.Columns[this.Field].DataType == typeof(Decimal)
                                            || dt.Columns[this.Field].DataType == typeof(double) ||
                                            dt.Columns[this.Field].DataType == typeof(float) ||
                                            dt.Columns[this.Field].DataType == typeof(long) ||
                                            dt.Columns[this.Field].DataType == typeof(Single)
                                        )
                                        {

                                            this.ContentControl.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
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
            this.ValueControl.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.ValueControl.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            SetColumnDisplayFormat();
            this.CaptionControl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.CaptionControl.Width = this._CaptionLabelWidth;
            this.ValueControl.Properties.AutoHeight = false;
            this.ValueControl.Properties.Mask.UseMaskAsDisplayFormat = false;
            this.ValueControl.Properties.ValidateOnEnterKey = true;
            //this.ValueControl.Height = this.Height;
            this.ValueControl.CausesValidation = false;
            this.ValueControl.Properties.DisplayFormat.FormatType = this._KzxFormatType;
            this.ValueControl.Properties.DisplayFormat.FormatString = this._KzxFormatString;
            this.ValueControl.Properties.BorderStyle = this._BorderStyle;
            if (this._BorderStyle == BorderStyles.NoBorder)
            {
                this.ValueControl.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            }
            SetColumnDisplayFormat();
            //            if (this.Field.Equals("sCode", StringComparison.OrdinalIgnoreCase) == false &&
            //this.Field.Equals("sBillNo", StringComparison.OrdinalIgnoreCase) == false)
            //            {
            this.ValueControl.Validating += new CancelEventHandler(DataValidating);
            //}
            BindingEvent(this.ValueControl, PluginInfoTable);
            SetAppearance();
            SetBackColor();

            if (this.DesignMode == true)
            {
                this.DesigeCaption = GetLanguage(this.MessageCode, this.DesigeCaption);
            }
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
            this.ValueControl.CausesValidation = e;
        }

        private void ValueControl_Enter(object sender, EventArgs e)
        {
            this._Value = this.ValueControl.EditValue == null || this.ValueControl.EditValue == DBNull.Value ? string.Empty : this.ValueControl.EditValue.ToString();
            this.ValueControl.CausesValidation = true;
            this.ValueControl.SelectAll();
            this._IsSelectedAll = true;
        }

        private void ValueControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (this._IsSelectedAll == true)
            {
                this.ValueControl.SelectAll();
                this._IsSelectedAll = false;
            }
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

        /// <summary>
        /// 设置背景色
        /// </summary>
        private void SetBackColor()
        {
            
            if (this._TextBackColor != Color.White)
            {
                this.ValueControl.BackColor = this._TextBackColor;
            }
            else
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

        private ToolTip toolTip = null;
        /// <summary>
        /// 限制文本输入长度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValueControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        /// <summary>
        /// 文本内容改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValueControl_EditValueChanged(object sender, EventArgs e)
        {
            
        }


    }
}
