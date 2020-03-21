using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kzx.UserControl.UITypeEdit
{
    public partial class frmTextUiTypeEditor : Form
    {

        private object _Instance = null;
        private System.ComponentModel.ITypeDescriptorContext _context;

        public string Xml
        {
            get
            {
                return this.richTextBox1.Text;
            }
            set
            {
                this.richTextBox1.Text = value;
            }
        }

        public frmTextUiTypeEditor(System.ComponentModel.ITypeDescriptorContext context, object e)
        {
            InitializeComponent();
            if (e != null)
            {
                this.Xml = e.ToString();
            }
        }

        private void frmTextUiTypeEditor_Load(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
