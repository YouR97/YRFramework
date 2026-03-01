using System;

namespace YRFramework.Runtime.Utility
{
    /// <summary>
    /// 通用函数
    /// </summary>
    public static partial class YRUtility
    {
        /// <summary>
        /// 时间实用函数
        /// </summary>
        public static class Time
        {
            /// <summary>
            /// 获取当前时间
            /// </summary>
            /// <param name="isUTC"></param>
            /// <returns></returns>
            public static DateTime GetCurTime(bool isUTC = false)
            {
                return isUTC ? DateTime.UtcNow : DateTime.Now;
            }

            /// <summary>
            /// 判断是否生日
            /// </summary>
            /// <param name="dateTime"></param>
            /// <param name="month"></param>
            /// <param name="day"></param>
            /// <returns></returns>
            public static bool IsBirthDay(DateTime dateTime, int month, int day)
            {
                return dateTime.Month == month && dateTime.Day == day;
            }

            /// <summary>
            /// 判断是否上午
            /// </summary>
            /// <param name="dateTime"></param>
            /// <returns></returns>
            public static bool IsAM(DateTime dateTime)
            {
                return dateTime.Hour < 12;
            }
        }
    }
}