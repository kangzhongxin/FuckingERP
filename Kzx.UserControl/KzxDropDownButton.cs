using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using Kzx.UserControl;
using Kzx.UserControl.UITypeEdit;
using Kzx.UserControls.UITypeEdit;
using ControlEventArgs = Kzx.UserControl.ControlEventArgs;
using TextUiTypEdit = Kzx.UserControl.TextUiTypEdit;

namespace KzxUserControls
{
    public class KzxDropDownButton : DropDownButton, ILayoutControl
    {
        /// <summary>
        /// 下拉项单击事件委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void DropDownItemClickEventHandler(object sender, DropDownItemClickEventArgs e);

        /// <summary>
        /// 下拉项单击事件
        /// </summary>
        public event DropDownItemClickEventHandler ButtonClick;

        private DevExpress.XtraBars.PopupMenu popupMenu1 = new DevExpress.XtraBars.PopupMenu();
        private DevExpress.XtraBars.BarManager barManager1 = new DevExpress.XtraBars.BarManager();

        public KzxDropDownButton()
            : base()
        {
            this.DropDownControl = popupMenu1;
            this.popupMenu1.Manager = this.barManager1;
        }

        private void LayoutControl()
        {
            if (this.DesignMode == true)
            {
                this.DesigeCaption = GetLanguage(this.MessageCode, this.DesigeCaption);

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
            string filepath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "KzxCommon.dll");
            string text = string.Empty;
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
            return string.IsNullOrWhiteSpace(text) == true ? defaultMessage : text;
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

        private DataTable _ItemsTable = null;
        /// <summary>
        /// 下拉按钮集合
        /// </summary>
        [Category("自定义"), Description("ItemsTable,下拉按钮集合"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [McDisplayName("ItemsTable")]
        public DataTable ItemsTable
        {
            get
            {
                return this._ItemsTable;
            }
            set
            {
                this._ItemsTable = value;

            }
        }


        private string _Items = string.Empty;
        /// <summary>
        /// 下拉按钮集合
        /// </summary>
        [Category("按钮集合"), Description("Items,下拉按钮集合"), Browsable(true), Editor(typeof(KzxDropDownButtonItemUiTypeEdit), typeof(UITypeEditor))]
        [McDisplayName("Items")]
        public string Items
        {
            get
            {
                return this._Items;
            }
            set
            {
                this._Items = value;
                this._ItemsTable = KzxDropDownButton.SerializeItems(value);
                popupMenu1.ItemLinks.Clear();
                this.barManager1.Items.Clear();

                for (int i = 0; i < this._ItemsTable.Rows.Count; i++)
                {
                    DevExpress.XtraBars.BarButtonItem b = new DevExpress.XtraBars.BarButtonItem();
                    b.ItemClick += new ItemClickEventHandler(OnItemClick);
                    if (this.DesignMode == true)
                    {
                        b.Caption = GetLanguage(this._ItemsTable.Rows[i]["MessageCode"].ToString(), this._ItemsTable.Rows[i]["DesigeCaption"] == DBNull.Value ? string.Empty : this._ItemsTable.Rows[i]["DesigeCaption"].ToString());
                    }
                    else
                    {
                        b.Caption = this._ItemsTable.Rows[i]["DesigeCaption"] == DBNull.Value ? string.Empty : this._ItemsTable.Rows[i]["DesigeCaption"].ToString();
                    }
                    b.Tag = this._ItemsTable.Rows[i]["DllName"] == DBNull.Value ? string.Empty : this._ItemsTable.Rows[i]["DllName"].ToString();
                    popupMenu1.AddItem(b);
                    this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { b });
                }
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

        private string _DesigeCaption = string.Empty;
        /// <summary>
        /// 没有多语言的情况下的默认显示标题
        /// </summary>
        [Category("多语言"), Description("DesigeCaption,没有多语言的情况下的默认显示标题"), Browsable(true)]
        [McDisplayName("DesigeCaption")]
        public virtual string DesigeCaption
        {
            get
            {
                return this.Text.Trim();
            }
            set
            {
                this.Text = value;
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
        /// 控件事件
        /// </summary>
        public event KzxControlOperateEventHandler KzxControlOperate;

        /// <summary>
        /// 获取多语言文本事件
        /// </summary>
        public event KzxGetLanguageEventHandler KzxGetLanguage;

        #region 方法

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
                BindingEvent(this, this._PluginInfoTable);
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
                return this.ForeColor;
            }
            set
            {
                this._LabelForeColor = value;
                this.ForeColor = value;
            }
        }

        /// <summary>
        /// 控钮类型
        /// </summary>
        [Category("外观"), Description("KzxButtonStyle,控钮类型"), Browsable(true)]
        [McDisplayName("KzxButtonStyle")]
        public DevExpress.XtraEditors.Controls.BorderStyles KzxButtonStyle
        {
            get
            {
                return this.ButtonStyle;
            }
            set
            {
                this.ButtonStyle = value;
            }
        }

        /// <summary>
        /// 按钮图片
        /// </summary>
        [Category("外观"), Description("KzxImage,按钮图片"), Browsable(true)]
        [McDisplayName("KzxImage")]
        public System.Drawing.Image KzxImage
        {
            get
            {
                return this.Image;
            }
            set
            {
                this.Image = value;
            }
        }

        /// <summary>
        /// 图片显示位置
        /// </summary>
        [Category("外观"), Description("KzxImageLocation,图片显示位置"), Browsable(true)]
        [McDisplayName("KzxImageLocation")]
        public DevExpress.XtraEditors.ImageLocation KzxImageLocation
        {
            get
            {
                return this.ImageLocation;
            }
            set
            {
                this.ImageLocation = value;
            }
        }

        #region 事件处理

        protected virtual void UserClick(object sender, EventArgs e)
        {
            RaiseEvent(sender, "Click", e);
        }

        protected virtual void UserArrowButtonClick(object sender, EventArgs e)
        {
            RaiseEvent(sender, "ArrowButtonClick", e);
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

        protected virtual void UserButtonClick(object sender, DropDownItemClickEventArgs e)
        {
            this._DllName = e.DllName;
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

        #region 列的序列化与反序列化

        public static DataTable CreateSerializeItemsTable()
        {
            DataTable table = new DataTable("Items");

            DataColumn c = new DataColumn();
            c.ColumnName = "ColIndex";
            c.DataType = typeof(int);
            c.AutoIncrement = true;
            c.AutoIncrementSeed = 1;
            c.AutoIncrementStep = 1;

            table.Columns.Add(c);

            table.Columns.Add("DesigeCaption", typeof(string));
            table.Columns.Add("Key", typeof(string));
            table.Columns.Add("MessageCode", typeof(string));
            table.Columns.Add("DllName", typeof(string));
            return table;
        }

        public static DataTable SerializeItems(string columnsXml)
        {
            DataRow row = null;
            XmlNode node = null;
            DataTable table = KzxDropDownButton.CreateSerializeItemsTable();

            if (string.IsNullOrWhiteSpace(columnsXml) == true)
            {
                return table;
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(columnsXml);
            table.TableName = doc.DocumentElement.Name;
            for (int i = 0; i < doc.DocumentElement.ChildNodes.Count; i++)
            {
                row = table.NewRow();
                node = doc.DocumentElement.ChildNodes[i];
                if (node.Attributes["Key"] != null)
                {
                    row["Key"] = node.Attributes["Key"].Value;
                }
                if (node.Attributes["DesigeCaption"] != null)
                {
                    row["DesigeCaption"] = node.Attributes["DesigeCaption"].Value;
                }
                if (node.Attributes["MessageCode"] != null)
                {
                    row["MessageCode"] = node.Attributes["MessageCode"].Value;
                }
                if (node.Attributes["DllName"] != null)
                {
                    row["DllName"] = node.Attributes["DllName"].Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }

        public static string DeserializeItems(DataTable columnTable)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb);
            writer.WriteStartElement(columnTable.TableName == string.Empty ? "table1" : columnTable.TableName);
            foreach (DataRow row in columnTable.Rows)
            {
                writer.WriteStartElement("Row");
                writer.WriteAttributeString("Key", row["Key"].ToString());
                writer.WriteAttributeString("DesigeCaption", row["DesigeCaption"].ToString());
                writer.WriteAttributeString("MessageCode", row["MessageCode"].ToString());
                writer.WriteAttributeString("DllName", row["DllName"].ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
            return sb.ToString();
        }

        #endregion

        public virtual void SetLayout()
        {
            TableLayoutPanel panel = this.Parent as TableLayoutPanel;
            if (panel != null)
            {
                panel.SetRowSpan(this, this._LayoutRowSpan);
                panel.SetColumnSpan(this, this._LayoutColumnSpan);
            }
        }

        public void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.ButtonClick != null)
            {
                DropDownItemClickEventArgs args = new DropDownItemClickEventArgs();
                args.ItemClickEventArgs = e;
                args.DllName = e.Item.Tag == null ? string.Empty : e.Item.Tag.ToString();
                args.DesigeCaption = e.Item.Caption;

                this.ButtonClick(e.Item, args);
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

#if EVENT
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            RaiseEvent(this, "Click", e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            RaiseEvent(this, "DoubleClick", e);
        }
#endif
        #endregion
    }

    /// <summary>
    /// 下拉按钮类
    /// </summary>
    public class KzxDropDownButtonItem
    {
        /// <summary>
        /// 构造
        /// </summary>
        public KzxDropDownButtonItem()
        {
        }

        public static KzxDropDownButtonItem Converter(DataRowView row)
        {
            KzxDropDownButtonItem xmlrow = null;
            xmlrow = new KzxDropDownButtonItem();
            xmlrow.Key = row["Key"].ToString();
            xmlrow.DesigeCaption = row["DesigeCaption"].ToString();
            xmlrow.MessageCode = row["MessageCode"].ToString();
            xmlrow.DllName = row["DllName"].ToString();

            return xmlrow;
        }

        private string _Key = string.Empty;
        /// <summary>
        /// 源单FrmName
        /// </summary>
        [Category("数据"), Description("Key,源单FrmName"), Browsable(true)]
        [McDisplayName("Key")]
        public string Key
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

        private string _DesigeCaption = string.Empty;
        /// <summary>
        /// 源单名称
        /// </summary>
        [Category("数据"), Description("DesigeCaption,源单名称"), Browsable(true)]
        [McDisplayName("DesigeCaption")]
        public string DesigeCaption
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

        private string _MessageCode = string.Empty;
        /// <summary>
        /// 多语言标识
        /// </summary>
        [Category("多语言"), Description("MessageCode,多语言标识"), Browsable(true)]
        [McDisplayName("MessageCode")]
        public string MessageCode
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
    }

    public class DropDownItemClickEventArgs : EventArgs
    {
        private string _DesigeCaption = string.Empty;
        /// <summary>
        /// 源单名称
        /// </summary>
        public string DesigeCaption
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

        private string _DllName = string.Empty;
        /// <summary>
        /// 自定义弹出窗口的配置关键字
        /// </summary>
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

        private ItemClickEventArgs _ItemClickEventArgs = null;
        /// <summary>
        /// 系统事件参数
        /// </summary>
        public ItemClickEventArgs ItemClickEventArgs
        {
            get
            {
                return this._ItemClickEventArgs;
            }
            set
            {
                this._ItemClickEventArgs = value;
            }
        }

        public DropDownItemClickEventArgs()
            : base()
        {
        }
    }
}
