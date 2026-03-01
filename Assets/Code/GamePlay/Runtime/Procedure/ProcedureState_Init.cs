using YRFramework.Runtime.FSM;

namespace GamePlay.Runtime.Procedure
{
    /// <summary>
    /// 流程状态-初始化
    /// </summary>
    public sealed class ProcedureState_Init : FsmStateEntity
    {
        public override void OnEnter(FsmControllerEntity fsmControllerEntity)
        {
            base.OnEnter(fsmControllerEntity);

            Game.Setting.ApplyAllSetting();

            fsmController.ChangeState<ProcedureState_Home>();
        }
    }
}