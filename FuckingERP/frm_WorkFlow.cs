using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Base;
using Kzx.UserControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace FuckingERP
{
    public partial class frmWorkFlow : DockContent
    {
        //暂时保存流程图数据
        private DataView flowMapDV = new DataView();
        //暂时保存流程图设计数据
        private DataTable dtDesign = new DataTable();
        private string sFilter = string.Empty;
        int iCount = 0;
        string sModel = "";
        string sModelName = "";
        Boolean bDesign = false;
        Boolean bAddBtn = false;
        Boolean bAddLabel = false;
        Boolean bAddLine = false;
        Boolean bFirstControl = false;
        object selectControl = null;
        private static frmWorkFlow fInstance;
        public frmWorkFlow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 单例
        /// </summary>
        /// <returns></returns>
        public static frmWorkFlow GetInstance()
        {
            if (fInstance == null)
            {
                fInstance = new frmWorkFlow();
            }
            return fInstance;
        }

        private void frm_WorkFlow_FormClosed(object sender, FormClosedEventArgs e)
        {
            fInstance = null;
        }

        private void frmWorkFlow_Load(object sender, EventArgs e)
        {
            //创建大模块
            CreateModule();
            this.TabPage1.PageVisible = false;
            this.TabPage2.PageVisible = false;
            this.TabPage3.PageVisible = false;

            this.kzxsBtnAddButton.Enabled = false;
            this.kzxsBtnAddLabel.Enabled = false;
            this.kzxsBtnAddLine.Enabled = false;

            this.Btn_SaveButton.Enabled = false;
            this.Btn_SaveLabel.Enabled = false;
            this.Btn_SaveLine.Enabled = false;

            //用于保存界面设计元素
            dtDesign.Columns.Add("uGuid", typeof(String));
            dtDesign.Columns.Add("sModelCode", typeof(String));
            dtDesign.Columns.Add("sID", typeof(String));
            dtDesign.Columns.Add("sFrmName", typeof(String));
            dtDesign.Columns.Add("sCaption", typeof(String));
            dtDesign.Columns.Add("bHorizon", typeof(String));
            dtDesign.Columns.Add("bSolid", typeof(String));
            dtDesign.Columns.Add("iFontSize", typeof(String));
            dtDesign.Columns.Add("sColor", typeof(String));
            dtDesign.Columns.Add("bUnderLine", typeof(String));
            dtDesign.Columns.Add("sSysMode", typeof(String));
            dtDesign.Columns.Add("iLineWidth", typeof(String));
            dtDesign.Columns.Add("sMsgID", typeof(String));
            dtDesign.Columns.Add("sType", typeof(String));
            dtDesign.Columns.Add("iLeft", typeof(String));
            dtDesign.Columns.Add("iTop", typeof(String));
            dtDesign.Columns.Add("iWidth", typeof(String));
            dtDesign.Columns.Add("iHeight", typeof(String));
            dtDesign.Columns.Add("sBtnPositon", typeof(String));
            dtDesign.Columns.Add("sArrowType", typeof(String));
            dtDesign.Columns.Add("mImage", typeof(String));
            dtDesign.Columns.Add("iOrder", typeof(String));
            dtDesign.Columns.Add("bActive", typeof(String));
            dtDesign.Columns.Add("sFrmType", typeof(String));
        }

        #region  模块处理

        //创建主目录
        public void CreateModule()
        {
            //此处拿DataTable来存储数据库的所有菜单节点，此处用销售模块来举例
            DataTable dtSalesOrder = new DataTable();
            dtSalesOrder.Columns.Add("sID", typeof(Guid));
            dtSalesOrder.Columns.Add("sModel", typeof(string));
            dtSalesOrder.Columns.Add("iOrder", typeof(int));
            dtSalesOrder.Rows.Add(new object[] { Guid.NewGuid(), "销售模块", 1 });

            DataRow[] dr = dtSalesOrder.Select("", "iOrder");

            //存在子菜单生产树结构
            if (dr.Length > 0)
            {
                foreach (DataRow row in dr)
                {
                    DevExpress.XtraEditors.SimpleButton sb = new DevExpress.XtraEditors.SimpleButton();
                    sb.Name = row["sID"].ToString();
                    sb.Text = row["sModel"].ToString();
                    sb.Tag = row["sModel"].ToString();
                    sb.TabIndex = int.Parse(row["iOrder"].ToString());
                    sb.Dock = DockStyle.Top;
                    sb.Height = 25;
                    sb.Click += onBtnClick;
                    panelControl1.Controls.Add(sb);

                }
            }
        }

        private void onBtnClick(object sender, EventArgs e)
        {
            sModel = ((DevExpress.XtraEditors.SimpleButton)sender).Name.ToString().Trim();
            sModelName = ((DevExpress.XtraEditors.SimpleButton)sender).Text.ToString().Trim();
            //根据选择的菜单模块，加载当前菜单的流程图
            CreateFlowMap(((DevExpress.XtraEditors.SimpleButton)sender).Name.ToString().Trim(), ((DevExpress.XtraEditors.SimpleButton)sender).Text.ToString().Trim());
        }

        #endregion

        /// <summary>
        /// 新增直线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kzxsBtnAddLine_Click(object sender, EventArgs e)
        {
            KzxLine line = new KzxLine();

            bAddLine = true;
            line.Name = "Line";
            line.Tag = "";
            line.lineWidth = int.Parse(this.cmb_LineWidth.Text.ToString());
            string sColor = this.cmb_ColorLine.Text.ToString().Trim();
            switch (sColor)
            {
                case "Black":
                    line.LineColor = KzxLine.ColorType.Black;
                    break;
                case "Blue":
                    line.LineColor = KzxLine.ColorType.Blue;
                    break;
                case "Green":
                    line.LineColor = KzxLine.ColorType.Green;
                    break;
                case "Lime":
                    line.LineColor = KzxLine.ColorType.Lime;
                    break;
                case "Red":
                    line.LineColor = KzxLine.ColorType.Red;
                    break;
                case "Yellow":
                    line.LineColor = KzxLine.ColorType.Yellow;
                    break;
                default:
                    line.LineColor = KzxLine.ColorType.Black;
                    break;
            }
            line.IsSolid = !this.chk_Solid.Checked;
            switch (this.cmb_ArrowType.Text.ToString().Trim())
            {
                case "Start":
                    line.ArrowPosition = KzxLine.ArrowType.Start;
                    break;
                case "End":
                    line.ArrowPosition = KzxLine.ArrowType.End;
                    break;
                case "All":
                    line.ArrowPosition = KzxLine.ArrowType.All;
                    break;
                case "None":
                    line.ArrowPosition = KzxLine.ArrowType.None;
                    break;
            }
            if (this.chk_Horizon.Checked)
            {
                line.LineStyle = KzxLine.LineType.Horizontal;
                line.Left = int.Parse(this.edt_LeftLine.Text.ToString());
                line.Top = int.Parse(this.edt_TopLine.Text.ToString());
                line.Width = int.Parse(this.edt_WidthLine.Text.ToString());
                line.Height = int.Parse(this.cmb_LineWidth.Text.ToString()) * 2 + 10;
            }
            else
            {
                line.LineStyle = KzxLine.LineType.Vertical;
                line.Left = int.Parse(this.edt_LeftLine.Text.ToString());
                line.Top = int.Parse(this.edt_TopLine.Text.ToString());
                line.Width = int.Parse(this.cmb_LineWidth.Text.ToString()) * 2 + 10;
                line.Height = int.Parse(this.edt_HeightLine.Text.ToString());
            }
            line.Visible = true;

            line.Click += onClick;
            MoveClass.BarcodeControl(line);
            if (bFirstControl)
            {
                MoveClass.BarcodeCreate();
                bFirstControl = false;
            }
            this.TabControl2.Enabled = true;
            this.TabControl1.SelectedTabPage = TabPage3;
            this.TabPage3.PageVisible = true;

            this.kzxsBtnAddButton.Enabled = true;
            this.kzxsBtnAddLabel.Enabled = true;
            this.kzxsBtnAddLine.Enabled = true;
        }

        private void onClick(object sender, EventArgs e)
        {

            if (sender is DevExpress.XtraEditors.SimpleButton)
            {
                this.TabPage1.PageVisible = true;
                this.TabPage2.PageVisible = false;
                this.TabPage3.PageVisible = false;
                this.TabControl1.SelectedTabPage = TabPage1;
                BillInfo info = ((DevExpress.XtraEditors.SimpleButton)sender).Tag as BillInfo;

                if ((selectControl != null) && (selectControl is DevExpress.XtraEditors.SimpleButton) &&
                     Equals(((DevExpress.XtraEditors.SimpleButton)sender).Name.ToString().ToUpper(), ((DevExpress.XtraEditors.SimpleButton)selectControl).Name.ToString().ToUpper()))
                {
                }
                else
                {
                    this.Edt_FrmBtn.Text = info.sFrmName;
                }
                this.Edt_LeftBtn.Text = ((DevExpress.XtraEditors.SimpleButton)sender).Left.ToString();
                this.Edt_TopBtn.Text = ((DevExpress.XtraEditors.SimpleButton)sender).Top.ToString();
                this.Edt_WidthBtn.Text = ((DevExpress.XtraEditors.SimpleButton)sender).Width.ToString();
                this.Edt_HightBtn.Text = ((DevExpress.XtraEditors.SimpleButton)sender).Height.ToString();
                this.Edt_MsgBtn.Text = info.sMsgID;
                picture1.Image = ((DevExpress.XtraEditors.SimpleButton)sender).Image;
            }
            else if (sender is DevExpress.XtraEditors.LabelControl)
            {
                this.TabPage1.PageVisible = false;
                this.TabPage2.PageVisible = true;
                this.TabPage3.PageVisible = false;
                this.TabControl1.SelectedTabPage = TabPage2;
                BillInfo info = ((DevExpress.XtraEditors.LabelControl)sender).Tag as BillInfo;

                this.edt_LeftLabel.Text = ((DevExpress.XtraEditors.LabelControl)sender).Left.ToString();
                this.edt_TopLabel.Text = ((DevExpress.XtraEditors.LabelControl)sender).Top.ToString();
                this.edt_WidthLabel.Text = ((DevExpress.XtraEditors.LabelControl)sender).Width.ToString();
                this.edt_HeightLabel.Text = ((DevExpress.XtraEditors.LabelControl)sender).Height.ToString();

                this.edt_FrmLabel.Text = info.sFrmName;

                this.cmb_ColorLabel.Text = ((DevExpress.XtraEditors.LabelControl)sender).ForeColor.Name.ToString();
                this.cmb_SizeLabel.Text = ((DevExpress.XtraEditors.LabelControl)sender).Font.Size.ToString();
                this.chk_Line.Checked = ((DevExpress.XtraEditors.LabelControl)sender).Font.Underline;
            }
            else if (sender is KzxLine)
            {
                this.TabPage1.PageVisible = false;
                this.TabPage2.PageVisible = false;
                this.TabPage3.PageVisible = true;
                this.TabControl1.SelectedTabPage = TabPage3;

                this.edt_LeftLine.Text = ((KzxLine)sender).Left.ToString();
                this.edt_TopLine.Text = ((KzxLine)sender).Top.ToString();
                this.edt_WidthLine.Text = ((KzxLine)sender).Width.ToString();
                this.edt_HeightLine.Text = ((KzxLine)sender).Height.ToString();

                this.cmb_ArrowType.Text = ((KzxLine)sender).ArrowPosition.ToString();
                this.cmb_ColorLine.Text = ((KzxLine)sender).LineColor.ToString();
                this.cmb_LineWidth.Text = ((KzxLine)sender).lineWidth.ToString();
                this.chk_Solid.Checked = !((KzxLine)sender).IsSolid;
                if (((KzxLine)sender).LineStyle == KzxLine.LineType.Horizontal)
                {
                    this.chk_Horizon.Checked = true;
                }
                else
                {
                    this.chk_Horizon.Checked = false;
                }
            }

            selectControl = sender;
        }


        private void Btn_SaveLabel_Click(object sender, EventArgs e)
        {
            BillInfo info = new BillInfo();
            //info.sID = edt_FrmLabel.EditValue.ToString();
            info.sFrmName = edt_FrmLabel.Text.ToString();
            info.sFrmCaption = edt_CaptionLabel.Text.Trim();
            info.sMsgID = edt_MsgLabel.Text.ToString().Trim();

            if (bAddLabel)
            {
                DevExpress.XtraEditors.LabelControl lbl = new DevExpress.XtraEditors.LabelControl();
                lbl.Left = int.Parse(this.edt_LeftLabel.Text.Trim());
                lbl.Top = int.Parse(this.edt_TopLabel.Text.Trim());
                lbl.Width = int.Parse(this.edt_WidthLabel.Text.Trim());
                lbl.Height = int.Parse(this.edt_HeightLabel.Text.Trim());
                lbl.Name = this.edt_FrmLabel.Text.Trim();
                lbl.Text = this.edt_CaptionLabel.Text.Trim();
                lbl.Tag = info;

                if (this.edt_FrmLabel.Text.Trim() != "")
                {
                    lbl.Cursor = Cursors.Hand;
                }
                FontStyle fs = new FontStyle();
                if (this.chk_Line.Checked)
                {
                    fs = FontStyle.Underline;
                }
                else
                {
                    fs = FontStyle.Bold;
                }

                Font f = new Font("宋体", float.Parse(this.cmb_SizeLabel.Text.ToString()), fs);
                lbl.Font = f;

                lbl.ForeColor = Color.FromName(this.cmb_ColorLabel.Text.ToString());
                lbl.Visible = true;
                TabControl2.SelectedTabPage.Controls.Add(lbl);

                lbl.Click += onClick;
                MoveClass.BarcodeControl(lbl);
                if (bFirstControl)
                {
                    MoveClass.BarcodeCreate();
                    bFirstControl = false;
                }
                this.TabControl2.Enabled = true;
                bAddLabel = false;

                this.kzxsBtnAddButton.Enabled = true;
                this.kzxsBtnAddLabel.Enabled = true;
                this.kzxsBtnAddLine.Enabled = true;
                this.sBtnDelete.Enabled = true;
            }
            else if ((selectControl is DevExpress.XtraEditors.LabelControl) && (DevExpress.XtraEditors.LabelControl)selectControl != null)
            {
                ((DevExpress.XtraEditors.LabelControl)selectControl).Left = int.Parse(this.edt_LeftLabel.Text.Trim());
                ((DevExpress.XtraEditors.LabelControl)selectControl).Top = int.Parse(this.edt_TopLabel.Text.Trim());
                ((DevExpress.XtraEditors.LabelControl)selectControl).Width = int.Parse(this.edt_WidthLabel.Text.Trim());
                ((DevExpress.XtraEditors.LabelControl)selectControl).Height = int.Parse(this.edt_HeightLabel.Text.Trim());
                ((DevExpress.XtraEditors.LabelControl)selectControl).Name = this.edt_FrmLabel.Text.Trim();
                ((DevExpress.XtraEditors.LabelControl)selectControl).Text = this.edt_CaptionLabel.Text.Trim();
                ((DevExpress.XtraEditors.LabelControl)selectControl).Tag = info;

                if (this.edt_FrmLabel.Text.Trim() != "")
                {
                    ((DevExpress.XtraEditors.LabelControl)selectControl).Cursor = Cursors.Hand;
                }
                FontStyle fs = new FontStyle();
                if (this.chk_Line.Checked)
                {
                    fs = FontStyle.Underline;
                }
                else
                {
                    fs = FontStyle.Bold;
                }

                Font f = new Font("宋体", float.Parse(this.cmb_SizeLabel.Text.ToString()), fs);
                ((DevExpress.XtraEditors.LabelControl)selectControl).Font = f;

                ((DevExpress.XtraEditors.LabelControl)selectControl).ForeColor = Color.FromName(this.cmb_ColorLabel.Text.ToString());
            }

        }

        private void Btn_SaveLine_Click(object sender, EventArgs e)
        {
            if (bAddLine)
            {
                KzxLine line = new KzxLine();

                line.Name = "Line";
                line.Tag = "";
                line.lineWidth = int.Parse(this.cmb_LineWidth.Text.ToString());
                string sColor = this.cmb_ColorLine.Text.ToString().Trim();
                switch (sColor)
                {
                    case "Black":
                        line.LineColor = KzxLine.ColorType.Black;
                        break;
                    case "Blue":
                        line.LineColor = KzxLine.ColorType.Blue;
                        break;
                    case "Green":
                        line.LineColor = KzxLine.ColorType.Green;
                        break;
                    case "Lime":
                        line.LineColor = KzxLine.ColorType.Lime;
                        break;
                    case "Red":
                        line.LineColor = KzxLine.ColorType.Red;
                        break;
                    case "Yellow":
                        line.LineColor = KzxLine.ColorType.Yellow;
                        break;
                    default:
                        line.LineColor = KzxLine.ColorType.Black;
                        break;
                }
                line.IsSolid = !this.chk_Solid.Checked;
                switch (this.cmb_ArrowType.Text.ToString().Trim())
                {
                    case "Start":
                        line.ArrowPosition = KzxLine.ArrowType.Start;
                        break;
                    case "End":
                        line.ArrowPosition = KzxLine.ArrowType.End;
                        break;
                    case "All":
                        line.ArrowPosition = KzxLine.ArrowType.All;
                        break;
                    case "None":
                        line.ArrowPosition = KzxLine.ArrowType.None;
                        break;
                }
                if (this.chk_Horizon.Checked)
                {
                    line.LineStyle = KzxLine.LineType.Horizontal;
                    line.Left = int.Parse(this.edt_LeftLine.Text.ToString());
                    line.Top = int.Parse(this.edt_TopLine.Text.ToString());
                    line.Width = int.Parse(this.edt_WidthLine.Text.ToString());
                    line.Height = int.Parse(this.cmb_LineWidth.Text.ToString()) * 2 + 10;
                }
                else
                {
                    line.LineStyle = KzxLine.LineType.Vertical;
                    line.Left = int.Parse(this.edt_LeftLine.Text.ToString());
                    line.Top = int.Parse(this.edt_TopLine.Text.ToString());
                    line.Width = int.Parse(this.cmb_LineWidth.Text.ToString()) * 2 + 10;
                    line.Height = int.Parse(this.edt_HeightLine.Text.ToString());
                }
                line.Visible = true;

                line.Click += onClick;
                MoveClass.BarcodeControl(line);
                if (bFirstControl)
                {
                    MoveClass.BarcodeCreate();
                    bFirstControl = false;
                }
                this.TabControl2.Enabled = true;
                bAddLine = false;

                TabControl2.SelectedTabPage.Controls.Add(line);
                this.kzxsBtnAddButton.Enabled = true;
                this.kzxsBtnAddLabel.Enabled = true;
                this.kzxsBtnAddLine.Enabled = true;
                this.sBtnDelete.Enabled = true;
            }
            else if ((selectControl is KzxLine) && (KzxLine)selectControl != null)
            {
                ((KzxLine)selectControl).lineWidth = int.Parse(this.cmb_LineWidth.Text.ToString());
                string sColor = this.cmb_ColorLine.Text.ToString().Trim();
                switch (sColor)
                {
                    case "Black":
                        ((KzxLine)selectControl).LineColor = KzxLine.ColorType.Black;
                        break;
                    case "Blue":
                        ((KzxLine)selectControl).LineColor = KzxLine.ColorType.Blue;
                        break;
                    case "Green":
                        ((KzxLine)selectControl).LineColor = KzxLine.ColorType.Green;
                        break;
                    case "Lime":
                        ((KzxLine)selectControl).LineColor = KzxLine.ColorType.Lime;
                        break;
                    case "Red":
                        ((KzxLine)selectControl).LineColor = KzxLine.ColorType.Red;
                        break;
                    case "Yellow":
                        ((KzxLine)selectControl).LineColor = KzxLine.ColorType.Yellow;
                        break;
                    default:
                        ((KzxLine)selectControl).LineColor = KzxLine.ColorType.Black;
                        break;
                }
                ((KzxLine)selectControl).IsSolid = !this.chk_Solid.Checked;
                switch (this.cmb_ArrowType.Text.ToString().Trim())
                {
                    case "Start":
                        ((KzxLine)selectControl).ArrowPosition = KzxLine.ArrowType.Start;
                        break;
                    case "End":
                        ((KzxLine)selectControl).ArrowPosition = KzxLine.ArrowType.End;
                        break;
                    case "All":
                        ((KzxLine)selectControl).ArrowPosition = KzxLine.ArrowType.All;
                        break;
                    case "None":
                        ((KzxLine)selectControl).ArrowPosition = KzxLine.ArrowType.None;
                        break;
                }
                if (this.chk_Horizon.Checked)
                {
                    ((KzxLine)selectControl).LineStyle = KzxLine.LineType.Horizontal;
                    ((KzxLine)selectControl).Left = int.Parse(this.edt_LeftLine.Text.ToString());
                    ((KzxLine)selectControl).Top = int.Parse(this.edt_TopLine.Text.ToString());
                    ((KzxLine)selectControl).Width = int.Parse(this.edt_WidthLine.Text.ToString());
                    ((KzxLine)selectControl).Height = int.Parse(this.cmb_LineWidth.Text.ToString()) * 2 + 10;
                }
                else
                {
                    ((KzxLine)selectControl).LineStyle = KzxLine.LineType.Vertical;
                    ((KzxLine)selectControl).Left = int.Parse(this.edt_LeftLine.Text.ToString());
                    ((KzxLine)selectControl).Top = int.Parse(this.edt_TopLine.Text.ToString());
                    ((KzxLine)selectControl).Width = int.Parse(this.cmb_LineWidth.Text.ToString()) * 2 + 10;
                    ((KzxLine)selectControl).Height = int.Parse(this.edt_HeightLine.Text.ToString());
                }
            }
        }
        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kzxsBtnAddButton_Click(object sender, EventArgs e)
        {
            bAddBtn = true;
            this.kzxsBtnAddButton.Enabled = false;
            this.kzxsBtnAddLabel.Enabled = false;
            this.kzxsBtnAddLine.Enabled = false;
            //获得窗体列表 
            this.TabPage1.PageVisible = true;
            this.TabPage2.PageVisible = false;
            this.TabPage3.PageVisible = false;
            this.TabControl1.SelectedTabPage = TabPage1;
            ClearText(0);

        }

        #region 清除信息

        private void ClearText(int flag)
        {
            if (flag == 0)
            {
                this.Edt_FrmBtn.Text = "";
                this.Edt_CaptionBtn.Text = "";
                this.cmb_FrmStyleBtn.SelectedIndex = -1;
                this.Edt_MsgBtn.Text = "";
                this.Edt_LeftBtn.Text = "10";
                this.Edt_TopBtn.Text = "10";
                this.Edt_WidthBtn.Text = "80";
                this.Edt_HightBtn.Text = "80";

            }
            else if (flag == 1)
            {
                this.edt_FrmLabel.Text = "";
                this.edt_CaptionLabel.Text = "";
                this.cmb_FrmStyleLabel.SelectedIndex = -1;
                this.edt_MsgLabel.Text = "";
                this.edt_LeftLabel.Text = "10";
                this.edt_TopLabel.Text = "10";
                this.edt_WidthLabel.Text = "100";
                this.edt_HeightLabel.Text = "20";
                this.cmb_ColorLabel.SelectedIndex = 0;
                this.cmb_SizeLabel.SelectedIndex = 7;
                this.chk_Line.Checked = false;
            }
            else if (flag == 2)
            {
                this.edt_LeftLine.Text = "10";
                this.edt_TopLine.Text = "10";
                this.edt_WidthLine.Text = "100";
                this.edt_HeightLine.Text = "10";
                this.cmb_ArrowType.SelectedIndex = 0;
                this.cmb_LineWidth.SelectedIndex = 1;
                this.cmb_ColorLine.SelectedIndex = 0;
                this.chk_Horizon.Checked = true;
            }
        }

        #endregion
        /// <summary>
        /// 注释
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kzxsBtnAddLabel_Click(object sender, EventArgs e)
        {
            bAddLabel = true;
            this.kzxsBtnAddButton.Enabled = false;
            this.kzxsBtnAddLabel.Enabled = false;
            this.kzxsBtnAddLine.Enabled = false;
            //获得窗体列表
            //GetFrmList(1);
            this.TabPage1.PageVisible = false;
            this.TabPage2.PageVisible = true;
            this.TabPage3.PageVisible = false;
            this.TabControl1.SelectedTabPage = TabPage2;
            ClearText(1);
        }

        //创建流程图
        public void CreateFlowMap(string sModelID, string sModelName)
        {
            //查找是否存在流程图   
            string sFilter = "sModelCode =  '" + sModelID + "'";
            TabControl2.Visible = true;
            Boolean bExists = false;
            //查找模块流程图是否存在
            foreach (DevExpress.XtraTab.XtraTabPage tabPage in TabControl2.TabPages)
            {
                if (tabPage.Name == sModelID)
                {
                    tabPage.PageVisible = true;
                    TabControl2.SelectedTabPage = tabPage;
                    bExists = true;
                }
                else
                {
                    tabPage.PageVisible = false;
                }
            }
            //不存在需要增加页面
            if (!bExists)
            {
                if (flowMapDV.Table == null || flowMapDV.Table.Rows.Count < 1)
                {
                    DevExpress.XtraTab.XtraTabPage newPage = new DevExpress.XtraTab.XtraTabPage();
                    newPage.Name = sModelID;
                    newPage.Text = sModelName;
                    TabControl2.TabPages.Add(newPage);
                    TabControl2.SelectedTabPage = newPage;
                }
                else
                {
                    DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                    page.Name = sModelID;
                    page.Text = sModelName;
                    TabControl2.TabPages.Add(page);
                    TabControl2.SelectedTabPage = page;
                    //循环数据，动态生成按钮或直线
                    foreach (DataRow dr in flowMapDV.Table.Rows)
                    {
                        BillInfo info = new BillInfo();
                        //info.sModel = dr["sModelCode"].ToString();
                        //info.sID = dr["sID"].ToString();
                        info.sFrmName = dr["sFrmName"].ToString();
                        info.sFrmCaption = dr["sCaption"].ToString();
                        info.sFrmType = dr["sFrmType"].ToString();
                        info.sMsgID = dr["sMsgID"].ToString();

                        //如果是按钮，则创建按钮
                        if (Equals(dr["sType"].ToString().ToUpper(), "Btn".ToUpper()))
                        {
                            DevExpress.XtraEditors.SimpleButton sb = new DevExpress.XtraEditors.SimpleButton();
                            sb.Left = int.Parse(dr["iLeft"].ToString());
                            sb.Top = int.Parse(dr["iTop"].ToString());
                            sb.Width = int.Parse(dr["iWidth"].ToString());
                            sb.Height = int.Parse(dr["iHeight"].ToString());
                            sb.Name = dr["sFrmName"].ToString();
                            sb.Text = dr["sCaption"].ToString();

                            sb.Tag = info;
                            if (dr["mImage"] != DBNull.Value)
                            {
                                sb.Image = convertImg((Byte[])(dr["mImage"]));
                            }

                            sb.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
                            sb.Cursor = Cursors.Hand;
                            sb.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                            sb.Visible = true;
                            TabControl2.SelectedTabPage.Controls.Add(sb);

                            sb.Click += onClick;
                        }
                        else if (Equals(dr["sType"].ToString().ToUpper(), "Label".ToUpper())) //如果是Label，则创建Label
                        {
                            DevExpress.XtraEditors.LabelControl lbl = new DevExpress.XtraEditors.LabelControl();
                            lbl.Left = int.Parse(dr["iLeft"].ToString());
                            lbl.Top = int.Parse(dr["iTop"].ToString());
                            lbl.Width = int.Parse(dr["iWidth"].ToString());
                            lbl.Height = int.Parse(dr["iHeight"].ToString());
                            lbl.Name = dr["sFrmName"].ToString();
                            lbl.Text = dr["sCaption"].ToString();

                            lbl.Tag = info;
                            if (dr["sFrmName"].ToString() != "")
                            {
                                lbl.Cursor = Cursors.Hand;
                            }
                            FontStyle fs = new FontStyle();
                            if (Boolean.Parse(dr["bUnderLine"].ToString()))
                            {
                                fs = FontStyle.Underline;
                            }
                            else
                            {
                                fs = FontStyle.Bold;
                            }
                            Font f = new Font("宋体", int.Parse(dr["iFontSize"].ToString()), fs);
                            lbl.Font = f;

                            lbl.ForeColor = Color.FromName(dr["sColor"].ToString());
                            lbl.Visible = true;
                            TabControl2.SelectedTabPage.Controls.Add(lbl);

                            lbl.Click += onClick;
                        }
                        else if (Equals(dr["sType"].ToString().ToUpper(), "Line".ToUpper())) //如果是Line，则创建Line
                        {
                            KzxLine line = new KzxLine();
                            TabControl2.SelectedTabPage.Controls.Add(line);

                            line.Name = dr["sFrmName"].ToString();
                            line.Tag = "";
                            line.lineWidth = int.Parse(dr["iLineWidth"].ToString());
                            string sColor = dr["sColor"].ToString().Trim();
                            switch (sColor)
                            {
                                case "Black":
                                    line.LineColor = KzxLine.ColorType.Black;
                                    break;
                                case "Blue":
                                    line.LineColor = KzxLine.ColorType.Blue;
                                    break;
                                case "Green":
                                    line.LineColor = KzxLine.ColorType.Green;
                                    break;
                                case "Lime":
                                    line.LineColor = KzxLine.ColorType.Lime;
                                    break;
                                case "Red":
                                    line.LineColor = KzxLine.ColorType.Red;
                                    break;
                                case "Yellow":
                                    line.LineColor = KzxLine.ColorType.Yellow;
                                    break;
                                default:
                                    line.LineColor = KzxLine.ColorType.Black;
                                    break;
                            }
                            line.IsSolid = Boolean.Parse(dr["bSolid"].ToString());
                            switch (dr["sArrowType"].ToString().Trim())
                            {
                                case "Start":
                                    line.ArrowPosition = KzxLine.ArrowType.Start;
                                    break;
                                case "End":
                                    line.ArrowPosition = KzxLine.ArrowType.End;
                                    break;
                                case "All":
                                    line.ArrowPosition = KzxLine.ArrowType.All;
                                    break;
                                case "":
                                    line.ArrowPosition = KzxLine.ArrowType.None;
                                    break;
                            }
                            if (Boolean.Parse(dr["bHorizon"].ToString()))
                            {
                                line.LineStyle = KzxLine.LineType.Horizontal;
                                line.Left = int.Parse(dr["iLeft"].ToString());
                                line.Top = int.Parse(dr["iTop"].ToString()) - int.Parse(dr["iLineWidth"].ToString()) * 2 - 5;
                                line.Width = int.Parse(dr["iWidth"].ToString()) - int.Parse(dr["iLeft"].ToString());
                                line.Height = int.Parse(dr["iLineWidth"].ToString()) * 2 + 10;
                            }
                            else
                            {
                                line.LineStyle = KzxLine.LineType.Vertical;
                                line.Left = int.Parse(dr["iLeft"].ToString()) - int.Parse(dr["iLineWidth"].ToString()) * 2 - 5;
                                line.Top = int.Parse(dr["iTop"].ToString());
                                line.Width = int.Parse(dr["iLineWidth"].ToString()) * 2 + 10;
                                line.Height = int.Parse(dr["iHeight"].ToString()) - int.Parse(dr["iTop"].ToString());
                            }
                            line.Visible = true;

                            line.Click += onClick;
                        }
                    }
                } 
            } 
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        #region 图片处理
        /// <summary>
        /// 点击上传图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picture1_Click(object sender, EventArgs e)
        {
            if (this.openDialog1.ShowDialog() == DialogResult.OK)
            {
                picture1.Image = Image.FromFile(this.openDialog1.FileName);
            }
        }
        /// <summary>
        /// 用流的方式来存储图片
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private byte[] convertByte(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, img.RawFormat);
            byte[] bytes = ms.ToArray();
            ms.Close();
            return bytes;
        }
        private Image convertImg(byte[] datas)
        {
            MemoryStream ms = new MemoryStream(datas);
            Image img = Image.FromStream(ms, true);
            ms.Close();
            return img;
        }

        #endregion

        #region 窗体列表处理

        //获得窗体列表
        private void GetFrmList(int flag)
        {
            string sFrmName = "";
            if (flag == 0)
            {
                sFrmName = this.Edt_FrmBtn.Text.ToString();
                this.Edt_FrmBtn.Properties.View.Columns.Clear();
                this.Edt_FrmBtn.EditValue = "";
                try
                {
                    this.Edt_FrmBtn.Properties.DataSource = null;

                    this.Edt_FrmBtn.Properties.View.Columns.AddField("sFrmName");
                    this.Edt_FrmBtn.Properties.View.Columns["sFrmName"].Caption = "窗体编码";
                    this.Edt_FrmBtn.Properties.View.Columns["sFrmName"].Width = 80;
                    this.Edt_FrmBtn.Properties.View.Columns["sFrmName"].Visible = true;
                    //this.Edt_FrmBtn.Properties.View.Columns["sFrmName"].Alignment = DevExpress.Utils.HorzAlignment.Near;

                    this.Edt_FrmBtn.Properties.View.Columns.AddField("sCaption");
                    this.Edt_FrmBtn.Properties.View.Columns["sCaption"].Caption = "窗体描述";
                    this.Edt_FrmBtn.Properties.View.Columns["sCaption"].Width = 80;
                    this.Edt_FrmBtn.Properties.View.Columns["sCaption"].Visible = true;
                    //this.Edt_FrmBtn.Properties.View.Columns["sCaption"].Alignment = DevExpress.Utils.HorzAlignment.Near;

                    this.Edt_FrmBtn.Properties.View.Columns.AddField("sFrmType");
                    this.Edt_FrmBtn.Properties.View.Columns["sFrmType"].Caption = "窗体类型";
                    this.Edt_FrmBtn.Properties.View.Columns["sFrmType"].Width = 80;
                    this.Edt_FrmBtn.Properties.View.Columns["sFrmType"].Visible = true;
                    //this.Edt_FrmBtn.Properties.View.Columns["sFrmType"].Alignment = DevExpress.Utils.HorzAlignment.Near;

                    this.Edt_FrmBtn.Properties.View.Columns.AddField("sMsgID");
                    this.Edt_FrmBtn.Properties.View.Columns["sMsgID"].Caption = "语言标识";
                    this.Edt_FrmBtn.Properties.View.Columns["sMsgID"].Width = 80;
                    this.Edt_FrmBtn.Properties.View.Columns["sMsgID"].Visible = true;
                    //this.Edt_FrmBtn.Properties.View.Columns["sMsgID"].Alignment = DevExpress.Utils.HorzAlignment.Near;

                    this.Edt_FrmBtn.Properties.View.Columns.AddField("sID");
                    this.Edt_FrmBtn.Properties.View.Columns["sID"].Caption = "节点编码";
                    this.Edt_FrmBtn.Properties.View.Columns["sID"].Width = 20;
                    //this.Edt_FrmBtn.Properties.View.Columns["sID"].Alignment = DevExpress.Utils.HorzAlignment.Near;
                    this.Edt_FrmBtn.Properties.View.Columns["sID"].Visible = false;

                    this.Edt_FrmBtn.Properties.DisplayMember = "sFrmName";
                    this.Edt_FrmBtn.Properties.ValueMember = "sFrmName";

                    //this.Edt_FrmBtn.Text = sFrmName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "软件提示！");
                    throw ex;
                }
            }
            else if (flag == 1)
            {
                sFrmName = this.edt_FrmLabel.Text.ToString();
                this.edt_FrmLabel.Properties.View.Columns.Clear();
                this.edt_FrmLabel.EditValue = "";
                try
                {
                    this.edt_FrmLabel.Properties.DataSource = null;

                    this.edt_FrmLabel.Properties.View.Columns.AddField("sFrmName");
                    this.edt_FrmLabel.Properties.View.Columns["sFrmName"].Caption = "窗体编码";
                    this.edt_FrmLabel.Properties.View.Columns["sFrmName"].Width = 80;
                    this.edt_FrmLabel.Properties.View.Columns["sFrmName"].Visible = true;
                    //this.edt_FrmLabel.Properties.View.Columns["sFrmName"].Alignment = DevExpress.Utils.HorzAlignment.Near;

                    this.edt_FrmLabel.Properties.View.Columns.AddField("sCaption");
                    this.edt_FrmLabel.Properties.View.Columns["sCaption"].Caption = "窗体描述";
                    this.edt_FrmLabel.Properties.View.Columns["sCaption"].Width = 80;
                    this.edt_FrmLabel.Properties.View.Columns["sCaption"].Visible = true;
                    //this.edt_FrmLabel.Properties.View.Columns["sCaption"].Alignment = DevExpress.Utils.HorzAlignment.Near;

                    this.edt_FrmLabel.Properties.View.Columns.AddField("sFrmType");
                    this.edt_FrmLabel.Properties.View.Columns["sFrmType"].Caption = "窗体类型";
                    this.edt_FrmLabel.Properties.View.Columns["sFrmType"].Width = 80;
                    this.edt_FrmLabel.Properties.View.Columns["sFrmType"].Visible = true;
                    //this.edt_FrmLabel.Properties.View.Columns["sFrmType"].Alignment = DevExpress.Utils.HorzAlignment.Near;

                    this.edt_FrmLabel.Properties.View.Columns.AddField("sMsgID");
                    this.edt_FrmLabel.Properties.View.Columns["sMsgID"].Caption = "语言标识";
                    this.edt_FrmLabel.Properties.View.Columns["sMsgID"].Width = 80;
                    this.edt_FrmLabel.Properties.View.Columns["sMsgID"].Visible = true;
                    //this.edt_FrmLabel.Properties.View.Columns["sMsgID"].Alignment = DevExpress.Utils.HorzAlignment.Near;

                    this.edt_FrmLabel.Properties.View.Columns.AddField("sID");
                    this.edt_FrmLabel.Properties.View.Columns["sID"].Caption = "节点编码";
                    this.edt_FrmLabel.Properties.View.Columns["sID"].Width = 20;
                    //this.edt_FrmLabel.Properties.View.Columns["sID"].Alignment = DevExpress.Utils.HorzAlignment.Near;
                    this.edt_FrmLabel.Properties.View.Columns["sID"].Visible = false;

                    this.edt_FrmLabel.Properties.DisplayMember = "sFrmName";
                    this.edt_FrmLabel.Properties.ValueMember = "sFrmName";

                    //this.Edt_FrmBtn.Text = sFrmName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "软件提示！");
                    throw ex;
                }
            }

        }

        #endregion
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_SaveButton_Click_1(object sender, EventArgs e)
        {
            BillInfo info = new BillInfo();
            //info.sID = Edt_FrmBtn.EditValue.ToString();
            info.sFrmName = Edt_FrmBtn.Text.ToString();
            info.sFrmCaption = Edt_CaptionBtn.Text.Trim();
            info.sMsgID = Edt_MsgBtn.Text.ToString().Trim();

            if (bAddBtn)
            {
                DevExpress.XtraEditors.SimpleButton sb = new DevExpress.XtraEditors.SimpleButton();
                sb.Left = int.Parse(Edt_LeftBtn.Text.Trim());
                sb.Top = int.Parse(Edt_TopBtn.Text.Trim());
                sb.Width = int.Parse(Edt_WidthBtn.Text.Trim());
                sb.Height = int.Parse(Edt_HightBtn.Text.Trim());
                sb.Name = Edt_FrmBtn.Text.Trim();
                sb.Text = Edt_CaptionBtn.Text.Trim();

                sb.Tag = info;

                sb.Image = picture1.Image;
                sb.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
                sb.Cursor = Cursors.Hand;
                sb.Visible = true;
                TabControl2.SelectedTabPage.Controls.Add(sb);

                sb.Click += onClick;
                MoveClass.BarcodeControl(sb);
                if (bFirstControl)
                {
                    MoveClass.BarcodeCreate();
                    bFirstControl = false;
                }
                this.TabControl2.Enabled = true;
                bAddBtn = false;

                this.kzxsBtnAddButton.Enabled = true;
                this.kzxsBtnAddLabel.Enabled = true;
                this.kzxsBtnAddLine.Enabled = true;
            }
            else if ((selectControl is DevExpress.XtraEditors.SimpleButton) && (DevExpress.XtraEditors.SimpleButton)selectControl != null)
            {
                ((DevExpress.XtraEditors.SimpleButton)selectControl).Left = int.Parse(Edt_LeftBtn.Text.Trim());
                ((DevExpress.XtraEditors.SimpleButton)selectControl).Top = int.Parse(Edt_TopBtn.Text.Trim());
                ((DevExpress.XtraEditors.SimpleButton)selectControl).Width = int.Parse(Edt_WidthBtn.Text.Trim());
                ((DevExpress.XtraEditors.SimpleButton)selectControl).Height = int.Parse(Edt_HightBtn.Text.Trim());
                ((DevExpress.XtraEditors.SimpleButton)selectControl).Name = Edt_FrmBtn.Text.Trim();
                ((DevExpress.XtraEditors.SimpleButton)selectControl).Text = Edt_CaptionBtn.Text.Trim();

                ((DevExpress.XtraEditors.SimpleButton)selectControl).Tag = info;
                ((DevExpress.XtraEditors.SimpleButton)selectControl).Image = picture1.Image;
                ((DevExpress.XtraEditors.SimpleButton)selectControl).ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
                ((DevExpress.XtraEditors.SimpleButton)selectControl).Cursor = Cursors.Hand;
            }
        }
        /// <summary>
        /// 点击图片，上传图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picture1_Click_1(object sender, EventArgs e)
        {
            if (this.openDialog1.ShowDialog() == DialogResult.OK)
            {
                picture1.Image = Image.FromFile(this.openDialog1.FileName);
            }
        }
        /// <summary>
        /// 设计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Design_Click(object sender, EventArgs e)
        {
            if (this.TabControl2.SelectedTabPage == null) return;

            GetFrmList(0);
            GetFrmList(1);

            bDesign = true;
            this.Btn_SaveButton.Enabled = true;
            this.Btn_SaveLabel.Enabled = true;
            this.Btn_SaveLine.Enabled = true;

            this.Btn_Design.Enabled = false;
            this.Btn_Post.Enabled = true;
            this.kzxsBtnAddButton.Enabled = true;
            this.kzxsBtnAddLabel.Enabled = true;
            this.kzxsBtnAddLine.Enabled = true;
            this.Btn_Cancel.Enabled = true;

            Boolean bHasControl = false;
            foreach (Control c in TabControl2.SelectedTabPage.Controls)
            {
                if ((c is DevExpress.XtraEditors.SimpleButton) || (c is DevExpress.XtraEditors.LabelControl)
                     || (c is KzxLine))
                {
                    MoveClass.BarcodeControl(c);
                    iCount = iCount + 1;
                    bHasControl = true;
                }
            }
            if (bHasControl)
            {
                MoveClass.BarcodeCreate();
            }
            else
            {
                bFirstControl = true;
            }
            this.Btn_Design.Enabled = false;
            this.Btn_Post.Enabled = true;
        }

        private void Btn_Post_Click(object sender, EventArgs e)
        {
            for (int i = dtDesign.Rows.Count - 1; i >= 0; i--)
            {
                if (Equals(dtDesign.Rows[i]["sModelCode"].ToString().Trim().ToUpper(), sModel.ToUpper()))
                    dtDesign.Rows[i].Delete();
            }
             
            int iOrder = 0; 
            foreach (Control c in TabControl2.SelectedTabPage.Controls)
            {
                //string sID = "";
                string sFrmName = "";
                string sCaption = "";
                string sType = "";
                string sBtnPositon = "";
                string sArrowType = "";
                string sFrmType = "";
                string sMsgID = "";
                string sColor = "";
                Boolean bSolid = true;
                Boolean bUnderLine = false;
                Boolean bHorzion = true;

                int iLeft = 0;
                int iTop = 0;
                int iWidth = 0;
                int iHeight = 0;
                int iLineWidth = 0;
                float iFontSize = 0;
                BillInfo info = c.Tag as BillInfo;

                if (c is DevExpress.XtraEditors.SimpleButton)
                {
                    //Button按钮
                    //sID = info.sID;
                    sFrmName = c.Name.ToString().Trim();
                    sCaption = c.Text.ToString().Trim();
                    sFrmType = info.sFrmType;
                    sMsgID = info.sMsgID;
                    sType = "Btn";
                    iLeft = c.Left;
                    iTop = c.Top;
                    iWidth = c.Width;
                    iHeight = c.Height;
                    sBtnPositon = "TopCenter";
                }
                else if (c is DevExpress.XtraEditors.LabelControl)
                {
                    //Label控件
                    //sID = info.sID;
                    sFrmName = c.Name.ToString().Trim();
                    sCaption = c.Text.ToString().Trim();
                    sFrmType = info.sFrmType;
                    sMsgID = info.sMsgID;
                    sType = "Label";
                    iLeft = c.Left;
                    iTop = c.Top;
                    iWidth = c.Width;
                    iHeight = c.Height;

                    if (c.Font.Underline)  //下划线
                    {
                        bUnderLine = true;
                    }
                    iFontSize = ((DevExpress.XtraEditors.LabelControl)c).Font.Size; //字体大小
                    sColor = ((DevExpress.XtraEditors.LabelControl)c).ForeColor.Name.ToString(); //字体颜色

                }
                else if (c is KzxLine)
                {
                    sCaption = "Line";
                    sType = "Line";
                    if (((KzxLine)c).ArrowPosition == KzxLine.ArrowType.Start)
                    {
                        sArrowType = "Start";
                    }
                    else if (((KzxLine)c).ArrowPosition == KzxLine.ArrowType.End)
                    {
                        sArrowType = "End";
                    }
                    else if (((KzxLine)c).ArrowPosition == KzxLine.ArrowType.All)
                    {
                        sArrowType = "All";
                    }
                    else if (((KzxLine)c).ArrowPosition == KzxLine.ArrowType.None)
                    {
                        sArrowType = "None";
                    }
                    //颜色
                    if (((KzxLine)c).LineColor == KzxLine.ColorType.Black)
                    {
                        sColor = "Black";
                    }
                    else if (((KzxLine)c).LineColor == KzxLine.ColorType.Blue)
                    {
                        sColor = "Blue";
                    }
                    else if (((KzxLine)c).LineColor == KzxLine.ColorType.Green)
                    {
                        sColor = "Green";
                    }
                    else if (((KzxLine)c).LineColor == KzxLine.ColorType.Lime)
                    {
                        sColor = "Lime";
                    }
                    else if (((KzxLine)c).LineColor == KzxLine.ColorType.Red)
                    {
                        sColor = "Red";
                    }
                    else if (((KzxLine)c).LineColor == KzxLine.ColorType.Yellow)
                    {
                        sColor = "Yellow";
                    }
                    else if (((KzxLine)c).LineColor == KzxLine.ColorType.Black)
                    {
                        sColor = "Black";
                    }
                    iLineWidth = ((KzxLine)c).lineWidth;

                    if (!((KzxLine)c).IsSolid)
                    {
                        bSolid = false;
                    }

                    if (((KzxLine)c).LineStyle == KzxLine.LineType.Horizontal)
                    {
                        bHorzion = true;
                        iLeft = ((KzxLine)c).Left;
                        iTop = ((KzxLine)c).Top + ((KzxLine)c).lineWidth * 2 + 5;
                        iWidth = ((KzxLine)c).Width + ((KzxLine)c).Left;
                        iHeight = ((KzxLine)c).Top + ((KzxLine)c).lineWidth * 2 + 5;
                    }
                    else
                    {
                        bHorzion = false;
                        iLeft = ((KzxLine)c).Left + ((KzxLine)c).lineWidth * 2 + 5;
                        iTop = ((KzxLine)c).Top;
                        iWidth = ((KzxLine)c).Left + ((KzxLine)c).lineWidth * 2 + 5;
                        iHeight = ((KzxLine)c).Top + ((KzxLine)c).Height;
                    }

                    //gc[i] = c;
                }

                if ((c is DevExpress.XtraEditors.SimpleButton) || (c is DevExpress.XtraEditors.LabelControl)
                    || (c is KzxLine))
                {
                    DataRow newRow = dtDesign.NewRow();
                    newRow["uGuid"] = Guid.NewGuid().ToString("D");
                    newRow["sModelCode"] = sModel;
                    //newRow["sID"] = sID;
                    newRow["sFrmName"] = sFrmName;
                    newRow["sCaption"] = sCaption;
                    newRow["sType"] = sType;
                    newRow["iLeft"] = iLeft;
                    newRow["iTop"] = iTop;
                    newRow["iWidth"] = iWidth;
                    newRow["iHeight"] = iHeight;
                    newRow["sBtnPositon"] = sBtnPositon;
                    newRow["sArrowType"] = sArrowType;
                    if (c is DevExpress.XtraEditors.SimpleButton)
                    {
                        if (((DevExpress.XtraEditors.SimpleButton)c).Image != null)
                            newRow["mImage"] = convertByte(((DevExpress.XtraEditors.SimpleButton)c).Image);
                    }
                    else
                    {
                        newRow["mImage"] = null;
                    }
                    //newRow["mImage"] = sModel;
                    newRow["iOrder"] = iOrder;
                    newRow["bActive"] = 1;
                    newRow["sFrmType"] = sFrmType;
                    newRow["sMsgID"] = sMsgID;
                    newRow["sColor"] = sColor;
                    newRow["iLineWidth"] = iLineWidth;
                    newRow["iFontSize"] = iFontSize;
                    newRow["sSysMode"] = "";
                    newRow["bSolid"] = bSolid;
                    newRow["bUnderLine"] = bUnderLine;
                    newRow["bHorizon"] = bHorzion;
                    dtDesign.Rows.Add(newRow);

                    iOrder = iOrder + 1;
                }
            }

            //TODO:保存到数据库操作

            this.Btn_SaveButton.Enabled = false;
            this.Btn_SaveLabel.Enabled = false;
            this.Btn_SaveLine.Enabled = false;

            bDesign = false;
            bAddBtn = false;
            bAddLabel = false;
            bAddLine = false;

            this.Btn_Design.Enabled = true;
            this.Btn_Post.Enabled = false;
            this.kzxsBtnAddButton.Enabled = false;
            this.kzxsBtnAddLabel.Enabled = false;
            this.kzxsBtnAddLine.Enabled = false;
            this.Btn_Cancel.Enabled = false;

            this.TabControl2.Enabled = true;

            //TODO:从数据库加载数据流并绑定数据  


            this.TabControl2.TabPages.Remove(this.TabControl2.SelectedTabPage);
            CreateFlowMap(sModel, sModelName);
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Cancel_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sBtnDelete_Click(object sender, EventArgs e)
        {
            if (selectControl is DevExpress.XtraEditors.SimpleButton)
            {
                MoveClass.BarcodeCancel(((DevExpress.XtraEditors.SimpleButton)selectControl));
                ((DevExpress.XtraEditors.SimpleButton)selectControl).Parent.Controls.Remove(((DevExpress.XtraEditors.SimpleButton)selectControl));
            }
            else if (selectControl is DevExpress.XtraEditors.LabelControl)
            {
                MoveClass.BarcodeCancel(((DevExpress.XtraEditors.LabelControl)selectControl));
                ((DevExpress.XtraEditors.LabelControl)selectControl).Parent.Controls.Remove(((DevExpress.XtraEditors.LabelControl)selectControl));
            }
            else if (selectControl is KzxLine)
            {
                MoveClass.BarcodeCancel(((KzxLine)selectControl));
                ((KzxLine)selectControl).Parent.Controls.Remove(((KzxLine)selectControl));
            }
        }
    }
}
