using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace YRFramework.Runtime.Utility
{
    /// <summary>
    /// 通用函数
    /// </summary>
    public static partial class YRUtility
    {
        /// <summary>
        /// 编码实用类
        /// </summary>
        public static class Encode
        {
            /// <summary>
            /// Base64编码
            /// </summary>
            /// <param name="isUTC"></param>
            /// <returns></returns>
            public static string Base64Encode(string data)
            {
                byte[] mids = Encoding.UTF8.GetBytes(data);

                return Convert.ToBase64String(mids);
            }

            /// <summary>
            /// Base64解码
            /// </summary>
            /// <param name="isUTC"></param>
            /// <returns></returns>
            public static string Base64Decode(string base64EncodedData)
            {
                byte[] mids = Convert.FromBase64String(base64EncodedData);

                return Encoding.UTF8.GetString(mids);
            }
        }
    }
}