using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Kzx.UserControl.UITypeEdit
{
    public partial class ucKzxMaskType : System.Windows.Forms.UserControl
    {
        private IWindowsFormsEditorService iwfeds = null;
        private object _Instance = null;
        private System.ComponentModel.ITypeDescriptorContext _context;
        private string _value = "None";

        public string Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        public ucKzxMaskType(IWindowsFormsEditorService obj,System.ComponentModel.ITypeDescriptorContext context, object e)
        {
            InitializeComponent();
            iwfeds = obj;
            if (e != null)
            {
                this.Value = e.ToString();
                this.listBox1.SelectedItem = this.Value;
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex < 0)
            {
                this.listBox1.SelectedIndex = 0;
            }
            this._value = this.listBox1.SelectedItem.ToString();
            iwfeds.CloseDropDown();
        }
    }
}
