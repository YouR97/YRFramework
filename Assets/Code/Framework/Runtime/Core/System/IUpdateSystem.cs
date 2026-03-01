namespace YRFramework.Runtime.Core.System
{
    public interface IUpdateSystem : ISystem
    {
        void OnUpdate(float deltaTime, float realtimeSinceStartup);
    }
}