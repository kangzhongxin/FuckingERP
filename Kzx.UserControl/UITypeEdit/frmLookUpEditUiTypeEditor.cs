using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Utils.Design;
using DevExpress.XtraEditors.Controls;

namespace Kzx.UserControl.UITypeEdit
{
    public partial class frmLookUpEditUiTypeEditor : Form
    {
        private object _Instance = null;
        //private KzxLookUpEdit _KzxLookUpEdit;
        private System.ComponentModel.ITypeDescriptorContext _context;
        private string _Xml = string.Empty;
        private List<KzxLookUpColumnInfo> _Columns = new List<KzxLookUpColumnInfo>();


        public string Xml
        {
            get
            {
                StringBuilder xmlsb = new StringBuilder();
                xmlsb.Append("<columns> ");
                for (int i = 0; i < this._Columns.Count; i++)
                {
                    xmlsb.Append(KzxLookUpEdit.WriteObject(this._Columns[i]));
                }
                xmlsb.Append("</columns>");
                return xmlsb.ToString();
            }

        }

        public frmLookUpEditUiTypeEditor(System.ComponentModel.ITypeDescriptorContext context, object e)
        {
            InitializeComponent();
            if (e != null)
            {
                this._Xml = e.ToString();
            }

            this._context = context;
            this._Instance = context.Instance;
            //this._KzxLookUpEdit = (KzxLookUpEdit)context.Instance;
        }



        private void frmLookUpEditUiTypeEditor_Load(object sender, EventArgs e)
        {
            XmlNode node = null;
            XmlDocument doc = new XmlDocument();
            KzxLookUpColumnInfo info = null;
            if (string.IsNullOrWhiteSpace(this._Xml) == true)
            {
                return;
            }
            doc.LoadXml(this._Xml);
            for (int i = 0; i < doc.DocumentElement.ChildNodes.Count; i++)
            {
                node = doc.DocumentElement.ChildNodes[i];
                info = KzxLookUpEdit.ReadObject(node);
                this._Columns.Add(info);
            }
            doc = null;
            for (int i = 0; i < this._Columns.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Text = this._Columns[i].Caption;
                item.Tag = this._Columns[i];
                this.listView1.Items.Add(item);
            }
            if (this.listView1.Items.Count > 0)
            {
                this.listView1.Items[0].Selected = true;
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListViewItem item = e.Item;
            if (e.Item.Selected == false)
            {
                return;
            }
            for (int i = 0; i < this._Columns.Count; i++)
            {
                if (this._Columns[i].Caption.Equals(item.Text) == true)
                {
                    this.label2.Text = item.Text + " 属性";
                    this.mcPropertyGrid1.SelectedObject = this._Columns[i];
                }
            }
        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            KzxLookUpColumnInfo page = new KzxLookUpColumnInfo();
            ListViewItem item = new ListViewItem();
            page.Caption = KzxLookUpEdit.CreateName(this._Columns, "Column");
            page.Visible = true;
            page.Width = 50;
            this._Columns.Add(page);
            item.Tag = page;
            item.Text = page.Caption;
            this.listView1.Items.Add(item);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            KzxLookUpColumnInfo page = new KzxLookUpColumnInfo();
            if (this.listView1.SelectedItems.Count <= 0)
            {
                return;
            }
            ListViewItem item = this.listView1.SelectedItems[0];
            page = item.Tag as KzxLookUpColumnInfo;
            this._Columns.Remove(item.Tag as KzxLookUpColumnInfo);
            this.listView1.Items.Remove(item);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            KzxLookUpColumnInfo item;

            for (int i = 0; i < this.listView1.SelectedItems.Count; i++)
            {
                System.Windows.Forms.ListViewItem listViewItem = this.listView1.SelectedItems[i];
                int index = this.listView1.SelectedItems[i].Index - 1;
                if (index < 0) return;
                item = this.listView1.SelectedItems[i].Tag as KzxLookUpColumnInfo;
                this._Columns.Remove(item);
                this._Columns.Insert(index, item);
                this.listView1.Items.Remove(this.listView1.SelectedItems[i]);
                this.listView1.Items.Insert(index, listViewItem);
                listViewItem.Selected = true;
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            KzxLookUpColumnInfo item;

            for (int i = this.listView1.SelectedItems.Count - 1; i > -1; i--)
            {
                System.Windows.Forms.ListViewItem listViewItem = this.listView1.SelectedItems[i];
                int index = this.listView1.SelectedItems[i].Index + 1;
                if (index > this.listView1.Items.Count - 1) return;
                item = this.listView1.SelectedItems[i].Tag as KzxLookUpColumnInfo;
                this._Columns.Remove(item);
                this._Columns.Insert(index, item);
                this._Columns.Insert(index, item);
                this.listView1.Items.Remove(this.listView1.SelectedItems[i]);
                this.listView1.Items.Insert(index, listViewItem);
                listViewItem.Selected = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mcPropertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label.Equals("caption", StringComparison.OrdinalIgnoreCase) == true)
            {
                for (int i = 0; i < this.listView1.Items.Count; i++)
                {
                    System.Windows.Forms.ListViewItem listViewItem = this.listView1.Items[i];
                    if (listViewItem.Text.Equals(e.OldValue.ToString(), StringComparison.OrdinalIgnoreCase) == true)
                    {
                        listViewItem.Text = e.ChangedItem.Value.ToString();
                    }
                }
            }
        }
    }
}
