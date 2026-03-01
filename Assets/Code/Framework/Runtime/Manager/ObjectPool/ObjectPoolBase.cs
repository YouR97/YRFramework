using System;

namespace YRFramework.Runtime
{
    /// <summary>
    /// 对象池基类
    /// </summary>
    public abstract class ObjectPoolBase : IDisposable
    {
        /// <summary>
        /// 对象池对象数量
        /// </summary>
        public abstract int Count
        {
            get;
        }

        /// <summary>
        /// 对象池优先级
        /// </summary>
        public abstract int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置对象池容量
        /// </summary>
        public abstract int Capacity
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置对象池过期时间(单位秒)
        /// </summary>
        public abstract float ExpireTime
        {
            get;
            set;
        }

        /// <summary>
        /// 轮询
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        internal abstract void Update(float elapseSeconds, float realElapseSeconds);

        public abstract void Dispose();

        /// <summary>
        /// 获取所有对象信息。
        /// </summary>
        /// <returns>所有对象信息。</returns>
        public abstract ObjectInfo[] GetAllObjectInfos();

        #region 释放
        /// <summary>
        /// 释放对象池中的可释放对象
        /// </summary>
        public abstract void Release();

        /// <summary>
        /// 释放对象池中指定数量对象
        /// </summary>
        /// <param name="releaseCount">尝试释放对象数量</param>
        public abstract void Release(int releaseCount);

        /// <summary>
        /// 释放对象池中的所有对象
        /// </summary>
        public abstract void ReleaseAll();

        /// <summary>
        /// 按时间释放对象池对象
        /// </summary>
        public abstract void ReleaseByTime();
        #endregion
    }
}