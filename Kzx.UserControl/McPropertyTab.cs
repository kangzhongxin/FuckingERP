using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace Kzx.UserControl
{
    // A McPropertyTab property tab lists properties by the 
    // category of the type of each property.
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class McPropertyTab : PropertyTab
    {
        private string _XmlFilePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\Components.xml";

        [BrowsableAttribute(true)]
        // This string contains a Base-64 encoded and serialized example property tab image.
        private string img = "AAEAAAD/////AQAAAAAAAAAMAgAAAFRTeXN0ZW0uRHJhd2luZywgVmVyc2lvbj0xLjAuMzMwMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWIwM2Y1ZjdmMTFkNTBhM2EFAQAAABVTeXN0ZW0uRHJhd2luZy5CaXRtYXABAAAABERhdGEHAgIAAAAJAwAAAA8DAAAA9gAAAAJCTfYAAAAAAAAANgAAACgAAAAIAAAACAAAAAEAGAAAAAAAAAAAAMQOAADEDgAAAAAAAAAAAAD///////////////////////////////////9ZgABZgADzPz/zPz/zPz9AgP//////////gAD/gAD/AAD/AAD/AACKyub///////+AAACAAAAAAP8AAP8AAP9AgP////////9ZgABZgABz13hz13hz13hAgP//////////gAD/gACA/wCA/wCA/wAA//////////+AAACAAAAAAP8AAP8AAP9AgP////////////////////////////////////8L";

        public McPropertyTab()
        {
        }

        // Returns the properties of the specified component extended with 
        // a CategoryAttribute reflecting the name of the type of the property.
        public override System.ComponentModel.PropertyDescriptorCollection GetProperties(object component, System.Attribute[] attributes)
        {
            string type = string.Empty;
            XmlDocument doc = new XmlDocument();
            XmlNode root;
            XmlNode node;
            PropertyDescriptorCollection props;
            if (attributes == null)
                props = TypeDescriptor.GetProperties(component);
            else
                props = TypeDescriptor.GetProperties(component, attributes);


            PropertyInfo pi = null;
            pi = component.GetType().GetProperty("____Parent");
            if (pi != null)
            {
                object parent = pi.GetValue(component, null);
                if (parent != null)
                {
                    type = parent.GetType().Name.ToLower();
                }
                else
                {
                    type = component.GetType().Name.ToLower();
                }
            }
            else
            {
                type = component.GetType().Name.ToLower();
            }
            if (System.IO.File.Exists(this._XmlFilePath) == true)
            {
                doc.Load(this._XmlFilePath);
                root = doc.DocumentElement;
                node = root.SelectSingleNode(type);
            }
            else
            {
                node = null;
            }

            List<PropertyDescriptor> propArray = new List<PropertyDescriptor>();
            for (int i = 0; i < props.Count; i++)
            {
                if (node != null)
                {
                    for (int k = 0; k < node.ChildNodes.Count; k++)
                    {
                        if (props[i].Name.ToLower().Equals(node.ChildNodes[k].Attributes["name"].Value.ToLower()) == true)
                        {
                            propArray.Add(TypeDescriptor.CreateProperty(props[i].ComponentType, props[i], new CategoryAttribute(props[i].Category)));
                        }
                    }
                }
                else
                {
                    propArray.Add(TypeDescriptor.CreateProperty(props[i].ComponentType, props[i], new CategoryAttribute(props[i].Category)));
                }
            }
            return new PropertyDescriptorCollection(propArray.ToArray());
        }

        public override System.ComponentModel.PropertyDescriptorCollection GetProperties(object component)
        {
            return this.GetProperties(component, null);
        }

        // Provides the name for the property tab.
        public override string TabName
        {
            get
            {
                return "Properties by Type";
            }
        }

        // Provides an image for the property tab.
        public override System.Drawing.Bitmap Bitmap
        {
            get
            {
                Bitmap bmp = new Bitmap(DeserializeFromBase64Text(img));
                return bmp;
            }
        }

        // This method can be used to retrieve an Image from a block of Base64-encoded text.
        private Image DeserializeFromBase64Text(string text)
        {
            Image img = null;
            byte[] memBytes = Convert.FromBase64String(text);
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(memBytes);
            img = (Image)formatter.Deserialize(stream);
            stream.Close();
            return img;
        }
    }
}
