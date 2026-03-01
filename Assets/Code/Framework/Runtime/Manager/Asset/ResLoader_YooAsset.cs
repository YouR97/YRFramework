using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;
using Object = UnityEngine.Object;

namespace YRFramework.Runtime
{
    public class ResLoader_YooAsset : IResLoader
    {
        /// <summary>
        /// 默认资源包名
        /// </summary>
        private const string DEFAULT_PACKAGE_NAME = "DefaultPackage";

        /// <summary>
        /// 资源句柄
        /// </summary>
        private class YRAssetHandle
        {
            /// <summary>
            /// 加载的资源
            /// </summary>
            private Object goResult;
            /// <summary>
            /// 路径
            /// </summary>
            private string path;
            /// <summary>
            /// 引用计数
            /// </summary>
            private int referenceCount;
            /// <summary>
            /// 资源操作句柄
            /// </summary>
            private AssetHandle handle;

            #region 属性
            /// <summary>
            /// 引用计数
            /// </summary>
            public int ReferenceCount { get { return referenceCount; } }

            /// <summary>
            /// 是否加载完毕
            /// </summary>
            public bool IsDone { get { return handle.IsDone; } }
            #endregion

            /// <summary>
            /// 等待资源加载
            /// </summary>
            /// <returns></returns>
            public async UniTask Wait()
            {
                await handle.Task;
            }

            /// <summary>
            /// 获取资源
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public T GetAsset<T>() where T : Object
            {
                if (!IsDone)
                    handle.WaitForAsyncComplete();

                if (goResult == null)
                    goResult = handle.AssetObject;

                if (goResult == null)
                    Debug.LogError($"加载资源失败!path:{path}");

                return goResult as T;
            }

            /// <summary>
            /// 增加引用计数
            /// </summary>
            public void InCreaseReferenceCount()
            {
                referenceCount++;
            }

            /// <summary>
            /// 减少引用计数
            /// </summary>
            public void DeCreaseReferenceCount()
            {
                referenceCount--;
            }

            /// <summary>
            /// 重置
            /// </summary>
            /// <param name="path"></param>
            /// <param name="op"></param>
            public void Reset(string path, AssetHandle handle)
            {
                this.path = path;
                this.handle = handle;
                goResult = null;
                referenceCount = 1;
            }

            public void Reset()
            {
                path = null;
                goResult = null;
                handle = default;
                referenceCount = 0;
            }

            public void Unload()
            {
                handle.Release();
                handle = null;
            }
        }

        /// <summary>
        /// 资源句柄池
        /// </summary>
        private readonly Stack<YRAssetHandle> stackAssetHandle = new();
        /// <summary>
        /// 资源句柄字典,key:路径，path:资源句柄
        /// </summary>
        private readonly Dictionary<string, YRAssetHandle> dicPath2Asset = new(StringComparer.Ordinal);
        /// <summary>
        /// 预加载路径集合
        /// </summary>
        private readonly HashSet<string> setPreloadingPath = new(StringComparer.Ordinal);

        public async UniTask Init()
        {
            YooAssets.Initialize(); // 初始化资源系统
            ResourcePackage package = YooAssets.CreatePackage(DEFAULT_PACKAGE_NAME); // 创建默认的资源包
            YooAssets.SetDefaultPackage(package); // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。

#if UNITY_EDITOR
            Debug.Log("编辑器资源模式");
            PackageInvokeBuildResult buildResult = EditorSimulateModeHelper.SimulateBuild(DEFAULT_PACKAGE_NAME);
            string packageRoot = buildResult.PackageRootDirectory;
            FileSystemParameters fileSystemParams = FileSystemParameters.CreateDefaultEditorFileSystemParameters(packageRoot);

            EditorSimulateModeParameters createParameters = new()
            {
                EditorFileSystemParameters = fileSystemParams
            };

            InitializationOperation initOperation = package.InitializeAsync(createParameters);
            await initOperation.Task;
            if (EOperationStatus.Succeed != initOperation.Status)
            {
                Debug.LogError($"资源包初始化失败：{initOperation.Error}");
                return;
            }

            RequestPackageVersionOperation requestPackageVersionOperation = package.RequestPackageVersionAsync();
            await requestPackageVersionOperation.Task;
            if (EOperationStatus.Succeed != requestPackageVersionOperation.Status)
            {
                Debug.LogError($"资源包初始化失败：{requestPackageVersionOperation.Error}");
                return;
            }

            UpdatePackageManifestOperation updatePackageManifestOperation = package.UpdatePackageManifestAsync(requestPackageVersionOperation.PackageVersion);
            await updatePackageManifestOperation.Task;
            if (EOperationStatus.Succeed != updatePackageManifestOperation.Status)
            {
                Debug.LogError($"资源包初始化失败：{updatePackageManifestOperation.Error}");
                return;
            }

            Debug.Log("资源包初始化成功");
#else
            Debug.Log("单机资源模式");
            FileSystemParameters fileSystemParams = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();

            OfflinePlayModeParameters createParameters = new()
            {
                BuildinFileSystemParameters = fileSystemParams
            };

            InitializationOperation initOperation = package.InitializeAsync(createParameters);
            await initOperation.Task;

            if (EOperationStatus.Succeed == initOperation.Status)
                Debug.Log("资源包初始化成功！");
            else
                Debug.LogError($"资源包初始化失败：{initOperation.Error}");
#endif
        }

