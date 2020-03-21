using Kzx.UserControl.UITypeEdit;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Kzx.UserControl
{
    /// <summary>
    /// 文本字符串的编辑框
    /// </summary>
    public class TextUiTypEdit : UITypeEditor
    {
        public TextUiTypEdit()
            : base()
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
                frmTextUiTypeEditor f = new frmTextUiTypeEditor(context, value);
                if (DialogResult.OK == iwfeds.ShowDialog(f))
                {
                    return f.Xml;
                }
            }
            return base.EditValue(context, provider, value);
        }

    }
}
