using System;
using System.Collections.Generic;
using UnityEngine;
using YRFramework.Runtime.Core.System;

namespace YRFramework.Runtime.FSM
{
    /// <summary>
    /// 状态控制器实体
    /// </summary>
    public abstract partial class FsmControllerEntity : Core.Entity.Entity, IInitSystem, IUpdateSystem
    {
        /// <summary>
        /// 状态字典
        /// </summary>
        private Dictionary<Type, FsmStateEntity> dicStates;

        /// <summary>
        /// 当前状态
        /// </summary>
        public FsmStateEntity CurState { get; private set; }

        public virtual void OnInit()
        {
            dicStates = new();
        }

        public virtual void OnUpdate(float deltaTime, float realtimeSinceStartup)
        {
            CurState?.OnUpdate(deltaTime, realtimeSinceStartup);
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public virtual void ChangeState<T>() where T : FsmStateEntity
        {
            if (!dicStates.TryGetValue(typeof(T), out FsmStateEntity fsmStateEntity))
            {
                Debug.LogError($"[{nameof(FsmControllerEntity)}]:不存在{typeof(T)}状态，切换失败");
                return;
            }

            CurState?.OnExit();
            CurState = fsmStateEntity;
            CurState.OnEnter(this);
        }

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T AddState<T>() where T : FsmStateEntity
        {
            T state = AddChid<T>();
            dicStates.Add(typeof(T), state);

            return state;
        }

        /// <summary>
        /// 添加状态，带一个参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P1"></typeparam>
        /// <param name="p1"></param>
        /// <returns></returns>
        protected T AddState<T, P1>(P1 p1) where T : FsmStateEntity
        {
            T state = AddChid<T, P1>(p1);
            dicStates.Add(typeof(T), state);

            return state;
        }

        /// <summary>
        /// 添加状态，带两个参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P1"></typeparam>
        /// <param name="p1"></param>
        /// <returns></returns>
        protected T AddState<T, P1, P2>(P1 p1, P2 p2) where T : FsmStateEntity
        {
            T state = AddChid<T, P1, P2>(p1, p2);
            dicStates.Add(typeof(T), state);

            return state;
        }

        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="fsmStateEntity"></param>
        protected void RemoveState(FsmStateEntity fsmStateEntity)
        {
            Type type = fsmStateEntity.GetType();
            bool isRemove = dicStates.Remove(type);
            if (!isRemove)
                return;

            if (fsmStateEntity == CurState)
                fsmStateEntity.OnExit();

            RemoveChild(fsmStateEntity);
        }

        public override void Dispose()
        {
            if (null != CurState)
            {
                CurState.OnExit();
                CurState = null;
            }

            if (null != dicStates)
            {
                foreach (var i in dicStates.Values)
                {
                    i.Dispose();
                }

                dicStates.Clear();
                dicStates = null;
            }

            // ClearBlcakboard(); // TODO

            base.Dispose();
        }
    }
}