namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI控制器接口
    /// </summary>
    public interface IUIController
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="rc"></param>
        void Awake(ReferenceCollector rc);

        void Show();

        /// <summary>
        /// 每帧更新
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="realtimeSinceStartup"></param>
        void Update(float deltaTime, float realtimeSinceStartup);

        void Hide();
    }
}