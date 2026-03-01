using YRFramework.Runtime.FSM;

namespace GamePlay.Runtime.Procedure
{
    /// <summary>
    /// 流程状态控制
    /// </summary>
    public sealed class Fsm_ProcedureController : FsmControllerEntity
    {
        public override void OnInit()
        {
            base.OnInit();

            AddState<ProcedureState_Init>();
            AddState<ProcedureState_Home>();

            ChangeState<ProcedureState_Init>();
        }
    }
}