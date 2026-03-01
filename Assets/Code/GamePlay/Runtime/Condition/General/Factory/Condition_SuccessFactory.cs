using cfg.Condition;

namespace GamePlay.Runtime.Condition
{
    /// <summary>
    /// (通用)成功条件-工厂类
    /// </summary>
    [ConditionFactory(E_ConditionType.Success)]
    public sealed class Condition_SuccessFactory : IConditionFactor
    {
        public ConditionLogicBase Create()
        {
            return Game.ReferencePool.Acquire<Condition_SuccessLogic>();
        }
    }
}