using cfg.Condition;
using YRFramework.Runtime;

namespace GamePlay.Runtime.Condition
{
    /// <summary>
    /// [通用]失败条件(无参数)
    /// </summary>
    public sealed class Condition_FailLogic : ConditionLogicBase
    {
        public override E_EventType[] EventTypes => new E_EventType[0] { };

        public Condition_FailLogic() : base(E_ConditionType.Fail) { }

        #region API
        protected override void OnCheck()
        {
            cacheResult = false;
            cacheProgress = 0f;
            cacheLocalization = string.Empty;
        }

        public override void Recycle()
        {
            Game.ReferencePool.Release(this);
        }
        #endregion
    }
}