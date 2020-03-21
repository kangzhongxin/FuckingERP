using System;

namespace Kzx.AppCore
{
    /// <summary>
    /// 提示消息类异常(可直接显示给用户看的异常信息)
    /// </summary>
    public class MessageException : Exception
    {
        #region 构造&初始化

        /// <summary>
        /// 构造消息类异常
        /// </summary>
        public MessageException()
            : base()
        {

        }

        /// <summary>
        /// 构造消息类异常
        /// </summary>
        /// <param name="pMessage">提示消息</param>
        public MessageException(string pMessage)
            : base(pMessage)
        {

        }

        /// <summary>
        /// 构造消息类异常
        /// </summary>
        /// <param name="pErrorCode">错误代码</param>
        /// <param name="pMessage">提示消息</param>
        public MessageException(string pErrorCode, string pMessage)
            : base(pMessage)
        {
            this.ErrorCode = pErrorCode;
        }

        /// <summary>
        /// 构造消息类异常
        /// </summary>
        /// <param name="pMessage">提示消息</param>
        /// <param name="pInnerException">异常</param>
        public MessageException(string pMessage, Exception pInnerException)
            : base(pMessage, pInnerException)
        {

        }

        /// <summary>
        /// 构造消息类异常
        /// </summary>
        /// <param name="pErrorCode">异常代码</param>
        /// <param name="pMessage">提示消息</param>
        /// <param name="pInnerException">异常</param>
        public MessageException(string pErrorCode, string pMessage, Exception pInnerException)
            : base(pMessage, pInnerException)
        {
            this.ErrorCode = pErrorCode;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取/设置 错误代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 获取/设置 多语言代码
        /// </summary>
        public string LanguageCode
        {
            get;
            set;
        }

        #endregion

        #region 抛出异常

        /// <summary>
        /// 抛出MessageException异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Throw(string message, params object[] args)
        {
            var msg = string.Format(message, args);
            throw new MessageException(msg);
        }

        /// <summary>
        /// 抛出MessageException异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <param name="args"></param>
        public static void Throw(string message, string code, params object[] args)
        {
            var msg = string.Format(message, args);
            throw new MessageException(code, msg);
        }

        /// <summary>
        /// 抛出MessageException异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <param name="innerException"></param>
        /// <param name="args"></param>
        public static void Throw(string message, string code, Exception innerException, params object[] args)
        {
            var msg = string.Format(message, args);
            throw new MessageException(code, msg, innerException);
        }

        #endregion
    }
}
