using UnityEngine;
using UnityEngine.UI;
using YRFramework.Runtime;
using YRFramework.Runtime.UI;

namespace GamePlay.Runtime.UI
{
    /// <summary>
    /// 主界面
    /// </summary>
    internal sealed class UIMainController : UIControllerBase
    {
        /// <summary>
        /// 高斯模糊背景
        /// </summary>
        private RawImage rawImageGaussianBlur;
        private Button btnBag;
        /// <summary>
        /// 返回
        /// </summary>
        private Button btnBack;

        protected override void Awake(ReferenceCollector rc)
        {
            btnBack = rc.RcGetComponent<Button>("BtnBack");
            btnBag = rc.RcGetComponent<Button>("BtnBag");
            rawImageGaussianBlur = rc.RcGetComponent<RawImage>("RawImageGaussianBlur");

            btnBag.onClick.AddListener(OnBag);
            btnBack.onClick.AddListener(OnBack);

            Debug.Log("初始化主界面");
        }

        public void Open()
        {
            rawImageGaussianBlur.texture = Utility.ScreenShot.GetScreenTexture2D();

            Debug.Log("打开主界面");
        }

        public override void Close()
        {
            Debug.Log("关闭主界面");
        }

        private void OnClose()
        {
            UIMainFactory.Close();
        }

        private void OnBag()
        {
            //await UIBagFactory.Open();
        }

        private void OnBack()
        {
            UIMainFactory.Close();
        }
    }
}