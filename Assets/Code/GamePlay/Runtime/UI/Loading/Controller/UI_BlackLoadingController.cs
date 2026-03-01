using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using YRFramework.Runtime;
using YRFramework.Runtime.UI;

namespace GamePlay.Runtime.UI
{
    /// <summary>
    /// 通用黑屏加载界面
    /// </summary>
    public sealed class UI_BlackLoadingController : UIControllerBase
    {
        #region UI
        /// <summary>
        /// 背景
        /// </summary>
        private RawImage rImgBg;
        /// <summary>
        /// 左黑屏
        /// </summary>
        private RectTransform rtsLeft;
        /// <summary>
        /// 右黑屏
        /// </summary>
        private RectTransform rtsRight;
        #endregion

        /// <summary>
        /// 进入动画
        /// </summary>
        private Tween tweenInLeft;
        private Tween tweenInRight;
        private Tween tweenOutLeft;
        private Tween tweenOutRight;
        private bool isNeedClose;
        private bool isEnterComplete;

        protected override void Awake(ReferenceCollector rc)
        {
            rImgBg = rc.RcGetComponent<RawImage>("Bg");
            rtsLeft = rc.RcGetComponent<RectTransform>("ImageLeft");
            rtsRight = rc.RcGetComponent<RectTransform>("ImageRight");
        }

        public void Open()
        {
            rImgBg.texture = Utility.ScreenShot.GetScreenTexture2D();
            rImgBg.enabled = true;
            isEnterComplete = false;
            isNeedClose = false;
            Vector2 startValue = new(0f, Screen.height);
            rtsLeft.sizeDelta = startValue;
            rtsRight.sizeDelta = startValue;

            Vector2 endValue = new(Screen.width * 0.5f, Screen.height);

            tweenInLeft = rtsLeft.DOSizeDelta(endValue, 0.618f).SetEase(Ease.InCubic).OnComplete(() => { isEnterComplete = true; }); // 先慢后快
            tweenInRight = rtsRight.DOSizeDelta(endValue, 0.618f).SetEase(Ease.InCubic);
        }

        protected override void Update(float deltaTime, float realtimeSinceStartup)
        {
            if (!isNeedClose || !isEnterComplete)
                return;

            tweenInLeft?.Kill();
            tweenInRight?.Kill();
            
            rImgBg.enabled = false;
            isNeedClose = false;
            isEnterComplete = false;
            
            Vector2 startValue = new(0f, Screen.height);

            tweenOutLeft = rtsLeft.DOSizeDelta(startValue, 0.618f).SetEase(Ease.OutCubic).OnComplete(OnClose); // 先慢后快
            tweenOutRight = rtsRight.DOSizeDelta(startValue, 0.618f).SetEase(Ease.OutCubic);
        }

        public void CloseByAnim()
        {
            isNeedClose = true;
        }

        public override void Close()
        {
            if (null != tweenInLeft)
            {
                tweenInLeft.Kill();
                tweenInLeft = null;
            }

            if (null != tweenInRight)
            {
                tweenInRight.Kill();
                tweenInRight = null;
            }

            if (null != tweenOutLeft)
            {
                tweenOutLeft.Kill();
                tweenOutLeft = null;
            }

            if (null != tweenOutRight)
            {
                tweenOutRight.Kill();
                tweenOutRight = null;
            }
        }

        private void OnClose()
        {
            UI_BlackLoadingFactory.Close();
        }
    }
}