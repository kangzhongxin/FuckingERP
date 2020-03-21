using DevExpress.XtraNavBar;
using FuckingERP.重写控件_弹窗类;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace FuckingERP
{
    public partial class frm_LeftMenu : DockContent
    {
        private static frm_LeftMenu fInstance; 
        public frm_LeftMenu()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 单例
        /// </summary>
        /// <returns></returns>
        public static frm_LeftMenu GetInstance()
        {
            if (fInstance == null)
            {
                fInstance = new frm_LeftMenu();
            }
            return fInstance;
        }
        /// <summary>
        /// 窗体关闭，释放内存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_LeftMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            fInstance = null;
        }
        /// <summary>
        /// 菜单选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBarControl1_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            if (e.Link.ItemName == "nbShowControl")
            {
                frm_ShowControl dummyDoc = CreateNewDocument("重写控件-弹窗类");
                if (this.DockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                {
                    dummyDoc.MdiParent = this;
                    dummyDoc.Show();
                }
                else
                    dummyDoc.Show(this.DockPanel);
            }
            else if (e.Link.ItemName == "nbDataControl")
            {

            }
            else if (e.Link.ItemName == "nbOtherControl")
            {

            }
        }


        private IDockContent FindDocument(string text)
        {
            if (this.DockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    if (form.Text == text)
                        return form as IDockContent;

                return null;
            }
            else
            {
                foreach (IDockContent content in this.DockPanel.Documents)
                    if (content.DockHandler.TabText == text)
                        return content;

                return null;
            }
        }
        private frm_ShowControl CreateNewDocument(string sCaption="")
        {
            frm_ShowControl dummyDoc = frm_ShowControl.GetInstance(); 
            int count = 1;
            string text = string.Empty;
            if (!string.IsNullOrEmpty(sCaption))
            {
                text = sCaption;
            }
            else
            {
                text = "Document" + count.ToString();
            } 
            dummyDoc.Text = text;
            return dummyDoc;
        }

    }
}
