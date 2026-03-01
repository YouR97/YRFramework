using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using YRFramework.Runtime.Manager;

namespace YRFramework.Runtime.ReferencePool
{
    /// <summary>
    /// 引用池管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRFramework/ReferencePoolManager")]
    public sealed class ReferencePoolManager : YRFrameworkManager, IInit
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.ReferencePool;
        #endregion

        /// <summary>
        /// 检查引用池到期时间间隔
        /// </summary>
        private const float CHECK_RELEASE_INTERVAL = 20f;
        /// <summary>
        /// 每帧最大销毁数量
        /// </summary>
        private const int MAX_DESTOR_BY_FRAME = 5;
        /// <summary>
        /// 引用池
        /// </summary>
        private readonly Dictionary<Type, ReferenceCollection> dicReferenceCollection = new();
        /// <summary>
        /// 等待销毁的类型
        /// </summary>
        private List<Type> listWaitDestroy;
        /// <summary>
        /// 轮询任务id
        /// </summary>
        private int timerTaskId;

        /// <summary>
        /// 获取引用池数量
        /// </summary>
        [BoxGroup("引用池管理器"), LabelText("引用池类型数量"), ShowInInspector, PropertyOrder(0)]
        public int Count 
        {
            get 
            {
                lock (dicReferenceCollection)
                {
                    return dicReferenceCollection.Count;
                }
            }
        }

        async UniTask IInit.OnInit()
        {
            listWaitDestroy = new();
            timerTaskId = FrameworkGameEnter.Timer.AddLoopTask(CHECK_RELEASE_INTERVAL, OnCheckReleaseTime);

            await UniTask.CompletedTask;
        }

        void IInit.OnRelease()
        {
            FrameworkGameEnter.Timer.RemoveTask(timerTaskId);

            if (null != listWaitDestroy)
            {
                listWaitDestroy.Clear();
                listWaitDestroy = null;
            }
        }

        /// <summary>
        /// 获取所有引用池的信息
        /// </summary>
        /// <returns></returns>
        [BoxGroup("引用池管理器"), LabelText("引用池信息"), ShowInInspector, PropertyOrder(0)]
        public ReferencePoolInfo[] GetAllReferencePoolInfos()
        {
            int index = 0;
            ReferencePoolInfo[] results = null;

            lock (dicReferenceCollection)
            {
                results = new ReferencePoolInfo[dicReferenceCollection.Count];
                foreach (var referenceCollection in dicReferenceCollection)
                {
                    results[index++] = new ReferencePoolInfo(referenceCollection.Key,
                        referenceCollection.Value.UnusedReferenceCount,
                        referenceCollection.Value.UsingReferenceCount,
                        referenceCollection.Value.AcquireReferenceCount,
                        referenceCollection.Value.ReleaseReferenceCount,
                        referenceCollection.Value.AddReferenceCount,
                        referenceCollection.Value.RemoveReferenceCount);
                }
            }

            return results;
        }

        /// <summary>
        /// 清除所有引用池
        /// </summary>
        public void ClearAll()
        {
            lock (dicReferenceCollection) 
            {
                foreach (ReferenceCollection referenceCollection in dicReferenceCollection.Values)
                {
                    referenceCollection.RemoveAll();
                }

                dicReferenceCollection.Clear();
            }
        }

        /// <summary>
        /// 从引用池获取引用。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        /// <returns>引用。</returns>
        public T Acquire<T>() where T : IDisposable, new()
        {
            return GetReferenceCollection(typeof(T)).Acquire<T>();
        }

        /// <summary>
        /// 从引用池获取引用。
        /// </summary>
        /// <typeparam name="referenceType">引用类型。</typeparam>
        /// <returns>引用。</returns>
        public IDisposable Acquire(Type referenceType)
        {
            InternalCheckReferenceType(referenceType);

            return GetReferenceCollection(referenceType).Acquire();
        }

