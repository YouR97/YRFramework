using Cysharp.Threading.Tasks;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.Manager;

namespace GamePlay.Runtime.YRCamera
{
    /// <summary>
    /// 相机管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRGamePlay/CameraManager")]
    public sealed class CameraManager : YRFrameworkManager, IInit
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.Camera;
        #endregion

        /// <summary>
        /// 主相机
        /// </summary>
        [SerializeField]
        private Camera mainCamera;
        /// <summary>
        /// UI相机
        /// </summary>
        [SerializeField]
        private Camera uiCamera;

        //private 

        async UniTask IInit.OnInit()
        {
            await UniTask.CompletedTask;
        }

        void IInit.OnRelease()
        {

        }

        public Camera MainCamera
        {
            get { return mainCamera; }
        }

        public Camera UICamera
        {
            get { return uiCamera; }
        }
    }
}