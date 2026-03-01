namespace YRFramework.Runtime.Storage
{
    /// <summary>
    /// 本地缓存数据工厂基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class StorageDataFactoryBase<T> where T : IStorageData
    {
        protected static T Get(string key)
        {
            return FrameworkGameEnter.Storage.GetStorage<T>(key);
        }
    }
}