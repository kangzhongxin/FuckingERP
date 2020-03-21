using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kzx.AppCore
{
    /// <summary>
    /// 本地日志操作
    /// </summary>
    public static class LogCore
    {
        #region 字段

        private static readonly Encoding _lastErrorLogEncoding = Encoding.UTF8;
        private static readonly string _lastErrorLogFolderPath;
        private static readonly string _lastErrorLogFilePath;

        #endregion

        #region 构造&初始化

        static LogCore()
        {
            _lastErrorLogFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
            _lastErrorLogFilePath = Path.Combine(_lastErrorLogFolderPath, "KzxERPErrorLast.txt");
        }

        #endregion

        #region 记录·最后日志

        /// <summary>
        /// 记录最后错误
        /// </summary>
        /// <param name="pEx"></param>
        public static void LastError(Exception pEx)
        {
            if (pEx == null)
                return;

            LastError(pEx.ToString());
        }

        /// <summary>
        /// 记录最后错误
        /// </summary>
        /// <param name="pLog"></param>
        /// <param name="pArgs"></param>
        public static void LastError(string pLog, params string[] pArgs)
        {
            if (string.IsNullOrWhiteSpace(pLog))
                return;

            try
            {
                var logText = string.Format(pLog, pArgs);

                //创建目录
                if (!Directory.Exists(_lastErrorLogFolderPath))
                    Directory.CreateDirectory(_lastErrorLogFolderPath);

                //写日志
                using (var writer = new StreamWriter(_lastErrorLogFilePath, false, _lastErrorLogEncoding))
                {
                    writer.Write(logText);
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception)
            {
                //不做任何处理
            }
        }

        #endregion
    }
}
