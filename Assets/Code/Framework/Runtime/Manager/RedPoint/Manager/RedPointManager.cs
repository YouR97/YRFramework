using Cysharp.Threading.Tasks;
using UnityEngine;
using YRFramework.Runtime.Manager;

namespace YRFramework.Runtime.RedPoint
{
    /// <summary>
    /// 红点管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRFramework/RedPointManager")]
    public sealed partial class RedPointManager : YRFrameworkManager, IInit
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.RedPoint;
        #endregion

        async UniTask IInit.OnInit()
        {
            await UniTask.CompletedTask;
        }

        void IInit.OnRelease()
        {
        }
    }
}