using YRFramework.Runtime.FSM;

namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI状态机状态实体
    /// </summary>
    public class UIFsmStateEntity : FsmStateEntity
    {
        /// <summary>
        /// UI状态机控制器
        /// </summary>
        protected FSM_UIControllerEntity uiFsmController;

        public override void OnEnter(FsmControllerEntity fsmControllerEntity)
        {
            base.OnEnter(fsmControllerEntity);

            uiFsmController = (FSM_UIControllerEntity)fsmControllerEntity;
        }
    }
}