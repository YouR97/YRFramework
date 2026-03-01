using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using YRFramework.Runtime.Manager;

namespace YRFramework.Runtime.ID
{
    /// <summary>
    /// ID管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRFramework/IDManager")]
    public sealed class IDManager : YRFrameworkManager, IInit
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.ID;
        #endregion

        /// <summary>
        /// 最大id
        /// </summary>
        private const int MAX_ID = int.MaxValue;
        /// <summary>
        /// id字典，记录各系统的自增id
        /// </summary>
        [BoxGroup("ID管理器"), LabelText("ID字典"), ShowInInspector, PropertyOrder(0), ReadOnly]
        private Dictionary<E_FrameworkManagerType, int> dicId;

        public async UniTask OnInit()
        {
            dicId = new Dictionary<E_FrameworkManagerType, int>();

            await UniTask.CompletedTask;
        }

        public void OnRelease()
        {
            if (null != dicId)
            {
                dicId.Clear();
                dicId = null;
            }
        }

        #region API
        /// <summary>
        /// 生成id
        /// </summary>
        /// <returns></returns>
        public int GetID(E_FrameworkManagerType managerType)
        {
            dicId.TryGetValue(managerType, out int curId);

            int id = ++curId;

            if (id >= MAX_ID)
                id = 1;

            dicId[managerType] = id;
            
            return id;
        }
        #endregion
    }
}