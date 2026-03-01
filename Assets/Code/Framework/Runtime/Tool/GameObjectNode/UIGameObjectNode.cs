using UnityEngine;

namespace YRFramework.Runtime
{
    /// <summary>
    /// UI节点
    /// </summary>
    public class UIGameObjectNode : GameObjectNode
    {
        /// <summary>
        /// 引用
        /// </summary>
        protected ReferenceCollector rc { get; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="root"></param>
        public UIGameObjectNode(GameObject root, bool isShow = false) : base(root, isShow)
        {
            rc = root.GetComponent<ReferenceCollector>();
        }
    }
}