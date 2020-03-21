using System;

namespace Kzx.AppCore
{
    /// <summary>
    /// 跳出异常
    /// </summary>
    [Serializable]
    public class BreakException : Exception
    {
        public BreakException()
            : base()
        { }
    }
}
