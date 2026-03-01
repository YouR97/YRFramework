using Cysharp.Threading.Tasks;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.Manager;

namespace GamePlay.Runtime.Player
{
    /// <summary>
    /// 玩家信息管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRGamePlay/PlayerInfoManager")]
    public sealed partial class PlayerInfoManager : YRFrameworkManager, IInit
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.PlayerInfo;
        #endregion

        public async UniTask OnInit()
        {
            InitFightInfo();

            await UniTask.CompletedTask;
        }

        public void OnRelease()
        {
            ReleaseFightInfo();
        }
    }
}