using System;
using System.Collections.Generic;

namespace YRFramework.Runtime.Event
{
    /// <summary>
    /// 泛型事件管理器-优化(递归深度监测，GC优化)
    /// </summary>
    public partial class EventManager<TEvent> where TEvent : struct, Enum
    {
        /// <summary>
        /// 最大事件递归深度
        /// </summary>
        private const int MAX_RECURSION_DEPTH = 5;

        /// <summary>
        /// 事件回调列表池
        /// </summary>
        private Stack<List<Delegate>> stackDelegatePool;
        /// <summary>
        /// 事件栈
        /// </summary>
        private Stack<TEvent> stackEvent;

        private void InitOptimize()
        {
            stackDelegatePool = new Stack<List<Delegate>>();
            stackEvent = new Stack<TEvent>(MAX_RECURSION_DEPTH + 1);
        }

        private void ReleaseOptimize()
        {
            if (null != stackEvent)
            {
                stackEvent.Clear();
                stackEvent = null;
            }

            if (null != stackDelegatePool)
            {
                stackDelegatePool.Clear();
                stackDelegatePool = null;
            }
        }

        #region 私有方法
        /// <summary>
        /// 从对象池获取临时执行列表
        /// </summary>
        private List<Delegate> GetTempExecuteList()
        {
            if (stackDelegatePool.TryPop(out List<Delegate> list))
                return list;

            return new List<Delegate>(8);
        }

        /// <summary>
        /// 回收临时执行列表到对象池
        /// </summary>
        private void ReCycleTempExecuteList(List<Delegate> list)
        {
            if (stackDelegatePool.Count < MAX_RECURSION_DEPTH)
            {
                list.Clear();
                stackDelegatePool.Push(list);
            }
        }

        /// <summary>
        /// 事件栈深度检查
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private bool EventStackReachMax()
        {
            if (stackEvent.Count > MAX_RECURSION_DEPTH)
                return true;

            return false;
        }
        #endregion
    }
}