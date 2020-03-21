using System;

namespace Kzx.AppCore
{
    /// <summary>
    /// 时间日期操作扩展类
    /// </summary>
    public static class DateTimeExt
    {
        #region 扩展·转换·ToDateTimeExt

        /// <summary>
        /// 扩展·字符串转为日期
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeExt(this string me)
        {
            if (string.IsNullOrEmpty(me))
                return DateTime.MinValue;

            DateTime result = DateTime.MinValue;
            if (DateTime.TryParse(me, out result))
                return result;

            return DateTime.MinValue;
        }

        /// <summary>
        /// 扩展·字符串转为日期
        /// </summary>
        /// <param name="me"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeExt(this string me, string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                return ToDateTimeExt(me);

            try
            {
                return DateTime.ParseExact(me, format, global::System.Threading.Thread.CurrentThread.CurrentCulture);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        #endregion

        #region 扩展·转换·ToDateTimeOrDefaultExt

        /// <summary>
        /// 扩展·字符串转为日期可空类型
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static DateTime? ToDateTimeOrDefaultExt(this string me)
        {
            if (string.IsNullOrEmpty(me))
                return null;

            DateTime result = DateTime.MinValue;
            if (DateTime.TryParse(me, out result))
                return result;

            return null;
        }

        /// <summary>
        /// 扩展·字符串转为日期可空类型
        /// </summary>
        /// <param name="me"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime? ToDateTimeOrDefaultExt(this string me, string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                return ToDateTimeOrDefaultExt(me);

            try
            {
                return DateTime.ParseExact(me, format, global::System.Threading.Thread.CurrentThread.CurrentCulture);
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
