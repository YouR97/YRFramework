using UnityEngine;
using YRFramework.Runtime.FSM;

namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI打开状态(UI实例已经创建，处于UI栈中，未激活不可见，只能切换显示状态进行显示)
    /// </summary>
    public sealed class UIStateEntity_Open : UIFsmStateEntity
    {
        public override void OnEnter(FsmControllerEntity fsmControllerEntity)
        {
            base.OnEnter(fsmControllerEntity);

            uiFsmController.UIEntity.IsActive = false;
            if (FrameworkGameEnter.UI.TryGetUIGroupRoot(uiFsmController.UIEntity.Info.GroupType, out Transform tsRoot))
                uiFsmController.UIEntity.GoRoot.transform.SetParent(tsRoot);

            FrameworkGameEnter.UI.AddUIStack(uiFsmController.UIEntity);

            uiFsmController.UIEntity.CurState = E_UIState.Open;
            FrameworkGameEnter.Event.Broadcast(E_EventType.UIOpen, uiFsmController.UIEntity.Info.Type); // 发送UI打开事件
        }
    }
}