using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using YRFramework.Runtime.Collections;
using YRFramework.Runtime.Core.Entity;
using YRFramework.Runtime.Core.Scene;
using YRFramework.Runtime.Core.System;
using YRFramework.Runtime.Manager;

namespace YRFramework.Runtime.Entity
{
    /// <summary>
    /// 实体管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRGamePlay/EntityManager")]
    public sealed class EntityManager : YRFrameworkManager, IInit,IUpdate
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.Entity;
        #endregion

        /// <summary>
        /// 不会包含ECSEntity的实体集合
        /// </summary>
        private Dictionary<Type, HashSet<IEntity>> dicTypeWithEntity = new();
        /// <summary>
        /// 场景实体
        /// </summary>
        private Dictionary<E_SceneType, IScene> dicSceneEntity = new();
        /// <summary>
        /// 实体类型系统字典
        /// </summary>
        private DDictionary<IEntity, Type, ISystem> ddEntityTypeSystem = new();
        private UpdateSystems updateSystems = new();

        async UniTask IInit.OnInit()
        {
            //throw new NotImplementedException();
            await UniTask.CompletedTask;
        }

        void IUpdate.OnUpdate(float deltaTime, float realtimeSinceStartup)
        {
            updateSystems.DicUpdateSystems[Core.E_UpdateType.Update].SystemUpdate(deltaTime, realtimeSinceStartup);
        }

        void IInit.OnRelease()
        {
            foreach (ISystem iSystem in ddEntityTypeSystem)
            {
                if (iSystem is IEntity)
                    continue;

                //FrameworkGameEnter.ReferencePool.Release(iSystem);
                //ReferencePool.Release(iSystem);
            }
        }

        #region API
        /// <summary>
        /// 添加实体，所有实体都要添加
        /// </summary>
        /// <param name="iEntity"></param>
        public void AddEntity(IEntity iEntity)
        {
            Type type = iEntity.GetType();
            if (!dicTypeWithEntity.TryGetValue(type, out HashSet<IEntity> setEntity))
            {
                setEntity = new();
                dicTypeWithEntity[type] = setEntity;
            }

            if (setEntity.Contains(iEntity))
                throw new Exception($"[{nameof(EntityManager)}]:添加实体失败，已经存在，实体ID：{iEntity.ID}");

            // EventData.Instance.AddEventEntity(entity); // TODO
            setEntity.Add(iEntity);
        }

        /// <summary>
        /// 尝试获取对应类型的所有实体
        /// </summary>
        /// <param name="type"></param>
        /// <param name="setEntity"></param>
        /// <returns></returns>
        public bool TeyGetEntitys<T>(out HashSet<IEntity> setEntity) where T : class, IEntity, new()
        {
            Type type = typeof(T);
            if (TryGetEntitys(type, out setEntity))
                return true;

            return false;
        }

