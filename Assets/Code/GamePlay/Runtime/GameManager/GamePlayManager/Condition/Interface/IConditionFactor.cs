namespace GamePlay.Runtime.Condition
{
    /// <summary>
    /// 条件工厂接口
    /// </summary>
    public interface IConditionFactor
    {
        ConditionLogicBase Create();
    }
}