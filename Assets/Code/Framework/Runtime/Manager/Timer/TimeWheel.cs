using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using YRFramework.Runtime.Utility;

namespace YRFramework.Runtime.Timer
{
    /// <summary>
    /// 时间轮
    /// </summary>
    [Serializable]
    internal sealed class TimeWheel
    {
        /// <summary>
        /// 每槽持续时间
        /// </summary>
        [LabelText("单个Tick时间"), ShowInInspector]
        public float TickDuration { get; private set; }
        /// <summary>
        /// 槽总数
        /// </summary>
        [LabelText("单个Tick时间"), ShowInInspector]
        public int SlotCount { get; private set; }
        /// <summary>
        /// 当前槽位索引
        /// </summary>
        public int CurSlotIndex { get; private set; }
        /// <summary>
        /// 当前任务id列表
        /// </summary>
        public List<int> CurListTaskId
        {
            get { return slotListTaskIds[CurSlotIndex]; }
        }

        /// <summary>
        /// 总轮次计时任务ID字典
        /// </summary>
        private List<int>[] slotListTaskIds;

        /// <summary>
        /// 一圈的总时间
        /// </summary>
        public float TotalDuration
        {
            get { return TickDuration * SlotCount; }
        }

        /// <summary>
        /// 是否完成一圈
        /// </summary>
        public bool IsCompleteTurn
        {
            get { return 0 == CurSlotIndex; }
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="tickDuration">每槽持续时间</param>
        /// <param name="slotCount">槽总数</param>
        public TimeWheel(float tickDuration, int slotCount)
        {
            TickDuration = tickDuration;
            SlotCount = slotCount;

            slotListTaskIds = new List<int>[slotCount];
            for (int i = 0; i < slotCount; ++i)
            {
                slotListTaskIds[i] = new List<int>();
            }
        }

        /// <summary>
        /// 前进一格
        /// </summary>
        /// <returns></returns>
        public void MoveStep()
        {
            CurSlotIndex = (CurSlotIndex + 1) % SlotCount;
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="targetSlot"></param>
        public void AddTask(int taskId, int targetSlot)
        {
            if (YRUtility.Collection.IsIndexOutOfRange(slotListTaskIds, targetSlot))
            {
                Debug.Log($"[{nameof(TimeWheel)}]:添加任务失败，越界：{targetSlot}");
                return;
            }

            List<int> listTaskID = slotListTaskIds[targetSlot];
            listTaskID.Add(taskId);
        }
    }
}