namespace YRFramework.Runtime.Utility
{
    /// <summary>
    /// 通用函数
    /// </summary>
    public static partial class YRUtility
    {
        /// <summary>
        /// 通用随机函数
        /// </summary>
        public static class Random
        {
            /// <summary>
            /// 随机一个bool
            /// </summary>
            /// <param name="probability">随机到true的概率(0f-1f)</param>
            /// <returns></returns>
            public static bool RangeBool(float probability = 0.5f)
            {
                return UnityEngine.Random.Range(0f, 1f) < probability;
            }

            /// <summary>
            /// 随机一个float
            /// </summary>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            public static float RangeFloat(float min, float max)
            {
                return UnityEngine.Random.Range(min, max);
            }

            /// <summary>
            /// 随机一个int
            /// </summary>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            public static int RangeInt(int min, int max)
            {
                return UnityEngine.Random.Range(min, max);
            }
        }
    }
}