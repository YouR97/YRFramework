using System.Collections.Generic;
using YRFramework.Runtime.Collections;
using YRFramework.Runtime.Core.Entity;

namespace YRFramework.Runtime.Core.System
{
    /// <summary>
    /// 更新系统
    /// </summary>
    public sealed class UpdateSystems
    {
        private Dictionary<E_UpdateType, StrongList<ISystem>> dicUpdateSystems;
        private Dictionary<IEntity, StrongList<ISystem>> dicEntityUpdateMap;

        public Dictionary<E_UpdateType, StrongList<ISystem>> DicUpdateSystems { get { return dicUpdateSystems; } }

        public UpdateSystems()
        {
            dicEntityUpdateMap = new();
            dicUpdateSystems = new Dictionary<E_UpdateType, StrongList<ISystem>>()
            {
                {E_UpdateType.Update, new StrongList<ISystem>(256, true) },
                {E_UpdateType.LateUpdate, new StrongList<ISystem>(256, true) },
                {E_UpdateType.FixedUpdate, new StrongList<ISystem>(256, true) },
            };
        }

        #region API
        /// <summary>
        /// 添加更新系统
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="system"></param>
        public void AddUpdateSystem(IEntity iEntity, ISystem iSystem)
        {
            E_UpdateType updateType = iSystem.GetUpdateSystemType();
            if (E_UpdateType.None == updateType || InUpdateMap(iEntity, iSystem))
                return;

            if (!dicEntityUpdateMap.TryGetValue(iEntity, out StrongList<ISystem> strongListSystem))
            {
                strongListSystem = new StrongList<ISystem>();
                dicEntityUpdateMap.Add(iEntity, strongListSystem);
            }

            if (!strongListSystem.Contains(iSystem))
            {
                strongListSystem.Add(iSystem);
                dicUpdateSystems[updateType].Add(iSystem);
            }
        }

        /// <summary>
        /// 是否拥有update系统,如果system为空那就是这个实体上有没有存在至少一个update系统如果不为空则为是否存在指定update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="system"></param>
        /// <returns></returns>
        public bool InUpdateMap(IEntity iEntity, ISystem iSystem = null)
        {
            if (null == iSystem)
            {
                if (dicEntityUpdateMap.ContainsKey(iEntity))
                    return true;
            }
            else
            {
                if (dicEntityUpdateMap.TryGetValue(iEntity, out StrongList<ISystem> strongListSystem))
                {
                    if (strongListSystem.Contains(iSystem))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 移除更新系统
        /// </summary>
        /// <param name="iEntity"></param>
        /// <param name="iSystem"></param>
        public void RemoveUpdateSystem(IEntity iEntity, ISystem iSystem = null)
        {
            if (!dicEntityUpdateMap.TryGetValue(iEntity, out StrongList<ISystem> strongListSystem))
                return;

            if (null == iSystem)
            {
                foreach (ISystem system in strongListSystem)
                {
                    E_UpdateType updateType = system.GetUpdateSystemType();
                    dicUpdateSystems[updateType].Remove(system);
                    strongListSystem.Remove(system);
                }
            }
            else
            {
                E_UpdateType updateType = iSystem.GetUpdateSystemType();
                if (E_UpdateType.None == updateType)
                    return;

                strongListSystem.Remove(iSystem);
                dicUpdateSystems[updateType].Remove(iSystem);
            }
        }
        #endregion
    }
}