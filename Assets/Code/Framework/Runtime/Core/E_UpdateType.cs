namespace YRFramework.Runtime.Core
{
    /// <summary>
    /// 更新类型
    /// </summary>
    public enum E_UpdateType : sbyte
    {
        None = YRConsts.INVALID_INT,
        Update = 0,
        LateUpdate = 1,
        FixedUpdate = 2,
    }
}