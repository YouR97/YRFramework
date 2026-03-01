namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI窗口状态
    /// </summary>
    public enum E_WindowState : sbyte
    {
        None = YRConsts.INVALID_INT,
        /// <summary>
        /// 保持当前窗口
        /// </summary>
        Exist,
        /// <summary>
        /// 隐藏窗口
        /// </summary>
        Hide,
        /// <summary>
        /// 卸载当前窗口
        /// </summary>
        Destroy,
    }
}