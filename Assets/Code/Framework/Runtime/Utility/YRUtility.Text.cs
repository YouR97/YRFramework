using System.Text;

namespace YRFramework.Runtime.Utility
{
    /// <summary>
    /// 通用函数
    /// </summary>
    public static partial class YRUtility
    {
        /// <summary>
        /// 文本实用函数
        /// </summary>
        public static class Text
        {
            private static readonly StringBuilder sb;

            static Text()
            {
                sb = new StringBuilder();
            }

            /// <summary>
            /// 高效字符串拼接
            /// </summary>
            /// <param name="text"></param>
            /// <param name="values"></param>
            /// <returns></returns>
            public static string Append(string text, params string[] values)
            {
                sb.Clear();
                sb.Append(text);
                foreach (string value in values)
                {
                    sb.Append(value);
                }

                return sb.ToString();
            }

            /// <summary>
            /// 高效字符串格式化拼接
            /// </summary>
            /// <param name="text"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static string Format(string text, params string[] value)
            {
                sb.Clear();
                sb.AppendFormat(text, value);

                return sb.ToString();
            }

            /// <summary>
            /// 百分比格式化
            /// </summary>
            /// <param name="value"></param>
            /// <param name="validDigits">小数位数(0-8),不在这个范围显示默认位数</param>
            /// <returns></returns>
            public static string ToPercentage(float value, int validDigits = 2)
            {
                return validDigits switch
                {
                    0 => string.Format($"{value:P0}"),
                    1 => string.Format($"{value:P1}"),
                    2 => string.Format($"{value:P2}"),
                    3 => string.Format($"{value:P3}"),
                    4 => string.Format($"{value:P4}"),
                    5 => string.Format($"{value:P5}"),
                    6 => string.Format($"{value:P6}"),
                    7 => string.Format($"{value:P7}"),
                    8 => string.Format($"{value:P8}"),
                    _ => string.Format($"{value:P}"),
                };
            }
        }
    }
}