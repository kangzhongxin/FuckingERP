using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Kzx.AppCore
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public static class MD5EncryptUtil
    {
        /// <summary>
        /// 获取MD5加密（32位）
        /// 全部小写
        /// </summary>
        /// <param name="value">要加密的值</param>
        /// <returns>加密后值（全部小写）,NULL值时返回""</returns>
        public static string EncryptOfLower(string value)
        {
            return MD5Helper(value, false);
        }

        /// <summary>
        /// 获取MD5加密（32位）
        /// 全部大写
        /// </summary>
        /// <param name="value">要加密的值</param>
        /// <returns>加密后值（全部大写）</returns>
        public static string EncryptOfUpper(string value)
        {
            return MD5Helper(value, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        private static string MD5Helper(string value, bool upper)
        {
            if (value == null) return "";

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] InBytes = Encoding.UTF8.GetBytes(value);
            byte[] OutBytes = md5.ComputeHash(InBytes);

            int outBytesLength = OutBytes.Length;
            string format = upper ? "X2" : "x2";
            StringBuilder sbString = new StringBuilder();

            for (int i = 0; i < outBytesLength; i++)
            {
                sbString.Append(OutBytes[i].ToString(format));
            }

            return sbString.ToString();
        }
    }
}
