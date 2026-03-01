namespace YRFramework.Runtime.Storage
{
    /// <summary>
    /// 本地缓存数据工厂接口
    /// </summary>
    public interface IStorageDataFactory
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public IStorageData Create();
    }
}