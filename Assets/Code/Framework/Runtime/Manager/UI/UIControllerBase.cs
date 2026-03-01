using System;

namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI控制器基类
    /// </summary>
    public abstract class UIControllerBase : IDisposable, IUIController
    {
        /// <summary>
        /// 是否接受输入
        /// </summary>
        protected bool isAcceptInput = true;

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; private set; }

        #region 接口方法
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="rc"></param>
        void IUIController.Awake(ReferenceCollector rc)
        {
            Awake(rc);
        }

        /// <summary>
        /// 显示
        /// </summary>
        void IUIController.Show()
        {
            IsShow = true;
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        void IUIController.Hide()
        {
            IsShow = false;
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="realtimeSinceStartup"></param>
        void IUIController.Update(float deltaTime, float realtimeSinceStartup)
        {
            Update(deltaTime, realtimeSinceStartup);
        }
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="rc"></param>
        protected abstract void Awake(ReferenceCollector rc);

        /// <summary>
        /// 每帧更新
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="realtimeSinceStartup"></param>
        protected virtual void Update(float deltaTime, float realtimeSinceStartup)
        {
        }

        public abstract void Close();

        /// <summary>
        /// 按钮点击方法
        /// </summary>
        /// <param name="action"></param>
        protected void OnClick(Action action)
        {
            if (!isAcceptInput)
                return;

            action?.Invoke();
        }

        public virtual void Dispose()
        {
        }
    }
}