using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraTab;
using Kzx.UserControls;

namespace Kzx.UserControl.UITypeEdit
{
    public partial class frmTabPagesUITypeEditor : Form
    {
        private object _Instance = null;
        private KzxTabControl _KzxTabControl;
        private System.ComponentModel.ITypeDescriptorContext _context;
        private XtraTabPageCollection _MsTabPages = null;

        public XtraTabPageCollection MsTabPages
        {
            get
            {
                return this._MsTabPages;
            }
            set
            {
                this._MsTabPages = value;
            }
        }

        public frmTabPagesUITypeEditor(System.ComponentModel.ITypeDescriptorContext context,object e)
        {
            InitializeComponent();
            if (e != null)
            {
                this._MsTabPages = (XtraTabPageCollection)e;
            }

            this._context = context;
            this._Instance = context.Instance;
        }

        private void frmTabPagesUITypeEditor_Load(object sender, EventArgs e)
        {
            this.listView1.MultiSelect = false;
            this.listView1.FullRowSelect = true;
            this._KzxTabControl = (KzxTabControl)this._context.Instance;
            for (int i = 0; i < this._MsTabPages.Count; i++)
            {
                ListViewItem item=new ListViewItem();
                item.Text=this._MsTabPages[i].Name;
                item.Tag = this._MsTabPages[i];
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
            for (int i = 0; i < this._MsTabPages.Count; i++)
            {
                if (this._MsTabPages[i].Name.Equals(item.Text) == true)
                {
                    this.label2.Text = item.Text + " 属性";
                    this.mcPropertyGrid1.SelectedObject = this._MsTabPages[i];
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            KzxTabPage page = new KzxTabPage();
            this._MsTabPages.Add(page);
            ListViewItem item = new ListViewItem();
            page.Name = KzxTabControl.CreateName((this._context.Instance as KzxTabControl).Container, typeof(KzxTabPage));
            page.Key = page.Name;
            page.Text = page.Name;
            page.DesigeCaption = page.Text;
            item.Text = page.Name;
            item.Tag = page;
            this.listView1.Items.Add(item);
            ((Control)(this._Instance)).FindForm().Site.Container.Add((IComponent)page, page.Name);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            XtraTabPage page = new XtraTabPage();
            if (this.listView1.SelectedItems.Count <= 0)
            {
                return;
            }
            ListViewItem item = this.listView1.SelectedItems[0];
            page = item.Tag as XtraTabPage;
            this._MsTabPages.Remove(item.Tag as XtraTabPage);
            this.listView1.Items.Remove(item);
            ((Control)(this._Instance)).FindForm().Site.Container.Remove((IComponent)page);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            XtraTabPage item;

            for (int i = 0; i < this.listView1.SelectedItems.Count; i++)
            {
                System.Windows.Forms.ListViewItem listViewItem = this.listView1.SelectedItems[i];
                int index = this.listView1.SelectedItems[i].Index - 1;
                if (index < 0) return;
                item = this.listView1.SelectedItems[i].Tag as XtraTabPage;
                this._MsTabPages.Remove(item);
                this._MsTabPages.Insert(index, item);
                this.listView1.Items.Remove(this.listView1.SelectedItems[i]);
                this.listView1.Items.Insert(index, listViewItem);
                listViewItem.Selected = true;
            } 
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            XtraTabPage item;

            for (int i = this.listView1.SelectedItems.Count - 1; i > -1; i--)
            {
                System.Windows.Forms.ListViewItem listViewItem = this.listView1.SelectedItems[i];
                int index = this.listView1.SelectedItems[i].Index + 1;
                if (index > this.listView1.Items.Count - 1) return;
                item = this.listView1.SelectedItems[i].Tag as XtraTabPage;
                this._MsTabPages.Remove(item);
                this._MsTabPages.Insert(index, item);
                this.listView1.Items.Remove(this.listView1.SelectedItems[i]);
                this.listView1.Items.Insert(index, listViewItem);
                listViewItem.Selected = true;
            } 
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (this._MsTabPages.Count > 0)
            {
               this._KzxTabControl.KzxSelectedIndex = 0;
            }
            this.Close();
        }
    }
}
