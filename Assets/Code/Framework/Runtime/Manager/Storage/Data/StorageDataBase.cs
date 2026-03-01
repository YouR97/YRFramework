using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace YRFramework.Runtime.Storage
{
    /// <summary>
    /// 本地缓存数据基类
    /// </summary>
    public abstract class StorageDataBase : INotifyPropertyChanged, IStorageData
    {
        /// <summary>
        /// 脏标记
        /// </summary>
        public bool IsDirty { get; protected set; }

        /// <summary>
        /// 属性变化回调事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected StorageDataBase()
        {
            PropertyChanged += (sender, arg) =>
            {
                IsDirty = true;
            };
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetField<T>(T value, ref T field, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) // 相同直接返回
                return false;

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return true;
        }
    }
}