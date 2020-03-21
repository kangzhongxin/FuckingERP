using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Kzx.UserControl.Extensions
{
    public static class GridViewExt
    {
        /// <summary> 扩展·设置指定行、单元格数值，以多个 DataRowView 对象引用为准，不受排序不全影响 </summary>
        public static void SetRowsCellValueExt(this GridView grid, IList<DataRowView> rows, string column, object value)
        {
            foreach (var rowView in rows) grid.SetRowCellValueExt(rowView, column, value);
        }

        /// <summary> 扩展·设置指定行、单元格数值，以单个 DataRowView 对象引用为准 </summary>
        public static void SetRowCellValueExt(this GridView grid, DataRowView rowView, string column, object value)
        {
            rowView.Row[column] = value;
            rowView.Row.Table.AcceptChanges();
        }

        /// <summary> 扩展·设置指定行、单元格数值，对全部行，不受排序不全影响 </summary>
        public static void SetAllRowCellValueExt(this GridView grid, string column, object value)
        {
            var array = Enumerable.Range(0, grid.RowCount).Select(grid.GetRow).Where(p => p is DataRowView).Cast<DataRowView>().ToArray();
            grid.SetRowsCellValueExt(array, column, value);
        }
    }
}
