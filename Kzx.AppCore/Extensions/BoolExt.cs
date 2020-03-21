/****************************************************************
 * Copyright (c) 2006-2XXX 上海印智互联信息技术有限公司
 * 产品名称：印智ERP
 * 描    述：布尔值 扩展操作类
 * 负 责 人：chen.q
 * 创 建 人：chen.q
 * 创建日期：2018-2-13
 * 曾经负责人：
 *---------------------------------------------------------------
 * 【修改记录】
 * {修改时间}：BY {修改人}，{修改描述}
 ****************************************************************/

using System;

namespace Kzx.AppCore
{
    /// <summary>
    /// 布尔扩展操作类
    /// </summary>
    public static class BoolExt
    {
        #region 扩展·转换·ToBoolExt

        /// <summary>
        /// 扩展·字符串转换为布尔值
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static bool ToBoolExt(this string me)
        {
            if (string.IsNullOrEmpty(me))
                return false;

            if ("1".Equals(me))
                me = "TRUE";

            return string.Equals(me, "TRUE", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 扩展·字符串转换为布尔值
        /// </summary>
        /// <param name="me"></param>
        /// <param name="trueValue"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool ToBoolExt(this string me, string trueValue, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(me))
                return false;

            if (ignoreCase)
                return string.Equals(me, trueValue, StringComparison.OrdinalIgnoreCase);
            else
                return string.Equals(me, trueValue);
        }

        /// <summary>
        /// 扩展·字符串转换为布尔值
        /// </summary>
        /// <param name="me"></param>
        /// <param name="trueValue"></param>
        /// <returns></returns>
        public static bool ToBoolExt(this string me, int trueValue)
        {
            if (string.IsNullOrEmpty(me))
                return false;

            return string.Equals(me, trueValue.ToString());
        }

        #endregion

        #region 扩展·转换·ToBoolOrDefaultExt

        /// <summary>
        /// 扩展·字符串转换为布尔值
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static bool? ToBoolOrDefaultExt(this string me)
        {
            if (string.IsNullOrEmpty(me))
                return null;

            if (string.Equals(me, "TRUE", StringComparison.OrdinalIgnoreCase))
                return true;
            else if (string.Equals(me, "FALSE", StringComparison.OrdinalIgnoreCase))
                return false;

            return null;
        }

        #endregion
    }
}
