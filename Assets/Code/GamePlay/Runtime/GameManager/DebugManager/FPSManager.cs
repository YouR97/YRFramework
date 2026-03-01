using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.Manager;
using YRFramework.Runtime.Utility;

namespace GamePlay.Runtime
{
    /// <summary>
    /// FPS管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRGamePlay/FPS")]
    public sealed class FPSManager : YRFrameworkManager, IInit, IUpdate
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.FPS;
        #endregion

        private bool isInit;

        /// <summary>
        /// 更新间隔
        /// </summary>
        [SerializeField]
        [LabelText("更新间隔(秒)")]
        [Range(0.1f, 5f)]
        private float updateInterval = 0.3f;
        /// <summary>
        /// 帧率颜色显示信息
        /// </summary>
        [SerializeField]
        [LabelText("帧率颜色显示列表(小于对应帧率显示对应颜色，-1表示无限制)")]
        private List<FPSInfo> listFPSShowInfo;
        /// <summary>
        /// 上一次刷新的时间
        /// </summary>
        private float lastInterval;
        /// <summary>
        /// 累计帧
        /// </summary>
        private int frames;
        /// <summary>
        /// 帧率
        /// </summary>
        private int fps;
        /// <summary>
        /// 样式
        /// </summary>
        private GUIStyle style;

        public async UniTask OnInit()
        {
            isInit = false;

            lastInterval = Time.realtimeSinceStartup;
            frames = 0;

            style = new GUIStyle
            {
                border = new RectOffset(10, 10, 10, 10),
                fontSize = 50,
                fontStyle = FontStyle.BoldAndItalic,
            };

            isInit = true;
            await UniTask.CompletedTask;
        }

        public void OnRelease()
        {
            isInit = false;
        }

        public void OnUpdate(float deltaTime, float realtimeSinceStartup)
        {
            ++frames;
            if (realtimeSinceStartup < updateInterval + lastInterval)
                return;

            fps = (int)(frames / (realtimeSinceStartup - lastInterval));
            frames = 0;
            lastInterval = realtimeSinceStartup;
        }

        private void OnGUI()
        {
            if (!isInit)
                return;

            if (YRUtility.Collection.IsEmpty(listFPSShowInfo))
            {
                GUI.Label(new Rect(10f, 10f, 400f, 400f), $"<color=#ffffff>{fps}</color>", style);
                return;
            }

            foreach (FPSInfo fpsShowInfo in listFPSShowInfo)
            {
                if(YRConsts.INVALID_FLOAT >= fpsShowInfo.frameRate || fps < fpsShowInfo.frameRate)
                {
                    GUI.Label(new Rect(10f, 10f, 400f, 400f), $"<color=#{ColorUtility.ToHtmlStringRGB(fpsShowInfo.color)}>{Mathf.RoundToInt(fps)}</color>", style);
                    return;
                }
            }
        }
    }
}