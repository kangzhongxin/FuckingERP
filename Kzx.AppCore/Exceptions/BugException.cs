using System;

namespace Kzx.AppCore
{
    /// <summary>
    /// 应用级逻辑BUG异常
    /// </summary>
    public class BugException : Exception
    {
        #region 构造&初始化

        /// <summary>
        /// 构造 应用级逻辑BUG异常
        /// </summary>
        public BugException()
            : base()
        {

        }

        /// <summary>
        /// 构造 应用级逻辑BUG异常
        /// </summary>
        /// <param name="pMessage"></param>
        public BugException(string pMessage)
            : base(pMessage)
        {

        }

        /// <summary>
        /// 构造 应用级逻辑BUG异常
        /// </summary>
        /// <param name="pMessage"></param>
        /// <param name="pErrorCode"></param>
        public BugException(string pMessage, string pErrorCode)
            : base(pMessage)
        {
            this.ErrorCode = pErrorCode;
        }

        /// <summary>
        /// 构造 应用级逻辑BUG异常
        /// </summary>
        /// <param name="pMessage"></param>
        /// <param name="pInnerException"></param>
        public BugException(string pMessage, Exception pInnerException)
            : base(pMessage, pInnerException)
        {

        }

        /// <summary>
        /// 构造 应用级逻辑BUG异常
        /// </summary>
        /// <param name="pMessage"></param>
        /// <param name="pErrorCode"></param>
        /// <param name="pInnerException"></param>
        public BugException(string pMessage, string pErrorCode, Exception pInnerException)
            : base(pMessage, pInnerException)
        {
            this.ErrorCode = pErrorCode;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取/设置 异常代码
        /// </summary>
        public string ErrorCode
        {
            get;
            set;
        }

        /// <summary>
        /// 获取/设置 多语言代码
        /// </summary>
        public string LanguageCode
        {
            get;
            set;
        }

        #endregion
    }
}
