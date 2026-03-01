using System.Collections;

namespace YRFramework.Runtime.Utility
{
    /// <summary>
    /// 通用函数
    /// </summary>
    public static partial class YRUtility
    {
        /// <summary>
        /// 容器函数
        /// </summary>
        public static class Collection
        {
            /// <summary>
            /// 检查容器是否为空
            /// </summary>
            /// <param name="text"></param>
            /// <param name="values"></param>
            /// <returns></returns>
            public static bool IsEmpty(ICollection collection)
            {
                if (null == collection)
                    return true;

                if (0 >= collection.Count)
                    return true;

                return false;
            }

            /// <summary>
            /// 是否越界
            /// </summary>
            /// <returns></returns>
            public static bool IsIndexOutOfRange(IList list, int index)
            {
                if (null == list)
                    return true;

                if (index < 0)
                    return true;

                if (index >= list.Count)
                    return true;

                return false;
            }
        }
    }
}