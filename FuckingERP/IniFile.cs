using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FuckingERP
{
    public class IniFile
    {
        public string Path;

        public IniFile(string path) 
        {
            this.Path = path;
        }
 
        #region 声明读写INI文件的API函数 

        [DllImport("kernel32")] 
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath); 

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, StringBuilder retVal, int size, string filePath); 

        #endregion

        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="section">段落</param>
        /// <param name="key">键</param>
        /// <param name="iValue">值</param>
        public void IniWriteValue(string section, string key, string iValue)
        {
            WritePrivateProfileString(section, key, iValue, this.Path);
        }

        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">段落</param>
        /// <param name="key">键</param>
        /// <returns>返回的键值</returns>
        public string IniReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(4000);

            int i = GetPrivateProfileString(section, key, "", temp, 4000, this.Path);
            return temp.ToString();
        }    
        
    }
}
