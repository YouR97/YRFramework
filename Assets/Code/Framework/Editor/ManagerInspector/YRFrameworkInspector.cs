using UnityEditor;
using UnityEngine;

namespace YRFramework.Editor
{
    /// <summary>
    /// 游戏框架Inspector抽象类
    /// </summary>
    public abstract class YRFrameworkInspector : UnityEditor.Editor
    {
        /// <summary>
        /// 是否正在编译
        /// </summary>
        private bool isCompiling = false;

        /// <summary>
        /// 绘制事件
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (isCompiling && !EditorApplication.isCompiling)
            {
                isCompiling = false;
                OnCompileComplete();
            }
            else if (!isCompiling && EditorApplication.isCompiling)
            {
                isCompiling = true;
                OnCompileStart();
            }
        }

        /// <summary>
        /// 编译开始事件。
        /// </summary>
        protected virtual void OnCompileStart()
        {
        }

        /// <summary>
        /// 编译完成事件。
        /// </summary>
        protected virtual void OnCompileComplete()
        {
        }

        protected bool IsPrefabInHierarchy(Object obj)
        {
            if (obj == null)
                return false;

            return PrefabUtility.GetPrefabAssetType(obj) != PrefabAssetType.Regular;
        }
    }
}