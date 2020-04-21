using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Kzx.UserControl
{
    /// <summary>
    /// 标签验证
    /// </summary>
    public partial class KzxLabel : KzxBaseControl
    {
        public KzxLabel()
        {
            InitializeComponent();
            //if (this.DesignMode == true)
            //{
            //    this.Size = new Size(70, 20);
            //}
            //注册事件
            //this.ValueControl.TextChanged += new EventHandler(UserTextChanged);
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

        /// <summary>
        /// 提示信息
        /// </summary>
        [Category("汽泡提示"), Description("ToolTipText,提示信息"), Browsable(true)]
        [McDisplayName("ToolTipText")]
        public override string ToolTipText
        {
            get
            {
                return this.CaptionControl.ToolTip;
            }
            set
            {
                this.CaptionControl.ToolTip = value;
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
        /// 标题控件
        /// </summary>
        public LabelControl CaptionLabel
        {
            get
            {
                return this.CaptionControl;
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
                return this.CaptionControl.ForeColor;
            }
            set
            {
                this._TextForeColor = value;
                this.CaptionControl.ForeColor = value;
            }
        }
         
        public void LayoutControl()
        {
            BindingEvent(this, PluginInfoTable);
            if (this.DesignMode == true)
            {
                this.DesigeCaption = GetLanguage(this.MessageCode, this.DesigeCaption);
            }
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="eventInfoTable">事件信息表</param>
        public override void BindingEvent(DataTable eventInfoTable)
        {
            BindingEvent(this, eventInfoTable);
        }

        private void KzxLabel_Load(object sender, EventArgs e)
        {
            LayoutControl();
            //UpdateDelegate d = LayoutControl;
            //this.BeginInvoke(d);
        }
    }
}
