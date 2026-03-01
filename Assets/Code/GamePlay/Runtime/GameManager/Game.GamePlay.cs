using System;
using YRFramework.Runtime;
using YRFramework.Runtime.Manager;
using GamePlay.Runtime.Loading;
using GamePlay.Runtime.Fight;
using GamePlay.Runtime.Level;
using GamePlay.Runtime.YRCamera;
using GamePlay.Runtime.Setting;
using GamePlay.Runtime.Player;
using GamePlay.Runtime.Condition;

namespace GamePlay.Runtime
{
    /// <summary>
    /// GamePlay组件入口
    /// </summary>
    public sealed partial class Game
    {
        #region 管理器
        /// <summary>
        /// 设置管理器
        /// </summary>
        public static SettingManager Setting { get; private set; }

        /// <summary>
        /// 加载管理器
        /// </summary>
        public static LoadingManager Loading { get; private set; }

        /// <summary>
        /// 条件管理器
        /// </summary>
        public static ConditionManager Condition { get; private set; }

        /// <summary>
        /// 相机管理器
        /// </summary>
        public static CameraManager Camera { get; private set; }

        /// <summary>
        /// 玩家信息管理器
        /// </summary>
        public static PlayerInfoManager PlayerInfo { get; private set; }

        /// <summary>
        /// 关卡管理器
        /// </summary>
        public static LevelManager Level { get; private set; }

        /// <summary>
        /// 战斗管理器
        /// </summary>
        public static FightManager Fight { get; private set; }
        #endregion

        private static void CollectGamePlayManager()
        {
            #region 校验
            if (!FrameworkGameEnter.TryGetManager(E_FrameworkManagerType.Setting, out SettingManager settingManager))
                throw new Exception($"获取'{nameof(SettingManager)}'组件失败");

            if (!FrameworkGameEnter.TryGetManager(E_FrameworkManagerType.Loading, out LoadingManager loadingManager))
                throw new Exception($"获取'{nameof(LoadingManager)}'组件失败");

            if(!FrameworkGameEnter.TryGetManager(E_FrameworkManagerType.Condition, out ConditionManager conditionManager))
                throw new Exception($"获取'{nameof(ConditionManager)}'组件失败");

            if (!FrameworkGameEnter.TryGetManager(E_FrameworkManagerType.Camera, out CameraManager cameraManager))
                throw new Exception($"获取'{nameof(CameraManager)}'组件失败");

            if (!FrameworkGameEnter.TryGetManager(E_FrameworkManagerType.PlayerInfo, out PlayerInfoManager playerInfoManager))
                throw new Exception($"获取'{nameof(PlayerInfoManager)}'组件失败");

            if (!FrameworkGameEnter.TryGetManager(E_FrameworkManagerType.Level, out LevelManager levelManager))
                throw new Exception($"获取'{nameof(LevelManager)}'组件失败");

            if (!FrameworkGameEnter.TryGetManager(E_FrameworkManagerType.Fight, out FightManager fightManager))
                throw new Exception($"获取'{nameof(FightManager)}'组件失败");
            #endregion

            Setting = settingManager;
            Loading = loadingManager;
            Condition = conditionManager;
            Camera = cameraManager;
            PlayerInfo = playerInfoManager;
            Level = levelManager;
            Fight = fightManager;
        }
    }
}