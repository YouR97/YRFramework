using cfg.Condition;

namespace GamePlay.Runtime.Condition
{
    /// <summary>
    /// (通用)复合条件-工厂类
    /// </summary>
    [ConditionFactory(E_ConditionType.Composite)]
    public sealed class Condition_CompositeFactory : IConditionFactor
    {
        public ConditionLogicBase Create()
        {
            return Game.ReferencePool.Acquire<Condition_CompositeLogic>();
        }
    }
}