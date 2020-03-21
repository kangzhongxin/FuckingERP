using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kzx.Common
{
    public class KzxMessageBox
    {
        /// <summary>
        /// 标题 
        /// </summary>
        private static string caption;

        /// <summary>
        /// 按钮(默认“OK”)
        /// </summary>
        private static MessageBoxButtons buttons;

        /// <summary>
        /// 图标(默认“information”)
        /// </summary>
        private static MessageBoxIcon icon;

        /// <summary>
        /// 默认按钮（默认“button1”）
        /// </summary>
        private static MessageBoxDefaultButton defaultButton;

        /// <summary>
        /// 静态构造函数，初始化数据
        /// </summary>
        static KzxMessageBox()
        {
            if (SysVar.loginType == 1)
            {
                caption = "Stephen's UserControl";
            }
            else
            {
                caption = sysClass.ssLoadMsgOrDefault("SYS000008", "Stephen's UserControl");
            }
            buttons = MessageBoxButtons.OK;
            icon = MessageBoxIcon.Information;
            defaultButton = MessageBoxDefaultButton.Button1;
        } 
        /// <summary>
        /// 显示具有指定文本、标题、按钮、图标和默认按钮的消息框
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        public static DialogResult Show(string text)
        {
            return Show(text, buttons, icon, defaultButton, null);
        }

        /// <summary>
        /// 显示具有指定文本、标题、按钮、图标和默认按钮的消息框 
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        public static DialogResult Show(string text, int pFormWidth = 0, int pFormHeight = 0)
        {
            return Show(text, buttons, icon, defaultButton, null, pFormWidth, pFormHeight);
        }

        /// <summary>
        /// 显示具有指定文本、标题、按钮、图标和默认按钮的消息框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static DialogResult Show(string text, Form parent)
        {
            return Show(text, buttons, icon, defaultButton, parent);
        }

        /// <summary>
        /// 显示具有指定文本、标题、按钮、图标和默认按钮的消息框
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="buttons">按钮</param>
        /// <returns></returns>
        public static DialogResult Show(string text, MessageBoxButtons buttons)
        {
            return Show(text, buttons, icon, defaultButton, null);
        }

        /// <summary>
        /// 显示具有指定文本、标题、按钮、图标和默认按钮的消息框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="buttons"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static DialogResult Show(string text, MessageBoxButtons buttons, Form parent)
        {
            return Show(text, buttons, icon, defaultButton, parent);
        }

        /// <summary>
        /// 显示具有指定文本、标题、按钮、图标和默认按钮的消息框
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="caption">标题</param>
        /// <param name="buttons">按钮</param>
        /// <param name="icon">图标</param>
        /// <returns></returns>
        public static DialogResult Show(string text, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(text, buttons, icon, defaultButton, null);
        }

        /// <summary>
        /// 显示具有指定文本、标题、按钮、图标和默认按钮的消息框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static DialogResult Show(string text, MessageBoxButtons buttons, MessageBoxIcon icon, Form parent)
        {
            return Show(text, buttons, icon, defaultButton, parent);
        }

        /// <summary>
        /// 显示具有指定文本、标题、按钮、图标和默认按钮的消息框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <param name="defaultButton"></param>
        /// <returns></returns>
        public static DialogResult Show(string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Show(text, buttons, icon, defaultButton, null);
        }

        /// <summary>
        /// 显示具有指定文本、标题、按钮、图标和默认按钮的消息框
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="caption">标题</param>
        /// <param name="buttons">按钮</param>
        /// <param name="icon">图标</param>
        /// <param name="defaultButton">默认按钮</param>
        /// <returns>System.Windows.Forms.DialogResult 值之一</returns>
        public static DialogResult Show(string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, Form parent, int pFormWidth = 0, int pFormHeight = 0)
        {
            using (frm_MessageBox frm = new frm_MessageBox(text, caption, buttons, icon, defaultButton))
            {
                if (parent == null || parent.IsDisposed)
                {
                    frm.StartPosition = FormStartPosition.CenterScreen; 
                    if (pFormWidth != 0) frm.Width = pFormWidth;
                    if (pFormHeight != 0) frm.Height = pFormHeight;

                    return frm.ShowDialog();
                }
                else
                {
                    frm.StartPosition = FormStartPosition.CenterParent; 
                    if (pFormWidth != 0) frm.Width = pFormWidth;
                    if (pFormHeight != 0) frm.Height = pFormHeight;

                    return frm.ShowDialog(parent);
                }
            }
        }

        public static DialogResult Show(Form parent, MessageBoxModel pMessageBoxModel)
        {
            using (frm_MessageBox frm = new frm_MessageBox(pMessageBoxModel))
            {
                if (parent == null || parent.IsDisposed)
                {
                    frm.StartPosition = FormStartPosition.CenterScreen; 
                    if (pMessageBoxModel.FormWidth != 0) frm.Width = pMessageBoxModel.FormWidth;
                    if (pMessageBoxModel.FormHeight != 0) frm.Height = pMessageBoxModel.FormHeight;

                    return frm.ShowDialog();
                }
                else
                {
                    frm.StartPosition = FormStartPosition.CenterParent; 
                    if (pMessageBoxModel.FormWidth != 0) frm.Width = pMessageBoxModel.FormWidth;
                    if (pMessageBoxModel.FormHeight != 0) frm.Height = pMessageBoxModel.FormHeight;

                    return frm.ShowDialog(parent);
                }
            }
        }
    }
}
