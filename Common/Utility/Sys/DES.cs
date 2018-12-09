using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utility.Sys
{
    public static class DES
    {

        public static readonly string DefaultKey = "???lS???";


        /// <summary>
        /// 创建key
        /// </summary>
        /// <returns></returns>
        public static string GenerateKey()
        {
            using (var des = new DESCryptoServiceProvider())
            {
                des.GenerateKey();
                return Encoding.ASCII.GetString(des.Key);
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string inputString, string key)
        {
            try
            {
                using (var des = new DESCryptoServiceProvider())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(inputString);
                    des.Key = Encoding.ASCII.GetBytes(key);
                    des.IV = Encoding.ASCII.GetBytes(key);
                    var ms = new MemoryStream();
                    var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                    byte[] data = ms.ToArray();
                    cs.Close();
                    ms.Close();
                    return Convert.ToBase64String(data);
                }
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(string inputString, string key)
        {
            try
            {
                using (var des = new DESCryptoServiceProvider())
                {
                    byte[] bytes = Convert.FromBase64String(inputString);
                    des.Key = Encoding.ASCII.GetBytes(key);
                    des.IV = Encoding.ASCII.GetBytes(key);
                    var ms = new MemoryStream();
                    var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();

                    byte[] data = ms.ToArray();

                    cs.Close();
                    ms.Close();
                    return Encoding.UTF8.GetString(data);
                }
            }
            catch
            {
                return "";
            }
        }

    }
}
