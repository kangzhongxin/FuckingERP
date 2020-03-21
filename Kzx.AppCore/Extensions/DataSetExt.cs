/****************************************************************
 * Copyright (c) 2006-2XXX 上海印智互联信息技术有限公司
 * 产品名称：印智ERP
 * 描    述：数据表集合操作扩展类
 * 负 责 人：chen.q
 * 创 建 人：chen.q
 * 创建日期：2019-1-11
 * 曾经负责人：
 *---------------------------------------------------------------
 * 【修改记录】
 * {修改时间}：BY {修改人}，{修改描述}
 ****************************************************************/

using System.Data;

namespace Kzx.AppCore
{
    /// <summary>
    /// 数据表集合操作扩展类
    /// </summary>
    public static class DataSetExt
    {
        #region 扩展·获取数据表

        /// <summary>
        /// 扩展·获取数据表
        /// </summary>
        /// <param name="pThis"></param>
        /// <param name="pTableName"></param>
        /// <returns></returns>
        public static DataTable GetDataTableExt(this DataSet pThis, string pTableName)
        {
            if (pThis == null || string.IsNullOrWhiteSpace(pTableName))
                return null;

            if (pThis.Tables.Contains(pTableName))
                return pThis.Tables[pTableName];

            return null;
        }

        #endregion


    }
}
