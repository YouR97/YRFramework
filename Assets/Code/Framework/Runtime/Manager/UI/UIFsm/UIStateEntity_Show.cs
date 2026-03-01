using YRFramework.Runtime.FSM;

namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI显示状态(处于UI栈中，激活，可以先看到)
    /// </summary>
    public sealed class UIStateEntity_Show : UIFsmStateEntity
    {
        public override void OnEnter(FsmControllerEntity fsmControllerEntity)
        {
            base.OnEnter(fsmControllerEntity);

            uiFsmController.UIEntity.CurState = E_UIState.Show;

            uiFsmController.UIEntity.IsActive = true;
            uiFsmController.UIEntity.iUIController.Show();

            FrameworkGameEnter.Event.Broadcast(E_EventType.UIShow, uiFsmController.UIEntity.Info.Type); // 发送UI显示事件
        }

        public override void OnUpdate(float deltaTime, float realtimeSinceStartup)
        {
            uiFsmController.UIEntity.iUIController.Update(deltaTime, realtimeSinceStartup);
        }
    }
}