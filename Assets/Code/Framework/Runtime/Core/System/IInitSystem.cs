using Cysharp.Threading.Tasks;

namespace YRFramework.Runtime.Core.System
{
    /// <summary>
    /// 系统初始化接口
    /// </summary>
    public interface IInitSystem : ISystem
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void OnInit();
    }

    /// <summary>
    /// 单参系统初始化接口
    /// </summary>
    public interface IInitSystem<in T1> : ISystem
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void OnInit(T1 world);
    }

    public interface IInitSystem<in T1, in T2> : ISystem
    {
        /// <summary>
        /// 双参初始化
        /// </summary>
        void OnInit(T1 p1, T2 p2);
    }
}