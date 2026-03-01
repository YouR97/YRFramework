using UnityEngine;
using YRFramework.Runtime.Core.Entity;

namespace YRFramework.Runtime.Core.Scene
{
    /// <summary>
    /// 场景工厂
    /// </summary>
    public static class SceneFactory
    {
        public static T ChangeScene<T>() where T : class, IEntity, IScene, new()
        {
            //IScene iScene = FrameworkGameEnter.Entity(); // TODO
            return default;// (T)iScene;
        }

    }
}