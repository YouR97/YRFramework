using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using YRFramework.Runtime.Utility;
using YRFramework.Runtime.Manager;

namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRFramework/UIManager")]
    public sealed partial class UIManager : YRFrameworkManager, IInit, IUpdate
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.UI;
        #endregion

        #region 私有变量
        /// <summary>
        /// 缓存的所有已创建的UI实体字典。Key:UI名，Value:UI实体
        /// </summary>
        private Dictionary<string, UIEntity> dicAllUI;
        /// <summary>
        /// UI信息字典。Key:UI名，Value:UI信息
        /// </summary>
        private Dictionary<string, UIInfo> dicAllUIInfo;
        /// <summary>
        /// UIRoot
        /// </summary>
        [SerializeField]
        [LabelText("UI根节点")]
        private Transform tsUIRoot;

        /// <summary>
        /// 临时列表，存储要移除的UI类型
        /// </summary>
        private List<string> listRemoveUIType;
        #endregion

        #region 接口方法
        async UniTask IInit.OnInit()
        {
            HierarchyInit();
            CentreInit();
            AutoDestoryInit();

            dicAllUI = new Dictionary<string, UIEntity>();
            dicAllUIInfo = new Dictionary<string, UIInfo>();
            listRemoveUIType = new List<string>();

            #region 校验UI根节点
            if (null == tsUIRoot)
            {
                Debug.LogError($"[{nameof(UIManager)}]UI根节点为空");
                return;
            }
            #endregion

            #region 获取所有UI工厂
            List<Type> listTypes = YRUtility.Assembly.GetTypes();
            foreach (Type type in listTypes)
            {
                UIFactoryAttribute uiFactoryAttribute = type.GetCustomAttribute<UIFactoryAttribute>(false);
                if (null == uiFactoryAttribute)
                    continue;

                if (dicAllUIInfo.ContainsKey(uiFactoryAttribute.UIName))
                {
                    Debug.LogError($"[{nameof(UIManager)}]已添加同类的UIInfo:{uiFactoryAttribute.UIName}");
                    continue;
                }

                if (Activator.CreateInstance(type) is not IUIFactory uiFactory)
                {
                    Debug.LogError($"[{nameof(UIManager)}]{uiFactoryAttribute.GetType().FullName}没有实现{nameof(IUIFactory)}");
                    continue;
                }

                dicAllUIInfo[uiFactoryAttribute.UIName] = new UIInfo(uiFactoryAttribute.UIName,
                    uiFactoryAttribute.UIGroupType,
                    uiFactoryAttribute.WindowType,
                    uiFactoryAttribute.IsCentre,
                    uiFactory);
            }
            #endregion

            await UniTask.CompletedTask;
        }

        void IUpdate.OnUpdate(float deltaTime, float realtimeSinceStartup)
        {
            CheckAutoDestoryByTime(realtimeSinceStartup); // 自动销毁检查
        }

        void IInit.OnRelease()
        {
            if (null != dicAllUIInfo)
            {
                dicAllUIInfo.Clear();
                dicAllUIInfo = null;
            }

            if (null != dicAllUI)
            {
                dicAllUI.Clear();
                dicAllUI = null;
            }

            AutoDestoryRelease();
            CentreRelease();
            HierarchyRelease();
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 创建UI
        /// </summary>
        public async UniTask<UIEntity> Create(string type)
        {
            if (!dicAllUIInfo.TryGetValue(type, out UIInfo uiInfo))
                throw new($"{LOG_PREFIX}没有{type}类型的{nameof(UIInfo)}");

            if (!TryGetUIByTyoe(type, out UIEntity uiEntity)) // 没有实例则创建
            {
                if (dicAllUI.Count >= maxUICount) // 超过最大数量，进行自动销毁
                    AutoDestoryByCount();

                GameObject goTemplate = await FrameworkGameEnter.Asset.LoadAssetAsync<GameObject>(type);
                FrameworkGameEnter.Asset.Unload(type);
                if (null == goTemplate)
                    return null;

                GameObject goUI = Instantiate(goTemplate, tsCacheUIRoot);
                goUI.name = type;

                uiEntity = uiInfo.UIFactory.Create(goUI, uiInfo);
                dicAllUI[uiInfo.Type] = uiEntity;
                try
                {
                    uiEntity.iUIController.Awake(uiEntity.GoRoot.GetOrAddComponent<ReferenceCollector>());
                }
                catch (Exception e)
                {
                    Debug.LogError($"[{nameof(UIManager)}]:创建UI失败，{e}");
                    dicAllUI.Remove(type);
                    uiEntity.Destroy();
                    FrameworkGameEnter.ReferencePool.Release(uiEntity);
                }
            }

            uiEntity.UINextState = E_UIState.Show;

            return uiEntity;
        }

        #region 关闭UI
        /// <summary>
        /// 关闭UI
        /// </summary>
        public void CloseUI(string type, bool isRefreshCentre = true)
        {
            if (!dicAllUI.TryGetValue(type, out UIEntity uiEntity))
            {
                Debug.LogWarning($"{LOG_PREFIX}{type}没有创建，不能关闭");
                return;
            }

            uiEntity.UINextState = E_UIState.Close;

            if (isRefreshCentre)
                RefreshCentreUI();
        }

        /// <summary>
        /// 关闭所有UI
        /// </summary>
        public void CloseAllUI(HashSet<string> setIgnore = null)
        {
            foreach (UIEntity uiEntity in dicAllUI.Values)
            {
                if (null != setIgnore && setIgnore.Contains(uiEntity.Info.Type)) // 忽略UI不处理
                    continue;

                CloseUI(uiEntity.Info.Type, false);
            }

            RefreshCentreUI(); // 刷新焦点
        }
        #endregion

        #region 释放UI
        /// <summary>
        /// 销毁UI
        /// </summary>
        /// <param name="type"></param>
        /// /// <param name="isRefreshCentre">是否刷新焦点</param>
        public void DestroyUI(string type, bool isRefreshCentre = true)
        {
            if (!dicAllUI.TryGetValue(type, out UIEntity uiEntity))
            {
                Debug.LogWarning($"{LOG_PREFIX}{type}没有创建,不能移除");
                return;
            }

            CloseUI(type, isRefreshCentre);

            dicAllUI.Remove(type);
            uiEntity.Destroy();
            FrameworkGameEnter.ReferencePool.Release(uiEntity);
        }

        /// <summary>
        /// 释放所有UI
        /// </summary>
        public void DestroyAllUI(HashSet<string> setIgnore = null)
        {
            listRemoveUIType.Clear();// RemoveUI里会调用字典的Remove,不能直接遍历字典

            foreach (UIEntity uiEntity in dicAllUI.Values)
            {
                if (null != setIgnore && setIgnore.Contains(uiEntity.Info.Type))
                    continue;

                listRemoveUIType.Add(uiEntity.Info.Type);
            }

            foreach (string type in listRemoveUIType)
            {
                DestroyUI(type);
            }
        }
        #endregion
        
        /// <summary>
        /// 尝试获取UI根据UI名
        /// </summary>
        /// <param name="type"></param>
        /// <param name="uiEntity"></param>
        /// <returns></returns>
        public bool TryGetUIByTyoe(string type, out UIEntity uiEntity)
        {
            return dicAllUI.TryGetValue(type, out uiEntity);
        }

        /// <summary>
        /// 获取UI组最上层的显示UI
        /// </summary>
        /// <param name="uiGroupType"></param>
        /// <param name="uiEntity"></param>
        /// <returns></returns>
        public bool TryGetShowTopUI(E_UIGroupType uiGroupType, out UIEntity uiEntity)
        {
            uiEntity = null;

            if (!dicUIStack.TryGetValue(uiGroupType, out LinkedList<UIEntity> linkShow))
                return false;

            if (null == linkShow)
                return false;

            if (null == linkShow.Last)
                return false;

            if (E_UIState.Show != linkShow.Last.Value.CurState)
                return false;

            uiEntity = linkShow.Last.Value;

            return true;
        }
        #endregion
    }
}