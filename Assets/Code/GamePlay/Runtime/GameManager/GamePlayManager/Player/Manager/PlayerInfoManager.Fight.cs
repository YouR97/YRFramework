using YRFramework.Runtime.Manager;

namespace GamePlay.Runtime.Player
{
    /// <summary>
    /// 玩家信息管理器-战斗
    /// </summary>
    public sealed partial class PlayerInfoManager : YRFrameworkManager
    {
        public PlayerFightInfo FightInfo { get; private set; }

        private void InitFightInfo()
        {
            FightInfo = new();
        }

        private void ReleaseFightInfo()
        {
            
        }
    }
}