using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using YRFramework.Runtime.Utility;

namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI管理器（层级管理）
    /// </summary>
    public sealed partial class UIManager
    {
        /// <summary>
        /// UI深度系数
        /// </summary>
        private const int RATIO = 1000;

        /// <summary>
        /// 被关闭的UI缓存到该节点下
        /// </summary>
        [SerializeField]
        [LabelText("缓存UI根节点")]
        internal Transform tsCacheUIRoot;
        
        /// <summary>
        /// UI组根节点字典
        /// </summary>
        private Dictionary<E_UIGroupType, GameObject> dicUIGroupRoot;
        /// <summary>
        /// UI栈
        /// </summary>
        private Dictionary<E_UIGroupType, LinkedList<UIEntity>> dicUIStack;

        /// <summary>
        /// 层级管理初始化
        /// </summary>
        private void HierarchyInit()
        {
            dicUIGroupRoot = new Dictionary<E_UIGroupType, GameObject>();
            dicUIStack = new Dictionary<E_UIGroupType, LinkedList<UIEntity>>();

            #region 校验关闭的UI根节点
            if (null == tsCacheUIRoot)
            {
                Debug.LogError($"[{nameof(UIManager)}]被关闭的UI根节点为空");
                YRUtility.Game.ExitGame();
                return;
            }
            #endregion

            #region 创建UI组根节点和添加UI组队列
            foreach (E_UIGroupType uiGroupType in Enum.GetValues(typeof(E_UIGroupType)))
            {
                dicUIStack[uiGroupType] = new LinkedList<UIEntity>();
                if (!TryAddUIGroup(uiGroupType))
                {
                    Debug.LogWarning($"[{nameof(UIManager)}]添加UI组失败:{uiGroupType}");
                    continue;
                }
            }
            #endregion
        }

        /// <summary>
        /// 层级管理释放
        /// </summary>
        private void HierarchyRelease()
        {
            if (null != dicUIGroupRoot)
            {
                dicUIGroupRoot.Clear();
                dicUIGroupRoot = null;
            }
            
            if (null != dicUIStack)
            {
                dicUIStack.Clear();
                dicUIStack = null;
            }
        }

        /// <summary>
        /// 获取UI组的根节点
        /// </summary>
        /// <param name="uiGroupType"></param>
        /// <param name="tsGroupRoot"></param>
        /// <returns></returns>
        internal bool TryGetUIGroupRoot(E_UIGroupType uiGroupType, out Transform tsGroupRoot)
        {
            tsGroupRoot = null;

            if (!dicUIGroupRoot.TryGetValue(uiGroupType, out GameObject goRoot))
                return false;

            if (null == goRoot)
                return false;

            tsGroupRoot = goRoot.transform;

            return true;
        }

        /// <summary>
        /// 加入UI栈
        /// </summary>
        /// <param name="uiEntity"></param>
        internal void AddUIStack(UIEntity uiEntity)
        {
            if (!dicUIStack.TryGetValue(uiEntity.Info.GroupType, out LinkedList<UIEntity> linkUI))
            {
                linkUI = new LinkedList<UIEntity>();
                dicUIStack[uiEntity.Info.GroupType] = linkUI;
            }

            if (E_UIShowType.Full == uiEntity.Info.WindowType) // 隐藏被覆盖的界面
                HideUIByUIGroup(uiEntity.Info.GroupType);

            if (uiEntity.Info.IsCentre)
                SetCentreUI(uiEntity);

            linkUI.AddLast(uiEntity);

            SortUI(uiEntity.Info.GroupType); // 对栈中的UI排序
        }

        /// <summary>
        /// 从UI栈移除UI
        /// </summary>
        /// <param name="uiEntity"></param>
        /// <returns></returns>
        internal bool RemoveUIStack(UIEntity uiEntity)
        {
            if (!dicUIStack.TryGetValue(uiEntity.Info.GroupType, out LinkedList<UIEntity> linkUI))
                return false;

            if (!linkUI.Remove(uiEntity)) // 移除失败
                return false;

            if (E_UIShowType.Full == uiEntity.Info.WindowType) // 重新显示被覆盖隐藏的界面
                ShowCoverUI(uiEntity.Info.GroupType);

            if (uiEntity.Info.IsCentre) // 被移除的界面是焦点界面，重新设置新的焦点界面
                RefreshCentreUI();

            return true;
        }

        #region 私有方法
        /// <summary>
        /// 尝试添加界面组
        /// </summary>
        /// <param name="uiGroupType"></param>
        /// <returns></returns>
        private bool TryAddUIGroup(E_UIGroupType uiGroupType)
        {
            if (dicUIGroupRoot.ContainsKey(uiGroupType))
                return false;

            GameObject goUIGroup = new(uiGroupType.ToString());
            RectTransform rts = goUIGroup.GetOrAddComponent<RectTransform>();
            rts.SetParent(tsUIRoot.transform);
            rts.anchorMin = Vector2.zero;
            rts.anchorMax = Vector2.one;
            rts.offsetMin = Vector2.zero;
            rts.offsetMax = Vector2.one;
            rts.sizeDelta = Vector2.zero;
            rts.localPosition = Vector3.zero;
            rts.localScale = Vector3.one;
            dicUIGroupRoot[uiGroupType] = goUIGroup;

            return true;
        }

        /// <summary>
        /// 获取UI组类型的起始渲染排序值
        /// </summary>
        /// <param name="uiGroupType"></param>
        /// <returns></returns>
        private int GetStartSortOrder(E_UIGroupType uiGroupType)
        {
            return (int)uiGroupType * RATIO;
        }

        /// <summary>
        /// 对显示中的UI排序渲染顺序
        /// </summary>
        private void SortUI(E_UIGroupType uiGroupType)
        {
            if (!dicUIStack.TryGetValue(uiGroupType, out LinkedList<UIEntity> linkShowUI))
                return;

            int sortOrder = GetStartSortOrder(uiGroupType); // 获取起始layer
            foreach (UIEntity uiEntity in linkShowUI)
            {
                uiEntity.SetSortOrder(++sortOrder);
            }
        }

        /// <summary>
        /// 隐藏被覆盖的界面,根据UI组
        /// </summary>
        /// <param name="uiGroupType"></param>
        private void HideUIByUIGroup(E_UIGroupType uiGroupType)
        {
            foreach (var kv in dicUIStack)
            {
                if (kv.Key > uiGroupType)
                    continue;

                foreach (UIEntity uiEntity in kv.Value)
                {
                    uiEntity.UINextState = E_UIState.Hide;
                }
            }
        }

        /// <summary>
        /// 显示被覆盖隐藏的UI
        /// </summary>
        private void ShowCoverUI(E_UIGroupType uiGroupType)
        {
            Array array = Enum.GetValues(typeof(E_UIGroupType));
            for (int i = array.Length - 1; i >= 0; --i)
            {
                E_UIGroupType groupType = (E_UIGroupType)array.GetValue(i);
                if (groupType > uiGroupType)
                    continue;

                if (!dicUIStack.TryGetValue(groupType, out LinkedList<UIEntity> linkShowUI))
                    continue;

                LinkedListNode<UIEntity> uiEntityNode = linkShowUI.Last;
                while (null != uiEntityNode)
                {
                    uiEntityNode.Value.UINextState = E_UIState.Show;
                    if (E_UIShowType.Full == uiEntityNode.Value.Info.WindowType)
                        return;

                    uiEntityNode = uiEntityNode.Previous;
                }
            }
        }
        #endregion
    }
}