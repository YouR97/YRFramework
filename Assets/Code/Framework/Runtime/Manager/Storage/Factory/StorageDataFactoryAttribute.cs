using System;

namespace YRFramework.Runtime.Storage
{
    /// <summary>
    /// 本地缓存数据工厂特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class StorageDataFactoryAttribute : Attribute
    {
        /// <summary>
        /// 缓存数据Key
        /// </summary>
        public string Key { get; private set; }

        private StorageDataFactoryAttribute() 
        { }

        public StorageDataFactoryAttribute(string key)
        {
            Key = key;
        }
    }
}