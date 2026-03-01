using YRFramework.Runtime.Storage;

namespace GamePlay.Runtime.Storage
{
    [StorageDataFactory(KEY)]
    public sealed class Storage_PictureSettingFactory : StorageDataFactoryBase<Storage_PictureSettingData>, IStorageDataFactory
    {
        /// <summary>
        /// 缓存Key值
        /// </summary>
        private const string KEY = nameof(Storage_PictureSettingData);

        IStorageData IStorageDataFactory.Create()
        {
            return new Storage_PictureSettingData();
        }

        internal static Storage_PictureSettingData GetData()
        {
            return Get(KEY);
        }
    }
}