using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Kzx.AppCore
{
    /// <summary>
    /// RSA 加密/解密工具
    /// </summary>
    public static class RSAEncryptUtil
    {
        #region 构造&初始化

        static RSAEncryptUtil()
        {

        }

        #endregion

        #region 加密

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="pValue">需要加密的字符串</param>
        /// <param name="pPublicKey">加密公共密钥</param>
        /// <returns></returns>
        public static string Encrypt(string pValue, string pPublicKey)
        {
            if (string.IsNullOrWhiteSpace(pPublicKey))
                throw new ArgumentException("解密失败，参数不正确。");

            if (string.IsNullOrWhiteSpace(pValue))
                return pValue;

            var valueBytes = Encoding.UTF8.GetBytes(pValue);

            using (var rsaCrypt = new RSACryptoServiceProvider())
            {
                rsaCrypt.PersistKeyInCsp = false;
                rsaCrypt.FromXmlString(pPublicKey);

                var bufferSize = (rsaCrypt.KeySize / 8 - 11);
                byte[] buffer = new byte[bufferSize];//待加密块

                using (MemoryStream msInput = new MemoryStream(valueBytes))
                {
                    using (MemoryStream msOutput = new MemoryStream())
                    {
                        int readLen;
                        while ((readLen = msInput.Read(buffer, 0, bufferSize)) > 0)
                        {
                            byte[] dataToEnc = new byte[readLen];
                            Array.Copy(buffer, 0, dataToEnc, 0, readLen);
                            byte[] encData = rsaCrypt.Encrypt(dataToEnc, false);
                            msOutput.Write(encData, 0, encData.Length);
                        }

                        byte[] result = msOutput.ToArray();
                        rsaCrypt.Clear();
                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        #endregion

        #region 解密

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="pValue">已加密字符串</param>
        /// <param name="pPrivateKey">私钥</param>
        /// <returns></returns>
        public static string Decrypt(string pValue, string pPrivateKey)
        {
            if (string.IsNullOrWhiteSpace(pPrivateKey))
                throw new ArgumentException("解密失败，参数不正确。");

            if (string.IsNullOrWhiteSpace(pValue))
                return pValue;

            var valueBytes = Convert.FromBase64String(pValue);

            using (RSACryptoServiceProvider rsaCrypt = new RSACryptoServiceProvider())
            {
                rsaCrypt.PersistKeyInCsp = false;
                rsaCrypt.FromXmlString(pPrivateKey);

                int keySize = rsaCrypt.KeySize / 8;
                byte[] buffer = new byte[keySize];
                MemoryStream msInput = new MemoryStream(valueBytes);
                MemoryStream msOutput = new MemoryStream();
                int readLen = msInput.Read(buffer, 0, keySize);

                while (readLen > 0)
                {
                    byte[] dataToDec = new byte[readLen];
                    Array.Copy(buffer, 0, dataToDec, 0, readLen);
                    byte[] decData = rsaCrypt.Decrypt(dataToDec, false);
                    msOutput.Write(decData, 0, decData.Length);
                    readLen = msInput.Read(buffer, 0, keySize);
                }

                msInput.Close();

                byte[] result = msOutput.ToArray();    //得到解密结果
                msOutput.Close();
                rsaCrypt.Clear();

                return Encoding.UTF8.GetString(result);
            }
        }

        #endregion
    }
}
