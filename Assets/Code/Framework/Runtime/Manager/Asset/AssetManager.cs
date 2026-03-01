using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;
using YRFramework.Runtime.Manager;
using Object = UnityEngine.Object;

namespace YRFramework.Runtime.Asset
{
    /// <summary>
    /// 资源管理器
    /// 注意，此类型中的API都要成对使用，每一次load(如preload,load,loadAsync)都要对应一次unload！
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRFramework/AssetManager")]
    public sealed class AssetManager : YRFrameworkManager, IInit
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.Asset;
        #endregion

        /// <summary>
        /// 资源加载接口
        /// </summary>
        private IResLoader iResLoader;

        async UniTask IInit.OnInit()
        {
#if UNITY_EDITOR
            /*if (!Application.isPlaying) // 编辑器模式未运行时用Editor加载
            {
                iResLoader = new ResLoader_Editor();
                await iResLoader.Init();
                FrameworkGameEnter.Event.Broadcast(E_EventType.AssetsLoadComplete);

                return;
            }*/
#endif
            iResLoader = new ResLoader_YooAsset();
            await iResLoader.Init();
            FrameworkGameEnter.Event.Broadcast(E_EventType.AssetsLoadComplete);
        }

        void IInit.OnRelease()
        {
            iResLoader?.OnDestroy();
        }

        /// <summary>
        /// 异步预加载，不需要资源的时候一定要调用<see cref="Unload"/>,
        /// 获取预加载的资源请调用<see cref="GetPreLoadResult{T}"/>,不能使用<see cref="LoadAsset{T}"/>或者<see cref="LoadAssetAsync{T}"/>,会产生多次引用计数
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        public async UniTask PostPreload<T>(string path) where T : Object
        {
            await iResLoader.PostPreload<T>(path);
        }

        /// <summary>
        /// 获取预加载的结果
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetPreLoadResult<T>(string path) where T : Object
        {
            return iResLoader.GetPreLoadResult<T>(path);
        }

        /// <summary>
        /// 等待所有预加载结束
        /// </summary>
        public async UniTask WaitAllPreloads()
        {
            await iResLoader.WaitAllPreload();
        }

        /// <summary>
        /// 同步加载资源，不需要资源的时候请调用<see cref="Unload"/>
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LoadAsset<T>(string path) where T : Object
        {
            return iResLoader.LoadAsyn<T>(path);
        }

        /// <summary>
        /// 异步加载资源，不需要资源的时候请调用<see cref="Unload"/>
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="onLoaded">加载完成后的回调</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        public async UniTask<T> LoadAssetAsync<T>(string path, Action<T> onLoaded = null) where T : Object
        {
            return await iResLoader.LoadAsync(path, onLoaded);
        }

        /// <summary>
        /// 异步等待加载场景
        /// </summary>
        /// <param name="path"></param>
        /// <param name="loadSceneMode"></param>
        /// <returns></returns>
        public async UniTask<SceneHandle> LoadSceneAsync(string path, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            return await iResLoader.LoadSceneAsync(path, loadSceneMode);
        }
        
        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="path"></param>
        /// <param name="loadSceneMode"></param>
        /// <returns></returns>
        public SceneHandle LoadScene(string path, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            return iResLoader.LoadSceneAsyn(path, loadSceneMode);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="path"></param>
        public void Unload(string path)
        {
            iResLoader.Unload(path);
        }
    }
}