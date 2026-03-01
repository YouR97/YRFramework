using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using YRFramework.Runtime.Manager;
using YRFramework.Runtime.Utility;

namespace YRFramework.Runtime.Timer
{
    /// <summary>
    /// 计时管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRFramework/TimerManager")]
    public sealed partial class TimerManager : YRFrameworkManager, IInit, IUpdate
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.Timer;
        #endregion

        #region 私有
        /// <summary>
        /// 时间轮列表
        /// </summary>
        private List<TimeWheel> listTimeWheel;
        /// <summary>
        /// 所有计时任务
        /// </summary>
        private Dictionary<int, TimerTask> dicAllTask;
        /// <summary>
        /// 等待每帧需要检查处理的任务id列表,（剩余时间小于minTickInterval时每帧检查）
        /// </summary>
        private List<int> listFrameCheckTaskId;
        /// <summary>
        /// 等待添加的任务id列表
        /// </summary>
        private List<int> listWaitAddTaskId;
        /// <summary>
        /// 最小Tick间隔
        /// </summary>
        private float minTickInterval;
        /// <summary>
        /// 基础计时器(单位：秒)
        /// </summary>
        private float timer;
        /// <summary>
        /// 累计计时(单位：秒)
        /// </summary>
        public float totalTimer;
        #endregion

        /// <summary>
        /// 等待每帧需要检查处理的临时任务id列表
        /// </summary>
        private List<int> listTempCheckExecuteTaskId;

        /// <summary>
        /// 时间轮总数
        /// </summary>
        public int TimeWheelCount
        {
            get { return listTimeWheel.Count; }
        }

        async UniTask IInit.OnInit()
        {
            dicAllTask = new();
            listTimeWheel = new List<TimeWheel>(listTimeWheelData.Count);
            for (int i = 0; i < listTimeWheelData.Count; ++i)
            {
                TimeWheelData wheelData = listTimeWheelData[i];
                listTimeWheel.Add(new TimeWheel(wheelData.TickDuration, wheelData.SlotCount));
            }
            listFrameCheckTaskId = new();
            listWaitAddTaskId = new();
            listTempCheckExecuteTaskId = new();

            if (listTimeWheel.Count <= 0)
                throw new Exception($"[{nameof(TimerManager)}]:未配置计时管理器数据");

            minTickInterval = listTimeWheel[0].TickDuration;
            timer = 0f;
            totalTimer = 0f;

            InitUniTask();

#if !UNITY_EDITOR
            listTimeWheelData.Clear();
            listTimeWheelData = null;
#endif

            await UniTask.CompletedTask;
        }

        void IUpdate.OnUpdate(float deltaTime, float realtimeSinceStartup)
        {
            timer += deltaTime; // 累计时间
            totalTimer += deltaTime;

            foreach (int taskId in listWaitAddTaskId)
            {
                if (!dicAllTask.TryGetValue(taskId, out TimerTask timerTask))
                    continue;

                AddTaskToTimeWheel(timerTask);
            }
            listWaitAddTaskId.Clear();

            if (timer < minTickInterval)
                CheckExecuteTask(); // 检查所有轮次

            while (timer >= minTickInterval)
            {
                timer -= minTickInterval;
                CheckExecuteTask(); // 检查所有轮次
                DoWheel(0);
            }
        }

        void IInit.OnRelease()
        {
            ReleaseUniTask();

            totalTimer = 0f;
            timer = 0f;

            if (null != listWaitAddTaskId)
            {
                listWaitAddTaskId.Clear();
                listWaitAddTaskId = null;
            }

            if (null != listTempCheckExecuteTaskId)
            {
                listTempCheckExecuteTaskId.Clear();
                listTempCheckExecuteTaskId = null;
            }

            if (null != listFrameCheckTaskId)
            {
                listFrameCheckTaskId.Clear();
                listFrameCheckTaskId = null;
            }

            if (null != dicAllTask)
            {
                dicAllTask.Clear();
                dicAllTask = null;
            }

            if (null != listTimeWheel)
            {
                listTimeWheel.Clear();
                listTimeWheel = null;
            }
        }

        #region API
        /// <summary>
        /// 添加循环任务(和RemoveTask对称调用)
        /// </summary>
        /// <param name="interval">任务触发间隔</param>
        /// <param name="callback">任务回调</param>
        /// <param name="isImmediatelyExecute">是否添加时立即执行</param>
        /// <returns></returns>
        public int AddLoopTask(float interval, Action callback, bool isImmediatelyExecute = false)
        {
            TimerTask timerTask = TimerTask.Create(interval, true, callback);
            dicAllTask[timerTask.TaskId] = timerTask;

            timerTask.SetTriggerTime(timerTask.Interval + totalTimer);
            if (timerTask.TiggerTime - totalTimer <= minTickInterval)
                AddTaskToFrameCheck(timerTask); // 小于最小间隔的添加到每帧检查
            else
                AddTaskToTimeWheel(timerTask); // 大于最小间隔的添加到时间轮

            if (isImmediatelyExecute)
                timerTask.Execute();

            return timerTask.TaskId;
        }

