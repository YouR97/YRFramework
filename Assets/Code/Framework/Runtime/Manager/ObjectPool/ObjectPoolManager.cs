using System;
using System.Collections.Generic;
using UnityEngine;
using YRFramework.Runtime.Manager;
using Object = UnityEngine.Object;

namespace YRFramework.Runtime.ObjectPool
{
    /// <summary>
    /// 对象池管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRFramework/ObjectPoolManager")]
    public sealed class ObjectPoolManager : YRFrameworkManager
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.ObjectPool;
        #endregion

        /// <summary>
        /// 默认优先级，越小越优先
        /// </summary>
        private const int DEFAULT_PRIORITY = 0;
        /// <summary>
        /// 默认对象池容量
        /// </summary>
        private const int DEFAULT_CAPACITY = 1000;
        /// <summary>
        /// 默认过期时间
        /// </summary>
        private const float DEFAULT_EXPIRETIME = float.MaxValue;

        /// <summary>
        /// 对象池字典
        /// </summary>
        private readonly Dictionary<Type, ObjectPoolBase> dicObjectPool = new Dictionary<Type, ObjectPoolBase>();
        /// <summary>
        /// 缓存的所有对象池
        /// </summary>
        private readonly List<ObjectPoolBase> listCachedPool = new List<ObjectPoolBase>();

        /// <summary>
        /// 创建对象池，有则直接返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="create"></param>
        /// <param name="initCount"></param>
        /// <returns></returns>
        public ObjectPool<T> CreatePool<T>(Func<T> create,
            int initCount = 0,
            int capacity = DEFAULT_CAPACITY,
            float expireTime = DEFAULT_EXPIRETIME,
            int priority = DEFAULT_PRIORITY) where T : ObjectBase
        {
            if (IsHasObjectPool<T>())
            {
                if (TryGetPool(out ObjectPool<T> pool))
                    return pool;
            }

            ObjectPool<T> objectPool = new(create, capacity, expireTime, priority, initCount);
            dicObjectPool[typeof(T)] = objectPool;

            return objectPool;
        }

        /// <summary>
        /// 是否存在对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool IsHasObjectPool<T>() where T : ObjectBase
        {
            return dicObjectPool.ContainsKey(typeof(T));
        }

        /// <summary>
        /// 尝试获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet<T>(out T value) where T : ObjectBase
        {
            value = default;

            if (TryGetPool(out ObjectPool<T> pool))
            {
                value = pool.Get();
                return true;
            }

            return false;
        }

        /// <summary>
        /// 尝试获取对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pool"></param>
        /// <returns></returns>
        public bool TryGetPool<T>(out ObjectPool<T> pool) where T : ObjectBase
        {
            pool = null;

            if (dicObjectPool.TryGetValue(typeof(T), out ObjectPoolBase objectPoolBase))
            {
                if (objectPoolBase is not ObjectPool<T> objectPool)
                    return false;

                pool = objectPool;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取所有对象池
        /// </summary>
        /// <param name="isSort">是否根据对象池的优先级排序。</param>
        /// <param name="listResult">所有对象池。</param>
        public void GetAllObjectPools(bool isSort, List<ObjectPoolBase> listResult)
        {
            if (null == listResult)
                listResult = new List<ObjectPoolBase>();

            listResult.Clear();
            foreach (ObjectPoolBase objectPool in dicObjectPool.Values)
            {
                listResult.Add(objectPool);
            }

            if (isSort)
                listResult.Sort(ObjectPoolComparer);
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void Recycle<T>(T value) where T : ObjectBase
        {
            if (TryGetPool(out ObjectPool<T> pool))
            {
                pool.Recycle(value);
                return;
            }

            if (value is Object o)
                Destroy(o);
        }

        /// <summary>
        /// 释放对象池
        /// </summary>
        public void ReleasePool<T>() where T : ObjectBase
        {
            if (!TryGetPool(out ObjectPool<T> pool))
            {
                if (IsHasObjectPool<T>())
                    dicObjectPool.Remove(typeof(T));

                return;
            }

            pool.Dispose();
            dicObjectPool.Remove(typeof(T));
        }

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        public void Release()
        {
            GetAllObjectPools(true, listCachedPool);
            foreach (ObjectPoolBase objectPool in listCachedPool)
            {
                objectPool.Release();
            }
        }

        /// <summary>
        /// 释放对象池中的所有未使用对象
        /// </summary>
        public void ReleaseAll()
        {
            GetAllObjectPools(true, listCachedPool);
            foreach (ObjectPoolBase objectPool in listCachedPool)
            {
                objectPool.ReleaseAll();
            }
        }

        /// <summary>
        /// 对象池排序
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static int ObjectPoolComparer(ObjectPoolBase a, ObjectPoolBase b)
        {
            return a.Priority.CompareTo(b.Priority);
        }
    }
}