using System;
using YRFramework.Runtime.Extension;

namespace YRFramework.Runtime
{
    /// <summary>
    /// 对象池的对象包装类
    /// </summary>
    public sealed class Object<T> : IDisposable where T : ObjectBase
    {
        /// <summary>
        /// 对象
        /// </summary>
        private T t;
        /// <summary>
        /// 获取计数
        /// </summary>
        private int getCount;

        /// <summary>
        /// 上次使用时间
        /// </summary>
        public DateTime LastUseTime
        {
            get { return t.LastUseTime; }
        }

        /// <summary>
        /// 对象的获取计数
        /// </summary>
        public int GetCount
        {
            get { return getCount; }
        }

        /// <summary>
        /// 初始化内部对象的新实例
        /// </summary>
        public Object()
        {
            t = null;
            getCount = 0;
        }

        /// <summary>
        /// 创建内部对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isGetCount"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Object<T> Create(T obj, bool isGetCount) 
        {
            Object<T> internalObject = FrameworkGameEnter.ReferencePool.Acquire<Object<T>>();
            internalObject.t = obj ?? throw new Exception("对象无效");
            if (isGetCount)
            {
                internalObject.getCount = 1;
                internalObject.t.OnGet();
            }
            else
                internalObject.getCount = 0;

            return internalObject;
        }

        public void Clear()
        {
            t = null;
            getCount = 0;
        }

        /// <summary>
        /// 查看对象
        /// </summary>
        /// <returns></returns>
        public T Peek() 
        {
            return t;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            ++getCount;
            t.LastUseTime = DateTime.UtcNow;
            t.OnGet();

            return t;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        public void Recycle()
        {
            t.OnRecycle();
            t.LastUseTime = DateTime.UtcNow;
            --getCount;
            if (getCount < 0)
                throw new Exception("对象'{0}'获取计数小于0".YRFormat(nameof(t)));
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="isShutdown"></param>
        public void Release(bool isShutdown)
        {
            t.Release(isShutdown);

            FrameworkGameEnter.ReferencePool.Release(t);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}