        /// <summary>
        /// 尝试获取对应类型的所有实体
        /// </summary>
        /// <param name="type"></param>
        /// <param name="setEntity"></param>
        /// <returns></returns>
        public bool TryGetEntitys(Type type, out HashSet<IEntity> setEntity)
        {
            if (dicTypeWithEntity.TryGetValue(type, out setEntity))
                return true;

            setEntity = null;
            return false;
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        public void RemoveEntity(IEntity iEntity)
        {
            if (!dicTypeWithEntity.TryGetValue(iEntity.GetType(), out HashSet<IEntity> setEntity) || !setEntity.Contains(iEntity))
                throw new Exception($"[{nameof(EntityManager)}]:不存在实体，id:{iEntity.ID}");

            // RemoveAllSystem(entity); // TODO
            // EventData.Instance.RemoveEventEntity(entity); //TODO
            setEntity.Remove(iEntity);
        }

        /// <summary>
        /// 获取指定类型的实体数量
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public int GetEntityCount(Type entityType)
        {
            if (!dicTypeWithEntity.TryGetValue(entityType, out HashSet<IEntity> setEntity))
                return 0;

            return setEntity.Count;
        }

        /// <summary>
        /// 加入Scene
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sceneType"></param>
        /// <returns></returns>
        public IScene AddSceneEntity<T>(E_SceneType sceneType) where T : class, IEntity, IScene, new()
        {
            if (dicSceneEntity.TryGetValue(sceneType, out IScene iScene))
            {
                Type type = typeof(T);
                if (iScene.GetType() == type)
                    throw new Exception($"[{nameof(EntityManager)}]:场景存在实体：{type.Name}");

                // RemoveSceneEntity(sceneType);
            }

            // TODO
            //T scene
            return null;
        }

        /// <summary>
        /// 自身又是实体又是Update系统
        /// </summary>
        /// <param name="iEntity"></param>
        public void AddUpdateSystem(IEntity iEntity)
        {
            if (iEntity is not ISystem iSystem)
                return;

            void Add(Type type)
            {
                ddEntityTypeSystem.Add(iEntity, type, iSystem);
                updateSystems.AddUpdateSystem(iEntity, iSystem);
            }

            switch (iSystem)
            {
                case IUpdateSystem:
                    {
                        Add(typeof(IUpdateSystem));
                    }
                    break;
                case ILateUpdateSystem:
                    {
                        Add(typeof(ILateUpdateSystem));
                    }
                    break;
                case IFixedUpdateSystem:
                    {
                        Add(typeof(IFixedUpdateSystem));
                    }
                    break;
                default:
                    {
                        Debug.LogError($"[{nameof(EntityManager)}]:未处理类型：{iSystem}");
                    }
                    break;
            }
        }

        /// <summary>
        /// 运行PreShowSystem
        /// </summary>
        /// <param name="iSystem"></param>
        /// <param name="isFirstShow"></param>
        public void RunPreShowSystem(ISystem iSystem, bool isFirstShow)
        {
            iSystem?.SystemPreShow(isFirstShow);
        }

        /// <summary>
        /// 运行ShowSystem
        /// </summary>
        /// <param name="entity"></param>
        public void RunShowSystem(Core.Entity.Entity entity)
        {
            if (entity is not ISystem iSystem)
                return;

            iSystem?.SystemShow();
            // TODO
            if (!updateSystems.InUpdateMap(entity))
            {
                if (ddEntityTypeSystem.TryGetValue(entity, typeof(IUpdateSystem), out ISystem iUpdateSystem))
                    updateSystems.AddUpdateSystem(entity, iUpdateSystem);

                if (ddEntityTypeSystem.TryGetValue(entity, typeof(ILateUpdateSystem), out ISystem iLateUpdateSystem))
                    updateSystems.AddUpdateSystem(entity, iLateUpdateSystem);

                if (ddEntityTypeSystem.TryGetValue(entity, typeof(IFixedUpdateSystem), out ISystem iFixedUpdateSystem))
                    updateSystems.AddUpdateSystem(entity, iFixedUpdateSystem);
            }

            foreach (IEntity subEntity in entity.SetChildren)
            {
                RunShowSystem((Core.Entity.Entity)subEntity);
            }

            foreach (IEntity component in entity.DicComponent.Values)
            {
                RunShowSystem((Core.Entity.Entity)component);
            }
        }

        /// <summary>
        /// 运行HideSystem
        /// </summary>
        /// <param name="entity"></param>
        public void RunHideSystem(Core.Entity.Entity entity)
        {
            if (entity is not ISystem iSystem)
                return;

            iSystem?.SystemHide();
            // TODO 
            updateSystems.RemoveUpdateSystem(entity);
            foreach (IEntity subEntity in entity.SetChildren)
            {
                RunHideSystem((Core.Entity.Entity)subEntity);
            }

            foreach (IEntity component in entity.DicComponent.Values)
            {
                RunHideSystem((Core.Entity.Entity)component);
            }
        }
        #endregion
    }
}