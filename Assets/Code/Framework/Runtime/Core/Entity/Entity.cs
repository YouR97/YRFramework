using System;
using System.Collections.Generic;
using YRFramework.Runtime.Core.System;

namespace YRFramework.Runtime.Core.Entity
{
    /// <summary>
    /// 实体类
    /// </summary>
    public abstract class Entity : IEntity, IVersion
    {
        #region 属性
        /// <summary>
        /// 父节点
        /// </summary>
        public IEntity Parent { get; private set; }

        /// <summary>
        /// 实体名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 实体状态
        /// </summary>
        public E_EntityState State { get; private set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsRuning { get { return E_EntityState.Running == State; } }

        /// <summary>
        /// 连续唯一id
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public int Verison { get; private set; }
        #endregion

        /// <summary>
        /// 组件字典
        /// </summary>
        public Dictionary<Type, IEntity> DicComponent { get; private set; } = new();

        /// <summary>
        /// 子节点集合
        /// </summary>
        public HashSet<IEntity> SetChildren { get; private set; } = new();

        /// <summary>
        /// 序列id
        /// </summary>
        private int serialId;

        #region API
        /// <summary>
        /// 脏标记
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="id"></param>
        public void OnDirty(IEntity parent, int id)
        {
            State = E_EntityState.Running;
            Parent = parent;
            serialId = 0;
            ID = id;
            ++Verison;
        }

        #region 添加组件
        /// <summary>
        /// 添加组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddComponent<T>() where T : class, IEntity
        {
            return (T)AddComponent(typeof(T));
        }

        /// <summary>
        /// 添加组件带一个参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddComponent<T, P1>(P1 p1) where T : class, IEntity
        {
            return (T)AddComponent(typeof(T), p1);
        }

        /// <summary>
        /// 添加组件带一个参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddComponent<T, P1, P2>(P1 p1, P2 p2) where T : class, IEntity
        {
            return (T)AddComponent(typeof(T), p1, p2);
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEntity AddComponent(Type type)
        {
            IEntity iEntity = CreateComponent(type);
            if (iEntity is IInitSystem iInitSystem)
                iInitSystem.OnInit();

            FrameworkGameEnter.Entity.AddUpdateSystem(iEntity);

            return iEntity;
        }

        /// <summary>
        /// 添加组件带一个参数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEntity AddComponent<P1>(Type type,P1 p1)
        {
            IEntity iEntity = CreateComponent(type);
            if (iEntity is IInitSystem<P1> iInitSystem)
                iInitSystem.OnInit(p1);

            FrameworkGameEnter.Entity.AddUpdateSystem(iEntity);

            return iEntity;
        }

        /// <summary>
        /// 添加组件带两个参数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEntity AddComponent<P1, P2>(Type type, P1 p1, P2 p2)
        {
            IEntity iEntity = CreateComponent(type);
            if (iEntity is IInitSystem<P1, P2> iInitSystem)
                iInitSystem.OnInit(p1, p2);

            FrameworkGameEnter.Entity.AddUpdateSystem(iEntity);

            return iEntity;
        }

        /// <summary>
        /// 是否存在组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool IsHasComponent<T>() where T : class, IEntity 
        {
            if (!TryGetComponent(out T component))
                return false;

            if (null == component)
                return false;

            return true;
        }

        /// <summary>
        /// 尝试获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetComponent<T>(out T component) where T : class, IEntity
        {
            if (!TryGetComponent(typeof(T), out IEntity iEntity))
            {
                component = null;
                return false;
            }

            component = (T)iEntity;
            return true;
        }

        /// <summary>
        /// 尝试获取组件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool TryGetComponent(Type type, out IEntity component)
        {
            if (null == DicComponent || !DicComponent.TryGetValue(type, out component))
            {
                component = null;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 删除组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public bool RemoveComponent<T>() where T : class, IEntity
        {
            return Remove<T>();
        }

        /// <summary>
        /// 移除组件
        /// </summary>
        /// <param name="type"></param>
        public bool RemoveComponent(Type type)
        {
            return Remove(type);
        }
        #endregion

        #region 实体
        /// <summary>
        /// 挂载实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddChid<T>()where T :class,IEntity
        {
            return (T)AddChild(typeof(T));
        }

        /// <summary>
        /// 挂载实体带一个参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddChid<T, P1>(P1 p1) where T : class, IEntity
        {
            return (T)AddChild(typeof(T), p1);
        }

        /// <summary>
        /// 挂载实体带两个参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddChid<T, P1, P2>(P1 p1, P2 p2) where T : class, IEntity
        {
            return (T)AddChild(typeof(T), p1, p2);
        }

        /// <summary>
        /// 挂载实体
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEntity AddChild(Type type)
        {
            IEntity iEntity = Create(type, false);
            if (iEntity is IInitSystem iInitSystem)
                iInitSystem.OnInit();

            FrameworkGameEnter.Entity.AddUpdateSystem(iEntity);

            return iEntity;
        }

        /// <summary>
        /// 挂载实体带一个参数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEntity AddChild<P1>(Type type, P1 p1)
        {
            IEntity iEntity = Create(type, false);
            if (iEntity is IInitSystem<P1> iInitSystem)
                iInitSystem.OnInit(p1);

            FrameworkGameEnter.Entity.AddUpdateSystem(iEntity);

            return iEntity;
        }

        /// <summary>
        /// 挂载实体带两个参数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEntity AddChild<P1, P2>(Type type, P1 p1, P2 p2)
        {
            IEntity iEntity = Create(type, false);
            if (iEntity is IInitSystem<P1, P2> iInitSystem)
                iInitSystem.OnInit(p1, p2);

            FrameworkGameEnter.Entity.AddUpdateSystem(iEntity);

            return iEntity;
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="iEntity"></param>
        public void RemoveChild(IEntity iEntity)
        {
            if (SetChildren.Remove(iEntity))
                Remove(iEntity);
        }
        #endregion
        #endregion

        /// <summary>
        /// 根据类型创建实体
        /// </summary>
        /// <typeparam name="T">必须实现IEntity接口</typeparam>
        /// <param name="isComponent">是否是组件</param>
        /// <returns></returns>
        protected virtual IEntity Create<T>(bool isComponent) where T : IEntity
        {
            return Create(typeof(T), isComponent);
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Dispose()
        {
            State = E_EntityState.Clear;
            Parent = null;
            // TODO
            ++Verison;
        }

        #region 私有方法
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="isComponent">是否是组件</param>
        /// <returns></returns>
        private IEntity Create(Type type, bool isComponent)
        {
            IEntity iEntity = (IEntity)FrameworkGameEnter.ReferencePool.Acquire(type);
            iEntity.OnDirty(this, serialId++);
            if (isComponent)
                DicComponent.Add(type, iEntity);
            else
                SetChildren.Add(iEntity);

            FrameworkGameEnter.Entity.AddEntity(iEntity);
            return iEntity;
        }

        /// <summary>
        /// 根据类型创建组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private IEntity CreateComponent<T>() where T : IEntity
        {
            return CreateComponent(typeof(T));
        }

        /// <summary>
        /// 根据类型创建组件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IEntity CreateComponent(Type type)
        {
            if (null != DicComponent && DicComponent.ContainsKey(type))
                throw new Exception($"[{nameof(Entity)}]:实体已具有组件：{type.FullName}");

            IEntity component = Create(type, true);
            return component;
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private bool Remove<T>() where T : IEntity
        {
            return Remove(typeof(T));
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool Remove(Type type)
        {
            if (!DicComponent.TryGetValue(type, out IEntity iEntity))
                return false;

            Remove(iEntity);
            return DicComponent.Remove(type);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="iEntity"></param>
        private void Remove(IEntity iEntity)
        {
            FrameworkGameEnter.Entity.RemoveEntity(iEntity);
            FrameworkGameEnter.ReferencePool.Release(iEntity);
        }
        #endregion
    }
}