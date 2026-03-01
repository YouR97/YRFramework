using Cysharp.Threading.Tasks;
using GamePlay.Runtime.Loading;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.Manager;

namespace GamePlay.Runtime.Fight
{
    /// <summary>
    /// 战斗管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRGamePlay/FightManager")]
    public sealed partial class FightManager : YRFrameworkManager, IInit, IUpdate
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.Fight;
        #endregion

        /// <summary>
        /// 最大生命值
        /// </summary>
        public int MaxSelfHp { get; private set; } = 2;
        /// <summary>
        /// 自己生命值
        /// </summary>
        public int SelfHp { get; private set; } = 2;

        /// <summary>
        /// 是否在战斗中
        /// </summary>
        public bool IsFighting { get; private set; }

        public async UniTask OnInit()
        {
            InitPoint();
            
            IsFighting = false;

            await UniTask.CompletedTask;
        }

        public void OnUpdate(float deltaTime, float realtimeSinceStartup)
        {
            if (!IsFighting)
                return;
        }

        public void OnRelease()
        {
            IsFighting = false;
        }

        /// <summary>
        /// 进入战斗
        /// </summary>
        /// <returns></returns>
        public async UniTask EnterFight()
        {
            await Game.Loading.StartLoading(new FightLoading());
        }
    }
}