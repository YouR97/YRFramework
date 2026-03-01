using System;
using YRFramework.Runtime;
using YRFramework.Runtime.Manager;

namespace GamePlay.Runtime
{
    /// <summary>
    /// Debug组件入口
    /// </summary>
    public sealed partial class Game
    {
        #region 管理器
        /// <summary>
        /// FPS管理器
        /// </summary>
        public static FPSManager FPS
        {
            get;
            private set;
        }
        #endregion

        private static void CollectDebugManager()
        {
            #region 校验
            if (!FrameworkGameEnter.TryGetManager(E_FrameworkManagerType.FPS,out FPSManager fpsManager))
                throw new Exception($"获取'{nameof(FPSManager)}'组件失败");
            #endregion

            FPS = fpsManager;
        }
    }
}