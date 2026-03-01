using Cysharp.Threading.Tasks;
using GamePlay.Runtime.UI;
using System;
using UnityEngine;
using YooAsset;

namespace GamePlay.Runtime.Loading
{
    /// <summary>
    /// 战斗Loading
    /// </summary>
    public sealed class FightLoading : LoadingBase
    {
        private bool isLoadComplete;

        public FightLoading(Func<UniTask> callback = null) : base(callback)
        {
        }

        protected override async UniTask OnLoadPre()
        {
            await UI_BlackLoadingFactory.Open();
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override async UniTask<E_LoadingResult> OnLoad()
        {
            float subProgress = 0.1f;
            Progress = subProgress;

            #region 加载战斗场景
            try
            {
                float addProgress = 0.9f - subProgress;
                SceneHandle sceneHande = Game.Asset.LoadScene(Consts.Scene.FIGHT1_SCENE); // 加载战斗场景1
                sceneHande.Completed += OnLoadSceneComplete;
                while (!isLoadComplete)
                {
                    Progress = subProgress + addProgress * sceneHande.Progress;
                    await UniTask.WaitForSeconds(0.1f);
                }

                //GameObject[] goRoots = hande.SceneObject.GetRootGameObjects();
                //ReferenceCollector referenceCollector = goRoots[0].GetComponent<ReferenceCollector>();
                //CinemachineCamera cinemachineCamera = referenceCollector.RcGetComponent<CinemachineCamera>("CinemachineCamera");
                Progress = subProgress + addProgress;
            }
            catch (Exception e)
            {
                Debug.LogError($"[{nameof(InitLoading)}]:加载主场景错误：{e}");
                return E_LoadingResult.Error;
            }
            #endregion

            await UI_FightFactory.Open();
            Progress = 1f;
            
            return E_LoadingResult.Success;
        }

        protected override async UniTask OnLoadAfter()
        {
            UI_BlackLoadingFactory.CloseByAnim();

            await UniTask.CompletedTask;
        }

        private void OnLoadSceneComplete(SceneHandle hande)
        {
            isLoadComplete = true;
            Game.Asset.Unload(Consts.Scene.FIGHT1_SCENE);
        }
    }
}