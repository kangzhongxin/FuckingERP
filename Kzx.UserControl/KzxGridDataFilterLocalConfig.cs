/****************************************************************
 * Copyright (c) 2006-200X 上海印智互联信息技术有限公司
 * 产品名称：印智ERP硬包
 * 描    述：表格数据过滤本地配置
 * 负 责 人：chen.q
 * 创 建 人：chen.q
 * 创建日期：2018-8-7
 * 曾经负责人：
 *---------------------------------------------------------------
 * 【修改记录】
 * {修改时间}：BY {修改人}，{修改描述}
 ****************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Kzx.AppCore;

namespace Kzx.UserControl
{
    /// <summary>
    /// 表格数据过滤本地配置
    /// </summary>
    public class KzxGridDataFilterLocalConfig
    {
        #region 字段

        private static readonly string _filePath = string.Empty;
        private static readonly string _filterConfigKey = "Filter";
        private static readonly string _clearConfigKey = "ClearStatus";

        private readonly string _section = string.Empty;
        private List<KzxGridDataFilterItem> _items = new List<KzxGridDataFilterItem>();
        private bool _isClear = false;

        #endregion

        #region 构造&初始化

        static KzxGridDataFilterLocalConfig()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Guid", "bFilter.ini");
        }

        public KzxGridDataFilterLocalConfig(string section)
        {
            if (string.IsNullOrWhiteSpace(section))
                throw new Exception("表格数据过滤配置实始化失败，窗体KEY不能为空。");

            _section = section;
            Read();
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取 是否已清除网格设置
        /// </summary>
        public bool IsClear
        {
            get { return _isClear; }
        }

        /// <summary>
        /// 获取本地数据过滤配置项
        /// </summary>
        public List<KzxGridDataFilterItem> Items
        {
            get { return _items; }
        }

        #endregion

        #region 设置

        /// <summary>
        /// 重新设置本地数据过滤配置项
        /// </summary>
        /// <param name="items"></param>
        public void Reset(IEnumerable<KzxGridDataFilterItem> items)
        {
            _isClear = false;
            _items.Clear();
            Set(items);
        }

        /// <summary>
        /// 设置本地数据过滤配置项
        /// </summary>
        /// <param name="items"></param>
        public void Set(IEnumerable<KzxGridDataFilterItem> items)
        {
            if (items == null)
                return;

            foreach (var item in items)
            {
                Set(item);
            }
        }

        /// <summary>
        /// 设置本地数据过滤配置项
        /// </summary>
        /// <param name="item"></param>
        public void Set(KzxGridDataFilterItem item)
        {
            if (item == null) return;

            if (string.IsNullOrWhiteSpace(item.FieldName))
                throw new Exception("字段名称不能为空。");

            //if (string.IsNullOrWhiteSpace(item.FieldDesc))
            //    throw new Exception("字段描述不能为空。");

            _isClear = false;

            var configItem = _items.Find(m => m.FieldName == item.FieldName);
            if (configItem == null)
            {
                _items.Add(item);
            }
            else
            {
                configItem.DataSetParentField = item.DataSetParentField;
                configItem.IsDatabaseFilter = item.IsDatabaseFilter;
                configItem.IsDataSetFilter = item.IsDataSetFilter;
            }
        }

        #endregion

        #region 设置·清除网格状态设置

        /// <summary>
        /// 设置·清除网格状态设置
        /// </summary>
        public void SetOfClearGridStatus()
        {
            _isClear = true;
            _items.Clear();
        }

        #endregion

        #region 保存

        /// <summary>
        /// 保存到配置文件
        /// </summary>
        public void Save()
        {
            if(string.IsNullOrWhiteSpace(_section))
                return;

            var saveItems = new List<KzxGridDataFilterItem>();
            foreach (var item in _items)
            {
                if (item.IsDataSetFilter
                    || item.IsDatabaseFilter
                    || !string.IsNullOrWhiteSpace(item.DataSetParentField))
                {
                    saveItems.Add(item);
                }
            }

            var configValue = KzxGridDataFilterItem.ToConfigValue(saveItems);
            var iniFile = new IniFileCore(_filePath);

            iniFile.Write(_section, _clearConfigKey, _isClear.ToString());
            iniFile.Write(_section, _filterConfigKey, configValue);
        }

        #endregion

        #region 读取

        /// <summary>
        /// 读取配置
        /// </summary>
        public void Read()
        {
            var iniFile = new IniFileCore(_filePath);
            var clearValue = iniFile.Read(_section, _clearConfigKey);
            var filterValue = iniFile.Read(_section, _filterConfigKey);
            var items = KzxGridDataFilterItem.ToFilterItems(filterValue);

            _items.Clear();
            _items.AddRange(items);

            _isClear = string.Equals("True", clearValue);
        }

        #endregion
    }
}
