using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kzx.UserControl.UITypeEdit
{
    public partial class frmGridControlColumnsUiTypeEditor : Form
    {
        private object _Instance = null;
        private System.ComponentModel.ITypeDescriptorContext _context;
        private string _Xml = string.Empty;
        private DataTable _Columns = new DataTable("Columns");

        public string Xml
        {
            get
            {
                this.dataGridView1.EndEdit();
                this.bindingSource1.EndEdit();
                this._Columns.AcceptChanges();
                return KzxGridControl.DeserializeColumns(this._Columns);
            }
        }

        public frmGridControlColumnsUiTypeEditor(System.ComponentModel.ITypeDescriptorContext context, object e)
        {
            InitializeComponent();
            if (e != null)
            {
                this._Xml = e.ToString();
            }

            DataTable t = new DataTable();
            t.Columns.Add("d", typeof(string));
            t.Columns.Add("s", typeof(string));
            t.Rows.Add(new object[] { "无", "None" });
            t.Rows.Add(new object[] { "左", "Left" });
            t.Rows.Add(new object[] { "右 ", "Right" });
            t.AcceptChanges();
            this.Column5.DisplayMember = "d";
            this.Column5.ValueMember = "s";
            this.Column5.DataSource = t;

            this.dataGridView1.AutoGenerateColumns = false;
            this._context = context;
            this._Instance = context.Instance;
            this._Columns = KzxGridControl.SerializeColumns(this._Xml);
            this.bindingSource1.DataSource = this._Columns;
            this.bindingSource1.Sort = "ColIndex ASC";
            this.dataGridView1.DataSource = this.bindingSource1;
        }

        private void frmGridControlColumnsUiTypeEditor_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.bindingSource1.AddNew();
            this.bindingSource1.EndEdit();
            this.bindingSource1.MoveLast();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (this.bindingSource1.Current != null)
            {
                this.bindingSource1.Remove(this.bindingSource1.Current);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bindingSource1_PositionChanged(object sender, EventArgs e)
        {
            if (this.bindingSource1.Position >= 0)
            {
                DataRowView row = this.bindingSource1.Current as DataRowView;
                XmlRow xmlrow = XmlRow.Converter(row);

                this.propertyGrid1.SelectedObject = xmlrow;
            }
            else
            {
                this.propertyGrid1.SelectedObject = null;
            }
        }

        private void bindingSource1_AddingNew(object sender, AddingNewEventArgs e)
        {
            int columnindex = 0;
            int colindex = 0;
            DataView view = this.bindingSource1.List as DataView;
            DataRowView rowview = view.AddNew();
            colindex = 1 + (view.Table.Compute("max(ColIndex)", string.Empty) == DBNull.Value ? 0 : Convert.ToInt32(view.Table.Compute("max(ColIndex)", string.Empty)));

            if (this._Instance is KzxGridControl)
            {
                columnindex = (KzxGridControl.ColumnCount++);
            }
            else if (this._Instance is KzxTreeList)
            {
                columnindex = (KzxTreeList.ColumnCount++);
            }
            rowview["ColIndex"] = colindex;
            rowview["Key"] = "column"+view.Count.ToString();
            rowview["Field"] =string.Empty;
            rowview["MessageCode"] = "0";
            rowview["DesigeCaption"] = "显示标题" + columnindex.ToString();
            rowview["ColumnType"] = ColumnType.Text.ToString();
            rowview["SourceTable"] = string.Empty;
            rowview["DisplayMemberPath"] = string.Empty;
            rowview["SelectedValuePath"] = string.Empty;
            rowview["MaxLength"] = 0;
            rowview["PasswordChar"] = string.Empty;
            rowview["ReadOnly"] = false;
            rowview["IsNull"] = true;
            rowview["IsUnique"] = false;
            rowview["Enabled"] = true;
            rowview["Visible"] = true;
            rowview["ValidateGroup"] = string.Empty;
            rowview["FieldCaption"] = string.Empty;
            rowview["Fixed"] = DevExpress.XtraGrid.Columns.FixedStyle.None.ToString();
            rowview["ParentField"] = string.Empty;
            rowview["ValueDependencyField"] = string.Empty;
            rowview["Width"] = 50;
            rowview["AllowValueRange"] = false;
            rowview["DataType"] = KzxDataType.Str.ToString();
            rowview["MaxValue"] = string.Empty;
            rowview["MinValue"] = string.Empty;
            rowview["RegexString"] = string.Empty;
            rowview["ToolTipMessageCode"] = string.Empty;
            rowview["ToolTipText"] = string.Empty;

            e.NewObject = rowview;
            XmlRow xmlrow = XmlRow.Converter(rowview);
            this.propertyGrid1.SelectedObject = xmlrow;
        }

        private void bindingSource1_ListChanged(object sender, ListChangedEventArgs e)
        {
            DataRowView rowview=null;
            if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                for (int i = 0; i < this.bindingSource1.Count; i++)
                {
                    rowview = this.bindingSource1[i] as DataRowView;
                    if (rowview != null)
                    {
                        rowview["ColIndex"] = i + 1;
                    }
                }
                if (this.bindingSource1.Current != null)
                {
                    rowview = this.bindingSource1.Current as DataRowView;
                    XmlRow xmlrow = XmlRow.Converter(rowview);

                    this.propertyGrid1.SelectedObject = xmlrow;
                }
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            DataRowView rowview = null;
            if (this.bindingSource1.Current != null)
            {
                rowview = this.bindingSource1.Current as DataRowView;
                rowview[e.ChangedItem.PropertyDescriptor.Name] = e.ChangedItem.Value;
                if (e.ChangedItem.PropertyDescriptor.Name.Equals("Field", StringComparison.OrdinalIgnoreCase) == true)
                {
                    DataRow[] rows = rowview.DataView.Table.Select("Field='" + (e.ChangedItem.Value == null ? string.Empty : e.ChangedItem.Value.ToString()) + "'", string.Empty, DataViewRowState.CurrentRows);
                    if (rows.Length > 1)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < rows.Length; i++)
                        {
                            sb.Append(rows[i]["DesigeCaption"].ToString() + "  \n");
                        }
                        MessageBox.Show("列名 [" + (e.ChangedItem.Value == null ? string.Empty : e.ChangedItem.Value.ToString()) + "] 不能重复!重复的列:" + "\n" + sb.ToString(), "提示");

                    }
                }
                this.bindingSource1.EndEdit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //上移
            DataRowView rowview = null;
            DataRowView prerowview = null;
            if (this.bindingSource1.Position > 0)
            {
                rowview=this.bindingSource1[this.bindingSource1.Position] as DataRowView;
                prerowview = this.bindingSource1[this.bindingSource1.Position - 1] as DataRowView;
                int index = Convert.ToInt32(rowview["ColIndex"]);
                rowview["ColIndex"] = prerowview["ColIndex"];
                prerowview["ColIndex"] = index;
                rowview.EndEdit();
                prerowview.EndEdit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //下移
            DataRowView rowview = null;
            DataRowView prerowview = null;
            if (this.bindingSource1.Position < this.bindingSource1.Count - 1)
            {
                prerowview = this.bindingSource1[this.bindingSource1.Position] as DataRowView;
                rowview = this.bindingSource1[this.bindingSource1.Position + 1] as DataRowView;
                int index = Convert.ToInt32(rowview["ColIndex"]);
                rowview["ColIndex"] = prerowview["ColIndex"];
                prerowview["ColIndex"] = index;
                rowview.EndEdit();
                prerowview.EndEdit();
            }
        }

        private void frmGridControlColumnsUiTypeEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataRow row = null;
            for (int i = 0; i < this._Columns.Rows.Count; i++)
            {
                row = this._Columns.Rows[i];
                if (row["Field"] == DBNull.Value || string.IsNullOrWhiteSpace(row["Field"].ToString()) == true)
                {
                    e.Cancel = true;
                    MessageBox.Show("列名 [" + row["DesigeCaption"].ToString() + "] 的数据库字段名(Field)没有值,请设置", "提示");
                    return;
                }
                DataRow[] rows = this._Columns.Select("Field='" + row["Field"].ToString() + "'", string.Empty, DataViewRowState.CurrentRows);
                if (rows.Length > 1)
                {
                    for (int h = 0; h < rows.Length; h++)
                    {
                        e.Cancel = true;
                        MessageBox.Show("列名 [" + row["DesigeCaption"].ToString() + "] 的数据库字段名(Field)有重复设置,重复列：" + "\n" + row["DesigeCaption"].ToString() + "\n", "提示");
                    }
                    return;
                }
            }
        }
    }
}
