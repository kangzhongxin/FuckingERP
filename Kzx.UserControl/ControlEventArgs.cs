using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kzx.UserControl
{
    public class ControlEventArgs : EventArgs
    {
        private String _EventId;
        /// <summary>
        /// 控件的事件名称
        /// </summary>
        public String EventId
        {
            get
            {
                return this._EventId;
            }
            set
            {
                this._EventId = value.ToLower();
            }
        }

        private String _TableName;
        /// <summary>
        /// 控件的表名称
        /// </summary>
        public String TableName
        {
            get
            {
                return this._TableName;
            }
            set
            {
                this._TableName = value;
            }
        }

        private String _FieldName;
        /// <summary>
        /// 字段名称
        /// </summary>
        public String FieldName
        {
            get
            {
                return this._FieldName;
            }
            set
            {
                this._FieldName = value;
            }
        }

        private string _Key = string.Empty;
        /// <summary>
        /// 控件标识
        /// </summary>
        public string Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        private object _CurrentControl = null;
        /// <summary>
        /// 触发事件的控件
        /// </summary>
        public object CurrentControl
        {
            get
            {
                return this._CurrentControl;
            }
            set
            {
                this._CurrentControl = value;
            }
        }

        private object _SystemEventArgs = null;
        /// <summary>
        /// 控件事件的参数
        /// </summary>
        public object SystemEventArgs
        {
            get
            {
                return this._SystemEventArgs;
            }
            set
            {
                this._SystemEventArgs = value;
            }
        }

        /// <summary>
        /// 构造
        /// </summary>
        public ControlEventArgs()
            : base()
        {
        }

        /// <summary>
        /// 取对象obj中的属性名为propertyName的属性
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>取到的值，可能会为null</returns>
        public static object GetPropertyValue(object obj, string propertyName)
        {
            object value = null;
            PropertyInfo pi = null;

            if (obj != null)
            {
                pi = obj.GetType().GetProperty(propertyName);
                if (pi != null)
                {
                    value = pi.GetValue(obj, null);
                }
            }
            return value;
        }

        /// <summary>
        /// 设置对象obj中的属性名为propertyName的属性的值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">属性值</param>
        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            PropertyInfo pi = null;
            if (obj != null)
            {
                pi = obj.GetType().GetProperty(propertyName);
                if (pi != null)
                {
                    pi.SetValue(obj, value, null);
                }
            }
        }
    }
}
