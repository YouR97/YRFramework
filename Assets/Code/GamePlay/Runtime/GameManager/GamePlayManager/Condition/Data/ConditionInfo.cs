using cfg.Condition;

namespace GamePlay.Runtime.Condition
{
    /// <summary>
    /// 条件信息
    /// </summary>
    public struct ConditionInfo
    {
        /// <summary>
        /// 条件类型
        /// </summary>
        public E_ConditionType ConditionType { get; private set; }

        /// <summary>
        /// 条件工厂
        /// </summary>
        internal IConditionFactor ConditionFactor { get; private set; }

        internal ConditionInfo(E_ConditionType conditionType, IConditionFactor conditionFactor)
        {
            ConditionType = conditionType;
            ConditionFactor = conditionFactor;
        }
    }
}