using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI焦点管理（键盘和手柄等操作时只有焦点界面会响应）
    /// </summary>
    public sealed partial class UIManager
    {
        /// <summary>
        /// 当前焦点UI
        /// </summary>
        public UIEntity CentreUI
        {
            get;
            private set;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 焦点UIPrefab
        /// </summary>
        [ShowInInspector]
        [LabelText("焦点UI")]
        private GameObject GoCentre
        {
            get
            {
                if (null == CentreUI)
                    return null;

                return CentreUI.GoRoot;
            }
        }
#endif

        /// <summary>
        /// 焦点初始化
        /// </summary>
        private void CentreInit()
        {
            CentreUI = null;
        }

        /// <summary>
        /// 焦点释放
        /// </summary>
        private void CentreRelease()
        {
            CentreUI = null;
        }

        /// <summary>
        /// 判断UI是否是当前焦点UI
        /// </summary>
        /// <param name="uiName"></param>
        /// <returns></returns>
        public bool IsCentreUI(string uiName)
        {
            if (null == CentreUI)
                return false;

            return CentreUI.Info.Type == uiName;
        }

        /// <summary>
        /// 设置焦点界面
        /// </summary>
        /// <param name="uiEntity"></param>
        internal void SetCentreUI(UIEntity uiEntity)
        {
            CentreUI = uiEntity;
        }

        /// <summary>
        /// 刷新焦点UI
        /// </summary>
        internal void RefreshCentreUI()
        {
            CentreUI = null;

            Array array = Enum.GetValues(typeof(E_UIGroupType));
            for (int i = array.Length - 1; i >= 0; --i)
            {
                if (!dicUIStack.TryGetValue((E_UIGroupType)array.GetValue(i), out LinkedList<UIEntity> linkShowUI))
                    continue;

                LinkedListNode<UIEntity> uiEntityNode = linkShowUI.Last;
                while (null != uiEntityNode)
                {
                    if (!dicAllUIInfo.TryGetValue(uiEntityNode.Value.Info.Type, out UIInfo uiInfo) || !uiInfo.IsCentre) // 不是焦点UI
                    {
                        uiEntityNode = uiEntityNode.Previous;
                        continue;
                    }

                    CentreUI = uiEntityNode.Value;
                    return;
                }
            }
        }
    }
}