using KzxUserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kzx.UserControls.UITypeEdit
{
    public partial class frmDropDownButtonItemUiTypeEdit : Form
    {
        private object _Instance = null;
        private System.ComponentModel.ITypeDescriptorContext _context;
        private string _Xml = string.Empty;
        private DataTable _Columns = new DataTable("items");

        public frmDropDownButtonItemUiTypeEdit(System.ComponentModel.ITypeDescriptorContext context, object e)
        {
            InitializeComponent();

            if (e != null)
            {
                this._Xml = e.ToString();
            }

            this.dataGridView1.AutoGenerateColumns = false;
            this._context = context;
            this._Instance = context.Instance;
            this._Columns = KzxDropDownButton.SerializeItems(this._Xml);

            this.bindingSource1.DataSource = this._Columns;
            this.bindingSource1.Sort = "ColIndex ASC";
            this.dataGridView1.DataSource = this.bindingSource1;

        }

        private void frmDropDownButtonItemUiTypeEdit_Load(object sender, EventArgs e)
        {

        }

        public string Xml
        {
            get
            {
                this.dataGridView1.EndEdit();
                this.bindingSource1.EndEdit();
                this._Columns.AcceptChanges();
                return KzxDropDownButton.DeserializeItems (this._Columns);
            }
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
                KzxDropDownButtonItem xmlrow = KzxDropDownButtonItem.Converter(row);

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

            rowview["Key"] = "button" + view.Count.ToString();
            rowview["MessageCode"] = "0";
            rowview["DesigeCaption"] = "显示标题";
            rowview["DllName"] = string.Empty;
            e.NewObject = rowview;
            KzxDropDownButtonItem xmlrow = KzxDropDownButtonItem.Converter(rowview);
            this.propertyGrid1.SelectedObject = xmlrow;
        }

        private void bindingSource1_ListChanged(object sender, ListChangedEventArgs e)
        {
            DataRowView rowview=null;
            if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                if (this.bindingSource1.Current != null)
                {
                    rowview = this.bindingSource1.Current as DataRowView;
                    KzxDropDownButtonItem xmlrow = KzxDropDownButtonItem.Converter(rowview);

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
    }
}
