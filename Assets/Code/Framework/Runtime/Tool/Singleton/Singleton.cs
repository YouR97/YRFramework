using System;

namespace YRFramework.Runtime
{
    /// <summary>
    /// 普通单例
    /// </summary>
    public abstract class Singleton<T> where T : Singleton<T>
    {
        /// <summary>
        /// 单例
        /// </summary>
        protected static T ins;

        /// <summary>
        /// 单例
        /// </summary>
        /// <value>The instance.</value>
        public static T Ins // 通过反射实例化，子类可以实现私有构造函数
        {
            get
            {
                ins ??= Activator.CreateInstance(typeof(T), true) as T;

                return ins;
            }
        }
    }
}