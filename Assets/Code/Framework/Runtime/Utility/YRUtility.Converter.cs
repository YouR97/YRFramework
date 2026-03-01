using System;

namespace YRFramework.Runtime.Utility
{
    /// <summary>
    /// 通用函数
    /// </summary>
    public static partial class YRUtility
    {
        /// <summary>
        /// 单位转换实用函数
        /// </summary>
        public static class Converter
        {
            /// <summary>
            /// 英寸转厘米(1 inches = 2.54cm)
            /// </summary>
            private const float InchesToCentimeters = 2.54f;
            /// <summary>
            /// 厘米转英寸(1cm = 0.3937 inches)
            /// </summary>
            private const float CentimetersToInches = 1f / InchesToCentimeters;

            /// <summary>
            /// 获取或设置屏幕每英寸像素
            /// </summary>
            public static float ScreenDpi
            {
                get; set;
            }

            #region 像素转换
            /// <summary>
            /// 将像素转换为厘米
            /// </summary>
            /// <param name="pixels">像素</param>
            /// <returns>厘米</returns>
            public static float GetCentimetersFromPixels(float pixels)
            {
                if (ScreenDpi <= 0f)
                    throw new Exception($"屏幕DPI错误，当前DPI:{ScreenDpi}");

                return InchesToCentimeters * pixels / ScreenDpi;
            }

            /// <summary>
            /// 将厘米转换为像素
            /// </summary>
            /// <param name="centimeters">厘米</param>
            /// <returns>像素</returns>
            public static float GetPixelsFromCentimeters(float centimeters)
            {
                if (ScreenDpi <= 0f)
                    throw new Exception($"屏幕DPI错误，当前DPI:{ScreenDpi}");

                return CentimetersToInches * centimeters * ScreenDpi;
            }

            /// <summary>
            /// 将像素转换为英寸
            /// </summary>
            /// <param name="pixels">像素</param>
            /// <returns>英寸</returns>
            public static float GetInchesFromPixels(float pixels)
            {
                if (ScreenDpi <= 0)
                    throw new Exception($"屏幕DPI错误，当前DPI:{ScreenDpi}");

                return pixels / ScreenDpi;
            }

            /// <summary>
            /// 将英寸转换为像素
            /// </summary>
            /// <param name="inches">英寸</param>
            /// <returns>像素</returns>
            public static float GetPixelsFromInches(float inches)
            {
                if (ScreenDpi <= 0)
                    throw new Exception($"屏幕DPI错误，当前DPI:{ScreenDpi}");

                return inches * ScreenDpi;
            }
            #endregion
        }
    }
}