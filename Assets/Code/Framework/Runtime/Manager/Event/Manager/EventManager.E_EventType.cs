namespace YRFramework.Runtime.Event
{
    /// <summary>
    /// 继承自泛型EventManager，使用E_EventType枚举作为事件标识
    /// </summary>
    public sealed class EventManager : EventManager<E_EventType>
    {
        // 此类现在继承自YRFramework.Runtime.Event
        // 所有核心功能都在基类中实现
        // 这里可以添加项目特定的扩展方法
    }
}