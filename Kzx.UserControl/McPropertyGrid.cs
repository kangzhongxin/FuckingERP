using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Kzx.UserControl
{
    /// <summary>
    /// 属性网格控件
    /// </summary>
    public partial class McPropertyGrid:PropertyGrid
    {
        /// <summary>
        /// 构造
        /// </summary>8
        /// 
        public McPropertyGrid()
            : base()
        {
            this.PropertyValueChanged += new PropertyValueChangedEventHandler(mcPropertyGrid1_PropertyValueChanged);
        }

        protected override PropertyTab CreatePropertyTab(Type tabType)
        {
            return new McPropertyTab();
        }

        /// <summary>
        /// 根据DesginCaption取得MessageCode
        /// </summary>
        /// <param name="caption">显示标题</param>
        public string DesignSetMsgId(string caption)
        {
            Object v = null;
            string sid = string.Empty;
             
    
            return sid;
        }

        private void mcPropertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            string sid = string.Empty;
            string value=string.Empty;
            System.ComponentModel.PropertyDescriptorCollection collection=null;
            
            if (e.ChangedItem.PropertyDescriptor != null)
            {
                if (e.ChangedItem.PropertyDescriptor.Name.Equals("DesigeCaption", StringComparison.OrdinalIgnoreCase) == true)
                {
                    value = (e.ChangedItem.Value == null ? string.Empty : e.ChangedItem.Value.ToString());
                    if (string.IsNullOrWhiteSpace(value) == false)
                    {
                        sid = DesignSetMsgId(value);
                    }
                    else
                    {
                        sid = string.Empty;
                    }

                    foreach (object obj in this.SelectedObjects)
                    {
                        collection = System.ComponentModel.TypeDescriptor.GetProperties(obj);
                        foreach (PropertyDescriptor v in collection)
                        {
                            if (v.Name.Equals("MessageCode", StringComparison.OrdinalIgnoreCase) == true)
                            {
                                v.SetValue(obj, sid);
                            }
                        }
                        if (obj is XmlRow)
                        {
                            ((XmlRow)obj).MessageCode = sid;
                            ((XmlRow)obj).RowView["MessageCode"] = sid;
                            ((XmlRow)obj).RowView.EndEdit();
                        }
                        else if (obj is KzxLookUpColumnInfo)
                        {

                        }
                    }
                    this.Refresh();
                }
                else if (e.ChangedItem.PropertyDescriptor.Name.Equals("Caption", StringComparison.OrdinalIgnoreCase) == true)
                {
                    value = (e.ChangedItem.Value == null ? string.Empty : e.ChangedItem.Value.ToString());
                    if (string.IsNullOrWhiteSpace(value) == false)
                    {
                        sid = DesignSetMsgId(value);
                    }
                    else
                    {
                        sid = string.Empty;
                    }

                    foreach (object obj in this.SelectedObjects)
                    {
                        collection = System.ComponentModel.TypeDescriptor.GetProperties(obj);
                        foreach (PropertyDescriptor v in collection)
                        {
                            if (v.Name.Equals("MessageCode", StringComparison.OrdinalIgnoreCase) == true)
                            {
                                v.SetValue(obj, sid);
                            }
                        }
                    }

                    this.Refresh();
                }
            }
        }
    }
}
