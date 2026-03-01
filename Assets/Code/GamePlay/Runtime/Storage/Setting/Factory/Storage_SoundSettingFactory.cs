using YRFramework.Runtime.Storage;

namespace GamePlay.Runtime.Storage
{
    /// <summary>
    /// 本地缓存-声音设置工厂
    /// </summary>
    [StorageDataFactory(KEY)]
    public sealed class Storage_SoundSettingFactory : StorageDataFactoryBase<Storage_SoundSettingData>, IStorageDataFactory
    {
        /// <summary>
        /// 缓存Key值
        /// </summary>
        private const string KEY = nameof(Storage_SoundSettingData);

        IStorageData IStorageDataFactory.Create()
        {
            return new Storage_SoundSettingData();
        }

        internal static Storage_SoundSettingData GetData()
        {
            return Get(KEY);
        }
    }
}