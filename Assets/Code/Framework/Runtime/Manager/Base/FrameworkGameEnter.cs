using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using YRFramework.Runtime.Utility;
using YRFramework.Runtime.Manager;
using YRFramework.Runtime.Asset;
using YRFramework.Runtime.Audio;
using YRFramework.Runtime.Base;
using YRFramework.Runtime.DataTable;
using YRFramework.Runtime.Event;
using YRFramework.Runtime.Localization;
using YRFramework.Runtime.ObjectPool;
using YRFramework.Runtime.ReferencePool;
using YRFramework.Runtime.UI;
using YRFramework.Runtime.Storage;
using YRFramework.Runtime.ID;
using YRFramework.Runtime.Timer;
using YRFramework.Runtime.Entity;
using YRFramework.Runtime.RedPoint;

namespace YRFramework.Runtime
{
    /// <summary>
    /// 框架入口
    /// </summary>
    public static class FrameworkGameEnter
    {
        /// <summary>
        /// 游戏框架场景编号
        /// </summary>
        internal const int YRFRAMEWORK_SCENE_ID = 0;

        #region 框架管理器
        private static BaseManager baseManager;
        private static EventManager eventManager;
        private static IDManager idManager;
        private static TimerManager timerManager;
        private static AssetManager assetManager;
        private static StorageManager storageManager;
        private static RedPointManager redPointManager;
        private static LocalizationManager localizationManager;
        private static ReferencePoolManager referencePoolManager;
        private static ObjectPoolManager objectPoolManager;
        private static DataTableManager dataTableManager;
        private static EntityManager entityManager;
        private static AudioManager audioManager;
        private static UIManager uiManager;
        #endregion

        #region 框架管理器(属性)
        /// <summary>
        /// 基础管理器
        /// </summary>
        public static BaseManager Base
        {
            get
            {
                if (null == baseManager && !TryGetManager(E_FrameworkManagerType.Base, out baseManager))
                    return null;

                return baseManager;
            }
        }

        /// <summary>
        /// 事件管理器
        /// </summary>
        public static EventManager Event
        {
            get
            {
                if (null == eventManager && !TryGetManager(E_FrameworkManagerType.Event, out eventManager))
                    return null;

                return eventManager;
            }
        }

        /// <summary>
        /// ID管理器
        /// </summary>
        public static IDManager ID
        {
            get
            {
                if (null == idManager && !TryGetManager(E_FrameworkManagerType.ID, out idManager))
                    return null;

                return idManager;
            }
        }

        /// <summary>
        /// 计时器管理器
        /// </summary>
        public static TimerManager Timer
        {
            get
            {
                if (null == timerManager && !TryGetManager(E_FrameworkManagerType.Timer, out timerManager))
                    return null;

                return timerManager;
            }
        }

        /// <summary>
        /// 资源管理器
        /// </summary>
        public static AssetManager Asset
        {
            get
            {
                if (null == assetManager && !TryGetManager(E_FrameworkManagerType.Asset, out assetManager))
                    return null;

                return assetManager;
            }
        }

        /// <summary>
        /// 本地缓存管理器
        /// </summary>
        public static StorageManager Storage
        {
            get
            {
                if (null == storageManager && !TryGetManager(E_FrameworkManagerType.Storage, out storageManager))
                    return null;

                return storageManager;
            }
        }

        /// <summary>
        /// 红点管理器
        /// </summary>
        public static RedPointManager RedPoint
        {
            get
            {
                if (null == redPointManager && !TryGetManager(E_FrameworkManagerType.RedPoint, out redPointManager))
                    return null;

                return redPointManager;
            }
        }

        /// <summary>
        /// 本地化管理器
        /// </summary>
        public static LocalizationManager Localization
        {
            get
            {
                if (null == localizationManager && !TryGetManager(E_FrameworkManagerType.Localization, out localizationManager))
                    return null;

                return localizationManager;
            }
        }

        /// <summary>
        /// 引用池管理器
        /// </summary>
        public static ReferencePoolManager ReferencePool
        {
            get
            {
                if (null == referencePoolManager && !TryGetManager(E_FrameworkManagerType.ReferencePool, out referencePoolManager))
                    return null;

                return referencePoolManager;
            }
        }

        /// <summary>
        /// 对象池管理器
        /// </summary>
        public static ObjectPoolManager ObjectPool
        {
            get
            {
                if (null == objectPoolManager && !TryGetManager(E_FrameworkManagerType.ObjectPool, out objectPoolManager))
                    return null;

                return objectPoolManager;
            }
        }

