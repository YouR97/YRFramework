using UnityEngine.UI;
using YRFramework.Runtime;
using YRFramework.Runtime.UI;

namespace GamePlay.Runtime.UI
{
    /// <summary>
    /// 图鉴界面
    /// </summary>
    public class UIIllustrationController : UIControllerBase
    {
        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button btnBack;

        protected override void Awake(ReferenceCollector rc)
        {
            btnBack = rc.RcGetComponent<Button>("BtnBack");

            btnBack.onClick.AddListener(OnBack);
        }

        public void Open()
        {
        }

        public override void Close()
        {
        }

        private void OnBack()
        {
            UIIllustrationFactory.Close();
        }
    }
}