using System;

namespace YRFramework.Runtime.Core.System
{
    /// <summary>
    /// 系统交换接口
    /// </summary>
    public interface ISystemCarryover : IDisposable
    {
        public object Carryover { get; set; }
    }
}