using Cysharp.Threading.Tasks;
using UnityEngine;
using YRFramework.Runtime.Extension;
using YRFramework.Runtime.Manager;
using YRFramework.Runtime.Utility;

namespace YRFramework.Runtime.Base
{
    /// <summary>
    /// 框架基础管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRFramework/BaseManager")]
    public sealed class BaseManager : YRFrameworkManager, IInit
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.Base;
        #endregion

        /// <summary>
        /// 正常游戏速度
        /// </summary>
        private const float NORMAL_GAME_SPEED = 1f;

        /// <summary>
        /// 默认DPI
        /// </summary>
        private const int DEFAULT_DPI = 96;
        /// <summary>
        /// 暂停前的游戏速度
        /// </summary>
        private float gameSpeedBeforePause;
        /// <summary>
        /// 游戏速度
        /// </summary>
        private float gameSpeed = 1f;
        /// <summary>
        /// 帧率x/s
        /// </summary>
        private int frameRate = 60;
        /// <summary>
        /// 是否绝不休眠
        /// </summary>
        private bool isNeverSleep = true;
        /// <summary>
        /// 是否在后台运行
        /// </summary>
        private bool isRunInBackground = true;

        #region 属性
        /// <summary>
        /// 获取或设置游戏帧率。
        /// </summary>
        public int FrameRate
        {
            get { return frameRate; }
            set { Application.targetFrameRate = frameRate = value; }
        }

        /// <summary>
        /// 获取或设置游戏速度。
        /// </summary>
        public float GameSpeed
        {
            get { return gameSpeed; }
            set { Time.timeScale = gameSpeed = value >= 0f ? value : 0f; }
        }

        /// <summary>
        /// 获取游戏是否暂停
        /// </summary>
        public bool IsGamePaused
        {
            get
            {
                return gameSpeed <= 0f;
            }
        }

        /// <summary>
        /// 获取是否正常游戏速度。
        /// </summary>
        public bool IsNormalGameSpeed { get { return gameSpeed == NORMAL_GAME_SPEED; } }

        /// <summary>
        /// 获取或设置是否禁止休眠。
        /// </summary>
        public bool IsNeverSleep
        {
            get { return isNeverSleep; }
            set
            {
                isNeverSleep = value;
                Screen.sleepTimeout = isNeverSleep ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
            }
        }
        #endregion

        async UniTask IInit.OnInit()
        {
            Debug.Log("Unity版本: ".Append(Application.unityVersion));

            #region 设置屏幕dpi
            YRUtility.Converter.ScreenDpi = Screen.dpi;
            if (YRUtility.Converter.ScreenDpi <= 0f)
                YRUtility.Converter.ScreenDpi = DEFAULT_DPI;
            #endregion

            gameSpeedBeforePause = gameSpeed;
            Time.timeScale = gameSpeed;

            Screen.sleepTimeout = isNeverSleep ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting; // 休眠设置
            Application.runInBackground = isRunInBackground;

            Application.lowMemory += OnLowMemory;

            await UniTask.CompletedTask;
        }

        void IInit.OnRelease()
        {
            Application.lowMemory -= OnLowMemory;
        }

        /// <summary>
        /// 暂停游戏。
        /// </summary>
        public void PauseGame()
        {
            if (IsGamePaused)
                return;

            gameSpeedBeforePause = GameSpeed;
            GameSpeed = 0f;

            FrameworkGameEnter.Event.Broadcast(E_EventType.GamePause);
        }

        /// <summary>
        /// 恢复游戏。
        /// </summary>
        public void ResumeGame()
        {
            if (!IsGamePaused)
                return;

            GameSpeed = gameSpeedBeforePause;

            FrameworkGameEnter.Event.Broadcast(E_EventType.GameResume);
        }

        /// <summary>
        /// 重置为正常游戏速度。
        /// </summary>
        public void ResetNormalGameSpeed()
        {
            if (IsNormalGameSpeed)
                return;

            GameSpeed = NORMAL_GAME_SPEED;
        }

        /// <summary>
        /// 内存清理
        /// </summary>
        private void OnLowMemory()
        {
            Debug.LogWarning("内存不足，清理内存");

            FrameworkGameEnter.ObjectPool.ReleaseAll();
        }
    }
}