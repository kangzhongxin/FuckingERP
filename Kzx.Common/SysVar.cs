using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kzx.Common
{
    public class SysVar
    { 
        public static string sPublicUserID = "Stephen-kzx";   
        public static string sUserID = "";   //用户代码
        public static string sUserName = "";  //用户名称
        public static string sPassWord = "";  //用户密码
        public static string sCompanyName = "";  //本机电脑名称
        public static string sCompanyIP = "";  //本机IP地址
        public static string sEmployeeProperty = "";//人员属性
        public static string sSysMsg = "";  //系统语言
        public static DataSet dsSystemMSG = new DataSet(); //中文Msg数据集
        public static List<string> lstLoadFormsName = new List<string>();//预加载模块名称
        /// <summary>
        /// 语言包数据字典,string:msgID,string:msgDesc (add by huangyq20170522：调整dataSet数据为数据字典 dictionary，提升查询效率)
        /// </summary>
        public static Dictionary<string, string> LanguageList = new Dictionary<string, string>();

        /// <summary>
        /// 表单XML文件字典，string:formName
        /// </summary>
        public static Dictionary<string, DataSet> FormXMLList = new Dictionary<string, DataSet>();

        /// <summary>
        /// 模块窗体数据集
        /// </summary>
        public static DataSet dsSystemFrmList = new DataSet();
         
        public static string sSecretKey = "";
        public static bool bFormFinish = true; 
        public static object frmChat = null;   //消息窗体
        public static object frmMain = null;   //主窗体
        public static object sLicFrmName = string.Empty;    //许可使用的模块
        public static string sCompany = "";  //客户注册名称
        /// <summary>
        /// 登录窗口类型 1表示华印 2表示印智
        /// </summary>
        public static int loginType = 2;
        /// <summary>
        /// 缓存文件目录
        /// </summary>
        public static string CachePath = AppDomain.CurrentDomain.BaseDirectory + "Guid\\";

        public struct CurryFormInfo
        {
            public string sFormName;  //当前窗体代码
            public string sFormType;  //当前窗体类型 frm:单据窗体    qry:查询窗体
            public DevExpress.XtraTab.XtraTabPage tpCuryPage;  //显示当前窗体的页
            public DataSet dsBillDataSet;//当前窗体单据数据集
            public Dictionary<string, BindingSource> bsBindingSource;   //绑定数据源字典
            public DataSet dsSysTableDetail;//当前窗体字段数据字典
            public List<string> lstAttachFiles;//需要上传的文件
            public object FocuseGridTree;//当前焦点的GridControl(treelist)
            public DevExpress.XtraGrid.GridControl MasterGridControl;//主表列表的GridControl
            public int iDataOperateId;//当前窗体数据操作状态 0:保存，1：新增，2：编辑，3：取消,4：删除，5：刷新（搜索），6：审核，7：消审，8：打印，9：导出
            public DataSet dsMainView;//当前窗体数据列表数据集（分页后的当前页数据）
            public BindingSource bsMainView;//当前窗体数据列表数据源
            public Boolean bHavTemplateCheck;//是否要多级审核标志
            public Boolean bAutoRefresh;//是否自动刷新，20161107，lfx
            public int iRefreshDuration;//刷新时长，20161107，lfx
            public DataSet dsListDataSet;//当前窗体数据列表所有数据集（包括分页后的所有页数据），lfx，20170502
            public DialogResult dialogResult;//记录KzxCommonDialog所做的操作是【选择】或【取消】,OK 或 Cancel，add by zhang.jz 2019.01.17
        }
        //当前窗体信息
        public static CurryFormInfo FCurryFormInfo;

        /// <summary>
        /// 遍历窗体所有子控件
        /// </summary>
        /// <typeparam name="T">控件类型</typeparam>
        /// <param name="form">窗体名</param>
        /// <returns></returns>
        public static void GetControlList<T>(System.Windows.Forms.Control.ControlCollection controlCollection, ref List<T> resultCollection)
where T : Control
        {
            foreach (Control control in controlCollection)
            {
                //if (control.GetType() == typeof(T))
                if (control is T) // This is cleaner
                    resultCollection.Add((T)control);

                if (control.HasChildren)
                    GetControlList(control.Controls, ref resultCollection);
            }
        }
    }
}
