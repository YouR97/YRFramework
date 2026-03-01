using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using YRFramework.Runtime.Manager;

namespace YRFramework.Runtime.Timer
{
    /// <summary>
    /// 计时器管理器-配置类
    /// </summary>
    public sealed partial class TimerManager : YRFrameworkManager
    {
        [SerializeField]
        [BoxGroup("计时器管理器-编辑"), LabelText("时间轮配置列表"), ShowInInspector, PropertyOrder(0)]
        private List<TimeWheelData> listTimeWheelData = new();
    }
}