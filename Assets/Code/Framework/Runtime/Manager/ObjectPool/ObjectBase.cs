using System;

namespace YRFramework.Runtime
{
    /// <summary>
    /// 引用池对象基类
    /// </summary>
    public abstract class ObjectBase : IDisposable
    {
        /// <summary>
        /// 对象具体值
        /// </summary>
        private object target;
        /// <summary>
        /// 上次使用时间
        /// </summary>
        private DateTime lastUseTime;

        /// <summary>
        /// 对象
        /// </summary>
        public object Target
        {
            get { return target; }
        }

        /// <summary>
        /// 上次使用时间
        /// </summary>
        public DateTime LastUseTime
        {
            get { return lastUseTime; }
            internal set { lastUseTime = value; }
        }

        public ObjectBase()
        {
            target = null;
            lastUseTime = default;
        }

        public virtual void Dispose()
        {
            target = null;
            lastUseTime = default;
        }

        protected void Initialize(object target)
        {
            this.target = target ?? throw new Exception("目标无效");
            lastUseTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 获取对象事件
        /// </summary>
        protected internal virtual void OnGet() { }

        /// <summary>
        /// 回收对象事件
        /// </summary>
        protected internal virtual void OnRecycle() { }

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="isShutdown">是否关闭对象池时触发</param>
        protected internal virtual void Release(bool isShutdown) { }
    }
}