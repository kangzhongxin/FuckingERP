using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Kzx.UserControl.UITypeEdit
{
    public class KzxMaskTypeUiTypeEdit : UITypeEditor
    {
        public KzxMaskTypeUiTypeEdit()
            : base()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService iwfeds = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (iwfeds != null)
            {
                ucKzxMaskType f = new ucKzxMaskType(iwfeds, context, value);
                iwfeds.DropDownControl(f);
                return f.Value;
            }
            return base.EditValue(context, provider, value);
        }
        
    }
}
