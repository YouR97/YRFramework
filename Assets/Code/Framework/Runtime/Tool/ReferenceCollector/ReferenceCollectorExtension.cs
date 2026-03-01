using UnityEngine;

namespace YRFramework.Runtime
{
    /// <summary>
    /// ReferenceCollector扩展脚本
    /// </summary>
    public static class ReferenceCollectorExtension
    {
        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rc"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T RcGetComponent<T>(this ReferenceCollector rc, string key) where T : Component
        {
            GameObject go = rc.Get<GameObject>(key);
            if (null == go)
                throw new System.NullReferenceException($"获取引用失败，key:{key}");

            return (T)go.GetComponent(typeof(T));
        }

        /// <summary>
        /// 获取Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rc"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T RcGetObject<T>(this ReferenceCollector rc, string key) where T : Object
        {
            return (T)rc.Get<Object>(key);
        }

        /// <summary>
        /// 获取GameObject
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static GameObject RcGetGameObject(this ReferenceCollector rc, string key)
        {
            return (GameObject)rc.GetObject(key);
        }
    }
}