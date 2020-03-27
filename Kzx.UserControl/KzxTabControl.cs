#define EVENT_NOT

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraTab;
using Kzx.UserControl.UITypeEdit;
using Kzx.UserControls;

namespace Kzx.UserControl
{
    /// <summary>
    /// 页签控件
    /// 作者：米超
    /// 时间：2014-6-7
    /// </summary>
    public partial class KzxTabControl : XtraTabControl, ILayoutControl
    {
        private ToolTip _ToolTip = new ToolTip();

        public KzxTabControl()
        {
            InitializeComponent();
            SetAppearance();
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

            }
            else
            {
                this.Enabled = this.DesigeEnabled;
                this.Visible = this.DesigeVisible;
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
                if (string.IsNullOrEmpty(this._Key)) return this.Name; 

                return this._Key;
            }
            set
            {
                this._Key = value;
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

        private bool _DesigeEnabled = true;
        /// <summary>
        /// 设计时的可用性
        /// </summary>
        [Category("特性"), Description("DesigeEnabled,设计时的可用性"), Browsable(true)]
        [McDisplayName("DesigeEnabled")]
        public bool DesigeEnabled
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

        private bool _DesigeVisibl = true;
        /// <summary>
        /// 设计时可见性
        /// </summary>
        [Category("特性"), Description("DesigeVisible,设计时可见性"), Browsable(true)]
        [McDisplayName("DesigeVisible")]
        public virtual bool DesigeVisible
        {
            get
            {
                return this._DesigeVisibl;
            }
            set
            {
                this._DesigeVisibl = value;
                if (this.DesignMode == true)
                {
                }
                else
                {
                    this.Visible = value;
                }
            }
        }

        private int _KzxSelectedIndex = -1;
        [Category("外观"), Description("KzxSelectedIndex,选中的页签下标"), Browsable(true)]
        [McDisplayName("KzxSelectedIndex")]
        public int KzxSelectedIndex
        {
            get
            {
                return this._KzxSelectedIndex;
            }
            set
            {
                this._KzxSelectedIndex = value;
                this.SelectedTabPageIndex = value;
            }
        }

        //
        // 摘要:
        //     Provides access to the tab control's page collection.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("页"), Description("TabPageCollection,页的集合"), Browsable(true), Editor(typeof(KzxTabPagesUiTypeEdit), typeof(UITypeEditor))]
        [McDisplayName("TabPageCollection")]
        public XtraTabPageCollection TabPageCollection
        {
            get
            {
                for (int i = this.TabPages.Count - 1; i >= 0; i--)
                {
                    if (!(this.TabPages[i] is KzxTabPage))
                    {
                        this.TabPages.RemoveAt(i);
                    }
                    else
                    {
                        KzxTabPage KzxtabPage = this.TabPages[i] as KzxTabPage;
                        if (KzxtabPage != null)
                        {
                            if (base.DesignMode)
                            {
                                KzxtabPage.LayoutControl();
                            }
                            else
                            {
                                KzxtabPage.KzxGetLanguage -= this.SetLanguage;
                                KzxtabPage.KzxGetLanguage += this.SetLanguage;
                            }
                        }
                    }
                }
                if (this.TabPages.Count == 0)
                {
                    this.TabPages.Add(new KzxTabPage
                    {
                        DesigeCaption = "签页"
                    });
                }
                return this.TabPages;
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
                BindingEvent(this, value);
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

        public static string CreateName(IContainer container, Type type)
        {
            ComponentCollection cc = container.Components;
            int min = Int32.MaxValue;
            int max = Int32.MinValue;
            int count = 0;

            for (int i = 0; i < cc.Count; i++)
            {
                //if ((cc[i] is Component) == false)
                //{
                //    continue;
                //}
                //Component comp = cc[i] as Component;
                if (cc[i].Site == null)
                {
                    continue;
                }
                if (cc[i].GetType() == type)
                {
                    string name = cc[i].Site.Name;
                    if (name.StartsWith(type.Name))
                    {
                        try
                        {
                            count++;
                            int value = Int32.Parse(name.Substring(type.Name.Length));

                            if (value < min)
                                min = value;

                            if (value > max)
                                max = value;
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex.ToString());
                        }
                    }
                }
            }// for

            if (count == 0)
                return type.Name + "1";

            else if (min > 1)
            {
                int j = min - 1;

                return type.Name + j.ToString();
            }
            else
            {
                int j = max + 1;

                return type.Name + j.ToString();
            }
        }

        public void LayoutControl()
        {
            this.KzxSelectedIndex = this._KzxSelectedIndex;
            if (this.DesignMode == true)
            {
                KzxTabPage page = null;

                for (int i = 0; i < this.TabPages.Count; i++)
                {
                    page = this.TabPages[0] as KzxTabPage;
                    if (page != null)
                    {
                        page.LayoutControl();
                    }
                }
            }
        }

        /// <summary>
        /// 获取多语言文本
        /// </summary>
        /// <param name="messageCode">语言文本标识</param>
        /// <param name="defaultMessage">默认的文本</param>
        /// <returns>取到的文本</returns>
        protected virtual string GetLanguage(string messageCode, string defaultMessage)
        {
            string text = string.Empty;
            if (this.KzxGetLanguage != null)
            {
                this.KzxGetLanguage(this, messageCode, ref text);
            }
            if (string.IsNullOrWhiteSpace(text) == true)
            {
                return defaultMessage;
            }
            else
            {
                return text;
            }
        }

        private void SetLanguage(object sender, string messageCode, ref string text)
        {
            if (this.KzxGetLanguage != null)
            {
                this.KzxGetLanguage(sender, messageCode, ref text);
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
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i] is ILayoutControl)
                {
                    ((ILayoutControl)this.Controls[i]).SetLayout();
                }
            }
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

        #endregion

        #region 方法

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
            args.FieldName = string.Empty;
            args.TableName = string.Empty;
            args.Key = this.Key;
            if (this.KzxControlOperate != null)
            {
                this.KzxControlOperate(this, args);
                e = args.SystemEventArgs;
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

        public void LoadTabPages()
        {
            for (int i = 0; i < this.TabPages.Count; i++)
            {
                this.SelectedTabPageIndex = i;
            }
            this.SelectedTabPageIndex = this.KzxSelectedIndex;
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

#if EVENT
        protected override void OnSelectedPageChanged(object sender, DevExpress.XtraTab.ViewInfo.ViewInfoTabPageChangedEventArgs e)
        {
            base.OnSelectedPageChanged(sender, e);
            RaiseEvent(this, "SelectedPageChanged", e);
        }

        protected override void OnSelectedPageChanging(object sender, DevExpress.XtraTab.ViewInfo.ViewInfoTabPageChangingEventArgs e)
        {
            RaiseEvent(this, "SelectedPageChanging", e);
        }

        protected override void OnCloseButtonClick(object sender, EventArgs e)
        {
            RaiseEvent(this, "CloseButtonClick", e);
        }
#endif
        #endregion
    }
}
