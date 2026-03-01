using Cysharp.Threading.Tasks;
using GamePlay.Runtime.Loading;
using GamePlay.Runtime.UI;
using YRFramework.Runtime.FSM;

namespace GamePlay.Runtime.Procedure
{
    /// <summary>
    /// 流程状态-Home
    /// </summary>
    public sealed class ProcedureState_Home : FsmStateEntity
    {
        public override void OnEnter(FsmControllerEntity fsmControllerEntity)
        {
            base.OnEnter(fsmControllerEntity);

            Game.Loading.StartLoading(new InitLoading(async () =>
            {
                await UI_HomeFactory.Open();
            })).Forget();
        }
    }
}