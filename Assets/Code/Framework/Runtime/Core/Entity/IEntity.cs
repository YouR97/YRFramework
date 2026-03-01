using System;

namespace YRFramework.Runtime.Core.Entity
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity : IDisposable, IContinuousID
    {
        /// <summary>
        /// 父实体节点
        /// </summary>
        public IEntity Parent { get; }

        /// <summary>
        /// 实体名
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 实体状态
        /// </summary>
        public E_EntityState State { get; }

        /// <summary>
        /// 是否运行中
        /// </summary>
        public bool IsRuning { get; }

        /// <summary>
        /// 脏标记
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="id"></param>
        public void OnDirty(IEntity parent, int id);
    }
}