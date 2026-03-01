using YRFramework.Runtime.Utility;

namespace YRFramework.Runtime.Extension
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 字符串开始是否是对应字符串(比API性能高)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool YRStartsWith(this string self, string value)
        {
            if (null == self)
                return null == value;

            if (null == value) // 上面判断了self不为空
                return false;

            if (self.Length < value.Length)
                return false;

            for (int i = 0; i < value.Length; ++i)
            {
                if (self[i] != value[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 字符串第一个字符是否是对应字符串(比API性能高)
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool YRStartsWith(this string self, char value)
        {
            if (null == self)
                return false;

            if (self.Length < 1)
                return false;

            return self[0] == value;
        }

        /// <summary>
        /// 字符串结束是否是对应字符串(比API性能高)
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool YREndsWith(this string self, string value)
        {
            if (null == self)
                return null == value;

            if (null == value) // 上面判断了self不为空
                return false;

            if (self.Length < value.Length)
                return false;

            for (int i = 1; i <= value.Length; ++i)
            {
                if (self[^i] != value[^i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 字符串最后一个字符是否是对应字符串(比API性能高)
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool YREndsWith(this string self, char value)
        {
            if (null == self)
                return false;

            if (self.Length < 1)
                return false;

            return self[^1] == value;
        }

        /// <summary>
        /// 按字符比较字符串是否相同(性能比API高)
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool YREquals(this string self, string value)
        {
            if (null == self)
                return null == value;

            if (null == value) // 上面判断了self不为空
                return false;

            if (self.Length != value.Length)
                return false;

            for (int i = 0; i < self.Length; ++i)
            {
                if (self[i] != value[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 是否无效(null或全是空格或指定字符串无效)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="invalid"></param>
        /// <returns></returns>
        public static bool IsInvalid(this string value, string invalid)
        {
            return string.IsNullOrWhiteSpace(value) || value.YREquals(invalid);
        }

        /// <summary>
        /// 高效字符串拼接
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Append(this string text, params string[] value)
        {
            return YRUtility.Text.Append(text, value);
        }

        /// <summary>
        /// 高效字符串格式化拼接
        /// </summary>
        /// <param name="text"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string YRFormat(this string text, params string[] values)
        {
            return YRUtility.Text.Format(text, values);
        }
    }
}