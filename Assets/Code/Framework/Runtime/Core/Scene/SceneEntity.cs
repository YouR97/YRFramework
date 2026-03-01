using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using YooAsset;
using YRFramework.Runtime.Core.System;

namespace YRFramework.Runtime.Core.Scene
{
    /// <summary>
    /// 场景实体
    /// </summary>
    public class SceneEntity : Entity.Entity, IInitSystem, IScene
    {
        /// <summary>
        /// 场景名
        /// </summary>
        protected virtual string SceneName { get; set; }

        public virtual void OnInit()
        {
            LoadScene().Forget();
        }

        /// <summary>
        /// 加载主页场景
        /// </summary>
        /// <returns></returns>
        private async UniTask LoadScene()
        {
            if (await LoadScene(SceneName, LoadSceneMode.Single))
                OnReady();
        }

        protected virtual void OnReady()
        {
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="name"></param>
        /// <param name="loadSceneMode"></param>
        /// <returns></returns>
        protected async UniTask<bool> LoadScene(string name, LoadSceneMode loadSceneMode)
        {
            int version = Verison;
            SceneHandle handle = await FrameworkGameEnter.Asset.LoadSceneAsync(name, loadSceneMode);
            FrameworkGameEnter.Asset.Unload(name);
            if (Verison != version)
                return false;
            else if (null != handle)
                return true;

            return false;
        }
    }
}