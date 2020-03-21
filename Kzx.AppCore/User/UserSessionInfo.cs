using System;

namespace Kzx.AppCore
{
    /// <summary>
    /// 用户会话信息
    /// </summary>
    public class UserSessionInfo
    {
        /// <summary>
        /// 用户ID（登录名）
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// TOKEN
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }
    }
}
