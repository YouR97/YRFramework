using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using YRFramework.Runtime.Manager;

namespace YRFramework.Runtime.Timer
{
    /// <summary>
    /// 计时器管理器-可以取消的等待任务
    /// </summary>
    public sealed partial class TimerManager : YRFrameworkManager
    {
        /// <summary>
        /// 等待中的任务池
        /// </summary>
        private Dictionary<int, CancellationTokenSource> dicWaitTask;

        private void InitUniTask()
        {
            dicWaitTask = new Dictionary<int, CancellationTokenSource>();
        }

        private void ReleaseUniTask()
        {
            if (null != dicWaitTask)
            {
                foreach (var i in dicWaitTask.Values)
                {
                    i.Cancel();
                    i.Dispose();
                }
            }
        }

        /// <summary>
        /// 等待时间任务
        /// </summary>
        /// <param name="waitTime"></param>
        /// <param name="callback"></param>
        /// <param name="cancellationTokenSource"></param>
        /// <param name="id"></param>
        private async void WaitTimeTask(float waitTime, Action callback, CancellationTokenSource cancellationTokenSource, int id)
        {
            try
            {
                await UniTask.WaitForSeconds(waitTime, cancellationToken: cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"[{nameof(TimerManager)}]取消等待任务，任务id:{id}");
                return;
            }
            finally
            {
                CancelWaitTask(id);
            }

            callback?.Invoke();
        }

        #region API
        /// <summary>
        /// 添加可以取消的异步等待任务(和CancelWaitTaskd对称调用)
        /// </summary>
        /// <param name="waitTime"></param>
        /// <param name="callback"></param>
        public int AddWaitTimeTask(float waitTime, Action callback)
        {
            CancellationTokenSource cancellationTokenSource = new();
            int waitTaskId = FrameworkGameEnter.ID.GetID(E_FrameworkManagerType.Timer);
            dicWaitTask.Add(waitTaskId, cancellationTokenSource);
            WaitTimeTask(waitTime, callback, cancellationTokenSource, waitTaskId);

            return waitTaskId;
        }

        /// <summary>
        /// 取消等待任务
        /// </summary>
        public void CancelWaitTask(int id)
        {
            if (!dicWaitTask.TryGetValue(id, out CancellationTokenSource cancellationTokenSource))
                return;

            dicWaitTask.Remove(id);
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
        #endregion
    }
}