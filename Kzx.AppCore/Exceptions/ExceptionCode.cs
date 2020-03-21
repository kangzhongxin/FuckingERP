namespace Kzx.AppCore
{
    /// <summary>
    /// 异常编码
    /// </summary>
    public struct ExceptionCode
    {
        private string _errorCode;
        private string _languageCode;
        private string _Message;

        private ExceptionCode(string pErrorCode, string pLanguageCode, string pMessage)
        {
            _errorCode = pErrorCode;
            _Message = pMessage;
            _languageCode = pLanguageCode;
        }

        public string ErrorCode
        {
            get { return _errorCode; }
        }

        public string Message
        {
            get { return _Message; }
        }

        public string LanguageCode
        {
            get { return _languageCode; }
        }

        /// <summary>
        /// 网络异常：请查看网络连接是否通畅
        /// </summary>
        public static ExceptionCode Network = new ExceptionCode("NETWORK", "SYS000226", "请查看网络连接是否通畅");

        /// <summary>
        /// 登录状态丢失
        /// </summary>
        public static ExceptionCode SessionError = new ExceptionCode("SESSION-ERR", "EE4A414B9620C83A", "登录状态丢失"); 
    }
}
