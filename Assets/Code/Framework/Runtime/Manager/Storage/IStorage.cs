using System.Collections.Generic;

namespace YRFramework.Runtime.Storage
{
    /// <summary>
    /// 本地存储接口
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// 脏标记
        /// </summary>
        public bool IsDirty { get; }

        /// <summary>
        /// 设置指定key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="objectdata"></param>
        public abstract void Set<T>(string key, T objectdata);

        /// <summary>
        /// 加载指定数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract string Load(string key);
        
        /// <summary>
        /// 加载字典
        /// </summary>
        /// <param name="key"></param>
        /// <param name="jsonData"></param>
        /// <param name="dicStorage"></param>
        public abstract void LoadToDic(string key, string jsonData, ref Dictionary<string, IStorageData> dicStorage);

        /// <summary>
        /// 保存数据
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// 清除所有本地数据
        /// </summary>
        public abstract void Clear();
    }
}