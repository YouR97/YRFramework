using TMPro;
using UnityEngine.UI;
using YRFramework.Runtime;
using YRFramework.Runtime.UI;
using YRFramework.Runtime.Utility;

namespace GamePlay.Runtime.UI
{
    /// <summary>
    /// 加载界面
    /// </summary>
    public class UI_LoadingController : UIControllerBase
    {
        #region UI
        /// <summary>
        /// 进度条
        /// </summary>
        private Slider slider;
        /// <summary>
        /// 进度文本
        /// </summary>
        private TextMeshProUGUI textProcess;
        #endregion

        protected override void Awake(ReferenceCollector rc)
        {
            #region 查找引用
            slider = rc.RcGetComponent<Slider>("Slider");
            textProcess = rc.RcGetComponent<TextMeshProUGUI>("TextProcess");
            #endregion
        }

        public void Open()
        {
            slider.value = 0f;
            textProcess.text = "0%";
        }

        protected override void Update(float deltaTime, float realtimeSinceStartup)
        {
            base.Update(deltaTime, realtimeSinceStartup);

            float progress = Game.Loading.ShowProgress;
            slider.value = progress;
            textProcess.text = YRUtility.Text.ToPercentage(progress, 0);
        }

        public override void Close()
        {
        }
    }
}