        /// <summary>
        /// 卸载资源系统
        /// </summary>
        public void OnDestroy()
        {
            YooAssets.Destroy();
        }

        public bool IsCanLocate<T>(string path) where T : Object
        {
            AssetInfo assetInfo = YooAssets.GetAssetInfo(path);

            return assetInfo != null && !string.IsNullOrEmpty(assetInfo.AssetPath);
        }

        /// <summary>
        /// 取资源句柄
        /// </summary>
        /// <param name="path"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        private YRAssetHandle GetAssetHandle(string path, AssetHandle handle)
        {
            if (!stackAssetHandle.TryPop(out YRAssetHandle assetHandle))
                assetHandle = new YRAssetHandle();

            assetHandle.Reset(path, handle);

            return assetHandle;
        }

        /// <summary>
        /// 回收资源句柄
        /// </summary>
        /// <param name="assetHandle"></param>
        private void ReturnAssetHandle(YRAssetHandle assetHandle)
        {
            if (assetHandle == null)
                return;

            assetHandle.Unload();
            assetHandle.Reset();
            stackAssetHandle.Push(assetHandle);
        }

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public T LoadAsyn<T>(string path) where T : Object
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Debug.LogError("同步加载资源失败,path为空");
                return null;
            }

            try
            {
                bool isCanLocate = IsCanLocate<Object>(path);
                if (!isCanLocate)
                {
                    Debug.LogError($"同步加载资源失败,资源不存在:{path}");
                    return null;
                }

                if (dicPath2Asset.TryGetValue(path, out YRAssetHandle assetHandle))
                {
                    assetHandle.InCreaseReferenceCount();
                    if (assetHandle.IsDone)
                        return assetHandle.GetAsset<T>();
                }
                else
                {
                    AssetHandle handle = YooAssets.LoadAssetSync<T>(path);
                    assetHandle = GetAssetHandle(path, handle);

                    dicPath2Asset[path] = assetHandle;
                }

                T result = assetHandle.GetAsset<T>();

                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message} \r\n {e.StackTrace}");
            }

            return null;
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="onLoaded"></param>
        /// <returns></returns>
        public async UniTask<T> LoadAsync<T>(string path, Action<T> onLoaded) where T : Object
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Debug.LogError("异步加载资源失败,path为空");
                return null;
            }

            try
            {
                bool isCanLocate = IsCanLocate<Object>(path);
                if (!isCanLocate)
                {
                    Debug.LogError($"加载资源失败,资源不存在:{path}");
                    return null;
                }

                if (dicPath2Asset.TryGetValue(path, out YRAssetHandle assetHandle))
                {
                    assetHandle.InCreaseReferenceCount();
                    if (assetHandle.IsDone)
                        return assetHandle.GetAsset<T>();
                }
                else
                {
                    AssetHandle handle = YooAssets.LoadAssetAsync<T>(path);
                    assetHandle = GetAssetHandle(path, handle);

                    dicPath2Asset[path] = assetHandle;
                }

                await assetHandle.Wait();
                T result = assetHandle.GetAsset<T>();
                onLoaded?.Invoke(result);

                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message} \r\n {e.StackTrace}");
            }

            return null;
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="path"></param>
        /// <param name="loadSceneMode"></param>
        /// <returns></returns>
        public SceneHandle LoadSceneAsyn(string path, LoadSceneMode loadSceneMode)
        {
            SceneHandle sceneHandle = YooAssets.LoadSceneSync(path, loadSceneMode);

            return sceneHandle;
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="path"></param>
        /// <param name="loadSceneMode"></param>
        /// <returns></returns>
        public async UniTask<SceneHandle> LoadSceneAsync(string path, LoadSceneMode loadSceneMode)
        {
            SceneHandle sceneHandle = YooAssets.LoadSceneAsync(path, loadSceneMode);
            await sceneHandle.Task;

            return sceneHandle;
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="path"></param>
        public void Unload(string path)
        {
            if (!dicPath2Asset.TryGetValue(path, out YRAssetHandle assetHandle))
                return;

            assetHandle.DeCreaseReferenceCount();
            if (assetHandle.ReferenceCount == 0)
            {
                dicPath2Asset.Remove(path);
                ReturnAssetHandle(assetHandle);
            }
        }

        /// <summary>
        /// 预加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public async UniTask PostPreload<T>(string path) where T : Object
        {
            setPreloadingPath.Add(path);

            await LoadAsync<T>(path, null);

            setPreloadingPath.Remove(path);
        }

        /// <summary>
        /// 获取预加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T GetPreLoadResult<T>(string path) where T : Object
        {
            if (!dicPath2Asset.TryGetValue(path, out YRAssetHandle assetHandle))
            {
                Debug.LogError($"资源没有预加载!path:{path}");
                return null;
            }

            return assetHandle.GetAsset<T>();
        }

        /// <summary>
        /// 等待所有预加载
        /// </summary>
        /// <returns></returns>
        public async UniTask WaitAllPreload()
        {
            while (setPreloadingPath.Count > 0)
            {
                await UniTask.DelayFrame(1);
            }
        }
    }
}
