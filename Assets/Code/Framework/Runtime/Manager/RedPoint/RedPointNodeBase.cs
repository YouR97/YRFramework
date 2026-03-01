using UnityEngine;

namespace YRFramework.Runtime.RedPoint
{
    /// <summary>
    /// 红点基类
    /// </summary>
    public abstract class RedPointNodeBase
    {
        /// <summary>
        /// 触发事件
        /// </summary>
        public E_EventType[] EventTypes { get; private set; }
    }
}