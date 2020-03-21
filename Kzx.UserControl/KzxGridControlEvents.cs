namespace Kzx.UserControl
{
    /// <summary>
    /// 表格控件全选/全消事件参数
    /// </summary>
    public class KzxGridControlChooseAllEventArg
    {
        /// <summary>
        /// 是否为选择状态
        /// </summary>
        public bool IsChecked { get; internal set; }
    }

    /// <summary>
    /// 表格控件全选/全消委托
    /// </summary>
    /// <param name="pIsChecked"></param>
    public delegate void KzxGridControlChooseAllEndHandle(KzxGridControlChooseAllEventArg pArg);

    /// <summary>
    /// 表格控件右键单元格复制行委托
    /// </summary>
    /// <param name="pIsChecked"></param>
    public delegate void KzxGridControlRightClickCopyHandle();
}
