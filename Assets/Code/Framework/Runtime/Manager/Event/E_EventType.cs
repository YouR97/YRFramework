namespace YRFramework.Runtime
{
    /// <summary>
    /// 事件类型
    /// </summary>
    public enum E_EventType
    {
        #region 基础
        /// <summary>
        /// 游戏暂停
        /// </summary>
        GamePause,
        /// <summary>
        /// 游戏恢复
        /// </summary>
        GameResume,
        #endregion

        /// <summary>
        /// 资源管理器加载完成
        /// </summary>
        AssetsLoadComplete,
        /// <summary>
        /// 所有配置加载完成
        /// </summary>
        AllConfigLoadComplete,

        #region UI
        /// <summary>
        /// UI打开
        /// </summary>
        UIOpen,
        /// <summary>
        /// UI显示
        /// </summary>
        UIShow,
        /// <summary>
        /// UI动画显示完成
        /// </summary>
        UIShowAnim,
        /// <summary>
        /// UI隐藏
        /// </summary>
        UIHide,
        /// <summary>
        /// UI动画隐藏完成
        /// </summary>
        UIHideAnim,
        /// <summary>
        /// UI关闭
        /// </summary>
        UIClose,
        #endregion
    }
}