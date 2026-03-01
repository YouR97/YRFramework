using Cysharp.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;
using YooAsset;
using Object = UnityEngine.Object;

namespace YRFramework.Runtime
{
    public interface IResLoader
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        UniTask Init();

        /// <summary>
        /// 能否定位
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        bool IsCanLocate<T>(string path) where T : Object;

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="onLoaded"></param>
        /// <returns></returns>
        T LoadAsyn<T>(string path) where T : Object;

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="onLoaded"></param>
        /// <returns></returns>
        UniTask<T> LoadAsync<T>(string path, Action<T> onLoaded) where T : Object;

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="path"></param>
        /// <param name="loadSceneMode"></param>
        /// <returns></returns>
        SceneHandle LoadSceneAsyn(string path, LoadSceneMode loadSceneMode);

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="path"></param>
        /// <param name="loadSceneMode"></param>
        /// <returns></returns>
        UniTask<SceneHandle> LoadSceneAsync(string path, LoadSceneMode loadSceneMode);

        /// <summary>
        /// 卸载
        /// </summary>
        /// <param name="path"></param>
        void Unload(string path);

        /// <summary>
        /// 预加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        UniTask PostPreload<T>(string path) where T : Object;

        /// <summary>
        /// 获取预加载的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        T GetPreLoadResult<T>(string path) where T : Object;

        /// <summary>
        /// 等待所有预加载
        /// </summary>
        /// <returns></returns>
        UniTask WaitAllPreload();

        /// <summary>
        /// 卸载资源系统
        /// </summary>
        void OnDestroy();
    }
}