/****************************************************************
 * Copyright (c) 2006-2XXX 上海印智互联信息技术有限公司
 * 产品名称：印智ERP
 * 描    述：数据行扩展操作
 * 负 责 人：chen.q
 * 创 建 人：chen.q
 * 创建日期：2019-1-11
 * 曾经负责人：
 *---------------------------------------------------------------
 * 【修改记录】
 * {修改时间}：BY {修改人}，{修改描述}
 ****************************************************************/

using System;
using System.Data;

namespace Kzx.AppCore
{
    /// <summary>
    /// 扩展·数据行操作
    /// </summary>
    public static class DataRowExt
    {
        #region 扩展·获取字段值·GetStringExt

        /// <summary>
        /// 扩展·获取字段值·字符串
        /// </summary>
        /// <param name="pThis"></param>
        /// <param name="pFileName"></param>
        /// <returns></returns>
        public static string GetStringExt(this DataRow pThis, string pFileName)
        {
            if (pThis == null)
                return string.Empty;

            return pThis[pFileName].ToString();
        }

        #endregion

        #region 扩展·获取字段值·GetBoolExt

        /// <summary>
        /// 扩展·获取字段值·布尔
        /// </summary>
        /// <param name="pThis"></param>
        /// <param name="pFileName"></param>
        /// <returns></returns>
        public static bool GetBoolExt(this DataRow pThis, string pFileName)
        {
            if (pThis == null)
                return false;

            var value = pThis[pFileName];
            if (value == null) return false;

            if (value is bool)
                return Convert.ToBoolean(value);
            else
                return string.Equals(value.ToString(), "True", StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region 扩展·获取字段值·GetIntExt

        /// <summary>
        /// 扩展·获取字段值·整型
        /// </summary>
        /// <param name="pThis"></param>
        /// <param name="pFileName"></param>
        /// <returns></returns>
        public static int GetIntExt(this DataRow pThis, string pFileName)
        {
            if (pThis == null)
                return 0;

            return pThis[pFileName].ToString().ToIntExt();
        }

        #endregion

        #region 扩展·获取字段值·GetDecimalExt

        /// <summary>
        /// 扩展·获取字段值·浮点
        /// </summary>
        /// <param name="pThis"></param>
        /// <param name="pFileName"></param>
        /// <returns></returns>
        public static decimal GetDecimalExt(this DataRow pThis, string pFileName)
        {
            if (pThis == null)
                return 0M;

            return pThis[pFileName].ToString().ToDecimalExt();
        }

        #endregion

        #region 扩展·获取字段值·GetDateTimeExt

        /// <summary>
        /// 扩展·获取字段值·日期时间
        /// </summary>
        /// <param name="pThis"></param>
        /// <param name="pFileName"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeExt(this DataRow pThis, string pFileName)
        {
            if (pThis == null)
                return DateTime.MinValue;

            var value = pThis[pFileName];
            if (value == null) return DateTime.MinValue;

            if (value is DateTime)
                return Convert.ToDateTime(value);
            else
                return value.ToString().ToDateTimeExt();
        }

        #endregion

        #region 扩展·获取字段值·GetGuidExt

        /// <summary>
        /// 扩展·获取字段值·Guid
        /// </summary>
        /// <param name="pThis"></param>
        /// <param name="pFileName"></param>
        /// <returns></returns>
        public static Guid GetGuidExt(this DataRow pThis, string pFileName)
        {
            if (pThis == null)
                return Guid.Empty;

            var value = pThis[pFileName];

            return value.ToString().ToGuidExt();
        }

        /// <summary>
        /// 扩展·获取字段值·Guid
        /// </summary>
        /// <param name="pThis"></param>
        /// <param name="pFileName"></param>
        /// <returns></returns>
        public static Guid? GetGuidOrDefaultExt(this DataRow pThis, string pFileName)
        {
            if (pThis == null)
                return null;

            var value = pThis[pFileName];

            return value.ToString().ToGuidOrDefaultExt();
        }

        #endregion
    }
}
