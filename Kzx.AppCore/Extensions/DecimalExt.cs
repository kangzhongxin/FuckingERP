/****************************************************************
 * Copyright (c) 2006-2XXX 上海印智互联信息技术有限公司
 * 产品名称：印智ERP
 * 描    述：Decimal 扩展操作类
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
    /// Decimal 扩展操作类
    /// </summary>
    public static class DecimalExt
    {
        #region 扩展·转换·ToDecimalExt

        /// <summary>
        /// 扩展·字符串转换为decimal
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static decimal ToDecimalExt(this string me)
        {
            if (string.IsNullOrEmpty(me))
                return 0M;

            var result = 0M;
            if (decimal.TryParse(me, out result))
                return result;

            return 0M;
        }

        /// <summary>
        /// 扩展·字符串转换为Decimal
        /// </summary>
        /// <param name="me"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToDecimalExt(this string me, decimal defaultValue)
        {
            if (string.IsNullOrEmpty(me))
                return defaultValue;

            var result = 0M;
            if (decimal.TryParse(me, out result))
                return result;

            return defaultValue;
        }

        #endregion

        #region 扩展·转换·ToDecimalOrDefaultExt

        /// <summary>
        /// 扩展·字符串转换为Decimal可空类型
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static decimal? ToDecimalOrDefaultExt(this string me)
        {
            if (string.IsNullOrEmpty(me))
                return null;

            var result = 0M;
            if (decimal.TryParse(me, out result))
                return result;

            return null;
        }

        #endregion

        #region decimal单价金额小数位控制，默认2位
        /// <summary>
        /// 格式化金额的小数位，暂时固定2位 四舍五入
        /// </summary>
        /// <param name="dValue">要格式化的数值</param>
        /// <returns></returns>
        public static decimal ToDecimalDigitsFormat(this decimal pValue)
        {
            return Math.Round(pValue, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 格式化金额的小数位 四舍五入
        /// </summary>
        /// <param name="pValue">要格式化的数值</param>
        /// <param name="decimalPlaces">保留的小数位</param>
        /// <returns></returns>
        public static decimal ToDecimalDigitsFormat(this decimal pValue, int pDecimalDigits)
        {
            return Math.Round(pValue, pDecimalDigits, MidpointRounding.AwayFromZero);
        }
        #endregion

    }
}
