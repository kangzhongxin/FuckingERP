using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Kzx.AppCore
{
    /// <summary>
    /// INI 配置文件操作
    /// </summary>
    public class IniFileCore
    {
        #region 字段

        private string _filePath = string.Empty;

        #endregion

        #region DDLIMPORT

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        #endregion

        #region 构造&初始化

        public IniFileCore(string pFilePath)
        {
            _filePath = pFilePath;
        }

        #endregion

        #region 写入

        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="pSection"></param>
        /// <param name="pKey"></param>
        /// <param name="pValue"></param>
        public void Write(string pSection, string pKey, string pValue)
        {
            WritePrivateProfileString(pSection, pKey, pValue, _filePath);
        }

        #endregion

        #region 读取

        /// <summary>
        /// 读取INI配置项值
        /// </summary>
        /// <param name="pSection"></param>
        /// <param name="pKey"></param>
        /// <returns></returns>
        public string Read(string pSection, string pKey)
        {
            if (!File.Exists(_filePath))
                return string.Empty;

            var value = new StringBuilder(20480);
            GetPrivateProfileString(pSection, pKey, "", value, 20480, _filePath);

            return value.ToString();
        }

        #endregion
    }
}
