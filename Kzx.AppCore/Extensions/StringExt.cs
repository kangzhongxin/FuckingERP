 
using System;

namespace Kzx.AppCore
{
    /// <summary>
    /// String 扩展操作类
    /// </summary>
    public static class StringExt
    {
        #region 扩展·转换·ToStringExt

        /// <summary>
        /// 扩展·转换为String类型
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string ToStringExt(this object me)
        {
            if (me == null) return string.Empty;

            return me.ToString();
        }

        #endregion

        #region 扩展·判断·IsNullOrEmptyExt

        /// <summary>
        /// 扩展·判断是否为空
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static bool IsNullOrEmptyExt(this string me)
        {
            return string.IsNullOrEmpty(me);
        }

        #endregion

        #region 扩展·判断·IsNullOrWhiteSpaceExt

        /// <summary>
        /// 扩展·判断是否为空
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpaceExt(this string me)
        {
            return string.IsNullOrWhiteSpace(me);
        }

        #endregion

        #region 扩展·比较·IgnoreCaseEqualExt
         
        /// <summary> 扩展·比较·IgnoreCaseEqualExt </summary>
        public static bool IgnoreCaseEqualExt(this string me, string toCompare)
        {
            return me.Equals(toCompare, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region 扩展·空格·TrimExt

        /// <summary>
        /// 扩展·去掉前后空格
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string TrimExt(this string me)
        {
            if (string.IsNullOrEmpty(me))
                return string.Empty;

            return me.Trim();
        }

        #endregion

        #region 扩展·空格·TrimEndExt

        /// <summary>
        /// 扩展·去掉前端空格
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string TrimStartExt(this string me)
        {
            if (string.IsNullOrEmpty(me))
                return string.Empty;

            return me.TrimStart();
        }

        #endregion

        #region 扩展·空格·TrimEndExt

        /// <summary>
        /// 扩展·去掉未端空格
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string TrimEndExt(this string me)
        {
            if (string.IsNullOrEmpty(me))
                return string.Empty;

            return me.TrimEnd();
        }

        #endregion

        #region 扩展·格式化·FormatExt

        /// <summary>
        /// 扩展·格式化字符串
        /// </summary>
        /// <param name="me"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatExt(this string me, params object[] args)
        {
            if (string.IsNullOrEmpty(me))
                return string.Empty;

            return string.Format(me, args);
        }

        #endregion

        #region 扩展·连接·ConcatExt

        /// <summary>
        /// 扩展·连接字符串
        /// </summary>
        /// <param name="me"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string ConcatExt(this string me, params string[] args)
        {
            if (args.Length == 0) return me;

            return string.Concat(me, string.Concat(args));
        }

        #endregion
    }
}