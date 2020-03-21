/****************************************************************
 * Copyright (c) 2006-2XXX 上海印智互联信息技术有限公司
 * 产品名称：印智ERP
 * 描    述：扩展数据表操作
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
    /// 扩展数据表操作
    /// </summary>
    public static class DataTableExt
    {
        #region 扩展·GetRowCountExt

        /// <summary>
        /// 扩展·获取数据表记录总数
        /// </summary>
        /// <param name="pThis"></param>
        /// <returns></returns>
        public static int GetRowCountExt(this DataTable pThis)
        {
            if (pThis == null)
                return 0;

            return pThis.Rows.Count;
        }

        #endregion
    }
}
