using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using YRFramework.Runtime.Manager;

namespace YRFramework.Runtime.Event
{
    /// <summary>
    /// 事件管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRFramework/EventManager")]
    public partial class EventManager<TEvent> : YRFrameworkManager, IInit where TEvent : struct, Enum
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.Event;
        #endregion

        /// <summary>
        /// 事件监听字典
        /// </summary>
        private Dictionary<TEvent, HashSet<Delegate>> dicEvent;
        /// <summary>
        /// 发送事件后回调
        /// </summary>
        private Action<TEvent> afterSendCallBack;

        /// <summary>
        /// 发送事件后回调
        /// </summary>
        public event Action<TEvent> AfterSendCallBack
        {
            add { afterSendCallBack += value; }
            remove { afterSendCallBack -= value; }
        }

        async UniTask IInit.OnInit()
        {
            dicEvent = new Dictionary<TEvent, HashSet<Delegate>>();

            await UniTask.CompletedTask;
        }

        void IInit.OnRelease()
        {
            RemoveAllListener();

            if (null != dicEvent)
            {
                dicEvent.Clear();
                dicEvent = null;
            }
        }

        #region API
        /// <summary>
        /// 添加无参监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public void AddListener(TEvent eventType, Action callback)
        {
            if (null == callback)
                return;

            AddListener(eventType, (Delegate)callback);
        }

        /// <summary>
        /// 添加有参监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public void AddListener<T>(TEvent eventType, Action<T> callback)
        {
            if (null == callback)
                return;

            AddListener(eventType, (Delegate)callback);
        }

        /// <summary>
        /// 移除所有监听
        /// </summary>
        public void RemoveAllListener()
        {
            dicEvent.Clear();
        }

        /// <summary>
        /// 移除无参监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public void RemoveListener(TEvent eventType, Action callback)
        {
            RemoveListener(eventType, (Delegate)callback);
        }

        /// <summary>
        /// 移除有参监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public void RemoveListener<T>(TEvent eventType, Action<T> callback)
        {
            RemoveListener(eventType, (Delegate)callback);
        }

        /// <summary>
        /// 无参广播
        /// </summary>
        /// <param name="eventType"></param>
        public void Broadcast(TEvent eventType)
        {
            try
            {
                if (!dicEvent.TryGetValue(eventType, out HashSet<Delegate> setEvent))
                {
                    afterSendCallBack?.Invoke(eventType);
                    return;
                }

                foreach (Delegate action in setEvent)
                {
                    if (action is Action callback)
                        callback?.Invoke();
                }

                afterSendCallBack?.Invoke(eventType);
            }
            catch (Exception e)
            {
                Debug.LogError($"[{nameof(EventManager<TEvent>)}]:无参{eventType}事件广播异常，错误信息：{e}");
            }
        }

        /// <summary>
        /// 有参广播
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventType"></param>
        /// <param name="arg"></param>
        public void Broadcast<T>(TEvent eventType, T arg)
        {
            try
            {
                if (!dicEvent.TryGetValue(eventType, out HashSet<Delegate> setEvent))
                    return;

                foreach (Delegate action in setEvent)
                {
                    if (action is Action<T> callbackArg)
                        callbackArg?.Invoke(arg);
                    else if (action is Action callback)
                        callback?.Invoke();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[{nameof(EventManager<TEvent>)}]:单参{eventType}事件广播异常，错误信息：{e}");
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 添加监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        private void AddListener(TEvent eventType, Delegate callback)
        {
            try
            {
                if (!dicEvent.TryGetValue(eventType, out HashSet<Delegate> setEvent))
                    dicEvent.Add(eventType, setEvent);

                setEvent.Add(callback);
            }
            catch (Exception e)
            {
                Debug.LogError($"[{nameof(EventManager<TEvent>)}]:注册{eventType}事件失败：{e}");
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="callback"></param>
        private void RemoveListener(TEvent eventType, Delegate callback)
        {
            try
            {
                if (!dicEvent.TryGetValue(eventType, out HashSet<Delegate> setEvent))
                    return;

                setEvent.Remove(callback);
            }
            catch (Exception e)
            {
                Debug.LogError($"[{nameof(EventManager<TEvent>)}]:取消注册{eventType}事件失败：{e}");
            }
        }
        #endregion
    }
}