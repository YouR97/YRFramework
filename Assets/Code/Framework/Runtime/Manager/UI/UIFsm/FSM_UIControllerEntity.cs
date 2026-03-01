using YRFramework.Runtime.FSM;

namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI状态控制实体
    /// </summary>
    public class FSM_UIControllerEntity : FsmControllerEntity
    {
        internal UIEntity UIEntity;

        public override void OnInit()
        {
            base.OnInit();

            UIEntity = (UIEntity)Parent;

            AddState<UIStateEntity_Open>();
            AddState<UIStateEntity_Show>();
            AddState<UIStateEntity_Hide>();
            AddState<UIStateEntity_Close>();
        }

        public override void OnUpdate(float deltaTime, float realtimeSinceStartup)
        {
            base.OnUpdate(deltaTime, realtimeSinceStartup);

            switch (UIEntity.UINextState)
            {
                case E_UIState.Open:
                    {
                        ChangeOpenState();
                    }
                    break;
                case E_UIState.Show:
                    {
                        ChangeShowState();
                    }
                    break;
                case E_UIState.Hide:
                    {
                        ChangeHideState();
                    }
                    break;
                case E_UIState.Close:
                    {
                        ChangeCloseState();
                    }
                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// 切换开启状态
        /// </summary>
        private void ChangeOpenState()
        {
            UIEntity.UINextState = E_UIState.None;

            if (E_UIState.Show == UIEntity.CurState || E_UIState.Open == UIEntity.CurState) // 当前状态不能切换Open
                return;

            if (E_UIState.Hide == UIEntity.CurState)
                ChangeState<UIStateEntity_Close>();

            ChangeState<UIStateEntity_Open>();
        }

        /// <summary>
        /// 切换显示状态
        /// </summary>
        private void ChangeShowState()
        {
            UIEntity.UINextState = E_UIState.None;

            if (E_UIState.Show == UIEntity.CurState)
                return;

            if (E_UIState.None == UIEntity.CurState || E_UIState.Close == UIEntity.CurState)
                ChangeState<UIStateEntity_Open>();

            ChangeState<UIStateEntity_Show>();
        }

        /// <summary>
        /// 切换隐藏状态
        /// </summary>
        private void ChangeHideState()
        {
            UIEntity.UINextState = E_UIState.None;

            if (E_UIState.Hide == UIEntity.CurState)
                return;

            if (E_UIState.Show != UIEntity.CurState)
                return;

            ChangeState<UIStateEntity_Hide>();
        }

        /// <summary>
        /// 切换关闭状态
        /// </summary>
        private void ChangeCloseState()
        {
            UIEntity.UINextState = E_UIState.None;

            if (E_UIState.None == UIEntity.CurState || E_UIState.Close == UIEntity.CurState)
                return;

            ChangeState<UIStateEntity_Close>();
        }
    }
}