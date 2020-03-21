using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kzx.UserControl
{
    public interface ILayoutControl
    {

        /// <summary>
        /// 没有多语言的情况下的默认显示标题
        /// </summary>
        string DesigeCaption { get; set; }
        /// <summary>
        /// 设计时的可用性
        /// </summary>
        bool DesigeEnabled { get; set; }
        /// <summary>
        /// 设计时可见性
        /// </summary>
        bool DesigeVisible { get; set; }
        /// <summary>
        /// 控件的唯一标识
        /// </summary>
        string Key { get; set; }
        /// <summary>
        /// 布局列号
        /// </summary>
        int LayoutColumn { get; set; }
        /// <summary>
        /// 布局跨的列数
        /// </summary>
        int LayoutColumnSpan { get; set; }
        /// <summary>
        /// 布局行号
        /// </summary>
        int LayoutRow { get; set; }
        /// <summary>
        /// 布局跨的行数
        /// </summary>
        int LayoutRowSpan { get; set; }
        /// <summary>
        /// 多语言环境下显示文本的对应标识
        /// </summary>
        string MessageCode { get; set; }

        /// <summary>
        /// 事件插件信息表
        /// </summary>
        DataTable PluginInfoTable { get; set; }

        /// <summary>
        /// 事件列表
        /// </summary>
        string EventList { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        string ToolTipText { get; set; }

        /// <summary>
        /// 提示多语言标识
        /// </summary>
        string ToolTipMessageCode { get; set; }

        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="valueControl">控件</param>
        /// <param name="eventInfoTable">事件信息表</param>
        void BindingEvent(Control valueControl, DataTable eventInfoTable);

        /// <summary>
        /// 设置布局
        /// </summary>
        void SetLayout();

        /// <summary>
        /// 控件被加载后调用的方法
        /// 此方法在控件还原后被窗口调用
        /// </summary>
        void KzxControlLoaded();

        /// <summary>
        /// 控件事件
        /// </summary>
        event KzxControlOperateEventHandler KzxControlOperate;
        /// <summary>
        /// 获取多语言文本事件
        /// </summary>
        event KzxGetLanguageEventHandler KzxGetLanguage;
    }
}
