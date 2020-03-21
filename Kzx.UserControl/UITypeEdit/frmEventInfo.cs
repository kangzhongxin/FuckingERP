using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kzx.UserControl.UITypeEdit
{
    public partial class frmEventInfo : Form
    {
        private ControlEventInfo _EventInfo = new ControlEventInfo();
        private object _Instance = null;
        private System.ComponentModel.ITypeDescriptorContext _context;
        private string _Xml = string.Empty;
        private string _Key = string.Empty;
        private DataTable _EventDataTable = new DataTable("EventList");

        public string Xml
        {
            get
            {
                return this._EventInfo == null ? string.Empty : ControlEventInfo.Serialize(this._EventInfo);
            }
        }

        public frmEventInfo(System.ComponentModel.ITypeDescriptorContext context, object e)
        {
            PropertyInfo pi = null;
            Object value = null;
            string key = string.Empty;
            DataRow[] rows = null;
            StringBuilder sb = new StringBuilder();

            InitializeComponent();
            this._context = context;
            this._Instance = context.Instance;

            if (e != null)
            {
                this._Xml = e.ToString();
            }

            object form = ((Control)(context.Instance)).TopLevelControl;
            pi = form.GetType().GetProperty("ActiveMdiChild");
            if (pi != null)
            {
                value = pi.GetValue(form, null);
                if (value != null)
                {
                    form = value;
                }
            }
            if (form != null)
            {
                pi = form.GetType().GetProperty("EventDataTable");
                if (pi != null)
                {
                    value = pi.GetValue(form, null);
                    if (value != null)
                    {
                        this._EventDataTable = value as DataTable;
                    }
                }
                pi = context.Instance.GetType().GetProperty("Key");
                if (pi != null)
                {
                    value = pi.GetValue(context.Instance, null);
                    if (value != null)
                    {
                        this._Key = value.ToString();
                    }
                }
            }
            if (this._EventDataTable != null)
            {
                rows = this._EventDataTable.Select("sKey='" + this._Key + "'", string.Empty, DataViewRowState.CurrentRows);
                for (int i = 0; i < rows.Length; i++)
                {
                    sb.Append(rows[i]["sEventName"].ToString() + "=" + rows[i]["sFileName"].ToString());
                    if (i < rows.Length - 1)
                    {
                        sb.Append("|");
                    }
                }
                if (sb.Length > 0)
                {
                    this._Xml = sb.ToString();
                }
            }
            this._EventInfo = ControlEventInfo.Deserialize(this._Xml);
        }

        private void frmEventInfo_Load(object sender, EventArgs e)
        {
            this._EventInfo.____Parent = this._Instance;
            this.propertyGrid1.SelectedObject = this._EventInfo;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            DataRow row = null;
            DataRow[] rows = null;

            rows = this._EventDataTable.Select("sKey='" + this._Key+"' and sEventName='" + e.ChangedItem.PropertyDescriptor.Name + "'", string.Empty, DataViewRowState.CurrentRows);

            if (e.ChangedItem.Value == null || string.IsNullOrWhiteSpace(e.ChangedItem.Value.ToString()) == true)
            {
                for (int i = rows.Length - 1; i >= 0; i--)
                {
                    rows[i]["sFileName"] = string.Empty;
                }
            }
            else
            {
                if (rows.Length > 0)
                {
                    for (int i = rows.Length - 1; i >= 0; i--)
                    {
                        rows[i]["sFileName"] = e.ChangedItem.Value;
                    }
                }
                else
                {
                    row = this._EventDataTable.NewRow();
                    row["uGuid2"] = Guid.NewGuid();
                    row["sKey"] = this._Key;
                    row["sEventName"] = e.ChangedItem.PropertyDescriptor.Name;
                    row["sFileName"] = e.ChangedItem.Value;
                    this._EventDataTable.Rows.Add(row);
                }
            }
        }
    }
}
