using System;
using System.IO;
using System.Security.Cryptography;

namespace YRFramework.Runtime.Utility
{
    /// <summary>
    /// 通用函数
    /// </summary>
    public static partial class YRUtility
    {
        /// <summary>
        /// Rijndael加密实用类
        /// </summary>
        public static class Rijndael
        {
            /// <summary>
            /// 加密key
            /// </summary>
            private static readonly byte[] rijndaelKey =
            {
                0x0F, 0x02, 0x01, 0x02, 0x01, 0x02, 0x01, 0x02,
                0x02, 0x02, 0x01, 0x02, 0x01, 0x02, 0x01, 0x02,
                0x04, 0x02, 0x01, 0x02, 0x01, 0x02, 0x01, 0x02,
                0x04, 0x02, 0x01, 0x02, 0x01, 0x02, 0x01, 0x02
            };

            /// <summary>
            /// 加密初始化向量
            /// </summary>
            private static readonly byte[] rijndaelIV =
            {
                0x01, 0x02, 0x0E, 0x02, 0x01, 0x90, 0x01, 0x02,
                0x01, 0x02, 0xFF, 0x02, 0x01, 0xFE, 0x01, 0x02
            };

            /// <summary>
            /// 加密string成Bytes数组
            /// </summary>
            /// <param name="isUTC"></param>
            /// <returns></returns>
            public static byte[] Encrypt(string data)
            {
                if (null == data || data.Length <= 0) // 参数校验
                    throw new ArgumentNullException("加密参数data");

                byte[] encrypted;
                using (RijndaelManaged rijAlg = new()) // 创建Rijndael加密
                {
                    rijAlg.Key = rijndaelKey;
                    rijAlg.IV = rijndaelIV;

                    // 创建加密器以执行流转换
                    ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                    // 创建用于加密的流
                    using MemoryStream msEncrypt = new();
                    using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                    using StreamWriter swEncrypt = new(csEncrypt);
                    {
                        swEncrypt.Write(data); // 将所有数据写入流
                        swEncrypt.Flush();
                        csEncrypt.FlushFinalBlock(); // 执行加密
                        encrypted = msEncrypt.ToArray();
                    }

                }

                return encrypted; // 从内存流中返回加密后的数据
            }

            /// <summary>
            /// 解密bytes数组成string
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public static string Decrypt(byte[] data)
            {
                if (null == data || data.Length <= 0) // 参数校验
                    throw new ArgumentNullException("加密参数data");

                string plaintext = null; // 声明用于保存解密文本的字符串
                using (RijndaelManaged rijAlg = new())
                {
                    rijAlg.Key = rijndaelKey;
                    rijAlg.IV = rijndaelIV;

                    // 创建解密器以执行流转换
                    ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                    // 创建用于解密的流
                    using MemoryStream msDecrypt = new(data);
                    using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                    using StreamReader srDecrypt = new(csDecrypt);

                    plaintext = srDecrypt.ReadToEnd(); // 从解密流字符串中读取解密字节
                }

                return plaintext;
            }
        }
    }
}