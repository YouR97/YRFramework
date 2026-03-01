using System;
using UnityEngine;

namespace YRFramework.Runtime
{
    /// <summary>
    /// 节点
    /// </summary>
    public class GameObjectNode : IDisposable
    {
        protected readonly GameObject goRoot;

        /// <summary>
        /// 是否释放
        /// </summary>
        private bool isDisoposed = false;

        #region 属性
        public GameObject GoRoot
        {
            get { return goRoot; }
        }

        public Transform TsRoot
        {
            get { return goRoot.transform; }
        }

        /// <summary>
        /// 是否显示(缩放控制)
        /// </summary>
        public bool IsScaleShow
        {
            get { return goRoot.transform; }
            set { goRoot.IsScaleShow(value); }
        }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive
        {
            get { return goRoot.activeSelf; }
            set { goRoot.SetActive(value); }
        }
        #endregion

        public GameObjectNode(GameObject root, bool isShow = false)
        {
            goRoot = root;

            IsScaleShow = isShow;
        }

        ~GameObjectNode()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this); // 通知GC，这个对象已经完全被清理
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisoposed)
                return;

            if (isDisoposed) // 释放托管资源
            {
                if (null != goRoot)
                    UnityEngine.Object.Destroy(goRoot);
            }

            // 释放非托管资源

            isDisoposed = true;
        }
    }
}