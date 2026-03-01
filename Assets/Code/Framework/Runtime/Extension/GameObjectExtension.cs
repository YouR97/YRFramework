using UnityEngine;

namespace YRFramework.Runtime
{
    public static class GameObjectExtension
    {
        /// <summary>
        /// 是否显示(通过Scale)
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isShow"></param>
        public static void IsScaleShow(this GameObject go, bool isShow)
        {
            go.transform.localScale = isShow ? Vector3.one : Vector3.zero;
        }

        /// <summary>
        /// 获取组件，没有则添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if (!go.TryGetComponent(out T t))
                t = go.AddComponent<T>();

            return t;
        }

        /// <summary>
        /// 递归设置Layer
        /// </summary>
        /// <param name="go"></param>
        /// <param name="layer"></param>
        public static void SetLayerRecursively(this GameObject go, int layer)
        {
            go.layer = layer;

            foreach (Transform tsChild in go.transform)
            {
                SetLayerRecursively(tsChild.gameObject, layer);
            }
        }

        /// <summary>
        /// 递归设置layer
        /// </summary>
        /// <param name="go"></param>
        /// <param name="layerName"></param>
        public static void SetLayerRecursively(this GameObject go, string layerName)
        {
            int layer = LayerMask.NameToLayer(layerName);

            go.SetLayerRecursively(layer);
        }

        /// <summary>
        /// 获取GameObject是否在场景中
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static bool IsInScene(this GameObject go)
        {
            return null != go.scene.name;
        }
    }
}