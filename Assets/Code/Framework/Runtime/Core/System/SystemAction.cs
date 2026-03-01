#if UNITY_EDITOR
using YRFramework.Runtime.Collections;
using YRFramework.Runtime.Profiling;
#endif

namespace YRFramework.Runtime.Core.System
{
    public static class SystemAction
    {
        public static bool SystemInitialize(this ISystem system)
        {
            if (system is IInitSystem initSystem)
            {
                initSystem.OnInit();
                return true;
            }

            return false;
        }

        public static bool SystemInitialize<P1>(this ISystem system, P1 p1)
        {
            if (system is IInitSystem<P1> initSystem)
            {
                initSystem.OnInit(p1);
                return true;
            }

            return false;
        }

        public static bool SystemInitialize<P1, P2>(this ISystem system, P1 p1, P2 p2)
        {
            if (system is IInitSystem<P1, P2> initSystem)
            {
                initSystem.OnInit(p1, p2);
                return true;
            }

            return false;
        }

        public static bool SystemPreShow(this ISystem system, bool p1)
        {
            if (system is IPreShowSystem preShowSystem)
            {
                preShowSystem.OnPreShow(p1);
                return true;
            }

            return false;
        }

        public static bool SystemShow(this ISystem system)
        {
            if (system is IShowSystem showSystem)
            {
                showSystem.OnShow();
                return true;
            }

            return false;
        }

        public static bool SystemHide(this ISystem system)
        {
            if (system is IHideSystem hideSystem)
            {
                hideSystem.OnHide();
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取系统更新类型
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public static E_UpdateType GetUpdateSystemType(this ISystem system)
        {
            if (system is IUpdateSystem)
            {
                return E_UpdateType.Update;
            }
            else if (system is ILateUpdateSystem)
            {
                return E_UpdateType.LateUpdate;
            }
            else if (system is IFixedUpdateSystem)
            {
                return E_UpdateType.FixedUpdate;
            }

            return E_UpdateType.None;
        }

        public static void SystemUpdate(this StrongList<ISystem> systems, float deltaTime, float realtimeSinceStartup)
        {
            foreach (ISystem system in systems)
            {
#if UNITY_EDITOR
                using (new YRProfiler(system.GetType().Name))
#endif
                {
                    ((IUpdateSystem)system).OnUpdate(deltaTime, realtimeSinceStartup);
                }
            }
        }

        public static void SystemLateUpdate(this StrongList<ISystem> systems, float deltaTime, float realtimeSinceStartup)
        {
            foreach (var system in systems)
            {
#if UNITY_EDITOR
                using (new YRProfiler(system.GetType().Name))
#endif
                {
                    ((ILateUpdateSystem)system).OnLateUpdate(deltaTime, realtimeSinceStartup);
                }
            }
        }

        public static void SystemFixedUpdate(this StrongList<ISystem> systems, float deltaTime, float realtimeSinceStartup)
        {
            foreach (var system in systems)
            {
#if UNITY_EDITOR
                using (new YRProfiler(system.GetType().Name))
#endif
                {
                    ((IFixedUpdateSystem)system).OnFixedUpdate(deltaTime, realtimeSinceStartup);
                }
            }
        }
    }
}