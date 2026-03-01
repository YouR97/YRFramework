namespace YRFramework.Runtime.Core.System
{
    public interface IFixedUpdateSystem : ISystem
    {
        void OnFixedUpdate(float deltaTime, float realtimeSinceStartup);
    }
}