        /// <summary>
        /// 将引用归还引用池。
        /// </summary>
        /// <param name="reference">引用。</param>
        public void Release(IDisposable disposable)
        {
            if (null == disposable)
                throw new Exception($"[{nameof(ReferencePoolManager)}]:引用无效");

            Type referenceType = disposable.GetType();
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Release(disposable);
        }

        /// <summary>
        /// 向引用池中追加指定数量的引用。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        /// <param name="count">追加数量。</param>
        public void Add<T>(int count) where T : class, IDisposable, new()
        {
            GetReferenceCollection(typeof(T)).Add<T>(count);
        }

        /// <summary>
        /// 向引用池中追加指定数量的引用。
        /// </summary>
        /// <param name="referenceType">引用类型。</param>
        /// <param name="count">追加数量。</param>
        public void Add(Type referenceType, int count)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Add(count);
        }

        /// <summary>
        /// 从引用池中移除指定数量的引用。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        /// <param name="count">移除数量。</param>
        public void Remove<T>(int count) where T : IDisposable
        {
            GetReferenceCollection(typeof(T)).Remove(count);
        }

        /// <summary>
        /// 从引用池中移除指定数量的引用。
        /// </summary>
        /// <param name="referenceType">引用类型。</param>
        /// <param name="count">移除数量。</param>
        public void Remove(Type referenceType, int count)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Remove(count);
        }

        /// <summary>
        /// 从引用池中移除所有的引用。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        public void RemoveAll<T>() where T : IDisposable
        {
            GetReferenceCollection(typeof(T)).RemoveAll();
        }

        /// <summary>
        /// 从引用池中移除所有的引用。
        /// </summary>
        /// <param name="referenceType">引用类型。</param>
        public void RemoveAll(Type referenceType)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).RemoveAll();
            dicReferenceCollection.Remove(referenceType);
        }

        /// <summary>
        /// 检查销毁时间
        /// </summary>
        private void OnCheckReleaseTime()
        {
            listWaitDestroy.Clear();

            foreach (ReferenceCollection referenceCollection in dicReferenceCollection.Values)
            {
                if (!referenceCollection.CheckUnused())
                    continue;

                listWaitDestroy.Add(referenceCollection.ReferenceType);
                if (MAX_DESTOR_BY_FRAME <= listWaitDestroy.Count)
                    break;
            }

            foreach (Type type in listWaitDestroy)
            {
                Debug.Log($"[{nameof(ReferencePoolManager)}]:清理引用池，类型：{type.Name}");
                RemoveAll(type);
            }
        }

        /// <summary>
        /// 内部引用类型检查
        /// </summary>
        /// <param name="referenceType"></param>
        /// <exception cref="Exception"></exception>
        private void InternalCheckReferenceType(Type referenceType)
        {
#if UNITY_EDITOR
            if (null == referenceType)
                throw new Exception($"[{nameof(ReferencePoolManager)}]:引用类型错误");

            if (!referenceType.IsClass || referenceType.IsAbstract)
                throw new Exception($"[{nameof(ReferencePoolManager)}]:引用类型不是非抽象类类型");

            if (!typeof(IDisposable).IsAssignableFrom(referenceType))
                throw new Exception($"[{nameof(ReferencePoolManager)}]:引用类型'{referenceType.FullName}'无效");
#endif
        }

        /// <summary>
        /// 获取引用池
        /// </summary>
        /// <returns></returns>
        private ReferenceCollection GetReferenceCollection(Type referenceType)
        {
            if (null == referenceType)
                throw new Exception($"[{nameof(ReferencePoolManager)}]:引用类型无效");

            ReferenceCollection referenceCollection;
            lock (dicReferenceCollection)
            {
                if (!dicReferenceCollection.TryGetValue(referenceType, out referenceCollection))
                {
                    referenceCollection = new ReferenceCollection(referenceType);
                    dicReferenceCollection[referenceType] = referenceCollection;
                }
            }

            return referenceCollection;
        }
    }
}