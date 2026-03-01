using CodiceApp.EventTracking.Plastic;
using System.Collections.Generic;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.Manager;

namespace GamePlay.Runtime.Condition
{
    /// <summary>
    /// 条件管理器-优化(递归深度监测，GC优化)
    /// </summary>
    public sealed partial class ConditionManager : YRFrameworkManager
    {
        /// <summary>
        /// 最大条件递归深度
        /// </summary>
        private const int MAX_RECURSION_DEPTH = 5;

        /// <summary>
        /// 事件栈
        /// </summary>
        private Stack<E_EventType> stackEvent;
        /// <summary>
        /// 临时条件信息列表对象池(防止递归调用时被clear)
        /// </summary>
        private Stack<List<ConditionLogicBase>> stackTempListConditionPool;

        private void InitOptimize()
        {
            stackEvent = new Stack<E_EventType>(MAX_RECURSION_DEPTH + 1);
            stackTempListConditionPool = new Stack<List<ConditionLogicBase>>();

            Game.Event.AfterSendCallBack += OnEventStackCheck;
        }

        private void ReleaseOptimize()
        {
            Game.Event.AfterSendCallBack -= OnEventStackCheck;

            if (null != stackTempListConditionPool)
            {
                stackTempListConditionPool.Clear();
                stackTempListConditionPool = null;
            }

            if (null != stackEvent)
            {
                stackEvent.Clear();
                stackEvent = null;
            }
        }

        /// <summary>
        /// 事件触发(先检查栈数量)
        /// </summary>
        /// <param name="eventType"></param>
        private void OnEventStackCheck(E_EventType eventType)
        {
            stackEvent.Push(eventType);
            if (stackEvent.Count > MAX_RECURSION_DEPTH)
            {
                string log = string.Empty;
                foreach (E_EventType type in stackEvent)
                {
                    log += $"{type},";
                }
                Debug.LogError($"[{nameof(ConditionManager)}]条件系统递归触发超过{MAX_RECURSION_DEPTH}次，请检查逻辑:{log}");
                stackEvent.Pop();

                return;
            }

            CheckByEvent(eventType);

            stackEvent.Pop();
        }

        /// <summary>
        /// 从对象池获取中间条件列表
        /// </summary>
        /// <returns></returns>
        private List<ConditionLogicBase> GetTempListCondition()
        {
            if (!stackTempListConditionPool.TryPop(out List<ConditionLogicBase> listCondition))
                listCondition = new List<ConditionLogicBase>();

            return listCondition;
        }

        /// <summary>
        /// 回收中间条件列表到对象池
        /// </summary>
        /// <param name="listCondition"></param>
        private void RecycleTempListCondition(List<ConditionLogicBase> listCondition)
        {
            if (null == listCondition)
                return;

            listCondition.Clear();
            stackTempListConditionPool.Push(listCondition);
        }
    }
}