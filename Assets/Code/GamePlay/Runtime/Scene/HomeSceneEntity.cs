using UnityEngine;
using YRFramework.Runtime.Core.Scene;

namespace GamePlay.Runtime.Scene
{
    /// <summary>
    /// 主页场景
    /// </summary>
    public sealed class HomeScene : SceneEntity
    {
        /// <summary>
        /// 场景名
        /// </summary>
        protected override string SceneName => Consts.Scene.HOME_SCENE;

        protected override void OnReady()
        {
            base.OnReady();

            //AddComponent<>
        }
    }
}