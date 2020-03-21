/****************************************************************
 * Copyright (c) 2006-2XXX 上海印智互联信息技术有限公司
 * 产品名称：印智ERP
 * 描    述：String 扩展操作类
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
    /// Int 扩展类
    /// </summary>
    public static class IntExt
    {
        #region 扩展·ToIntExt

        /// <summary>
        /// 扩展·字符串转换为Int
        /// </summary>
        /// <param name="me"></param>
        /// <returns>转换成功，返回对应的整型数值；转换失败，则返回0。</returns>
        public static int ToIntExt(this string me)
        {
            if (string.IsNullOrEmpty(me))
                return 0;

            float result = 0f;
            if (float.TryParse(me, out result))
                return (int)result;

            return 0;
        }

        /// <summary>
        /// 扩展·字符串转换为Int
        /// </summary>
        /// <param name="me"></param>
        /// <param name="defaultValue">转换失败时返回的默认值</param>
        /// <returns>转换成功，返回对应的整型数值；转换失败，则返回defaultValue。</returns>
        public static int ToIntExt(this string me, int defaultValue)
        {
            if (string.IsNullOrEmpty(me))
                return defaultValue;

            float result = 0f;
            if (float.TryParse(me, out result))
                return (int)result;

            return defaultValue;
        }

        #endregion

        #region 扩展·ToIntOrDefaultExt

        /// <summary>
        /// 扩展·字符串转换为Int可空类型
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static int? ToIntOrDefaultExt(this string me)
        {
            if (string.IsNullOrEmpty(me))
                return null;

            float result = 0f;
            if (float.TryParse(me, out result))
                return (int)result;

            return null;
        }

        #endregion
    }
}
