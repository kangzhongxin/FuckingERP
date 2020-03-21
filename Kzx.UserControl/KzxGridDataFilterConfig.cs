using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FuckingERP;

namespace Kzx.UserControl
{
    /// <summary>
    /// 表格数据过滤配置
    /// </summary>
    public class KzxGridDataFilterConfig
    {
        #region 字段

        private static readonly string _filePath = string.Empty;
        private static readonly string _configKey = "Filter";

        private readonly string _section = string.Empty;
        private List<KzxGridDataFilterItem> _items = new List<KzxGridDataFilterItem>();

        #endregion

        #region 构造&初始化

        static KzxGridDataFilterConfig()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Guid", "bFilter.ini");
        }

        public KzxGridDataFilterConfig(string section)
        {
            if (string.IsNullOrWhiteSpace(section))
                throw new Exception("表格数据过滤配置实始化失败，窗体KEY不能为空。");

            _section = section;
            Read();
        }

        #endregion

        #region 属性

        public List<KzxGridDataFilterItem> Items
        {
            get { return _items; }
        }

        #endregion

        #region 设置

        public void Set(KzxGridDataFilterItem item)
        {
            if (item == null) return;

            if (string.IsNullOrWhiteSpace(item.FieldName))
                throw new Exception("字段名称不能为空。");

            if (string.IsNullOrWhiteSpace(item.FieldDesc))
                throw new Exception("字段描述不能为空。");

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

        #region 保存

        /// <summary>
        /// 保存到配置文件
        /// </summary>
        public void Save()
        {
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
            var iniFile = new IniFile(_filePath);

            iniFile.IniWriteValue(_section, _configKey, configValue);
        }

        #endregion

        #region 读取

        /// <summary>
        /// 读取配置
        /// </summary>
        public void Read()
        {
            var iniFile = new IniFile(_filePath);
            var configValue = iniFile.IniReadValue(_section, _configKey);
            var items = KzxGridDataFilterItem.ToFilterItems(configValue);

            _items.Clear();
            _items.AddRange(items);
        }

        #endregion
    }
}
