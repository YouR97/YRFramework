namespace YRFramework.Runtime.Storage
{
    /// <summary>
    /// 本地缓存数据接口
    /// </summary>
    public interface IStorageData
    {
        /// <summary>
        /// 脏标记
        /// </summary>
        public bool IsDirty { get; }
    }
}