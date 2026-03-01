using Sirenix.OdinInspector;
using System;

namespace YRFramework.Runtime.Timer
{
    /// <summary>
    /// 时间轮配置数据
    /// </summary>
    [Serializable]
    internal struct TimeWheelData
    {
        [LabelText("每个槽位持续时间(秒)")]
        public float TickDuration;
        [LabelText("槽位数量")]
        public int SlotCount;

#if UNITY_EDITOR
        [LabelText("转一圈花费时间"), ShowInInspector]
        private string TotalDuration
        {
            get 
            {
                TimeSpan timeSpan = new(0, 0, 0, (int)(TickDuration * SlotCount));

                string res = string.Empty;
                if (timeSpan.Days > 0)
                    res += $"{timeSpan.Days}天";

                if(timeSpan.Hours > 0)
                    res += $"{timeSpan.Hours}小时";

                if (timeSpan.Minutes > 0)
                    res += $"{timeSpan.Minutes}分钟";

                if (timeSpan.Seconds > 0)
                    res += $"{timeSpan.Seconds}秒";

                if(timeSpan.Milliseconds > 0)
                    res += $"{timeSpan.Milliseconds}毫秒";

                return $"{res}------总毫秒:{timeSpan.TotalMilliseconds}";
            }
        }
#endif
    }
}