using UnityEngine;

namespace YRFramework.Runtime
{
    /// <summary>
    /// Mono单例
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static T ins;
        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object selfLock = new();

        /// <summary>
        /// 单例
        /// </summary>
        /// <value>The instance.</value>
        public static T Ins
        {
            get
            {
                if (ins == null)
                {
                    lock (selfLock)
                    {
                        ins = FindFirstObjectByType<T>();
                        if (ins == null)
                        {
                            GameObject obj = new()
                            {
                                name = $"{typeof(T).Name}(自动生成)"
                            };
                            ins = obj.AddComponent<T>();
                        }
                    }
                }

                return ins;
            }
        }

        /// <summary>
        /// 重载请确保调用
        /// </summary>
        protected virtual void Awake()
        {
            InitializeSingleton();
        }

        /// <summary>
        /// 初始化单例
        /// </summary>
        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying)
                return;

            ins = this as T;
        }
    }
}