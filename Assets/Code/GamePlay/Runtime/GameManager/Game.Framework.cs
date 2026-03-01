using YRFramework.Runtime;
using YRFramework.Runtime.Base;
using YRFramework.Runtime.Asset;
using YRFramework.Runtime.Event;
using YRFramework.Runtime.Storage;
using YRFramework.Runtime.Localization;
using YRFramework.Runtime.FSM;
using YRFramework.Runtime.ReferencePool;
using YRFramework.Runtime.ObjectPool;
using YRFramework.Runtime.DataTable;
using YRFramework.Runtime.Audio;
using YRFramework.Runtime.UI;
using YRFramework.Runtime.ID;
using YRFramework.Runtime.Timer;

namespace GamePlay.Runtime
{
    /// <summary>
    /// 框架组件入口
    /// </summary>
    public sealed partial class Game
    {
        #region 管理器
        /// <summary>
        /// 框架基础管理器
        /// </summary>
        public static BaseManager Base { get { return FrameworkGameEnter.Base; } }

        /// <summary>
        /// 资源加载管理器
        /// </summary>
        public static AssetManager Asset { get { return FrameworkGameEnter.Asset; } }

        /// <summary>
        /// ID管理器
        /// </summary>
        public static IDManager ID { get { return FrameworkGameEnter.ID; } }

        /// <summary>
        /// 计时器管理器
        /// </summary>
        public static TimerManager Timer { get { return FrameworkGameEnter.Timer; } }

        /// <summary>
        /// 本地缓存管理器
        /// </summary>
        public static StorageManager Storage { get { return FrameworkGameEnter.Storage; } }

        /// <summary>
        /// 本地化管理器
        /// </summary>
        public static LocalizationManager Localization { get { return FrameworkGameEnter.Localization; } }

        /// <summary>
        /// 事件管理器
        /// </summary>
        public static EventManager Event { get { return FrameworkGameEnter.Event; } }

        /// <summary>
        /// 引用池管理器
        /// </summary>
        public static ReferencePoolManager ReferencePool { get { return FrameworkGameEnter.ReferencePool; } }

        /// <summary>
        /// 对象池管理器
        /// </summary>
        public static ObjectPoolManager ObjectPool { get { return FrameworkGameEnter.ObjectPool; } }

        /// <summary>
        /// 配置表管理器
        /// </summary>
        public static DataTableManager DataTable { get { return FrameworkGameEnter.DataTable; } }

        /// <summary>
        /// UI管理器
        /// </summary>
        public static UIManager UI { get { return FrameworkGameEnter.UI; } }

        /// <summary>
        /// 音频管理器
        /// </summary>
        public static AudioManager Audio { get { return FrameworkGameEnter.Audio; } }
        #endregion
    }
}