        /// <summary>
        /// 添加指定时间触发任务(和RemoveTask对称调用)
        /// </summary>
        /// <returns></returns>
        public int AddTaskByTime(DateTime dateTime, Action callback)
        {
            DateTime curTime = YRUtility.Time.GetCurTime();
            TimeSpan timeSpan = dateTime - curTime;
            if (timeSpan.TotalSeconds <= 0f)
                return YRConsts.INVALID_INT;

            TimerTask timerTask = TimerTask.Create((float)timeSpan.TotalSeconds, false, callback);
            dicAllTask[timerTask.TaskId] = timerTask;

            timerTask.SetTriggerTime(timerTask.Interval + totalTimer);
            if (timerTask.TiggerTime - totalTimer <= minTickInterval)
                AddTaskToFrameCheck(timerTask); // 小于最小间隔的添加到每帧检查
            else
                AddTaskToTimeWheel(timerTask); // 大于最小间隔的添加到时间轮

            return timerTask.TaskId;
        }

        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="taskId"></param>
        public void RemoveTask(int taskId)
        {
            if (!dicAllTask.TryGetValue(taskId, out TimerTask timerTask))
                return;

            dicAllTask.Remove(taskId);
            timerTask.Recycle();
        }

        /// <summary>
        /// 移除多个任务
        /// </summary>
        /// <param name="taskIds"></param>
        public void RemoveTasks(params int[] taskIds)
        {
            foreach (int taskId in taskIds)
            {
                RemoveTask(taskId);
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 前进小轮次
        /// </summary>
        private void DoWheel(int wheelIndex)
        {
            TimeWheel curWheel = listTimeWheel[wheelIndex];
            curWheel.MoveStep();

            CheckTasksByWheel(wheelIndex);

            if (!curWheel.IsCompleteTurn)
                return;

            int nextWheel = wheelIndex + 1;
            if (nextWheel < listTimeWheel.Count) // 转完一圈触发下级轮前进
                DoWheel(nextWheel);
        }

        /// <summary>
        /// 按剩余时间添加任务
        /// </summary>
        /// <param name="timerTask"></param>
        /// <param name="delayMs"></param>
        private void AddTaskToTimeWheel(TimerTask timerTask)
        {
            float interval = timerTask.TiggerTime - totalTimer;

            for (int i = 0; i < listTimeWheel.Count; ++i)
            {
                TimeWheel timeWheel = listTimeWheel[i];
                if (interval < timeWheel.TotalDuration || i >= listTimeWheel.Count - 1) // 小于该轮时间或者达到最大轮
                {
                    int targetSlot = (timeWheel.CurSlotIndex + (int)(interval / timeWheel.TickDuration)) % timeWheel.SlotCount;
                    timeWheel.AddTask(timerTask.TaskId, targetSlot);
                    break;
                }
            }
        }

        /// <summary>
        /// 添加任务到每帧检查
        /// </summary>
        private void AddTaskToFrameCheck(TimerTask timerTask)
        {
            listFrameCheckTaskId.Add(timerTask.TaskId);
        }

        /// <summary>
        /// 处理每个轮的任务
        /// </summary>
        /// <param name="wheelIndex"></param>
        private void CheckTasksByWheel(int wheelIndex)
        {
            TimeWheel wheel = listTimeWheel[wheelIndex];

            List<int> curListTaskId = wheel.CurListTaskId;
            foreach (int taskId in curListTaskId)
            {
                if (!dicAllTask.TryGetValue(taskId, out TimerTask timerTask))
                    continue;

                if (timerTask.TiggerTime - totalTimer <= minTickInterval)
                    AddTaskToFrameCheck(timerTask);
                else
                    AddTaskToTimeWheel(timerTask);
            }

            curListTaskId.Clear();
        }

        /// <summary>
        /// 检查每帧要执行的任务
        /// </summary>
        private void CheckExecuteTask()
        {
            listTempCheckExecuteTaskId.AddRange(listFrameCheckTaskId);
            listFrameCheckTaskId.Clear();

            foreach (int taskId in listTempCheckExecuteTaskId)
            {
                if (!dicAllTask.TryGetValue(taskId, out TimerTask timerTask)) // 无该任务
                    continue;

                if (timerTask.TiggerTime > totalTimer) // 没有到触发时间
                {
                    AddTaskToFrameCheck(timerTask); // 没有触发的添加回列表
                    continue;
                }

                try
                {
                    timerTask.Execute();
                }
                catch (Exception e) 
                {
                    Debug.LogError($"[{nameof(TimerManager)}]异常情况销毁计时任务：{e}");
                    RemoveTask(taskId);
                    continue;
                }

                if (!timerTask.IsLoop) // 不是循环任务，销毁
                {
                    RemoveTask(taskId);
                    continue;
                }

                timerTask.SetTriggerTime(timerTask.TiggerTime + timerTask.Interval);

                if (timerTask.TiggerTime - totalTimer <= minTickInterval)
                    AddTaskToFrameCheck(timerTask);
                else
                    listWaitAddTaskId.Add(timerTask.TaskId);
            }

            listTempCheckExecuteTaskId.Clear();
        }
        #endregion
    }
}