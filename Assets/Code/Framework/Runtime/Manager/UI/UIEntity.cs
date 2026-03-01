using UnityEngine;
using UnityEngine.UI;
using YRFramework.Runtime.Core.System;

namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI实体
    /// </summary>
    public class UIEntity : Core.Entity.Entity, IPreShowSystem, IShowSystem, IUpdateSystem, IHideSystem
    {
        /// <summary>
        /// UI名
        /// </summary>
        protected string UIName { get; }

        #region 变量
        /// <summary>
        /// 根画布
        /// </summary>
        private Canvas canvasRoot;
        /// <summary>
        /// 射线检测
        /// </summary>
        private GraphicRaycaster graphicRaycaster;
        #endregion

        /// <summary>
        /// 上次关闭时间
        /// </summary>
        internal float preCloseTime;

        #region 属性
        /// <summary>
        /// 是否开启射线检测
        /// </summary>
        public bool IsRaycaster
        {
            get { return null != graphicRaycaster && graphicRaycaster.enabled; }
            set
            {
                if (null == graphicRaycaster)
                    return;

                graphicRaycaster.enabled = value;
            }
        }

        /// <summary>
        /// 根节点
        /// </summary>
        public GameObject GoRoot { get; private set; }
        
        /// <summary>
        /// UI信息
        /// </summary>
        public UIInfo Info { get; private set; }

        /// <summary>
        /// UI当前状态
        /// </summary>
        public E_UIState CurState { get; set; }

        /// <summary>
        /// UI下一个状态
        /// </summary>
        public E_UIState UINextState { get; internal set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        internal bool IsActive
        {
            get { return null != GoRoot && GoRoot.activeSelf; }
            set { GoRoot?.SetActive(value); }
        }

        /// <summary>
        /// UI控制器
        /// </summary>
        internal UIControllerBase UIController { get; private set; }

        /// <summary>
        /// UI控制器接口
        /// </summary>
        internal IUIController iUIController { get; private set; }
        #endregion

        /// <summary>
        /// 创建UI实体
        /// </summary>
        /// <param name="uiControllerBase"></param>
        /// <param name="goRoot"></param>
        /// <param name="uiInfo"></param>
        /// <returns></returns>
        public static UIEntity Create(UIControllerBase uiControllerBase, GameObject goRoot, UIInfo uiInfo)
        {
            UIEntity uiEntity = FrameworkGameEnter.ReferencePool.Acquire<UIEntity>();
            uiEntity.GoRoot = goRoot;
            uiEntity.canvasRoot = uiEntity.GoRoot.GetOrAddComponent<Canvas>();
            uiEntity.graphicRaycaster = uiEntity.GoRoot.GetOrAddComponent<GraphicRaycaster>();
            uiEntity.UIController = uiControllerBase;
            uiEntity.iUIController = uiControllerBase;
            uiEntity.Info = uiInfo;

            uiEntity.AddComponent<FSM_UIControllerEntity>();

            return uiEntity;
        }

        public UIEntity()
        {
            UIController?.Dispose();
            UIController = null;
            iUIController = null;
            canvasRoot = null;
            graphicRaycaster = null;

            if (null != GoRoot)
                Object.Destroy(GoRoot);
        }

        /// <summary>
        /// 显示之前
        /// </summary>
        /// <param name="isFirstShow"></param>
        void IPreShowSystem.OnPreShow(bool isFirstShow)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        void IShowSystem.OnShow()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="realtimeSinceStartup"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        void IUpdateSystem.OnUpdate(float deltaTime, float realtimeSinceStartup)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        void IHideSystem.OnHide()
        {
            throw new System.NotImplementedException();
        }

        #region API
        /// <summary>
        /// 销毁
        /// </summary>
        public void Destroy()
        {
            RemoveComponent<FSM_UIControllerEntity>();

            FrameworkGameEnter.ReferencePool.Release(this);
        }

        /// <summary>
        /// 设置渲染顺序
        /// </summary>
        /// <param name="sortingOrder"></param>
        public void SetSortOrder(int sortingOrder)
        {
            canvasRoot.sortingOrder = sortingOrder;
        }

        /// <summary>
        /// 尝试获取UI控制器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetUIController<T>(out T t) where T : UIControllerBase
        {
            if (null == UIController)
            {
                t = null;
                Debug.LogError($"{nameof(UIEntity)}获取{nameof(UIControllerBase)}失败");
                return false;
            }

            t = UIController as T;
            if (null == t)
            {
                Debug.LogError($"{nameof(UIEntity)}获取{nameof(t)}失败");
                return false;
            }

            return true;
        }
        #endregion

        public override void Dispose()
        {
            UIController?.Dispose();
            UIController = null;
            iUIController = null;
            canvasRoot = null;
            graphicRaycaster = null;

            if (null != GoRoot)
                Object.Destroy(GoRoot);

            base.Dispose();
        }
    }
}