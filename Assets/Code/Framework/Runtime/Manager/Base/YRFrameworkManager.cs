using Sirenix.OdinInspector;

namespace YRFramework.Runtime.Manager
{
    /// <summary>
    /// YR框架管理器抽象类
    /// </summary>
    public abstract class YRFrameworkManager : MonoSingleton<YRFrameworkManager>
    {
        /// <summary>
        /// 管理器类型
        /// </summary>
        [BoxGroup("管理器"), LabelText("管理器类型"), ShowInInspector, PropertyOrder(-1), ReadOnly]
        public abstract E_FrameworkManagerType ManagerType { get; protected set; }

        [BoxGroup("管理器"), LabelText("管理器索引"), ShowInInspector, PropertyOrder(-1), ReadOnly]
        public int Index { get { return (int)ManagerType; } }

        /// <summary>
        /// 框架管理器注册
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            FrameworkGameEnter.RegisterManager(this);
        }

        #region API
        /// <summary>
        /// 重置管理器名
        /// </summary>
        public void ResetName()
        {
            gameObject.name = $"{ManagerType}_{Index}";
        }
        #endregion
    }
}