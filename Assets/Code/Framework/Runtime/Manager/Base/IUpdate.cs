namespace YRFramework.Runtime
{
    /// <summary>
    /// 更新接口
    /// </summary>
    public interface IUpdate
    {
        /// <summary>
        /// 轮询
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="realtimeSinceStartup"></param>
        void OnUpdate(float deltaTime, float realtimeSinceStartup);
    }
}