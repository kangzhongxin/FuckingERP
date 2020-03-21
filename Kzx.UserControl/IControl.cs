using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kzx.UserControl
{
    /// <summary>
    /// 控件事件委托
    /// </summary>
    /// <param name="sender">事件发起者</param>
    /// <param name="e">事件参数</param>
    public delegate void KzxControlOperateEventHandler(object sender, ControlEventArgs e);

    /// <summary>
    /// 获取多语言文本事件委托
    /// </summary>
    /// <param name="sender">事件发起者</param>
    /// <param name="messageCode">语言标识</param>
    /// <param name="text">多语言的文本</param>
    public delegate void KzxGetLanguageEventHandler(object sender, string messageCode, ref string text);
    public interface IControl
    {
        /// <summary>
        /// 有Load方法
        /// </summary>
        bool HasLoad { get; }

        /// <summary>
        /// 被引用后允许修改
        /// true允许,false不允许
        /// </summary>
        bool AllowEdit { get; set; }

        /// <summary>
        /// 多语言环境下显示文本的对应标识
        /// </summary>
        string MessageCode { get; set; }

        /// <summary>
        /// 设计时的显示，方便设计员工识别
        /// </summary>
        string DesigeCaption { get; set; }

        /// <summary>
        /// 控件的唯一标识
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// True控件可用,False控件不可用
        /// </summary>
        Boolean Enabled { get; set; }

        /// <summary>
        /// True控件可见,False控件不可见
        /// </summary>
        Boolean Visible { get; set; }

        /// <summary>
        /// Tag标志,用于存储任何数据
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// 设计时的可用性
        /// </summary>
        Boolean DesigeEnabled { get; set; }

        /// <summary>
        /// 设计时可见性
        /// </summary>
        Boolean DesigeVisible { get; set; }

        /// <summary>
        /// 显示格式字符串
        /// </summary>
        String FormatString { get; set; }

        /// <summary>
        /// 数据库的字段名称
        /// </summary>
        String Field { get; set; }

        /// <summary>
        /// 数据库的表名
        /// </summary>
        String Table { get; set; }

        /// <summary>
        /// 源表名称
        /// </summary>
        String SourceTable { get; set; }

        /// <summary>
        /// 源表实际值字段
        /// </summary>
        String SelectedValuePath { get; set; }

        /// <summary>
        /// 源表显示值字段
        /// </summary>
        String DisplayMemberPath { get; set; }

        /// <summary>
        /// 可空性
        /// </summary>
        Boolean IsNull { get; set; }

        /// <summary>
        /// 只读性
        /// </summary>
        Boolean ReadOnly { get; set; }

        /// <summary>
        /// 唯一性验证组别
        /// </summary>
        String ValidateGroup { get; set; }

        /// <summary>
        /// 唯一性
        /// </summary>
        Boolean IsUnique { get; set; }

        /// <summary>
        /// 是否启用值范围验证
        /// </summary>
        Boolean AllowValueRange { get; set; }

        /// <summary>
        /// 值范围验证中的数据类型
        /// </summary>
        KzxDataType DataType { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        String DefaultValue { get; set; }

        /// <summary>
        /// 值范围验证中最大值
        /// </summary>
        String MaxValue { get; set; }

        /// <summary>
        /// 值范围验证中最小值
        /// </summary>
        String MinValue { get; set; }

        /// <summary>
        /// 正则表达式验证
        /// </summary>
        String RegexString { get; set; }

        /// <summary>
        /// 数据携带
        /// </summary>
        string ValueDependencyField { get; set; }

        /// <summary>
        /// 界面上所有对象的集合
        /// </summary>
        List<object> ObjectList { get; }

        /// <summary>
        /// 自定义弹出窗口的配置关键字
        /// </summary>
        string DllName { get; set; }

        /// <summary>
        /// 事件插件信息表
        /// </summary>
        DataTable PluginInfoTable { get; set; }

        /// <summary>
        /// 事件列表
        /// </summary>
        string EventList { get; set; }
        /// <summary>
        /// 指定父表
        /// </summary>
        string ParentTable { get; set; }
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
        /// 提示信息
        /// </summary>
        string ToolTipText { get; set; }

        /// <summary>
        /// 提示多语言标识
        /// </summary>
        string ToolTipMessageCode { get; set; }

        /// <summary>
        /// 数据长度不能超过数据库长度提示文本 add by huangyq20170519
        /// </summary>
        string ToolTipMaxLengthText { get; set; }

        /// <summary>
        /// 设置数据绑定
        /// </summary>
        /// <param name="binding">数据绑定对象</param>
        /// <return>int</return>
        int SetDataBinding(object binding);

        /// <summary>
        /// 设置下拉框的数据源
        /// </summary>
        /// <param name="binding">下拉框的数据源</param>
        /// <param name="displayMember">显示值字段名</param>
        /// <param name="valueMember">实际值字段名</param>
        /// <returns>int</returns>
        int SetSourceTableBinding(object binding, string displayMember, string valueMember);

        /// <summary>
        /// 是否触发验证事件
        /// </summary>
        /// <param name="e">true触发，false不触发</param>
        void SetCausesValidation(bool e);

        /// <summary>
        /// 取控件的值
        /// </summary>
        /// <return>Object</return>
        Object GetValue();

        /// <summary>
        /// 设置控件的值
        /// </summary>
        /// <param name="value">控件的值</param>
        /// <return>int</return>
        int SetValue(Object value);

        /// <summary>
        /// 合法性验证
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <returns>1,验证通过,其他验证失败</returns>
        int Validation(object sender);

        /// <summary>
        /// 还原默认值
        /// </summary>
        /// <return>void</return>
        void SetDefaultValue();

        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="valueControl">控件</param>
        /// <param name="eventInfoTable">事件信息表</param>
        void BindingEvent(Control valueControl, DataTable eventInfoTable);

        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="eventInfoTable">事件信息表</param>
        void BindingEvent(DataTable eventInfoTable);

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
        /// 清除所有的错误信息
        /// </summary>
        void ClearErrors();

        /// <summary>
        /// 控件事件（与单据对象交互）
        /// </summary>
        /// <summary>
        /// 格式:
        /// </summary>
        /// <summary>
        /// 1.object类型,控件本身
        /// </summary>
        /// <summary>
        /// 2.自定义参数,以EventArgs为基类,增加事件名称,控件的表名称，字段名称
        /// </summary>
        event KzxControlOperateEventHandler KzxControlOperate;

        /// <summary>
        /// 获取多语言文本事件
        /// </summary>
        event KzxGetLanguageEventHandler KzxGetLanguage;
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum KzxDataType
    {
        /// <summary>
        /// 字符串数据类型
        /// </summary>
        Str = 0,
        /// <summary>
        /// 32 位有符号整数数据类型
        /// </summary>
        Int,
        /// <summary>
        /// 一种可以包含货币符号的十进制数据类型
        /// </summary>
        Decimal,
        /// <summary>
        /// 双精度浮点数数据类型
        /// </summary>
        Double,
        /// <summary>
        /// 日期数据类型
        /// </summary>
        Date
    }
}
