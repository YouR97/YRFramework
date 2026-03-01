using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace GamePlay.Runtime.Loading
{
    /// <summary>
    /// 加载基类
    /// </summary>
    public abstract class LoadingBase
    {
        /// <summary>
        /// 继承类的最大进度
        /// </summary>
        private const float INNER_MAX_PROGRESS = 0.9f;

        /// <summary>
        /// 加载完成回调
        /// </summary>
        private readonly Func<UniTask> callbackFunc;
        /// <summary>
        /// 加载进度(0f-1f)
        /// </summary>
        private float progress;
        /// <summary>
        /// 是否运行中
        /// </summary>
        private bool isLoading;
        /// <summary>
        /// 取消异步任务
        /// </summary>
        private CancellationTokenSource cts;

        #region 属性
        /// <summary>
        /// 加载进度(0f-1f)
        /// </summary>
        public float Progress
        {
            get { return progress; }
            protected set { progress = value * INNER_MAX_PROGRESS; }
        }

        /// <summary>
        /// 是否加载中
        /// </summary>
        public bool IsLoading
        {
            get { return isLoading; }
        }
        #endregion

        protected LoadingBase(Func<UniTask> callback)
        {
            callbackFunc = callback;

            progress = 0f;
            isLoading = false;
            cts = new CancellationTokenSource();
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        public async UniTask StartLoad(float completeWaitTime = 0f)
        {
            #region 开始加载之前
            progress = 0f;
            isLoading = true;
            await OnLoadPre();
            GC.Collect(); // 清理GC
            #endregion

            #region 正式加载
            E_LoadingResult result = await OnLoad();
            if (!CheckResult(result))
            {
                isLoading = false;
                return;
            }
            #endregion

            #region 加载完成
            if (null != callbackFunc)
                await callbackFunc.Invoke(); // 加载完回调

            progress = 1f;
            await UniTask.WaitForSeconds(completeWaitTime);
            await OnLoadAfter();
            isLoading = false;
            #endregion
        }

        /// <summary>
        /// 加载之前
        /// </summary>
        /// <returns></returns>
        protected abstract UniTask OnLoadPre();

        /// <summary>
        /// 正式加载
        /// </summary>
        /// <returns></returns>
        protected abstract UniTask<E_LoadingResult> OnLoad();

        /// <summary>
        /// 加载完成之后
        /// </summary>
        /// <returns></returns>
        protected abstract UniTask OnLoadAfter();

        /// <summary>
        /// 检查结果
        /// </summary>
        /// <param name="result"></param>
        private bool CheckResult(E_LoadingResult result)
        {
            switch (result)
            {
                case E_LoadingResult.Success:
                    return true;
                case E_LoadingResult.Error:
                    {
                    }
                    return false;
                default:
                    {
                        Debug.LogError($"未处理类型:{result}");
                        return false;
                    }
            }
        }
    }
}