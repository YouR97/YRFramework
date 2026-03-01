using System;
using YRFramework.Runtime.Core.Entity;
using YRFramework.Runtime.Core.System;

namespace YRFramework.Runtime.Core.World
{
    /// <summary>
    /// 世界实体基类
    /// </summary>
    public abstract partial class WorldEntity : IDisposable, IEntity, IVersion, IInitSystem<int>, IUpdateSystem
    {
        /// <summary>
        /// ECS实例id
        /// </summary>
        private int ecsSerialId;

        #region 属性
        /// <summary>
        /// 父类
        /// </summary>
        public IEntity Parent { get; private set; }

        /// <summary>
        /// 实例ID
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 实体名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 实体状态
        /// </summary>
        public E_EntityState State { get; private set; }

        /// <summary>
        /// 是否运行中
        /// </summary>
        public bool IsRuning { get { return E_EntityState.Running == State; } }

        /// <summary>
        /// 版本号
        /// </summary>
        public int Verison { get; private set; }

        /// <summary>
        /// 最大组件数量
        /// </summary>
        public int MaxComponentCount { get; private set; }

        /// <summary>
        /// 当前帧时间间隔
        /// </summary>
        public float DeltaTime { get; private set; }

        /// <summary>
        /// 时间倍率
        /// </summary>
        public float TimeMultiple { get; private set; }
        #endregion

        public virtual void OnInit(int maxComponentCount)
        {
            MaxComponentCount = maxComponentCount;
        }

        public void OnUpdate(float deltaTime, float realtimeSinceStartup)
        {
            DeltaTime = deltaTime * TimeMultiple;
        }

        public void OnDirty(IEntity parent, int id)
        {
            State = E_EntityState.Running;
            Parent = parent;
            ecsSerialId = 0;
            ID = id;
            ++Verison;
        }

        /// <summary>
        /// 设置时间倍率
        /// </summary>
        /// <param name="timeMultiple"></param>
        protected virtual void SetTimeMultiple(float timeMultiple)
        {
            TimeMultiple = timeMultiple;
        }

        public virtual void Dispose()
        {
            ++Verison;
            ecsSerialId = 0;
        }
    }
}