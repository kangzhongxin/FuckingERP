using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FuckingERP
{
    public partial class BillInfo
    {
        /// <summary>
        /// 第1层表主键
        /// </summary>
        public const string uGuid1 = "uGuid1";

        /// <summary>
        /// 第2层表主键
        /// </summary>
        public const string uGuid2 = "uGuid2";

        /// <summary>
        /// 第3层表主键
        /// </summary>
        public const string uGuid3 = "uGuid3";

        /// <summary>
        /// 第4层表主键
        /// </summary>
        public const string uGuid4 = "uGuid4";

        /// <summary>
        /// 第5层表主键
        /// </summary>
        public const string uGuid5 = "uGuid5";

        /// <summary>
        /// 第6层表主键
        /// </summary>
        public const string uGuid6 = "uGuid6";

        private Guid _BillId = Guid.Empty;
        /// <summary>
        /// 单据记录的GUID主键(Sys_FrmXML)
        /// </summary>
        public Guid Sys_FrmXML_Id
        {
            get
            {
                return this._BillId;
            }
            set
            {
                this._BillId = value;
            }
        }

        private string _SpID = string.Empty;
        /// <summary>
        /// 上级结点代码
        /// </summary>
        public string SpID
        {
            get
            {
                return this._SpID;
            }
            set
            {
                this._SpID = value;
            }
        }

        private string _FormName = string.Empty;
        /// <summary>
        /// 窗口名称
        /// </summary>
        public string FormName
        {
            get
            {
                return this._FormName;
            }
            set
            {
                this._FormName = value;
            }
        }

        private string _BillCaption = string.Empty;
        /// <summary>
        /// 单据的名称
        /// </summary>
        public string BillCaption
        {
            get
            {
                return this._BillCaption;
            }
            set
            {
                this._BillCaption = value;
            }
        }

        private DataTable _Sys_TableName = BillInfo.Create_Sys_Table();
        /// <summary>
        /// 对应Sys_TableName表
        /// </summary>
        public DataTable Sys_TableName
        {
            get
            {
                return this._Sys_TableName;
            }
            set
            {
                this._Sys_TableName = value;
            }
        }

        private DataTable _Sys_TableDetail = new DataTable();
        /// <summary>
        /// 对应Sys_Table表
        /// </summary>
        public DataTable Sys_TableDetail
        {
            get
            {
                return this._Sys_TableDetail;
            }
            set
            {
                this._Sys_TableDetail = value;
            }
        }

        /// <summary>
        /// 生成表Sys_Table
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable Create_Sys_Table()
        {
            DataTable dt = new DataTable("Sys_Table");
            //Sys_TableName表
            dt.Columns.Add("uGuid1", typeof(Guid));             //sys_frmxml中uGuid1字段
            dt.Columns.Add("uGuid2", typeof(Guid));             //主关键字GUID
            dt.Columns.Add("sPTableName", typeof(string));      //父表名
            dt.Columns.Add("sTableName", typeof(string));       //表名
            dt.Columns.Add("sFrmName", typeof(string));       //窗口名称
            dt.Columns.Add("iIndex", typeof(int));       //窗口名称
            return dt;
        }

        /// <summary>
        /// 生成Sys_TableDetail
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable Create_Sys_TableDetail()
        {
            DataTable detaildt = new DataTable("Sys_TableDetail");
            detaildt.Columns.Add("uGuid1", typeof(Guid));       //sys_frmxml中uGuid1字段
            detaildt.Columns.Add("uGuid2", typeof(Guid));       //父表的uGuid2
            detaildt.Columns.Add("uGuid3", typeof(Guid));       //主键GUID
            detaildt.Columns.Add("sTableName", typeof(string));      //表名
            detaildt.Columns.Add("sField", typeof(string));     //字段名
            detaildt.Columns.Add("sFormat", typeof(string));    //格式化字符串
            detaildt.Columns.Add("bIsNull", typeof(Boolean));   //可空否
            detaildt.Columns.Add("sDefultValue", typeof(string));   //缺省值
            detaildt.Columns.Add("iOrder", typeof(int));            //字段顺序
            detaildt.Columns.Add("bVisible", typeof(Boolean));      //显示否
            detaildt.Columns.Add("sFieldDesc", typeof(string));     //字段描述
            detaildt.Columns.Add("sMsgID", typeof(string));         //字段描述MSG
            detaildt.Columns.Add("bReadOnly", typeof(Boolean));     //只读
            detaildt.Columns.Add("sParamFormula", typeof(string));  //查询设置SQL语句
            detaildt.Columns.Add("sEditType", typeof(string));  //编辑框类型
            detaildt.Columns.Add("sKeyFields", typeof(string));  //查询设置|返回字段
            detaildt.Columns.Add("sFieldCaption", typeof(string));  //查询设置|返回字段
            detaildt.Columns.Add("sParentField", typeof(string));  //查询设置|返回字段
            detaildt.Columns.Add("sValueDependencyField", typeof(string));  //查询设置|返回字段
            detaildt.Columns.Add("iEditWeight", typeof(int));  //查询设置|返回字段
            detaildt.Columns.Add("sGridTable", typeof(string));  //网格的Table
            detaildt.Columns.Add("iIsColumn", typeof(int));  //1表示是网格的列,0不是网格的列
            return detaildt;
        }

        public static DataTable Create_ParentTable()
        {
            DataTable detaildt = new DataTable("Sys_ParentTable");
            detaildt.Columns.Add("c1", typeof(string));
            detaildt.Columns.Add("c2", typeof(string));
            detaildt.Columns.Add("c3", typeof(string));
            detaildt.Columns.Add("c4", typeof(string));
            detaildt.Columns.Add("c5", typeof(string));
            detaildt.Columns.Add("c6", typeof(string));
            return detaildt;
        } 

        private string sGetFrmName = string.Empty;
        /// <summary>
        /// 页面代码
        /// </summary>
        public string sFrmName
        {
            get
            {
                return this.sGetFrmName;
            }
            set
            {
                this.sGetFrmName = value;
            }
        }

        private string sGetFrmCaption = string.Empty;
        /// <summary>
        /// 页面代码
        /// </summary>
        public string sFrmCaption
        {
            get
            {
                return this.sGetFrmCaption;
            }
            set
            {
                this.sGetFrmCaption = value;
            }
        }
         

        private string sGetID = string.Empty;
        /// <summary>
        /// 节点代码
        /// </summary>
        public string sID
        {
            get
            {
                return this.sGetID;
            }
            set
            {
                this.sGetID = value;
            }
        }

        private string sGetFrmType = string.Empty;
        /// <summary>
        /// 页面类型
        /// </summary>
        public string sFrmType
        {
            get
            {
                return this.sGetFrmType;
            }
            set
            {
                this.sGetFrmType = value;
            }
        }

        private string sGetMsgID = string.Empty;
        /// <summary>
        /// 多语言标识
        /// </summary>
        public string sMsgID
        {
            get
            {
                return this.sGetMsgID;
            }
            set
            {
                this.sGetMsgID = value;
            }
        }

        /// <summary>
        /// 构造
        /// </summary>
        public BillInfo()
        {
        }

    }
}
