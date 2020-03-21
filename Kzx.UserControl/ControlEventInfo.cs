using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kzx.UserControl
{
    /// <summary>
    /// 事件信息类,设计时用
    /// </summary>
    public class ControlEventInfo : INotifyPropertyChanged
    {
        public static ControlEventInfo Deserialize(string xml)
        {
            ControlEventInfo instance = new ControlEventInfo();
            PropertyInfo pi = null;
            string[] members = null;
            string[] array = xml.Split(new string[] { "|" }, StringSplitOptions.None);
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Length > 0)
                {
                    members = array[i].Split(new string[] { "=" }, StringSplitOptions.None);
                    if (members.Length > 0)
                    {
                        pi = instance.GetType().GetProperty(members[0]);
                        if (pi != null)
                        {
                            pi.SetValue(instance, (members.Length > 1 ? members[1] : string.Empty), null);
                        }
                    }
                }
            }
            return instance;
        }

        public static string Serialize(ControlEventInfo e)
        {
            StringBuilder sb = new StringBuilder(string.Empty);
            PropertyInfo[] piarray = null;
            PropertyInfo pi = null;
            object value = null;

            piarray = e.GetType().GetProperties();
            for (int i = 0; i < piarray.Length; i++)
            {
                pi = piarray[i];
                value = pi.GetValue(e, null);
                if (value != null)
                {
                    if (value.ToString().Length > 0)
                    {
                        sb.Append(pi.Name + "=" + value.ToString());
                        if (i < piarray.Length - 1)
                        {
                            sb.Append("|");
                        }
                    }
                }
            }
            return sb.ToString();
        }

        #region 属性

        private object _Parent = null;
        /// <summary>
        /// 所有者对象
        /// </summary>
        [Category("属性"), Description("Parent,所有者对象"), Browsable(false)]
        [McDisplayName("Parent")]
        public object ____Parent
        {
            get
            {
                return this._Parent;
            }
            set
            {
                this._Parent = value;
            }
        }

        private string _Click = string.Empty;
        /// <summary>
        /// 单击
        /// </summary>
        [Category("事件"), Description("Click,单击事件"), Browsable(true)]
        [McDisplayName("Click")]
        public virtual string Click
        {
            get
            {
                return this._Click;
            }
            set
            {
                this._Click = value;
                OnPropertyChanged("Click");
            }
        }

        private string _DoubleClick = string.Empty;
        /// <summary>
        /// 双击
        /// </summary>
        [Category("事件"), Description("DoubleClick,双击事件"), Browsable(true)]
        [McDisplayName("DoubleClick")]
        public virtual string DoubleClick
        {
            get
            {
                return this._DoubleClick;
            }
            set
            {
                this._DoubleClick = value;
                OnPropertyChanged("DoubleClick");
            }
        }

        private string _EditValueChanged = string.Empty;
        /// <summary>
        /// EditValue值改变后
        /// </summary>
        [Category("事件"), Description("EditValueChanged,EditValue值改变后"), Browsable(true)]
        [McDisplayName("EditValueChanged")]
        public virtual string EditValueChanged
        {
            get
            {
                return this._EditValueChanged;
            }
            set
            {
                this._EditValueChanged = value;
                OnPropertyChanged("EditValueChanged");
            }
        }

        private string _EditValueChanging = string.Empty;
        /// <summary>
        /// EditValue值改变中
        /// </summary>
        [Category("事件"), Description("EditValueChanging,EditValue值改变中"), Browsable(true)]
        [McDisplayName("EditValueChanging")]
        public virtual string EditValueChanging
        {
            get
            {
                return this._EditValueChanging;
            }
            set
            {
                this._EditValueChanging = value;
                OnPropertyChanged("EditValueChanging");
            }
        }

        private string _KeyDown = string.Empty;
        /// <summary>
        /// 键盘按键已按下
        /// </summary>
        [Category("事件"), Description("KeyDown,键盘按键已按下,按键还未放开弹起"), Browsable(true)]
        [McDisplayName("KeyDown")]
        public virtual string KeyDown
        {
            get
            {
                return this._KeyDown;
            }
            set
            {
                this._KeyDown = value;
                OnPropertyChanged("KeyDown");
            }
        }

        private string _KeyPress = string.Empty;
        /// <summary>
        /// 键盘按键按下
        /// </summary>
        [Category("事件"), Description("KeyPress,键盘按键按下并放开并弹起"), Browsable(true)]
        [McDisplayName("KeyPress")]
        public virtual string KeyPress
        {
            get
            {
                return this._KeyPress;
            }
            set
            {
                this._KeyPress = value;
                OnPropertyChanged("KeyPress");
            }
        }

        private string _KeyUp = string.Empty;
        /// <summary>
        /// 键盘按键已弹起
        /// </summary>
        [Category("事件"), Description("KeyUp,键盘按键已弹起"), Browsable(true)]
        [McDisplayName("KeyUp")]
        public virtual string KeyUp
        {
            get
            {
                return this._KeyUp;
            }
            set
            {
                this._KeyUp = value;
                OnPropertyChanged("KeyUp");
            }
        }

        private string _Enter = string.Empty;
        /// <summary>
        /// 进入控件
        /// </summary>
        [Category("事件"), Description("Enter,进入控件时发生"), Browsable(true)]
        [McDisplayName("Enter")]
        public virtual string Enter
        {
            get
            {
                return this._Enter;
            }
            set
            {
                this._Enter = value;
                OnPropertyChanged("Enter");
            }
        }

        private string _Leave = string.Empty;
        /// <summary>
        /// 输入焦点离开控件
        /// </summary>
        [Category("事件"), Description("Leave,输入焦点离开控件时发生"), Browsable(true)]
        [McDisplayName("Leave")]
        public virtual string Leave
        {
            get
            {
                return this._Leave;
            }
            set
            {
                this._Leave = value;
                OnPropertyChanged("Leave");
            }
        }

        private string _TextChanged = string.Empty;
        /// <summary>
        /// 文本改变
        /// </summary>
        [Category("事件"), Description("TextChanged,Text文本改变时发生"), Browsable(true)]
        [McDisplayName("TextChanged")]
        public virtual string TextChanged
        {
            get
            {
                return this._TextChanged;
            }
            set
            {
                this._TextChanged = value;
                OnPropertyChanged("TextChanged");
            }
        }

        private string _Validating = string.Empty;
        /// <summary>
        /// 控件验证时(前)
        /// </summary>
        [Category("事件"), Description("Validating,控件验证时发生"), Browsable(true)]
        [McDisplayName("Validating")]
        public virtual string Validating
        {
            get
            {
                return this._Validating;
            }
            set
            {
                this._Validating = value;
                OnPropertyChanged("Validating");
            }
        }

        private string _VisibleChanged = string.Empty;
        /// <summary>
        /// 可见与不可见转换
        /// </summary>
        [Category("事件"), Description("VisibleChanged,可见与不可见转换"), Browsable(true)]
        [McDisplayName("VisibleChanged")]
        public virtual string VisibleChanged
        {
            get
            {
                return this._VisibleChanged;
            }
            set
            {
                this._VisibleChanged = value;
                OnPropertyChanged("VisibleChanged");
            }
        }

        private string _ButtonClick = string.Empty;
        /// <summary>
        /// 下拉框按钮单击
        /// </summary>
        [Category("事件"), Description("ButtonClick,下拉框按钮单击"), Browsable(true)]
        [McDisplayName("ButtonClick")]
        public virtual string ButtonClick
        {
            get
            {
                return this._ButtonClick;
            }
            set
            {
                this._ButtonClick = value;
                OnPropertyChanged("ButtonClick");
            }
        }

        private string _Closed = string.Empty;
        /// <summary>
        /// 下拉框收起
        /// </summary>
        [Category("事件"), Description("Closed,下拉框收起时发生"), Browsable(true)]
        [McDisplayName("Closed")]
        public virtual string Closed
        {
            get
            {
                return this._Closed;
            }
            set
            {
                this._Closed = value;
                OnPropertyChanged("Closed");
            }
        }

        private string _CloseUp = string.Empty;
        /// <summary>
        /// 下拉框展开
        /// </summary>
        [Category("事件"), Description("CloseUp,下拉框展开时发生"), Browsable(true)]
        [McDisplayName("CloseUp")]
        public virtual string CloseUp
        {
            get
            {
                return this._CloseUp;
            }
            set
            {
                this._CloseUp = value;
                OnPropertyChanged("CloseUp");
            }
        }

        private string _CheckStateChanged = string.Empty;
        /// <summary>
        /// 复选框状值态改变
        /// </summary>
        [Category("事件"), Description("CheckStateChanged,复选框状态值改变时发生"), Browsable(true)]
        [McDisplayName("CheckStateChanged")]
        public virtual string CheckStateChanged
        {
            get
            {
                return this._CheckStateChanged;
            }
            set
            {
                this._CheckStateChanged = value;
                OnPropertyChanged("CheckStateChanged");
            }
        }

        private string _CheckedChanged = string.Empty;
        /// <summary>
        /// 复选框选中
        /// </summary>
        [Category("事件"), Description("CheckedChanged,复选框选中时发生"), Browsable(true)]
        [McDisplayName("CheckedChanged")]
        public virtual string CheckedChanged
        {
            get
            {
                return this._CheckedChanged;
            }
            set
            {
                this._CheckedChanged = value;
                OnPropertyChanged("CheckedChanged");
            }
        }

        private string _ItemCheck = string.Empty;
        /// <summary>
        /// 列表项选择后
        /// </summary>
        [Category("事件"), Description("ItemCheck,列表项选择后时发生"), Browsable(true)]
        [McDisplayName("ItemCheck")]
        public virtual string ItemCheck
        {
            get
            {
                return this._ItemCheck;
            }
            set
            {
                this._ItemCheck = value;
                OnPropertyChanged("ItemCheck");
            }
        }

        private string _ItemChecking = string.Empty;
        /// <summary>
        /// 列表项选择前
        /// </summary>
        [Category("事件"), Description("ItemChecking,列表项选择前时发生"), Browsable(true)]
        [McDisplayName("ItemChecking")]
        public virtual string ItemChecking
        {
            get
            {
                return this._ItemChecking;
            }
            set
            {
                this._ItemChecking = value;
                OnPropertyChanged("ItemChecking");
            }
        }

        private string _SelectedIndexChanged = string.Empty;
        /// <summary>
        /// 列表选中项下标改变后
        /// </summary>
        [Category("事件"), Description("SelectedIndexChanged,列表选中项下标改变后时发生"), Browsable(true)]
        [McDisplayName("SelectedIndexChanged")]
        public virtual string SelectedIndexChanged
        {
            get
            {
                return this._SelectedIndexChanged;
            }
            set
            {
                this._SelectedIndexChanged = value;
                OnPropertyChanged("SelectedIndexChanged");
            }
        }

        private string _SelectedValueChanged = string.Empty;
        /// <summary>
        /// 列表选中项改变后
        /// </summary>
        [Category("事件"), Description("SelectedValueChanged,列表选中项改变后时发生"), Browsable(true)]
        [McDisplayName("SelectedValueChanged")]
        public virtual string SelectedValueChanged
        {
            get
            {
                return this._SelectedValueChanged;
            }
            set
            {
                this._SelectedValueChanged = value;
                OnPropertyChanged("SelectedValueChanged");
            }
        }

        private string _ImageChanged = string.Empty;
        /// <summary>
        /// 图片改变后
        /// </summary>
        [Category("事件"), Description("ImageChanged,图片改变后时发生"), Browsable(true)]
        [McDisplayName("ImageChanged")]
        public virtual string ImageChanged
        {
            get
            {
                return this._ImageChanged;
            }
            set
            {
                this._ImageChanged = value;
                OnPropertyChanged("ImageChanged");
            }
        }

        private string _DateTimeChanged = string.Empty;
        /// <summary>
        /// 日期框的日期值改变后
        /// </summary>
        [Category("事件"), Description("DateTimeChanged,日期框的日期值改变后时发生"), Browsable(true)]
        [McDisplayName("DateTimeChanged")]
        public virtual string DateTimeChanged
        {
            get
            {
                return this._DateTimeChanged;
            }
            set
            {
                this._DateTimeChanged = value;
                OnPropertyChanged("DateTimeChanged");
            }
        }

        private string _CloseButtonClick = string.Empty;
        /// <summary>
        /// 页签关闭按钮单击时
        /// </summary>
        [Category("事件"), Description("CloseButtonClick,页签关闭按钮单击时发生"), Browsable(true)]
        [McDisplayName("CloseButtonClick")]
        public virtual string CloseButtonClick
        {
            get
            {
                return this._CloseButtonClick;
            }
            set
            {
                this._CloseButtonClick = value;
                OnPropertyChanged("CloseButtonClick");
            }
        }

        private string _SelectedPageChanged = string.Empty;
        /// <summary>
        /// 页签的选中页改变后
        /// </summary>
        [Category("事件"), Description("SelectedPageChanged,页签的选中页改变后发生"), Browsable(true)]
        [McDisplayName("SelectedPageChanged")]
        public virtual string SelectedPageChanged
        {
            get
            {
                return this._SelectedPageChanged;
            }
            set
            {
                this._SelectedPageChanged = value;
                OnPropertyChanged("SelectedPageChanged");
            }
        }

        private string _SelectedPageChanging = string.Empty;
        /// <summary>
        /// 页签的选中页改变中
        /// </summary>
        [Category("事件"), Description("SelectedPageChanging,页签的选中页改变中发生"), Browsable(true)]
        [McDisplayName("SelectedPageChanging")]
        public virtual string SelectedPageChanging
        {
            get
            {
                return this._SelectedPageChanging;
            }
            set
            {
                this._SelectedPageChanging = value;
                OnPropertyChanged("SelectedPageChanging");
            }
        }

        private string _EditorKeyDown = string.Empty;
        /// <summary>
        /// 网格的键盘按键已按下
        /// </summary>
        [Category("事件"), Description("EditorKeyDown,网格的键盘按键已按下时发生"), Browsable(true)]
        [McDisplayName("EditorKeyDown")]
        public virtual string EditorKeyDown
        {
            get
            {
                return this._EditorKeyDown;
            }
            set
            {
                this._EditorKeyDown = value;
                OnPropertyChanged("EditorKeyDown");
            }
        }

        private string _EditorKeyPress = string.Empty;
        /// <summary>
        /// 网格的键盘按键按下
        /// </summary>
        [Category("事件"), Description("EditorKeyPress,网格的键盘按键按下时发生"), Browsable(true)]
        [McDisplayName("EditorKeyPress")]
        public virtual string EditorKeyPress
        {
            get
            {
                return this._EditorKeyPress;
            }
            set
            {
                this._EditorKeyPress = value;
                OnPropertyChanged("EditorKeyPress");
            }
        }

        private string _EditorKeyUp = string.Empty;
        /// <summary>
        /// 网格的键盘按键已弹起
        /// </summary>
        [Category("事件"), Description("EditorKeyUp,网格的键盘按键已弹起时发生"), Browsable(true)]
        [McDisplayName("EditorKeyUp")]
        public virtual string EditorKeyUp
        {
            get
            {
                return this._EditorKeyUp;
            }
            set
            {
                this._EditorKeyUp = value;
                OnPropertyChanged("EditorKeyUp");
            }
        }

        private string _LoadCompleted = string.Empty;
        /// <summary>
        /// 图片控件中图片加载完成
        /// </summary>
        [Category("事件"), Description("LoadCompleted,异步操作图片控件中图片加载完成"), Browsable(true)]
        [McDisplayName("LoadCompleted")]
        public virtual string LoadCompleted
        {
            get
            {
                return this._LoadCompleted;
            }
            set
            {
                this._LoadCompleted = value;
                OnPropertyChanged("LoadCompleted");
            }
        }

        private string _LoadProgressChanged = string.Empty;
        /// <summary>
        /// 图片控件中图片加载的进度提示
        /// </summary>
        [Category("事件"), Description("LoadProgressChanged,异步操作图片控件中图片加载的进度提示"), Browsable(true)]
        [McDisplayName("LoadProgressChanged")]
        public virtual string LoadProgressChanged
        {
            get
            {
                return this._LoadProgressChanged;
            }
            set
            {
                this._LoadProgressChanged = value;
                OnPropertyChanged("LoadProgressChanged");
            }
        }

        private string _AfterFocusNode = string.Empty;
        /// <summary>
        /// 焦点结点改变后事件
        /// </summary>
        [Category("事件"), Description("AfterFocusNode,焦点结点改变后事件"), Browsable(true)]
        [McDisplayName("AfterFocusNode")]
        public virtual string AfterFocusNode
        {
            get
            {
                return this._AfterFocusNode;
            }
            set
            {
                this._AfterFocusNode = value;
                OnPropertyChanged("AfterFocusNode");
            }
        }

        private string _AfterCheckNode = string.Empty;
        /// <summary>
        /// 选中结点后事件
        /// </summary>
        [Category("事件"), Description("AfterCheckNode,选中结点后事件"), Browsable(true)]
        [McDisplayName("AfterCheckNode")]
        public virtual string AfterCheckNode
        {
            get
            {
                return this._AfterCheckNode;
            }
            set
            {
                this._AfterCheckNode = value;
                OnPropertyChanged("AfterCheckNode");
            }
        }

        private string _BeforeCheck = string.Empty;
        /// <summary>
        /// 勾选结点前事件(微软树)
        /// </summary>
        [Category("事件"), Description("BeforeCheck,勾选结点前事件(微软树)"), Browsable(true)]
        [McDisplayName("BeforeCheck")]
        public virtual string BeforeCheck
        {
            get
            {
                return this._BeforeCheck;
            }
            set
            {
                this._BeforeCheck = value;
                OnPropertyChanged("BeforeCheck");
            }
        }

        private string _AfterSelect = string.Empty;
        /// <summary>
        /// 选中结点后事件(微软树)
        /// </summary>
        [Category("事件"), Description("AfterSelect,选中结点后事件(微软树)"), Browsable(true)]
        [McDisplayName("AfterSelect")]
        public virtual string AfterSelect
        {
            get
            {
                return this._AfterSelect;
            }
            set
            {
                this._AfterSelect = value;
                OnPropertyChanged("AfterSelect");
            }
        }

        private string _AfterCheck = string.Empty;
        /// <summary>
        /// 勾选结点后事件(微软树)
        /// </summary>
        [Category("事件"), Description("AfterCheck,勾选结点后事件(微软树)"), Browsable(true)]
        [McDisplayName("AfterCheck")]
        public virtual string AfterCheck
        {
            get
            {
                return this._AfterCheck;
            }
            set
            {
                this._AfterCheck = value;
                OnPropertyChanged("AfterCheck");
            }
        }

        private string _AfterLabelEdit = string.Empty;
        /// <summary>
        /// 编辑结点后事件(微软树)
        /// </summary>
        [Category("事件"), Description("AfterLabelEdit,编辑结点后事件(微软树)"), Browsable(true)]
        [McDisplayName("AfterLabelEdit")]
        public virtual string AfterLabelEdit
        {
            get
            {
                return this._AfterLabelEdit;
            }
            set
            {
                this._AfterLabelEdit = value;
                OnPropertyChanged("AfterLabelEdit");
            }
        }

        private string _BeforeLabelEdit = string.Empty;
        /// <summary>
        /// 编辑结点前事件(微软树)
        /// </summary>
        [Category("事件"), Description("BeforeLabelEdit,编辑结点前事件(微软树)"), Browsable(true)]
        [McDisplayName("BeforeLabelEdit")]
        public virtual string BeforeLabelEdit
        {
            get
            {
                return this._BeforeLabelEdit;
            }
            set
            {
                this._BeforeLabelEdit = value;
                OnPropertyChanged("BeforeLabelEdit");
            }
        }

        private string _NodeMouseClick = string.Empty;
        /// <summary>
        /// 结点单击事件(微软树)
        /// </summary>
        [Category("事件"), Description("NodeMouseClick,结点单击事件(微软树)"), Browsable(true)]
        [McDisplayName("NodeMouseClick")]
        public virtual string NodeMouseClick
        {
            get
            {
                return this._NodeMouseClick;
            }
            set
            {
                this._NodeMouseClick = value;
                OnPropertyChanged("NodeMouseClick");
            }
        }

        private string _NodeMouseDoubleClick = string.Empty;
        /// <summary>
        /// 结点双击事件(微软树)
        /// </summary>
        [Category("事件"), Description("NodeMouseDoubleClick,结点双击事件(微软树)"), Browsable(true)]
        [McDisplayName("NodeMouseDoubleClick")]
        public virtual string NodeMouseDoubleClick
        {
            get
            {
                return this._NodeMouseDoubleClick;
            }
            set
            {
                this._NodeMouseDoubleClick = value;
                OnPropertyChanged("NodeMouseDoubleClick");
            }
        }

        private string _CellClick = string.Empty;
        /// <summary>
        /// 网格单元格单击
        /// </summary>
        [Category("事件"), Description("CellClick,网格单元格单击"), Browsable(true)]
        [McDisplayName("CellClick")]
        public virtual string CellClick
        {
            get
            {
                return this._CellClick;
            }
            set
            {
                this._CellClick = value;
                OnPropertyChanged("CellClick");
            }
        }

        private string _CellDoubleClick = string.Empty;
        /// <summary>
        /// 网格单元格双击
        /// </summary>
        [Category("事件"), Description("CellDoubleClick,网格单元格双击"), Browsable(true)]
        [McDisplayName("CellDoubleClick")]
        public virtual string CellDoubleClick
        {
            get
            {
                return this._CellDoubleClick;
            }
            set
            {
                this._CellDoubleClick = value;
                OnPropertyChanged("CellDoubleClick");
            }
        }

        private string _CellButtonClick = string.Empty;
        /// <summary>
        /// 网格单元格ButtonEdit列按钮单击
        /// </summary>
        [Category("事件"), Description("CellButtonClick,网格单元格ButtonEdit列按钮单击"), Browsable(true)]
        [McDisplayName("CellButtonClick")]
        public virtual string CellButtonClick
        {
            get
            {
                return this._CellButtonClick;
            }
            set
            {
                this._CellButtonClick = value;
                OnPropertyChanged("CellButtonClick");
            }
        }

        private string _CellValueChanged = string.Empty;
        /// <summary>
        /// 网格单元格值改变后
        /// </summary>
        [Category("事件"), Description("CellValueChanged,网格单元格值改变后"), Browsable(true)]
        [McDisplayName("CellValueChanged")]
        public virtual string CellValueChanged
        {
            get
            {
                return this._CellValueChanged;
            }
            set
            {
                this._CellValueChanged = value;
                OnPropertyChanged("CellValueChanged");
            }
        }

        private string _CellValueChanging = string.Empty;
        /// <summary>
        /// 网格单元格值改变中
        /// </summary>
        [Category("事件"), Description("CellValueChanging,网格单元格值改变中"), Browsable(true)]
        [McDisplayName("CellValueChanging")]
        public virtual string CellValueChanging
        {
            get
            {
                return this._CellValueChanging;
            }
            set
            {
                this._CellValueChanging = value;
                OnPropertyChanged("CellValueChanging");
            }
        }

        private string _FocuseRowChanged = string.Empty;
        /// <summary>
        /// 网格焦点行改变事件
        /// </summary>
        [Category("事件"), Description("FocuseRowChanged,网格焦点行改变事件"), Browsable(true)]
        [McDisplayName("FocuseRowChanged")]
        public virtual string FocuseRowChanged
        {
            get
            {
                return this._FocuseRowChanged;
            }
            set
            {
                this._FocuseRowChanged = value;
                OnPropertyChanged("FocuseRowChanged");
            }
        }

        private string _FocusedNodeChanged = string.Empty;
        /// <summary>
        /// 树焦点结点改变事件
        /// </summary>
        [Category("事件"), Description("FocusedNodeChanged,树焦点结点改变事件"), Browsable(true)]
        [McDisplayName("FocusedNodeChanged")]
        public virtual string FocusedNodeChanged
        {
            get
            {
                return this._FocusedNodeChanged;
            }
            set
            {
                this._FocusedNodeChanged = value;
                OnPropertyChanged("FocusedNodeChanged");
            }
        }

        private string _KzxValueChanged = string.Empty;
        /// <summary>
        /// 根据KzxValueChanged选择数据并同步数据
        /// </summary>
        [Category("事件"), Description("KzxValueChanged,自定义值改变后,值真正的改变后,在焦点离开时触发"), Browsable(true)]
        [McDisplayName("KzxValueChanged")]
        public virtual string KzxValueChanged
        {
            get
            {
                return this._KzxValueChanged;
            }
            set
            {
                this._KzxValueChanged = value;
                OnPropertyChanged("KzxValueChanged");
            }
        }

        private string _ShowingEditor = string.Empty;
        /// <summary>
        /// 启禁用单元格编辑
        /// </summary>
        [Category("事件"), Description("ShowingEditor,启禁用单元格编辑"), Browsable(true)]
        [McDisplayName("ShowingEditor")]
        public virtual string ShowingEditor
        {
            get
            {
                return this._ShowingEditor;
            }
            set
            {
                this._ShowingEditor = value;
                OnPropertyChanged("ShowingEditor");
            }
        }

        private string _ShownEditor = string.Empty;
        /// <summary>
        /// 显示单元格编辑控件事件
        /// </summary>
        [Category("事件"), Description("ShownEditor,显示单元格编辑控件事件"), Browsable(true)]
        [McDisplayName("ShownEditor")]
        public string ShownEditor
        {
            get
            {
                return this._ShownEditor;
            }
            set
            {
                this._ShownEditor = value;
            }
        }

        private string _FocusedColumnChanged = string.Empty;
        /// <summary>
        /// 网格焦点列改变事件
        /// </summary>
        [Category("事件"), Description("FocusedColumnChanged,网格焦点列改变事件"), Browsable(true)]
        [McDisplayName("FocusedColumnChanged")]
        public virtual string FocusedColumnChanged
        {
            get
            {
                return this._FocusedColumnChanged;
            }
            set
            {
                this._FocusedColumnChanged = value;
                OnPropertyChanged("FocusedColumnChanged");
            }
        }

        private string _KzxControlLoaded = string.Empty;
        /// <summary>
        /// 自定义控件被加载后事件
        /// </summary>
        [Category("事件"), Description("KzxControlLoaded,自定义控件被加载后事件"), Browsable(true)]
        [McDisplayName("KzxControlLoaded")]
        public string KzxControlLoaded
        {
            get
            {
                return this._KzxControlLoaded;
            }
            set
            {
                this._KzxControlLoaded = value;
            }
        }

        private string _DragOver = string.Empty;
        /// <summary>
        /// 拖放完成事件
        /// </summary>
        [Category("事件"), Description("DragOver,拖放完成事件"), Browsable(true)]
        [McDisplayName("DragOver")]
        public string DragOver
        {
            get
            {
                return this._DragOver;
            }
            set
            {
                this._DragOver = value;
            }
        }

        private string _QueryPopUp = string.Empty;
        /// <summary>
        /// 下拉框弹出前事件
        /// </summary>
        [Category("事件"), Description("QueryPopUp,下拉框弹出前事件"), Browsable(true)]
        [McDisplayName("QueryPopUp")]
        public string QueryPopUp
        {
            get
            {
                return this._QueryPopUp;
            }
            set
            {
                this._QueryPopUp = value;
            }
        }

        #endregion

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
