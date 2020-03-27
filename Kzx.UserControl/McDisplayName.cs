using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.Reflection;

namespace Kzx.UserControl
{
    /// <summary>
    /// 属性的DisplayNameAttribute特性
    /// </summary>
    public class McDisplayName : DisplayNameAttribute
    {
        private string _property = string.Empty;
        private string _displayname = string.Empty;
        private MethodInfo _methodInfo = null;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="e">属性名称</param>
        public McDisplayName(string e)
            : base(e)
        {
            this._property = e;
            this._displayname = GetDisplayName(this._property);
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return this._displayname;
            }
        }

        /// <summary>
        /// 从根目录下的Properties.xml取显示名称
        /// </summary>
        /// <param name="propertyname">属性名称</param>
        /// <returns>返回显示的名称</returns>
        protected virtual string GetDisplayName(string propertyname)
        {
            string msgid = string.Empty;
            string displayname = string.Empty;
            string filepath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Properties.xml";
            XmlDocument doc = null;
            XmlNode root = null;
            XmlNode node = null;
            XmlAttribute attr = null;
            string type = string.Empty;

            doc = new XmlDocument();
            displayname = propertyname;
            filepath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Properties.xml";
            if (System.IO.File.Exists(filepath))
            {
                doc.Load(filepath);
                root = doc.DocumentElement;
                attr = root.Attributes[0];
                if (string.IsNullOrEmpty(attr.Value))
                {
                    type = "0";
                }
                else
                {
                    type = attr.Value;
                }
                node = root.SelectSingleNode("//property[@name=\"" + propertyname + "\"]");
                if (node != null)
                {
                    attr = node.Attributes[Convert.ToInt32(type) + 1];
                    for (int i = 0; i < node.Attributes.Count; i++)
                    {
                        if (node.Attributes[i].Name.Equals("id") == true)
                        {
                            msgid = node.Attributes[i].Value;
                        }
                    }
                    displayname = GetLanguage(msgid, attr.Value);
                }
            }
            else
            {
                //filepath = Properties.Resources.Properties;
                //doc.LoadXml(filepath);
                //root = doc.DocumentElement;
                //attr = root.Attributes[0];
                //if (string.IsNullOrEmpty(attr.Value))
                //{
                //    type = "0";
                //}
                //else
                //{
                //    type = attr.Value;
                //}
                //node = root.SelectSingleNode("//property[@name=\"" + propertyname + "\"]");
                //if (node != null)
                //{
                //    attr = node.Attributes[Convert.ToInt32(type) + 1];
                //    displayname = attr.Value;
                //}
            }
            return displayname;
        }

        /// <summary>
        /// 获取多语言文本
        /// </summary>
        /// <param name="messageCode">语言文本标识</param>
        /// <param name="defaultMessage">默认的文本</param>
        /// <returns>取到的文本</returns>
        protected virtual string GetLanguage(string messageCode, string defaultMessage)
        {
            string text = string.Empty;

            try
            {
                text = defaultMessage;
                string filepath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "KzxCommon.dll");
                Assembly assembly = null;
                object obj = null;

                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                for (int i = 0; i < assemblies.Length; i++)
                {
                    if (assemblies[i].GetName().Name.Equals("KzxCommon", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        assembly = assemblies[i];
                        break;
                    }
                }
                if (assembly == null)
                {
                    assembly = Assembly.LoadFrom(filepath);
                }
                obj = assembly.CreateInstance("KzxCommon.sysClass");
                text = defaultMessage;
                if (_methodInfo == null)
                {
                    if (obj != null)
                    {
                        _methodInfo = obj.GetType().GetMethod("ssLoadMsg", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                        if (_methodInfo != null)
                        {
                            text = _methodInfo.Invoke(obj, new object[] { messageCode }).ToString();
                        }
                    }
                }
                else
                {
                    text = _methodInfo.Invoke(obj, new object[] { messageCode }).ToString();
                }
            }
            catch (Exception ex)
            {

            }
            return string.IsNullOrWhiteSpace(text) == true ? defaultMessage : text;
        }
    }
}
