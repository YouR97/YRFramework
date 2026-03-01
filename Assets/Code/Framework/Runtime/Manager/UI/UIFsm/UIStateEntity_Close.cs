using UnityEngine;
using YRFramework.Runtime.FSM;

namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI关闭状态(不处于UI栈中，缓存，等待打开或者销毁)
    /// </summary>
    public sealed class UIStateEntity_Close : UIFsmStateEntity
    {
        public override void OnEnter(FsmControllerEntity fsmControllerEntity)
        {
            base.OnEnter(fsmControllerEntity);

            uiFsmController.UIEntity.IsActive = false;
            uiFsmController.UIEntity.GoRoot.transform.SetParent(FrameworkGameEnter.UI.tsCacheUIRoot);
            uiFsmController.UIEntity.preCloseTime = Time.realtimeSinceStartup;

            FrameworkGameEnter.UI.RemoveUIStack(uiFsmController.UIEntity);

            uiFsmController.UIEntity.CurState = E_UIState.Close;
            FrameworkGameEnter.Event.Broadcast(E_EventType.UIClose, uiFsmController.UIEntity.Info.Type); // 发送UI关闭事件
        }
    }
}