using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kzx.Common
{
    public partial class frm_MessageBox : DevExpress.XtraEditors.XtraForm
    {
      
        /// <summary>
        /// 确定按钮
        /// </summary>
        private SimpleButton btn_OK;

        /// <summary>
        /// 取消按钮
        /// </summary>
        private SimpleButton btn_Cancel;

        /// <summary>
        /// 中止按钮
        /// </summary>
        private SimpleButton btn_Abort;

        /// <summary>
        /// 重试按钮
        /// </summary>
        private SimpleButton btn_Retry;

        /// <summary>
        /// 忽略按钮
        /// </summary>
        private SimpleButton btn_Ignore;

        /// <summary>
        /// 是按钮
        /// </summary>
        private SimpleButton btn_Yes;

        /// <summary>
        /// 否按钮
        /// </summary>
        private SimpleButton btn_No;

        /// <summary>
        /// 要在消息框中显示的文本
        /// </summary>
        private string text;

        /// <summary>
        /// 要在消息框的标题栏中显示的文本
        /// </summary>
        private string caption;

        /// <summary>
        ///  System.Windows.Forms.MessageBoxButtons 值之一，可指定在消息框中显示哪些按钮
        /// </summary>
        private MessageBoxButtons buttons;

        /// <summary>
        /// System.Windows.Forms.MessageBoxIcon 值之一，它指定在消息框中显示哪个图标
        /// </summary>
        private MessageBoxIcon icon;

        /// <summary>
        /// System.Windows.Forms.MessageBoxDefaultButton 值之一，可指定消息框中的默认按钮。
        /// </summary>
        private MessageBoxDefaultButton defaultButton;

        /// <summary>
        /// 消息弹出框参数实体
        /// </summary>
        MessageBoxModel _MessageBoxModel = default(MessageBoxModel);

        /// <summary>
        /// 支持修改弹出框的按钮标题描述
        /// </summary>
        /// <param name="pMessageBoxModel"></param>
        public frm_MessageBox(MessageBoxModel pMessageBoxModel)
        {
            InitializeComponent();
            if (pMessageBoxModel == null)
                pMessageBoxModel = new MessageBoxModel();

            this.ControlBox = false;
            this.text = pMessageBoxModel.MsgText;
            this.Text = pMessageBoxModel.FormText ?? "Stephen's UserControl";
            this.caption = pMessageBoxModel.FormText;
            this.buttons = pMessageBoxModel.MsgButton;
            this.icon = pMessageBoxModel.MsgIcon;
            this.defaultButton = pMessageBoxModel.MsgxDefaultButton;
            this._MessageBoxModel = pMessageBoxModel;
        }

        /// <summary>
        /// 显示一个具有指定文本、标题、按钮、图标、默认按钮的消息框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <param name="defaultButton"></param>
        public frm_MessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            InitializeComponent();
            this.ControlBox = false;
            this.text = text;
            this.Text = caption ?? "Stephen's UserControl";
            this.caption = caption;
            this.buttons = buttons;
            this.icon = icon;
            this.defaultButton = defaultButton;
        }

        private void frm_MessageBox_Load(object sender, EventArgs e)
        {
            int pannelLength = panelButton.Size.Width;
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    #region OK
                    this.btn_OK = new SimpleButton();
                    this.panelButton.SuspendLayout();
                    //btn_OK
                    this.btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                    | (System.Windows.Forms.AnchorStyles.Right)))));
                    this.btn_OK.Name = "btn_OK";
                    this.btn_OK.Size = new System.Drawing.Size(75, 27);
                    this.btn_OK.Location = new Point(pannelLength - 85, 10);
                    this.btn_OK.TabIndex = 0;
                    if (_MessageBoxModel != null)
                        this.btn_OK.Text = _MessageBoxModel.YesButtonText;
                    else
                        this.btn_OK.Text = sysClass.ssLoadMsgOrDefault("SYS000001", "确定(O)");//确定(O)
                    this.btn_OK.Margin = new Padding(0, 2, 2, 2);
                    this.btn_OK.Click += btn_OK_Click;
                    this.panelButton.Controls.Add(this.btn_OK);
                    this.panelButton.ResumeLayout();
                    #endregion
                    break;
                case MessageBoxButtons.OKCancel:
                    #region OKCancel
                    this.btn_OK = new SimpleButton();
                    this.btn_Cancel = new SimpleButton();
                    this.panelButton.SuspendLayout();
                    //btn_OK
                    this.btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                    | (System.Windows.Forms.AnchorStyles.Right)))));
                    this.btn_OK.Name = "btn_OK";
                    this.btn_OK.Size = new System.Drawing.Size(75, 27);
                    this.btn_OK.Location = new Point(pannelLength - 170, 10);
                    this.btn_OK.TabIndex = 0;
                    if (_MessageBoxModel != null)
                        this.btn_OK.Text = _MessageBoxModel.YesButtonText;
                    else
                        this.btn_OK.Text = sysClass.ssLoadMsgOrDefault("SYS000001", "确定(O)");//确定(O)
                    this.btn_OK.Margin = new Padding(0, 2, 2, 2);
                    this.btn_OK.Click += btn_OK_Click;
                    this.panelButton.Controls.Add(this.btn_OK);
                    //btn_Cancel
                    this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                    | (System.Windows.Forms.AnchorStyles.Right)))));
                    this.btn_Cancel.Name = "btn_Cancel";
                    this.btn_Cancel.Size = new System.Drawing.Size(75, 27);
                    this.btn_Cancel.Location = new Point(pannelLength - 85, 10);
                    this.btn_Cancel.TabIndex = 1;
                    if (_MessageBoxModel != null)
                        this.btn_Cancel.Text = _MessageBoxModel.CancleButtonText;
                    else
                        this.btn_Cancel.Text = sysClass.ssLoadMsgOrDefault("SYS000002", "取消(C)");//取消(C)
                    this.btn_Cancel.Margin = new Padding(0, 2, 2, 2);
                    this.btn_Cancel.Click += btn_Cancel_Click;
                    this.panelButton.Controls.Add(this.btn_Cancel);
                    this.panelButton.ResumeLayout();
                    if (defaultButton == MessageBoxDefaultButton.Button1)
                    {
                        this.btn_OK.Select();
                    }
                    else
                    {
                        this.btn_Cancel.Select();
                    }
                    #endregion
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    #region AbortRetryIgnore
                    this.btn_Abort = new SimpleButton();
                    this.btn_Retry = new SimpleButton();
                    this.btn_Ignore = new SimpleButton();
                    this.panelButton.SuspendLayout();
                    //btn_Abort
                    this.btn_Abort.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                    | (System.Windows.Forms.AnchorStyles.Right)))));
                    this.btn_Abort.Name = "btn_Abort";
                    this.btn_Abort.Size = new System.Drawing.Size(75, 27);
                    this.btn_Abort.Location = new Point(pannelLength - 255, 10);
                    this.btn_Abort.TabIndex = 0;
                    this.btn_Abort.Text = sysClass.ssLoadMsgOrDefault("SYS000003", "中止(A)");//中止(A)
                    this.btn_Abort.Margin = new Padding(0, 2, 2, 2);
                    this.btn_Abort.Click += btn_Abort_Click;
                    this.panelButton.Controls.Add(this.btn_Abort);
                    //btn_Retry
                    this.btn_Retry.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                    | (System.Windows.Forms.AnchorStyles.Right)))));
                    this.btn_Retry.Name = "btn_Retry";
                    this.btn_Retry.Size = new System.Drawing.Size(75, 27);
                    this.btn_Retry.Location = new Point(pannelLength - 170, 10);
                    this.btn_Retry.TabIndex = 1;
                    this.btn_Retry.Text = sysClass.ssLoadMsgOrDefault("SYS000004", "重试(R)");//重试(R)
                    this.btn_Retry.Margin = new Padding(0, 2, 2, 2);
                    this.btn_Retry.Click += btn_Retry_Click;
                    this.panelButton.Controls.Add(this.btn_Retry);
                    //btn_Ignore
                    this.btn_Ignore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                    | (System.Windows.Forms.AnchorStyles.Right)))));
                    this.btn_Ignore.Name = "btn_Ignore";
                    this.btn_Ignore.Size = new System.Drawing.Size(75, 27);
                    this.btn_Ignore.Location = new Point(pannelLength - 85, 10);
                    this.btn_Ignore.TabIndex = 2;
                    this.btn_Ignore.Text = sysClass.ssLoadMsgOrDefault("SYS000005", "忽略(I)");//忽略(I)
                    this.btn_Ignore.Margin = new Padding(0, 2, 2, 2);
                    this.btn_Ignore.Click += btn_Ignore_Click;
                    this.panelButton.Controls.Add(this.btn_Ignore);
                    this.panelButton.ResumeLayout();
                    if (defaultButton == MessageBoxDefaultButton.Button1)
                    {
                        this.btn_Abort.Select();
                    }
                    else if (defaultButton == MessageBoxDefaultButton.Button2)
                    {
                        this.btn_Retry.Select();
                    }
                    else if (defaultButton == MessageBoxDefaultButton.Button3)
                    {
                        this.btn_Ignore.Select();
                    }
                    #endregion
                    break;
                case MessageBoxButtons.YesNoCancel:
                    #region YesNoCancel
                    this.btn_Yes = new SimpleButton();
                    this.btn_No = new SimpleButton();
                    this.btn_Cancel = new SimpleButton();
                    this.panelButton.SuspendLayout();
                    //btn_Yes
                    this.btn_Yes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                    | (System.Windows.Forms.AnchorStyles.Right)))));
                    this.btn_Yes.Name = "btn_Yes";
                    this.btn_Yes.Size = new System.Drawing.Size(75, 27);
                    this.btn_Yes.Location = new Point(pannelLength - 255, 10);
                    this.btn_Yes.TabIndex = 0;
                    if (_MessageBoxModel != null)
                        this.btn_Yes.Text = _MessageBoxModel.YesButtonText;
                    else
                        this.btn_Yes.Text = sysClass.ssLoadMsgOrDefault("SYS000006", "是(Y)");//是(Y)
                    this.btn_Yes.Margin = new Padding(0, 2, 2, 2);
                    this.btn_Yes.Click += btn_Yes_Click;
                    this.panelButton.Controls.Add(this.btn_Yes);
                    //btn_No
                    this.btn_No.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                    | (System.Windows.Forms.AnchorStyles.Right)))));
                    this.btn_No.Name = "btn_No";
                    this.btn_No.Size = new System.Drawing.Size(75, 27);
                    this.btn_No.Location = new Point(pannelLength - 170, 10);
                    this.btn_No.TabIndex = 1;
                    if (_MessageBoxModel != null)
                        this.btn_No.Text = _MessageBoxModel.NoButtonText;
                    else
                        this.btn_No.Text = sysClass.ssLoadMsgOrDefault("SYS000007", "否(N)");//否(N)
                    this.btn_No.Margin = new Padding(0, 2, 2, 2);
                    this.btn_No.Click += btn_No_Click;
                    this.panelButton.Controls.Add(this.btn_No);
                    this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                   | (System.Windows.Forms.AnchorStyles.Right)))));
                    this.btn_Cancel.Name = "btn_Cancel";
                    this.btn_Cancel.Size = new System.Drawing.Size(75, 27);
                    this.btn_Cancel.Location = new Point(pannelLength - 85, 10);
                    this.btn_Cancel.TabIndex = 2;
                    if (_MessageBoxModel != null)
                        this.btn_Cancel.Text = _MessageBoxModel.CancleButtonText;
                    else
                        this.btn_Cancel.Text = sysClass.ssLoadMsgOrDefault("SYS000002", "取消(C)");//取消(C)
                    this.btn_Cancel.Margin = new Padding(0, 2, 2, 2);
                    this.btn_Cancel.Click += btn_Cancel_Click;
                    this.panelButton.Controls.Add(this.btn_Cancel);
                    this.panelButton.ResumeLayout();
                    if (defaultButton == MessageBoxDefaultButton.Button1)
                    {
                        this.btn_Yes.Select();
                    }
                    else if (defaultButton == MessageBoxDefaultButton.Button2)
                    {
                        this.btn_No.Select();
                    }
                    else if (defaultButton == MessageBoxDefaultButton.Button3)
                    {
                        this.btn_Cancel.Select();
                    }
                    #endregion
                    break;
                case MessageBoxButtons.YesNo:
                    #region YesNo
                    this.btn_Yes = new SimpleButton();
                    this.btn_No = new SimpleButton();
                    this.panelButton.SuspendLayout();
                    //btn_Yes
                    this.btn_Yes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                    | (System.Windows.Forms.AnchorStyles.Right)))));
                    this.btn_Yes.Name = "btn_Yes";
                    this.btn_Yes.Size = new System.Drawing.Size(75, 27);
                    this.btn_Yes.Location = new Point(pannelLength - 170, 10);
                    this.btn_Yes.TabIndex = 0;
                    if (_MessageBoxModel != null)
                        this.btn_Yes.Text = _MessageBoxModel.YesButtonText;
                    else
                        this.btn_Yes.Text = sysClass.ssLoadMsgOrDefault("SYS000006", "是(Y)");//是(Y)
                    this.btn_Yes.Margin = new Padding(0, 2, 2, 2);
                    this.btn_Yes.Click += btn_Yes_Click;
                    this.panelButton.Controls.Add(this.btn_Yes);
                    //btn_No
                    this.btn_No.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                    | (System.Windows.Forms.AnchorStyles.Right)))));
                    this.btn_No.Name = "btn_No";
                    this.btn_No.Size = new System.Drawing.Size(75, 27);
                    this.btn_No.Location = new Point(pannelLength - 85, 10);
                    this.btn_No.TabIndex = 1;
                    if (_MessageBoxModel != null)
                        this.btn_No.Text = _MessageBoxModel.NoButtonText;
                    else
                        this.btn_No.Text = sysClass.ssLoadMsgOrDefault("SYS000007", "否(N)");//否(N)
                    this.btn_No.Margin = new Padding(0, 2, 2, 2);
                    this.btn_No.Click += btn_No_Click;
                    this.panelButton.Controls.Add(this.btn_No);
                    this.panelButton.ResumeLayout();
                    if (defaultButton == MessageBoxDefaultButton.Button1)
                    {
                        this.btn_Yes.Select();
                    }
                    else
                    {
                        this.btn_No.Select();
                    }
                    #endregion
                    break;
                case MessageBoxButtons.RetryCancel:
                    #region RetryCancel
                    this.btn_Retry = new SimpleButton();
                    this.btn_Cancel = new SimpleButton();
                    this.panelButton.SuspendLayout();
                    //btn_Retry
                    this.btn_Retry.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                    | (System.Windows.Forms.AnchorStyles.Right)))));
                    this.btn_Retry.Name = "btn_Retry";
                    this.btn_Retry.Size = new System.Drawing.Size(75, 27);
                    this.btn_Retry.Location = new Point(pannelLength - 170, 10);
                    this.btn_Retry.TabIndex = 0;

                    this.btn_Retry.Text = sysClass.ssLoadMsgOrDefault("SYS000004", "重试(R)");//重试(R)
                    this.btn_Retry.Margin = new Padding(0, 2, 2, 2);
                    this.btn_Retry.Click += btn_Retry_Click;
                    this.panelButton.Controls.Add(this.btn_Retry);
                    //btn_Cancel
                    this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                   | (System.Windows.Forms.AnchorStyles.Right)))));
                    this.btn_Cancel.Name = "btn_Cancel";
                    this.btn_Cancel.Size = new System.Drawing.Size(75, 27);
                    this.btn_Cancel.Location = new Point(pannelLength - 85, 10);
                    this.btn_Cancel.TabIndex = 1;
                    this.btn_Cancel.Text = sysClass.ssLoadMsgOrDefault("SYS000002", "取消(C)");//取消(C)
                    this.btn_Cancel.Margin = new Padding(0, 2, 2, 2);
                    this.btn_Cancel.Click += btn_Cancel_Click;
                    this.panelButton.Controls.Add(this.btn_Cancel);
                    this.panelButton.ResumeLayout();
                    if (defaultButton == MessageBoxDefaultButton.Button1)
                    {
                        this.btn_Retry.Select();
                    }
                    else
                    {
                        this.btn_Cancel.Select();
                    }
                    #endregion
                    break;
            }

            this.Text = caption;

            this.lblMsg.Text = text;
            int moreHeight = this.lblMsg.Height - 35;
            if (moreHeight > 0)
            {
                this.Height += moreHeight;
            }
        }
        private void PaintIcon(Icon icon, int x, int y)
        {
            Graphics g = this.CreateGraphics();
            g.DrawIcon(icon, x, y);
        }

        void btn_No_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        void btn_Yes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        void btn_Ignore_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Ignore;
        }

        void btn_Retry_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
        }

        void btn_Abort_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
        }

        void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        void btn_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void frm_MessageBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.None)
            {
                if (e.KeyCode == Keys.O && btn_OK != null)
                {
                    btn_OK_Click(null, null);
                }
                if (e.KeyCode == Keys.C && btn_Cancel != null)
                {
                    btn_Cancel_Click(null, null);
                }
                if (e.KeyCode == Keys.A && btn_Abort != null)
                {
                    btn_Abort_Click(null, null);
                }
                if (e.KeyCode == Keys.R && btn_Retry != null)
                {
                    btn_Retry_Click(null, null);
                }
                if (e.KeyCode == Keys.I && btn_Ignore != null)
                {
                    btn_Ignore_Click(null, null);
                }
                if (e.KeyCode == Keys.Y && btn_Yes != null)
                {
                    btn_Yes_Click(null, null);
                }
                if (e.KeyCode == Keys.N && btn_No != null)
                {
                    btn_No_Click(null, null);
                }
            }
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.C)
            {
                string mCopyText = "-------------------------------";
                mCopyText += "\n";
                mCopyText += lblMsg.Text + "\n";
                mCopyText += "-------------------------------";
                Clipboard.SetText(mCopyText);
            }
        }

        private void frm_MessageBox_Paint(object sender, PaintEventArgs e)
        {
            Icon msgIcon;
            switch (icon)
            {
                case MessageBoxIcon.Error:
                    msgIcon = System.Drawing.SystemIcons.Error;
                    break;
                case MessageBoxIcon.Question:
                    msgIcon = System.Drawing.SystemIcons.Question;
                    break;
                case MessageBoxIcon.Exclamation:
                    msgIcon = System.Drawing.SystemIcons.Exclamation;
                    break;
                default:
                    msgIcon = System.Drawing.SystemIcons.Information;
                    break;
            }

            e.Graphics.DrawIcon(msgIcon, 40, 20);
        }
           
    }

    /// <summary>
    /// 弹出框实体
    /// </summary>
    public class MessageBoxModel
    {
        /// <summary>
        /// 弹出框标题
        /// </summary>
        public string FormText { get; set; }

        /// <summary>
        /// 弹出框宽度
        /// </summary>
        public int FormWidth { get; set; }

        /// <summary>
        /// 弹出框高度
        /// </summary>
        public int FormHeight { get; set; }

        /// <summary>
        /// 弹出框消息内容
        /// </summary>
        public string MsgText { get; set; }

        /// <summary>
        /// 文字大小
        /// </summary>
        public int FontSize { get; set; }

        /// <summary>
        /// “是”按钮标题
        /// </summary>
        public string YesButtonText { get; set; }

        /// <summary>
        /// “否”按钮标题
        /// </summary>
        public string NoButtonText { get; set; }

        /// <summary>
        /// “取消”按钮标题
        /// </summary>
        public string CancleButtonText { get; set; }

        /// <summary>
        /// 弹出框类型（提示型、选择型等）
        /// </summary>
        public MessageBoxButtons MsgButton = MessageBoxButtons.OK;

        /// <summary>
        /// 弹出框中显示的图标
        /// </summary>
        public MessageBoxIcon MsgIcon = MessageBoxIcon.Information;

        /// <summary>
        /// 弹出框默认选中的按钮
        /// </summary>
        public MessageBoxDefaultButton MsgxDefaultButton = MessageBoxDefaultButton.Button1;
    }
}
