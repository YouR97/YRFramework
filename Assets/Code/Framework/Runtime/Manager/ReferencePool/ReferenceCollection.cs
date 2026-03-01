using System;
using System.Collections.Generic;
using UnityEngine;

namespace YRFramework.Runtime.ReferencePool
{
    /// <summary>
    /// 引用收集器
    /// </summary>
    internal sealed class ReferenceCollection
    {
        /// <summary>
        /// 引用队列
        /// </summary>
        private readonly Queue<IDisposable> queueReference;
        /// <summary>
        /// 引用类型
        /// </summary>
        private readonly Type referenceType;
        /// <summary>
        /// 使用中的引用数量
        /// </summary>
        private int usingReferenceCount;
        /// <summary>
        /// 取得的引用数量
        /// </summary>
        private int acquireReferenceCount;
        /// <summary>
        /// 释放的引用数量
        /// </summary>
        private int releaseReferenceCount;
        /// <summary>
        /// 添加的引用数量
        /// </summary>
        private int addReferenceCount;
        /// <summary>
        /// 移除的引用数量
        /// </summary>
        private int removeReferenceCount;
        /// <summary>
        /// 引用池内部对象进入HitObjet列表之后的到期时间
        /// </summary>
        private float expireDuration;

        /// <summary>
        /// 最后一次使用对象池的时间
        /// </summary>
        private float expiredTime;

        #region 属性
        /// <summary>
        /// 类型
        /// </summary>
        public Type ReferenceType { get { return referenceType; } }

        /// <summary>
        /// 未使用的引用数量
        /// </summary>
        public int UnusedReferenceCount { get { return queueReference.Count; } }

        /// <summary>
        /// 正在使用的引用数量
        /// </summary>
        public int UsingReferenceCount { get { return usingReferenceCount; } }

        public int AcquireReferenceCount { get { return acquireReferenceCount; } }

        public int ReleaseReferenceCount { get { return releaseReferenceCount; } }

        /// <summary>
        /// 添加的引用数量
        /// </summary>
        public int AddReferenceCount { get { return addReferenceCount; } }

        /// <summary>
        /// 移除的引用数量
        /// </summary>
        public int RemoveReferenceCount { get { return removeReferenceCount; } }
        #endregion

        public ReferenceCollection(Type referenceType)
        {
            queueReference = new Queue<IDisposable>();
            this.referenceType = referenceType;
            usingReferenceCount = 0;
            acquireReferenceCount = 0;
            releaseReferenceCount = 0;
            addReferenceCount = 0;
            removeReferenceCount = 0;
            expireDuration = 60f; // 如果这个池子超过一分钟没有使用则释放所有的引用
        }

        /// <summary>
        /// 获取引用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T Acquire<T>() where T : IDisposable, new()
        {
            if (typeof(T) != referenceType)
                throw new($"[{nameof(ReferenceCollection)}]:类型错误, 错误类型：{typeof(T)}，正确类型：{referenceType}");

            return (T)Acquire();
        }

        /// <summary>
        /// 获取引用
        /// </summary>
        /// <returns></returns>
        public IDisposable Acquire()
        {
            ++usingReferenceCount;
            ++acquireReferenceCount;

            lock (queueReference)
            {
                if (queueReference.TryDequeue(out IDisposable iDisposable))
                {
                    iDisposable.Dispose();
                    return iDisposable;
                }
            }

            ++addReferenceCount;
            return (IDisposable)Activator.CreateInstance(referenceType);
        }

        /// <summary>
        /// 释放引用
        /// </summary>
        /// <param name="reference"></param>
        /// <exception cref="Exception"></exception>
        public void Release(IDisposable iDisposable)
        {
            lock (queueReference)
            {
#if UNITY_EDITOR
                if (queueReference.Contains(iDisposable))
                    throw new Exception($"[{nameof(ReferenceCollection)}]:{iDisposable}引用已经释放");
#endif
                queueReference.Enqueue(iDisposable);
            }

            iDisposable.Dispose();
            ++releaseReferenceCount;
            --usingReferenceCount;

            expiredTime = Time.realtimeSinceStartup + expireDuration;
        }

        /// <summary>
        /// 添加引用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <exception cref="Exception"></exception>
        public void Add<T>(int count) where T : class, IDisposable, new()
        {
            if (typeof(T) != referenceType)
                throw new Exception($"[{nameof(ReferenceCollection)}]:类型错误，需要类型：{referenceType},当前类型：{typeof(T)}");

            lock (queueReference)
            {
                addReferenceCount += count;
                while (count-- > 0)
                {
                    queueReference.Enqueue(new T());
                }
            }
        }

        /// <summary>
        /// 添加引用
        /// </summary>
        /// <param name="count"></param>
        public void Add(int count)
        {
            lock (queueReference)
            {
                addReferenceCount += count;
                while (count-- > 0)
                {
                    queueReference.Enqueue((IDisposable)Activator.CreateInstance(referenceType));
                }
            }
        }

        /// <summary>
        /// 移除引用
        /// </summary>
        /// <param name="count"></param>
        public void Remove(int count)
        {
            lock (queueReference)
            {
                while (count > 0 && queueReference.TryDequeue(out _))
                {
                    --count;
                    ++removeReferenceCount;
                }
            }
        }

        /// <summary>
        /// 移除所有引用
        /// </summary>
        public void RemoveAll()
        {
            lock (queueReference)
            {
                removeReferenceCount += queueReference.Count;
                queueReference.Clear();
            }
        }

        /// <summary>
        /// 检查是否过期
        /// </summary>
        /// <returns></returns>
        public bool CheckUnused()
        {
            // 没有引用，并且过期
            return 0 == usingReferenceCount && Time.realtimeSinceStartup >= expiredTime;
        }
    }
}