using YRFramework.Runtime.FSM;

namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI隐藏状态(处于UI栈中，不可见，可以重复打开)
    /// </summary>
    public sealed class UIStateEntity_Hide : UIFsmStateEntity
    {
        public override void OnEnter(FsmControllerEntity fsmControllerEntity)
        {
            base.OnEnter(fsmControllerEntity);

            uiFsmController.UIEntity.iUIController.Hide();
            uiFsmController.UIEntity.IsActive = false;

            uiFsmController.UIEntity.CurState = E_UIState.Hide;
            FrameworkGameEnter.Event.Broadcast(E_EventType.UIHide, uiFsmController.UIEntity.Info.Type); // 发送UI隐藏事件
        }
    }
}