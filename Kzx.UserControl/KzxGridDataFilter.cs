using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data; 

namespace Kzx.UserControl
{
    /// <summary>
    /// 表格数据过滤配置
    /// </summary>
    public class KzxGridDataFilter
    {
        #region 字段

        private object _filterConfigLoadLock = new object();
        private bool _isFilterConfigLoaded = false;

        private string _mainTableName = string.Empty;
        private string _formName = string.Empty;
        private string _localConfigSection = string.Empty;
        private List<KzxGridDataFilterItem> _filterConfigItems = new List<KzxGridDataFilterItem>();

        #endregion

        #region 构造&初始化

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMainTableName"></param>
        /// <param name="pFormName"></param>
        /// <param name="pLocalConfigSection"></param>
        public KzxGridDataFilter(string pMainTableName, string pFormName, string pLocalConfigSection)
        {
            _mainTableName = pMainTableName;
            _formName = pFormName;
            _localConfigSection = pLocalConfigSection;
        }

        #endregion

        #region 获取

        /// <summary>
        /// 获取数据过滤配置项
        /// </summary>
        /// <returns></returns>
        public List<KzxGridDataFilterItem> GetFilterConfigItems()
        {
            lock (_filterConfigLoadLock)
            {
                if (!_isFilterConfigLoaded)
                {
                    ReloadConfig();
                    _isFilterConfigLoaded = true;
                }

                return _filterConfigItems;
            }
        }

        #endregion

        #region 加载

        private void ReloadConfig()
        {
            _filterConfigItems.Clear();

            //* ------------------------------- *
            //* 先读取本地个性配置
            //* ------------------------------- *
            //如果存在本地配置则以本主配置为准
            var filterConfig = new KzxGridDataFilterLocalConfig(_localConfigSection);

            //解决清除网格状态设置，不加载模块管理配置Bug
            //if (filterConfig.IsClear)
            //    return;

            if (filterConfig.Items != null && filterConfig.Items.Count != 0)
            {
                _filterConfigItems.AddRange(filterConfig.Items);
                return;
            }
             
        }

        #endregion

        #region 保存配置·本地

        /// <summary>
        /// 保存配置到本地
        /// </summary>
        public void SaveConfigToLocal(List<KzxGridDataFilterItem> pItems)
        {
            var filterConfig = new KzxGridDataFilterLocalConfig(_localConfigSection);
            filterConfig.Reset(pItems);
            filterConfig.Save();
        }

        #endregion

      
    }
}
