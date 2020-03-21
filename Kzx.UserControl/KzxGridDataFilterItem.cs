using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kzx.UserControl
{
    /// <summary>
    /// 数据表格过滤项
    /// </summary>
    [Serializable]
    public class KzxGridDataFilterItem
    {
        /// <summary>
        /// 过滤字段名称
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 过滤字段描述
        /// </summary>
        public string FieldDesc { get; set; }

        /// <summary>
        /// 过滤父级字段名称
        /// </summary>
        public string DataSetParentField { get; set; }

        /// <summary>
        /// 是否在本地数据集过滤
        /// </summary>
        public bool IsDataSetFilter { get; set; }

        /// <summary>
        /// 是否为数据库查询过滤
        /// </summary>
        public bool IsDatabaseFilter { get; set; }

        public string[] ToStringArray()
        {
            return new string[] 
            { 
                this.FieldName,
                this.FieldDesc,
                this.IsDataSetFilter.ToString(),
                this.DataSetParentField,
                this.IsDatabaseFilter.ToString(),
            };
        }

        /// <summary>
        /// 转换为配置值
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string ToConfigValue(IEnumerable<KzxGridDataFilterItem> items)
        {
            if (items == null) return string.Empty;

            var value = new StringBuilder();
            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.FieldName))
                    throw new Exception("转换为数据表过滤配置值失败，字段名称不能为空。");

                //if (string.IsNullOrWhiteSpace(item.FieldDesc))
                //    throw new Exception("转换为数据表过滤配置值失败，字段描述不能为空。");

                value.AppendFormat("{0}|{1}|{2}|{3}|{4},", item.FieldName, item.FieldDesc, item.IsDataSetFilter, item.DataSetParentField, item.IsDatabaseFilter);
            }

            return value.ToString();
        }

        /// <summary>
        /// 转换为过滤项信息
        /// </summary>
        /// <param name="configValue"></param>
        /// <returns></returns>
        public static List<KzxGridDataFilterItem> ToFilterItems(string configValue)
        {
            if (string.IsNullOrWhiteSpace(configValue))
                return new List<KzxGridDataFilterItem>();

            var items = new List<KzxGridDataFilterItem>();
            var filterArr = configValue.Split(',');

            foreach (var filter in filterArr)
            {
                if (string.IsNullOrWhiteSpace(filter))
                    continue;

                var info = filter.Split('|');
                if (info.Length < 4) continue;

                var item = new KzxGridDataFilterItem();
                item.FieldName = info[0].Trim();
                item.FieldDesc = info[1].Trim();
                item.IsDataSetFilter = string.Equals(info[2].Trim(), "True", StringComparison.OrdinalIgnoreCase);
                item.DataSetParentField = info[3].Trim();

                if (info.Length > 4)
                    item.IsDatabaseFilter = string.Equals(info[4].Trim(), "True", StringComparison.OrdinalIgnoreCase);

                if (!string.IsNullOrWhiteSpace(item.FieldName)
                    && !string.IsNullOrWhiteSpace(item.FieldDesc))
                {
                    var addedItem = items.Find(m => m.FieldName == item.FieldName);
                    if (addedItem == null)
                    {
                        items.Add(item);
                    }
                    else
                    {
                        addedItem.DataSetParentField = item.DataSetParentField;
                        addedItem.IsDatabaseFilter = item.IsDatabaseFilter;
                        addedItem.IsDataSetFilter = item.IsDataSetFilter;
                    }
                }
            }

            return items;
        }
    }
}
