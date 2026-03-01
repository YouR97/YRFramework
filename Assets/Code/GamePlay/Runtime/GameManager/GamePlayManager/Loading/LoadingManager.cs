using Cysharp.Threading.Tasks;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.Manager;

namespace GamePlay.Runtime.Loading
{
    /// <summary>
    /// 加载管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRFramework/LoadingManager")]
    public sealed class LoadingManager : YRFrameworkManager, IInit
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.Loading;
        #endregion

        /// <summary>
        /// 当前Loading
        /// </summary>
        private LoadingBase curLoading;
        /// <summary>
        /// 加载表现
        /// </summary>
        private LoadingDisplay loadingDisplay;

        #region 属性
        /// <summary>
        /// 是否加载中
        /// </summary>
        public bool IsLoading
        {
            get
            {
                if (null == curLoading)
                    return false;

                return curLoading.IsLoading;
            }
        }

        /// <summary>
        /// 实际进度
        /// </summary>
        public float Progress
        {
            get
            {
                if (null == curLoading)
                    return 0f;

                return curLoading.Progress;
            }
        }

        /// <summary>
        /// 表现进度
        /// </summary>
        public float ShowProgress
        {
            get 
            {
                if (null == loadingDisplay)
                    return Progress;

                loadingDisplay.Update(Progress);

                return loadingDisplay.ShowProgress;
            }
        }
        #endregion

        async UniTask IInit.OnInit()
        {
            loadingDisplay = new LoadingDisplay();

            await UniTask.CompletedTask;
        }

        void IInit.OnRelease()
        {
            curLoading = null;
            loadingDisplay = null;
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        /// <param name="loading">加载类型</param>
        /// <param name="lerpTime">进度变化的插值时间</param>
        /// <param name="completeWaitTime">完成加载的等待时间</param>
        /// <returns></returns>
        public async UniTask StartLoading(LoadingBase loading, float lerpTime = 1f, float completeWaitTime = 0.5f)
        {
            if (null == loading)
            {
                Debug.LogError("开始Loading失败，空");
                return;
            }

            if (null != curLoading && curLoading.IsLoading)
            {
                Debug.LogError("有Loading没有结束");
                return;
            }

            if (lerpTime < 0f)
            {
                Debug.LogError($"加载的进度变化时间不能小于0");
                return;
            }

            if (completeWaitTime < 0f)
            {
                Debug.LogError($"加载完成的等待时间不能小于0");
                return;
            }

            loadingDisplay.SetInfo(lerpTime);
            curLoading = loading;

            await curLoading.StartLoad(lerpTime + completeWaitTime);
        }
    }
}