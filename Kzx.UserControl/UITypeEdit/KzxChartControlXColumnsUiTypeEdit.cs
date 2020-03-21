using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Kzx.UserControl.UITypeEdit
{
    class KzxChartControlXColumnsUiTypeEdit : UITypeEditor
    {
        public KzxChartControlXColumnsUiTypeEdit()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService iwfeds = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (iwfeds != null)
            {
                frmKzxChartControlXColumnsUiTypeEdit f = new frmKzxChartControlXColumnsUiTypeEdit(context, value);
                if (DialogResult.OK == iwfeds.ShowDialog(f))
                {
                    return f.XColumns;
                }
            }
            return base.EditValue(context, provider, value);
        }
    }
}
