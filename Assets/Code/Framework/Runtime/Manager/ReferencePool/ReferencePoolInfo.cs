using Sirenix.OdinInspector;
using System;
using System.Runtime.InteropServices;

namespace YRFramework.Runtime.ReferencePool
{
    /// <summary>
    /// 引用池信息
    /// </summary>
    [StructLayout(LayoutKind.Auto)] // 内存对齐
    public readonly struct ReferencePoolInfo
    {
        /// <summary>
        /// 类型
        /// </summary>
        [LabelText("引用类型"), ShowInInspector]
        private readonly Type type;
        /// <summary>
        /// 未使用的引用数量
        /// </summary>
        [LabelText("未使用的引用数量"), ShowInInspector]
        private readonly int unusedReferenceCount;
        /// <summary>
        /// 正在使用的引用数量
        /// </summary>
        [LabelText("正在使用的的引用数量"), ShowInInspector]
        private readonly int usingReferenceCount;
        /// <summary>
        /// 获取引用数量
        /// </summary>
        [LabelText("已经获取的的引用数量"), ShowInInspector]
        private readonly int acquireReferenceCount;
        /// <summary>
        /// 释放的引用数量
        /// </summary>
        [LabelText("已经释放的的引用数量"), ShowInInspector]
        private readonly int releaseReferenceCount;
        /// <summary>
        /// 添加的引用数量
        /// </summary>
        [LabelText("已经添加的的引用数量"), ShowInInspector]
        private readonly int addReferenceCount;
        /// <summary>
        /// 移除的引用数量
        /// </summary>
        [LabelText("已经移除的引用数量"), ShowInInspector]
        private readonly int removeReferenceCount;

        /// <summary>
        /// 初始化引用池信息的新实例。
        /// </summary>
        /// <param name="type">引用池类型。</param>
        /// <param name="unusedReferenceCount">未使用引用数量。</param>
        /// <param name="usingReferenceCount">正在使用引用数量。</param>
        /// <param name="acquireReferenceCount">获取引用数量。</param>
        /// <param name="releaseReferenceCount">归还引用数量。</param>
        /// <param name="addReferenceCount">增加引用数量。</param>
        /// <param name="removeReferenceCount">移除引用数量。</param>
        public ReferencePoolInfo(Type type, int unusedReferenceCount, int usingReferenceCount, int acquireReferenceCount, int releaseReferenceCount, int addReferenceCount, int removeReferenceCount)
        {
            this.type = type;
            this.unusedReferenceCount = unusedReferenceCount;
            this.usingReferenceCount = usingReferenceCount;
            this.acquireReferenceCount = acquireReferenceCount;
            this.releaseReferenceCount = releaseReferenceCount;
            this.addReferenceCount = addReferenceCount;
            this.removeReferenceCount = removeReferenceCount;
        }
    
        /// <summary>
        /// 获取应用类型
        /// </summary>
        public readonly Type Type { get { return type; } }

        /// <summary>
        /// 获取未使用的引用数量
        /// </summary>
        public readonly int UnusedReferenceCount { get {  return unusedReferenceCount; } }

        /// <summary>
        /// 获取正在使用的引用数量
        /// </summary>
        public readonly int UsingReferenceCount { get { return usingReferenceCount; } }

        /// <summary>
        /// 获取引用数量
        /// </summary>
        public readonly int AcquireReferenceCount { get { return acquireReferenceCount; } }

        /// <summary>
        /// 获取释放的引用数量
        /// </summary>
        public readonly int ReleaseReferenceCount { get { return releaseReferenceCount; } }

        /// <summary>
        /// 添加的引用数量
        /// </summary>
        public readonly int AddReferenceCount { get { return addReferenceCount; } }

        /// <summary>
        /// 移除的引用数量
        /// </summary>
        public readonly int RemoveReferenceCount { get { return removeReferenceCount; } }
    }
}