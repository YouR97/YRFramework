namespace YRFramework.Runtime.Core.System
{
    public interface IPreShow
    {
    }

    public interface IPreShowSystem : ISystem
    {
        void OnPreShow(bool isFirstShow);
    }
}