using cfg.Condition;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.Manager;
using YRFramework.Runtime.Utility;

namespace GamePlay.Runtime.Condition
{
    /// <summary>
    /// 条件回调委托
    /// </summary>
    /// <param name="conditionResult">条件结果</param>
    /// <param name="conditionId">条件id</param>
    public delegate void ConditionCallBack(bool conditionResult, int conditionId);

    /// <summary>
    /// 条件管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRFramework/ConditionManager")]
    public sealed partial class ConditionManager : YRFrameworkManager, IInit
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.Condition;
        #endregion

        #region 私有变量
        /// <summary>
        /// 条件执行者字典
        /// </summary>
        private Dictionary<int, ConditionExecuter> dicConditionExecuter;
        /// <summary>
        /// 根据触发事件，记录条件的嵌套字典
        /// </summary>
        private Dictionary<E_EventType, HashSet<ConditionLogicBase>> dicConditionLogic;
        /// <summary>
        /// 字符串和条件枚举名对应字典
        /// </summary>
        private Dictionary<string, E_ConditionType> dicConditionType;
        /// <summary>
        /// 条件信息字典
        /// </summary>
        private Dictionary<E_ConditionType, ConditionInfo> dicAllConditionInfo;
        #endregion

        async UniTask IInit.OnInit()
        {
            dicConditionExecuter = new Dictionary<int, ConditionExecuter>();
            dicConditionLogic = new Dictionary<E_EventType, HashSet<ConditionLogicBase>>();
            dicConditionType = new Dictionary<string, E_ConditionType>();
            dicAllConditionInfo = new Dictionary<E_ConditionType, ConditionInfo>();

            #region 获取所有条件工厂
            List<Type> listTypes = YRUtility.Assembly.GetTypes();
            foreach (Type type in listTypes)
            {
                ConditionFactoryAttribute conditionFactoryAttribute = type.GetCustomAttribute<ConditionFactoryAttribute>(false);
                if (null == conditionFactoryAttribute)
                    continue;

                if (dicAllConditionInfo.ContainsKey(conditionFactoryAttribute.ConditionType))
                {
                    Debug.LogError($"[{nameof(ConditionManager)}]已添加同类的{nameof(ConditionInfo)}:{conditionFactoryAttribute.ConditionType}");
                    continue;
                }

                if (Activator.CreateInstance(type) is not IConditionFactor iConditionFactor)
                {
                    Debug.LogError($"[{nameof(ConditionManager)}]{conditionFactoryAttribute.GetType().FullName}没有实现{nameof(IConditionFactor)}");
                    continue;
                }

                dicConditionType[conditionFactoryAttribute.ConditionType.ToString()] = conditionFactoryAttribute.ConditionType;
                dicAllConditionInfo[conditionFactoryAttribute.ConditionType] = new ConditionInfo(conditionFactoryAttribute.ConditionType, iConditionFactor);
            }

            compositeConditionFactor = dicAllConditionInfo[E_ConditionType.Composite].ConditionFactor;
            #endregion

            InitOptimize();

            await UniTask.CompletedTask;
        }

        void IInit.OnRelease()
        {
            ReleaseOptimize();

            if (null != dicAllConditionInfo)
            {
                dicAllConditionInfo.Clear();
                dicAllConditionInfo = null;
            }

            if (null != dicConditionType)
            {
                dicConditionType.Clear();
                dicAllConditionInfo = null;
            }

            if (null != dicConditionLogic)
            {
                dicConditionLogic.Clear();
                dicConditionLogic = null;
            }

            if (null != dicConditionExecuter)
            {
                dicConditionExecuter.Clear();
                dicConditionExecuter = null;
            }
        }

        #region API
        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public int AddCondition(string condition, ConditionCallBack success)
        {
            ConditionExecuter conditionExecuter = Game.ReferencePool.Acquire<ConditionExecuter>();

            int id = Game.ID.GetID(E_FrameworkManagerType.Condition);
            conditionExecuter.InitData(id, ParseExpression(condition, id, null), success);
            dicConditionExecuter.Add(conditionExecuter.Id, conditionExecuter);

            return id;
        }

        /// <summary>
        /// 移除条件
        /// </summary>
        /// <param name="id"></param>
        public void RemoveCondition(int id)
        {
            if (!dicConditionExecuter.TryGetValue(id, out ConditionExecuter conditionExecuter))
                return;

            dicConditionExecuter.Remove(id);
            Game.ReferencePool.Release(conditionExecuter);
        }

        /// <summary>
        /// 检查条件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void CheckCondition(int id, bool isRecheck = true)
        {
            if (!dicConditionExecuter.TryGetValue(id, out ConditionExecuter conditionExecuter))
                return;

            conditionExecuter.Execute(isRecheck);
        }
        #endregion

        /// <summary>
        /// 通过事件检查
        /// </summary>
        private void CheckByEvent(E_EventType eventType)
        {
            if (!dicConditionLogic.TryGetValue(eventType, out HashSet<ConditionLogicBase> setCondition))
            {
                stackEvent.Pop();
                return;
            }

            List<ConditionLogicBase> listTempCondition = GetTempListCondition(); // 防止递归调用，同一份数据被修改
            listTempCondition.AddRange(setCondition);

            for (int i = 0; i < listTempCondition.Count; ++i)
            {
                ConditionLogicBase condition = listTempCondition[i];
                condition.SetIsRecheck(); // 只设置该条件要重新计算
                CheckCondition(condition.ConditionExecuterId, false);
            }

            RecycleTempListCondition(listTempCondition);
        }

        /// <summary>
        /// 通过事件注册条件
        /// </summary>
        internal void RegisterConditionEvent(E_EventType[] eventTypes, ConditionLogicBase condition)
        {
            foreach (E_EventType eventType in eventTypes)
            {
                if (!dicConditionLogic.TryGetValue(eventType, out HashSet<ConditionLogicBase> setCondition))
                {
                    setCondition = new HashSet<ConditionLogicBase>();
                    dicConditionLogic.Add(eventType, setCondition);
                }

                setCondition.Add(condition);
            }
        }

        /// <summary>
        /// 通过事件取消注册条件
        /// </summary>
        internal void UnRegisterConditionEvent(E_EventType[] eventTypes, ConditionLogicBase condition)
        {
            foreach (E_EventType eventType in eventTypes)
            {
                if (!dicConditionLogic.TryGetValue(eventType, out HashSet<ConditionLogicBase> setCondition))
                    continue;

                setCondition.Remove(condition);
            }
        }
    }
}