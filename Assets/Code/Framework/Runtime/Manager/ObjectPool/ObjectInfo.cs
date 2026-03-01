using System;
using System.Runtime.InteropServices;

namespace YRFramework.Runtime
{
    /// <summary>
    /// 对象信息
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public readonly struct ObjectInfo
    {
        /// <summary>
        /// 上次使用时间
        /// </summary>
        private readonly DateTime lastUseTime;
        /// <summary>
        /// 获取引用
        /// </summary>
        private readonly int getCount;

        #region 属性
        /// <summary>
        /// 获取对象上次使用时间。
        /// </summary>
        public readonly DateTime LastUseTime
        {
            get { return lastUseTime; }
        }

        /// <summary>
        /// 获取对象是否正在使用。
        /// </summary>
        public readonly bool IsInUse
        {
            get  {  return getCount > 0; }
        }

        /// <summary>
        /// 获取对象的获取计数
        /// </summary>
        public readonly int GetCount
        {
            get { return getCount; }
        }
        #endregion

        /// <summary>
        /// 初始化对象信息
        /// </summary>
        /// <param name="priority">对象的优先级。</param>
        /// <param name="lastUseTime">对象上次使用时间。</param>
        /// <param name="getCount">对象的获取计数。</param>
        public ObjectInfo(DateTime lastUseTime, int getCount)
        {
            this.lastUseTime = lastUseTime;
            this.getCount = getCount;
        }
    }
}