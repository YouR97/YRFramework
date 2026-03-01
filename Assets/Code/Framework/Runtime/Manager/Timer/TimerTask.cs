using System;
using YRFramework.Runtime.Manager;

namespace YRFramework.Runtime.Timer
{
    /// <summary>
    /// 计时器任务
    /// </summary>
    internal sealed class TimerTask : IDisposable
    {
        private bool isDisposed;

        #region 属性
        /// <summary>
        /// 任务id
        /// </summary>
        public int TaskId { get; private set; }
        /// <summary>
        /// 触发间隔(单位:秒)
        /// </summary>
        public float Interval { get; private set; }
        /// <summary>
        /// 触发时间(单位:秒)
        /// </summary>
        public float TiggerTime { get; private set; }
        /// <summary>
        /// 是否循环任务
        /// </summary>
        public bool IsLoop { get; private set; }
        /// <summary>
        /// 任务回调
        /// </summary>
        public Action CallbackAction { get; private set; }
        #endregion

        internal static TimerTask Create(float interval, bool isLoop, Action callback)
        {
            TimerTask timerTask = FrameworkGameEnter.ReferencePool.Acquire<TimerTask>();

            timerTask.TaskId = FrameworkGameEnter.ID.GetID(E_FrameworkManagerType.ReferencePool);
            timerTask.Interval = interval;
            timerTask.TiggerTime = interval;
            timerTask.IsLoop = isLoop;
            timerTask.CallbackAction = callback;

            return timerTask;
        }

        public TimerTask()
        {
            TaskId = YRConsts.INVALID_INT;
            Interval = long.MaxValue;
            TiggerTime = long.MaxValue;
            CallbackAction = null;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="curTime"></param>
        public void Execute()
        {
            CallbackAction?.Invoke();
        }

        /// <summary>
        /// 设置触发时间
        /// </summary>
        public void SetTriggerTime(float remainingTime)
        {
            TiggerTime = remainingTime;
        }

        /// <summary>
        /// 回收
        /// </summary>
        public void Recycle()
        {
            FrameworkGameEnter.ReferencePool.Release(this);
        }

        #region IDisposable
        private void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

                TaskId = YRConsts.INVALID_INT;
                Interval = long.MaxValue;
                TiggerTime = long.MaxValue;
                CallbackAction = null;

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                isDisposed = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~TimerTask()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}