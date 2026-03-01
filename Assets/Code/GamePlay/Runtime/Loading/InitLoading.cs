using Cysharp.Threading.Tasks;
using GamePlay.Runtime.UI;
using System;
using UnityEngine;
using YooAsset;

namespace GamePlay.Runtime.Loading
{
    /// <summary>
    /// 游戏初始化Loading
    /// </summary>
    public sealed class InitLoading : LoadingBase
    {
        private bool isLoadComplete;

        public InitLoading(Func<UniTask> callback = null) : base(callback)
        {
        }

        protected override async UniTask OnLoadPre()
        {
            isLoadComplete = false;
            await UI_LoadingFactory.Open();
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override async UniTask<E_LoadingResult> OnLoad()
        {
            float subProgress = 0.1f;
            Progress = subProgress;

            #region 加载本地设置
            try
            {
                Game.Setting.LoadSetting(); // 加载本地设置
                subProgress = 0.2f;
                Progress = subProgress;
            }
            catch (Exception e)
            {
                Debug.LogError($"[{nameof(InitLoading)}]:加载本地设置错误：{e}");
                return E_LoadingResult.Error;
            }
            #endregion

            #region 加载配置
            try
            {
                Game.DataTable.LoadDataTable(); // 加载配置表
                subProgress = 0.3f;
                Progress = subProgress;
            }
            catch (Exception e)
            {
                Debug.LogError($"[{nameof(InitLoading)}]:加载配置错误：{e}");
                return E_LoadingResult.Error;
            }
            #endregion

            #region 加载主页场景
            try
            {
                float addProgress = 1f - subProgress;
                SceneHandle sceneHande = Game.Asset.LoadScene(Consts.Scene.HOME_SCENE); // 加载主场景
                sceneHande.Completed += OnLoadSceneComplete;
                while (!isLoadComplete)
                {
                    Progress = subProgress + addProgress * sceneHande.Progress;
                    await UniTask.WaitForSeconds(0.05f);
                }
                Progress = subProgress + addProgress;
            }
            catch (Exception e)
            {
                Debug.LogError($"[{nameof(InitLoading)}]:加载主场景错误：{e}");
                return E_LoadingResult.Error;
            }
            #endregion
            
            return E_LoadingResult.Success;
        }

        protected override async UniTask OnLoadAfter()
        {
            UI_LoadingFactory.Close();

            await UniTask.CompletedTask;
        }

        private void OnLoadSceneComplete(SceneHandle hande)
        {
            isLoadComplete = true;
            Game.Asset.Unload(Consts.Scene.HOME_SCENE);
        }
    }
}