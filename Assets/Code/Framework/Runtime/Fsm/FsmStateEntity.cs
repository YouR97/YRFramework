namespace YRFramework.Runtime.FSM
{
    /// <summary>
    /// 状态机实体
    /// </summary>
    public abstract class FsmStateEntity : Core.Entity.Entity
    {
        /// <summary>
        /// 状态控制实体
        /// </summary>
        protected FsmControllerEntity fsmController;

        public virtual void OnEnter(FsmControllerEntity fsmControllerEntity)
        {
            fsmController = fsmControllerEntity;
        }

        public virtual void OnUpdate(float deltaTime, float realtimeSinceStartup)
        {
            
        }

        public virtual void OnExit()
        {
            
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected void ChangeState<T>() where T : FsmStateEntity
        {
            fsmController.ChangeState<T>();
        }
    }
}