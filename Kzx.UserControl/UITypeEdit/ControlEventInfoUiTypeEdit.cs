using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Kzx.UserControl.UITypeEdit
{
    public class ControlEventInfoUiTypeEdit : UITypeEditor
    {
        public ControlEventInfoUiTypeEdit()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            string table = string.Empty;
            string field = string.Empty;
            string key = string.Empty;
            PropertyInfo pi = null;
            object v = null;

            IWindowsFormsEditorService iwfeds = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (iwfeds != null)
            {
                if (context.Instance is ILayoutControl)
                {
                    pi = context.Instance.GetType().GetProperty("Key");
                    if (pi != null)
                    {
                        v = pi.GetValue(context.Instance,null);
                        if (v != null)
                        {
                            key = v.ToString();
                        }
                    }
                    if (key.Length <= 0 || string.IsNullOrWhiteSpace(key) == true)
                    {
                        MessageBox.Show("请录入控件的Key(控件标识)属性值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return string.Empty;
                    }
                }
                else if (context.Instance is IControl)
                {
                    //    pi = context.Instance.GetType().GetProperty("Table");
                    //    if (pi != null)
                    //    {
                    //        v = pi.GetValue(context.Instance);
                    //        if (v != null)
                    //        {
                    //            table = v.ToString();
                    //        }
                    //    }
                    //    if (table.Length <= 0 || string.IsNullOrWhiteSpace(table)==true)
                    //    {
                    //        MessageBox.Show("请录入控件的Table(数据表名)属性值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //        return string.Empty;
                    //    }
                    //    pi = context.Instance.GetType().GetProperty("Field");
                    //    if (pi != null)
                    //    {
                    //        v = pi.GetValue(context.Instance);
                    //        if (v != null)
                    //        {
                    //            field = v.ToString();
                    //        }
                    //    }
                    //    if (field.Length <= 0 || string.IsNullOrWhiteSpace(field) == true)
                    //    {
                    //        MessageBox.Show("请录入控件的Field(字段名称)属性值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //        return string.Empty;
                    //    }
                    //    pi = context.Instance.GetType().GetProperty("Key");
                    //    if (pi != null)
                    //    {
                    //        v = pi.GetValue(context.Instance);
                    //        if (v != null)
                    //        {
                    //            key = v.ToString();
                    //        }
                    //    }
                    //}
                    pi = context.Instance.GetType().GetProperty("Key");
                    if (pi != null)
                    {
                        v = pi.GetValue(context.Instance, null);
                        if (v != null)
                        {
                            key = v.ToString();
                        }
                    }
                    if (key.Length <= 0 || string.IsNullOrWhiteSpace(key) == true)
                    {
                        MessageBox.Show("请录入控件的Key(控件标识)属性值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return string.Empty;
                    }
                }
                if (key.Length > 0)
                {
                    frmEventInfo f = new frmEventInfo(context, value);
                    if (DialogResult.OK == iwfeds.ShowDialog(f))
                    {
                        return f.Xml;
                    }
                }
            }
            return base.EditValue(context, provider, value);
        }
    }
}
