using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Kzx.UserControl.UITypeEdit
{
    public partial class frmKzxChartControlXColumnsUiTypeEdit : Form
    {
        private DataTable dt = null;
        private PropertyInfo pi = null;
        private System.ComponentModel.ITypeDescriptorContext _context;
        private string _XColumns = string.Empty;
        private DataTable sourceDt = new DataTable("source");
        private Dictionary<string, string> _captoinDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public string XColumns
        {
            get
            {
                this._XColumns = string.Empty;
                DataRow[] rows;
                this.dgGrid.EndEdit();
                rows = this.sourceDt.Select("isselected=1");
                for (int i = 0; i < rows.Length; i++)
                {
                    this._XColumns += rows[i]["column"].ToString() + ",";
                }
                return this._XColumns;
            }
        }

        public frmKzxChartControlXColumnsUiTypeEdit(System.ComponentModel.ITypeDescriptorContext context, object e)
        {
            object v=null;
            InitializeComponent();
            if (e != null)
            {
                this._XColumns = e.ToString();
            }
            MethodInfo mi = context.Instance.GetType().GetMethod("GetColumnCaptionDictionary");
            if (mi != null)
            {
                v = mi.Invoke(context.Instance, null);
                if (v != null)
                {
                    this._captoinDictionary = (Dictionary<string, string>)v;
                }
            }
            pi = context.Instance.GetType().GetProperty("KzxDataSource");
            if (pi != null)
            {
                dt = pi.GetValue(context.Instance, null) as DataTable;
                if (dt != null)
                {
                    InitDataSource(dt,this._XColumns);
                }
            }

        }

        private void InitDataSource(DataTable dt,string s)
        {
            string[] array = s.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            DataRow row = null;
            sourceDt.Columns.Add("column", typeof(string));
            sourceDt.Columns.Add("columncaption", typeof(string));
            sourceDt.Columns.Add("isselected", typeof(int));
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                row = sourceDt.NewRow();
                row["column"] = dt.Columns[i].ColumnName;
                if (array.Contains(dt.Columns[i].ColumnName) == true)
                {
                    row["isselected"] = 1;
                }
                else
                {
                    row["isselected"] = 0;
                }
                if (this._captoinDictionary.ContainsKey(dt.Columns[i].ColumnName) == true)
                {
                    row["columncaption"] = this._captoinDictionary[dt.Columns[i].ColumnName];
                }
                else
                {
                    row["columncaption"] = dt.Columns[i].ColumnName;
                }
                sourceDt.Rows.Add(row);
            }
        }

        private void frmKzxChartControlXColumnsUiTypeEdit_Load(object sender, EventArgs e)
        {
            this.dgGrid.DataSource = sourceDt;
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            DataRowView rowview = null;
            for (int i = 0; i < this.dgGrid.Rows.Count; i++)
            {
                rowview = this.dgGrid.Rows[i].DataBoundItem as DataRowView;
                if (rowview != null)
                {
                    rowview["isselected"] = 0;
                }
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            DataRowView rowview = null;
            for (int i = 0; i < this.dgGrid.Rows.Count; i++)
            {
                rowview = this.dgGrid.Rows[i].DataBoundItem as DataRowView;
                if (rowview != null)
                {
                    rowview["isselected"] = 1;
                }
            }
        }
    }
}
