using cfg.Condition;
using System;

namespace GamePlay.Runtime.Condition
{
    /// <summary>
    /// 条件工厂特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ConditionFactoryAttribute : Attribute
    {
        /// <summary>
        /// 条件类型
        /// </summary>
        public E_ConditionType ConditionType { get; private set; }

        private ConditionFactoryAttribute()
        { }

        public ConditionFactoryAttribute(E_ConditionType conditionType)
        {
            ConditionType = conditionType;
        }
    }
}