        /// <summary>
        /// 配置管理器
        /// </summary>
        public static DataTableManager DataTable
        {
            get
            {
                if (null == dataTableManager && !TryGetManager(E_FrameworkManagerType.DataTable, out dataTableManager))
                    return null;

                return dataTableManager;
            }
        }

        /// <summary>
        /// 实体管理器
        /// </summary>
        public static EntityManager Entity
        {
            get 
            {
                if (null == entityManager && !TryGetManager(E_FrameworkManagerType.Entity, out entityManager))
                    return null;

                return entityManager;
            }
        }

        /// <summary>
        /// 音频管理器
        /// </summary>
        public static AudioManager Audio
        {
            get
            {
                if (null == audioManager && !TryGetManager(E_FrameworkManagerType.Audio, out audioManager))
                    return null;

                return audioManager;
            }
        }

        /// <summary>
        /// UI管理器
        /// </summary>
        public static UIManager UI 
        {
            get
            {
                if (null == uiManager && !TryGetManager(E_FrameworkManagerType.UI, out uiManager))
                    return null;

                return uiManager;
            }
        }
        #endregion

        /// <summary>
        /// 管理器列表
        /// </summary>
        private static YRFrameworkManager[] managers;

        /// <summary>
        /// 初始化管理器
        /// </summary>
        public static async UniTask OnInit()
        {
            foreach (YRFrameworkManager manager in managers)
            {
                if (manager is IInit iIint)
                    await iIint.OnInit();
            }
        }

        /// <summary>
        /// 轮询
        /// </summary>
        /// <param name="deltaTime">逻辑时间间隔，单位秒</param>
        /// <param name="realtimeSinceStartup">真实时间间隔，单位秒</param>
        public static void OnUpdate(float deltaTime, float realtimeSinceStartup)
        {
            foreach (YRFrameworkManager manager in managers)
            {
                if (manager is IUpdate iUpdate)
                    iUpdate.OnUpdate(deltaTime, realtimeSinceStartup);
            }
        }

        /// <summary>
        /// 关闭游戏框架。
        /// </summary>
        /// <param name="shutdownType">关闭游戏框架类型。</param>
        public static void Shutdown(E_ShutdownType shutdownType)
        {
            for (int i = managers.Length - 1; i >= 0; --i) // 逆序释放
            {
                YRFrameworkManager manager = managers[i];

                if (null == manager)
                    continue;

                if (manager is IInit iInit)
                    iInit.OnRelease();
            }
            managers = null;

            if (E_ShutdownType.None == shutdownType)
            {
                Debug.Log($"[{nameof(FrameworkGameEnter)}]退出游戏框架 ({shutdownType})...");
                return;
            }

            if (E_ShutdownType.Restart == shutdownType)
            {
                Debug.Log($"[{nameof(FrameworkGameEnter)}]重启游戏和游戏框架 ({shutdownType})...");
                SceneManager.LoadScene(YRFRAMEWORK_SCENE_ID);
                return;
            }

            if (E_ShutdownType.Quit == shutdownType)
            {
                Debug.Log($"[{nameof(FrameworkGameEnter)}]退出游戏和框架 ({shutdownType})...");
                YRUtility.Game.ExitGame();
                return;
            }
        }

        /// <summary>
        /// 获取游戏管理器
        /// </summary>
        /// <param name="type">要获取的游戏框架组件类型。</param>
        /// <param name="manager"></param>
        /// <returns>要获取的游戏框架组件。</returns>
        public static bool TryGetManager<T>(E_FrameworkManagerType managerType, out T manager) where T : YRFrameworkManager
        {
            int index = (int)managerType;
            if (index < 0 || index >= (int)E_FrameworkManagerType.Max)
            {
                manager = null;
                Debug.LogError($"获取{managerType}框架管理器失败");

                return false;
            }

            manager = (T)managers[index];

            return true;
        }

        /// <summary>
        /// 注册游戏管理器
        /// </summary>
        /// <param name="manager">要注册的游戏管理器</param>
        internal static void RegisterManager(YRFrameworkManager manager)
        {
            if (null == manager)
            {
                Debug.LogError($"{nameof(manager)}注册失败，空");
                return;
            }

            if (null == managers)
                managers = new YRFrameworkManager[(int)E_FrameworkManagerType.Max];

            managers[(int)manager.ManagerType] = manager;
        }
    }
}