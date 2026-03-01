using cfg.Condition;

namespace GamePlay.Runtime.Condition
{
    /// <summary>
    /// (通用)失败条件-工厂类
    /// </summary>
    [ConditionFactory(E_ConditionType.Fail)]
    public sealed class Condition_FailFactory : IConditionFactor
    {
        public ConditionLogicBase Create()
        {
            Condition_FailLogic condition_FailLogic = Game.ReferencePool.Acquire<Condition_FailLogic>();



            return condition_FailLogic;
        }
    }
}