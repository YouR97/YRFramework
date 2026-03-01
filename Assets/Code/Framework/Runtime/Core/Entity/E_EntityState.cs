namespace YRFramework.Runtime.Core.Entity
{
    /// <summary>
    /// 实体状态枚举
    /// </summary>
    public enum E_EntityState : sbyte
    {
        None = YRConsts.INVALID_INT,
        /// <summary>
        /// 运行中
        /// </summary>
        Running,
        /// <summary>
        /// 清除
        /// </summary>
        Clear,
    }
}