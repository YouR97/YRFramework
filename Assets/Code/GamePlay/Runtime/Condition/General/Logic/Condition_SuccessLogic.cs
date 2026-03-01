using cfg.Condition;
using YRFramework.Runtime;

namespace GamePlay.Runtime.Condition
{
    /// <summary>
    /// [通用]成功条件(无参数)
    /// </summary>
    public sealed class Condition_SuccessLogic : ConditionLogicBase
    {
        public override E_EventType[] EventTypes => new E_EventType[0] { };

        public Condition_SuccessLogic() : base(E_ConditionType.Success) { }

        #region API
        protected override void OnCheck()
        {
            cacheResult = true;
            cacheProgress = 1f;
            cacheLocalization = string.Empty;
        }

        public override void Recycle()
        {
            Game.ReferencePool.Release(this);
        }
        #endregion
    }
}