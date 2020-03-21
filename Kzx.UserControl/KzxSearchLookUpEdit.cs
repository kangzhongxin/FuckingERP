using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Utils.Design;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid; 

namespace Kzx.UserControl
{
    public class KzxSearchLookUpEdit : LookUpEdit
    {
        public KzxSearchLookUpEdit()
            : base()
        {
        }

        /// <summary>
        /// 选中项的下标
        /// </summary>
        [Category("自定义"), Description("ItemIndex,选中项的下标"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [McDisplayName("ItemIndex")]
        public int ItemIndex
        {
            get
            {
                return base.ItemIndex;
            }
        }
    }
}