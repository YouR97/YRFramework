using System;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;
using Object = UnityEngine.Object;

namespace YRFramework.Runtime
{
    /// <summary>
    /// 编辑器模式下的
    /// </summary>
    public class ResLoader_Editor : IResLoader
    {
        public UniTask Init()
        {
            Debug.Log("编辑器未运行的资源模式");
            return UniTask.CompletedTask;
        }

        public void OnDestroy()
        {
            
        }

        public bool IsCanLocate<T>(string path) where T : Object
        {
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath<T>(path) != null;
#else
            return true;
#endif
        }

        public T LoadAsyn<T>(string path) where T : Object
        {
#if UNITY_EDITOR
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);

            return asset;
#else
            return default;
#endif
        }

        public UniTask<T> LoadAsync<T>(string path, Action<T> onLoaded) where T : Object
        {
#if UNITY_EDITOR
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            onLoaded?.Invoke(asset);

            return UniTask.FromResult(asset);
#else
            return UniTask.FromResult(default(T));
#endif
        }

        public void Unload(string path)
        {
        }

        public UniTask PostPreload<T>(string path) where T : Object
        {
            return UniTask.CompletedTask;
        }

        public T GetPreLoadResult<T>(string path) where T : Object
        {
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath<T>(path);
#else
            return null;
#endif
        }

        public UniTask WaitAllPreload()
        {
            return UniTask.CompletedTask;
        }

        public SceneHandle LoadSceneAsyn(string path, LoadSceneMode loadSceneMode)
        {
            throw new NotImplementedException();
        }

        public UniTask<SceneHandle> LoadSceneAsync(string path, LoadSceneMode loadSceneMode)
        {
            throw new NotImplementedException();
        }
    }
}