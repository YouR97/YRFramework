namespace YRFramework.Runtime.Manager
{
    /// <summary>
    /// 框架管理器类型
    /// </summary>
    public enum E_FrameworkManagerType : byte
    {
        #region 框架
        /// <summary>
        /// 基础管理器
        /// </summary>
        Base,
        /// <summary>
        /// 事件管理器
        /// </summary>
        Event,
        /// <summary>
        /// ID管理器
        /// </summary>
        ID,
        /// <summary>
        /// 计时器管理器
        /// </summary>
        Timer,
        /// <summary>
        /// 资源管理器
        /// </summary>
        Asset,
        /// <summary>
        /// 本地缓存管理器
        /// </summary>
        Storage,
        /// <summary>
        /// 红点管理器
        /// </summary>
        RedPoint,
        /// <summary>
        /// 本地化管理器
        /// </summary>
        Localization,
        /// <summary>
        /// 引用池管理器
        /// </summary>
        ReferencePool,
        /// <summary>
        /// 对象池管理器
        /// </summary>
        ObjectPool,
        /// <summary>
        /// 配置管理器
        /// </summary>
        DataTable,
        /// <summary>
        /// 实体管理器
        /// </summary>
        Entity,
        /// <summary>
        /// 声音管理器
        /// </summary>
        Audio,
        /// <summary>
        /// UI管理器
        /// </summary>
        UI,
        #endregion

        #region GamePlay
        /// <summary>
        /// 设置管理器
        /// </summary>
        Setting,
        /// <summary>
        /// 加载管理器
        /// </summary>
        Loading,
        /// <summary>
        /// 条件管理器
        /// </summary>
        Condition,
        /// <summary>
        /// 相机管理器
        /// </summary>
        Camera,
        /// <summary>
        /// 玩家管理器
        /// </summary>
        PlayerInfo,
        /// <summary>
        /// 关卡管理器
        /// </summary>
        Level,
        /// <summary>
        /// 战斗管理器
        /// </summary>
        Fight,
        #endregion

        #region Debug
        /// <summary>
        /// 帧率显示管理器
        /// </summary>
        FPS,
        #endregion

        /// <summary>
        /// 占位
        /// </summary>
        Max,
    }
}