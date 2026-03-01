using System;
using System.Collections.Generic;
using UnityEngine;
using YRFramework.Runtime.Extension;

namespace YRFramework.Runtime.ObjectPool
{
    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> : ObjectPoolBase where T : ObjectBase
    {
        /// <summary>
        /// 对象池，基于队列实现，排前面的一定是使用时间最远的
        /// </summary>
        private readonly Queue<Object<T>> queuePool = new();
        /// <summary>
        /// 对象和对象包装字典
        /// </summary>
        private readonly Dictionary<object, Object<T>> dicObject = new();
        /// <summary>
        /// 对象创建回调
        /// </summary>
        private readonly Func<T> createFunc;

        /// <summary>
        /// 对象池优先级
        /// </summary>
        private int priority;
        /// <summary>
        /// 对象池容量
        /// </summary>
        private int capacity;
        /// <summary>
        /// 对象过期时间(秒)
        /// </summary>
        private float expireTime;
        /// <summary>
        /// 释放时间计时器
        /// </summary>
        private float releaseTimer;

        #region 属性
        /// <summary>
        /// 对象池对象数量
        /// </summary>
        public override int Count
        {
            get { return queuePool.Count; }
        }

        /// <summary>
        /// 优先级
        /// </summary>
        public override int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        /// <summary>
        /// 对象池容量
        /// </summary>
        public override int Capacity
        {
            get { return capacity; }
            set
            {
                if (value <= 0)
                    throw new Exception("容量无效");

                if (value == capacity)
                    return;

                capacity = value;
                Release();
            }
        }

        /// <summary>
        /// 过期时间
        /// </summary>
        public override float ExpireTime
        {
            get { return expireTime; }
            set
            {
                if (value <= 0f)
                    throw new Exception("过期时间无效");

                if (value == ExpireTime)
                    return;

                expireTime = value;
                ReleaseByTime();
            }
        }
        #endregion
        
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="create">实例创建回调</param>
        /// <param name="capacity">对象池容量</param>
        /// <param name="expireTime">对象过期时间</param>
        /// <param name="priority">对象池优先级</param>
        /// <param name="initCount">初始容量</param>
        public ObjectPool(Func<T> create, int capacity, float expireTime, int priority, int initCount = 0)
        {
            createFunc = create;
            this.capacity = capacity;
            this.expireTime = expireTime;
            this.priority = priority;

            if (expireTime <= 0f)
                throw new Exception("过期时间无效");

            if (capacity <= 0)
                throw new Exception("容量无效");

            if (capacity < initCount)
                throw new Exception("初始数量大于容量");

            for (int i = 0; i < initCount; ++i)
            {
                queuePool.Equals(Create(false));
            }

            releaseTimer = 0f;
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            releaseTimer += realElapseSeconds;
            if (releaseTimer >= expireTime)
                ReleaseByTime();
        }

        public override void Dispose()
        {
            foreach (Object<T> internalObject in dicObject.Values)
            {
                internalObject.Release(true);
            }

            queuePool.Clear();
            dicObject.Clear();
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            if (queuePool.TryDequeue(out Object<T> internalObject))
                return internalObject.Get();

            T target = Create(true);

            return target;
        }

        /// <summary>
        /// 根据名字获取对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Get(string name) // TODO
        {
            if (queuePool.TryDequeue(out Object<T> internalObject))
                return internalObject.Get();

            T target = Create(true);

            return target;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="value"></param>
        public void Recycle(T target)
        {
            if (null == target)
                return;

            Recycle(target.Target);
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="target"></param>
        /// <exception cref="Exception"></exception>
        public void Recycle(object target)
        {
            if (target == null)
                return;

            if (TryGetObject(target, out Object<T> internalObject))
            {
                internalObject.Recycle();
                queuePool.Enqueue(internalObject);
                if (Count > capacity)
                    Release();
            }
            else
                throw new Exception("错误，没有获取到对象的包装类，对象'{0}'".YRFormat(nameof(target)));
        }

        #region 释放对象
        /// <summary>
        /// 释放对象
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override void Release()
        {
            Release(Count - capacity);
        }

        /// <summary>
        /// 释放对象池中指定数量的对象
        /// </summary>
        /// <param name="toReleaseCount">尝试释放对象数量。</param>
        public override void Release(int toReleaseCount)
        {
            while (toReleaseCount > 0 && queuePool.TryDequeue(out Object<T> internalObject))
            {
                --toReleaseCount;
                dicObject.Remove(internalObject.Get());
                internalObject.Release(false);
            }
        }

        /// <summary>
        /// 释放对象池所有对象
        /// </summary>
        /// <param name="toReleaseCount">尝试释放对象数量。</param>
        public override void ReleaseAll()
        {
            Release(Count);
        }

        /// <summary>
        /// 按时间释放对象
        /// </summary>
        public override void ReleaseByTime()
        {
            while (queuePool.TryPeek(out Object<T> internalObject))
            {
                TimeSpan timeSpan = DateTime.UtcNow - internalObject.LastUseTime;
                if (timeSpan.TotalSeconds >= expireTime)
                {
                    queuePool.Dequeue();
                    dicObject.Remove(internalObject.Get());
                    internalObject.Release(false);
                }
                else
                    break;
            }

            releaseTimer = 0f;
        }
        #endregion

        /// <summary>
        /// 获取所有对象信息
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override ObjectInfo[] GetAllObjectInfos()
        {
            List<ObjectInfo> results = new List<ObjectInfo>();
            foreach (Object<T> internalObject in queuePool)
            {
                results.Add(new ObjectInfo(internalObject.LastUseTime, internalObject.GetCount));
            }

            return results.ToArray();
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="isGetCount">是否设置获取计数</param>
        /// <returns></returns>
        private T Create(bool isGetCount)
        {
            T target = createFunc.Invoke();
            Object<T> internalObject = Object<T>.Create(target, isGetCount);

            dicObject[target] = internalObject;

            return target;
        }

        /// <summary>
        /// 获取对象的包装类
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private bool TryGetObject(object target, out Object<T> internalObject)
        {
            if (null == target)
            {
                Debug.LogError("对象无效");
                internalObject = null;

                return false;
            }

            if (dicObject.TryGetValue(target, out internalObject))
                return true;

            return false;
        }
    }
}