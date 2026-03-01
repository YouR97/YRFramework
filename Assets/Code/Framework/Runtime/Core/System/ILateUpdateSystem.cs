namespace YRFramework.Runtime.Core.System
{
    public interface ILateUpdateSystem : ISystem
    {
        void OnLateUpdate(float deltaTime, float realtimeSinceStartup);
    }
}