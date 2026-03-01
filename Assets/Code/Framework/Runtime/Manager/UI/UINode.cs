using System;
using System.Collections.Generic;

namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI节点
    /// </summary>
    public sealed class UINode : IDisposable
    {
        /// <summary>
        /// 创建UI空节点
        /// </summary>
        /// <param name="uiType"></param>
        /// <returns></returns>
        public static UINode CreateEmptyNode(IUIFactory iUIFactory)
        {
            UINode uiNode = FrameworkGameEnter.ReferencePool.Acquire<UINode>();
            uiNode.UiEntity = null;
            uiNode.UIFactory = iUIFactory;

            return uiNode;
        }

        /// <summary>
        /// 创建UI节点
        /// </summary>
        /// <param name="iUIFactory"></param>
        /// <returns></returns>
        public static UINode CreateNode(IUIFactory iUIFactory)
        {
            UINode uiNode = FrameworkGameEnter.ReferencePool.Acquire<UINode>();
            uiNode.UiEntity = null;
            uiNode.UIFactory = iUIFactory;

            return uiNode;
        }

        public static void DestoryNode(UINode uiNode)
        {
            // TODO
        }

        /// <summary>
        /// UI名
        /// </summary>
        public string Name;
        /// <summary>
        /// UI工厂
        /// </summary>
        public IUIFactory UIFactory;
        /// <summary>
        /// 当有新的UI显示，当前窗口保持什么状态
        /// </summary>
        public E_WindowState NextActionState;
        /// <summary>
        /// 当前窗口状态
        /// </summary>
        public E_UIState UIState;

        /// <summary>
        /// 父UI节点
        /// </summary>
        public UINode Parent;
        /// <summary>
        /// 子节点
        /// </summary>
        public List<UINode> listChild;
        /// <summary>
        /// 加载的UI实体
        /// </summary>
        public UIEntity UiEntity;

        /// <summary>
        /// 显示前执行
        /// </summary>
        /// <param name="isFirstOpen"></param>
        public void PreShow(bool isFirstOpen)
        {
            if (null == UiEntity)
                return;

            FrameworkGameEnter.Entity.RunPreShowSystem(UiEntity, isFirstOpen);
            foreach (UINode uiEntity in listChild)
            {
                uiEntity.PreShow(isFirstOpen);
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            if (null == UiEntity)
                return;

            FrameworkGameEnter.Entity.RunShowSystem(UiEntity);
            foreach (UINode uiEntity in listChild)
            {
                uiEntity.Show();
            }
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            if (null == UiEntity)
                return;

            FrameworkGameEnter.Entity.RunHideSystem(UiEntity);
            foreach (UINode uiEntity in listChild)
            {
                uiEntity.Hide();
            }
        }

        public void Dispose()
        {
        }
    }
}