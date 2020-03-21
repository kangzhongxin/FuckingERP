using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kzx.AppCore
{
    /// <summary>
    /// GUID操作扩展类
    /// </summary>
    public static class GuidExt
    {
        #region 扩展·转换·ToGuidExt

        /// <summary>
        /// 扩展·字符串转为GUID
        /// </summary>
        /// <param name="pThis"></param>
        /// <returns></returns>
        public static Guid ToGuidExt(this string pThis)
        {
            if (string.IsNullOrEmpty(pThis))
                return Guid.Empty;

            Guid result;
            if (Guid.TryParse(pThis, out result))
                return result;

            return Guid.Empty;
        }

        /// <summary>
        /// 扩展·字符串转为GUID
        /// </summary>
        /// <param name="pThis"></param>
        /// <returns></returns>
        public static Guid? ToGuidOrDefaultExt(this string pThis)
        {
            if (string.IsNullOrEmpty(pThis))
                return null;

            Guid result;
            if (Guid.TryParse(pThis, out result))
                return result;

            return null;
        }

        #endregion
    }
}
