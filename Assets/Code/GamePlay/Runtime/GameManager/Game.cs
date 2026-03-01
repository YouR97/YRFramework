using Cysharp.Threading.Tasks;
using GamePlay.Runtime.Procedure;
using Sirenix.OdinInspector;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.Manager;

namespace GamePlay.Runtime
{
    /// <summary>
    /// 所有管理器入口
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class Game : MonoSingleton<Game>
    {
        /// <summary>
        /// 是否初始化
        /// </summary>
        private bool isInit;

        /// <summary>
        /// 是否开启Log
        /// </summary>
        [SerializeField]
        [HideInInspector]
        private bool isEnableLog;

        /// <summary>
        /// 根节点
        /// </summary>
        public RootEntity Root { get; private set; }

        #region 外部控制
        /// <summary>
        /// Debug根节点
        /// </summary>
        public GameObject GoDebugRoot;

        /// <summary>
        /// 是否开启Log
        /// </summary>
        [ShowInInspector]
        [LabelText("是否开启Log")]
        public bool IsEnableLog
        {
            get { return isEnableLog; }
            set
            {
                isEnableLog = value;
                SetLogEnable(isEnableLog);
            }
        }
        #endregion

        private Game() { }

        #region 生命周期
        protected override void Awake()
        {
            base.Awake();

            isInit = false;
#if !DEBUG
            isEnableLog = false;
            
            if (null != GoDebugRoot)
                DestroyImmediate(GoDebugRoot);
#endif

            SetLogEnable(isEnableLog);

            DontDestroyOnLoad(gameObject);
        }

        private async void Start()
        {
            CollectGamePlayManager();
#if DEBUG
            CollectDebugManager();
#endif

            await FrameworkGameEnter.OnInit();

            isInit = true;
            Root = ReferencePool.Acquire<RootEntity>();
            Root.OnDirty(null, 0);
            Root.AddComponent<Fsm_ProcedureController>();
        }

        private void Update()
        {
            if (!isInit)
                return;

            #region 每帧更新
            FrameworkGameEnter.OnUpdate(Time.deltaTime, Time.realtimeSinceStartup);
            #endregion
        }

        private void OnDestroy()
        {
            isInit = false;

            FrameworkGameEnter.Shutdown(E_ShutdownType.Quit);
        }

        private void OnApplicationQuit()
        {
            StopAllCoroutines(); // 停止所有协程
        }
        #endregion

        /// <summary>
        /// 是否开启Log
        /// </summary>
        private void SetLogEnable(bool isEnable)
        {
            Debug.unityLogger.logEnabled = isEnable;
        }

        /// <summary>
        /// 重置管理器名
        /// </summary>
        [Button("重置管理器名")]
        private void OnResetName()
        {
            YRFrameworkManager[] frameworkManagers = gameObject.transform.GetComponentsInChildren<YRFrameworkManager>();
            foreach (YRFrameworkManager manager in frameworkManagers)
            {
                manager.ResetName();
            }
        }
    }
}