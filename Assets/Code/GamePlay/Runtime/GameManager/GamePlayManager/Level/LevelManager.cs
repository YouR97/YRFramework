using UnityEngine;
using YRFramework.Runtime.Manager;

namespace GamePlay.Runtime.Level
{
    /// <summary>
    /// 关卡管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRGamePlay/LevelManager")]
    public sealed class LevelManager : YRFrameworkManager
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.Level;
        #endregion

        #region 属性
        /// <summary>
        /// 是否在关卡中
        /// </summary>
        public bool IsInLevel
        {
            get;
            private set;
        }
        #endregion
    }
}