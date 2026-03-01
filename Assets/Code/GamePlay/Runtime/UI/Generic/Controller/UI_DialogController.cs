using System;
using TMPro;
using UnityEngine.UI;
using YRFramework.Runtime;
using YRFramework.Runtime.UI;

namespace GamePlay.Runtime.UI
{
    /// <summary>
    /// 通用选择对话框界面
    /// </summary>
    public class UI_DialogController : UIControllerBase
    {
        #region UI
        /// <summary>
        /// 标题
        /// </summary>
        private TMP_Text textTitle;
        /// <summary>
        /// 内容
        /// </summary>
        private TMP_Text textContent;
        /// <summary>
        /// 按钮1文本
        /// </summary>
        private TMP_Text textBtn1;
        /// <summary>
        /// 按钮2文本
        /// </summary>
        private TMP_Text textBtn2;
        /// <summary>
        /// 按钮3文本
        /// </summary>
        private TMP_Text textBtn3;
        /// <summary>
        /// 按钮1
        /// </summary>
        private Button btn1;
        /// <summary>
        /// 按钮2
        /// </summary>
        private Button btn2;
        /// <summary>
        /// 按钮3
        /// </summary>
        private Button btn3;
        #endregion

        #region 私有
        /// <summary>
        /// 回调1
        /// </summary>
        private Action callback1;
        /// <summary>
        /// 回调2
        /// </summary>
        private Action callback2;
        /// <summary>
        /// 回调3
        /// </summary>
        private Action callback3;
        /// <summary>
        /// 关闭回调
        /// </summary>
        private Action close;
        #endregion

        #region 属性
        /// <summary>
        /// 是否显示按钮1
        /// </summary>
        public bool IsShowBtn1
        {
            set { btn1.gameObject.SetActive(value); }
        }

        /// <summary>
        /// 是否显示按钮2
        /// </summary>
        public bool IsShowBtn2
        {
            set { btn2.gameObject.SetActive(value); }
        }

        /// <summary>
        /// 是否显示按钮3
        /// </summary>
        public bool IsShowBtn3
        {
            set { btn3.gameObject.SetActive(value); }
        }
        #endregion

        protected override void Awake(ReferenceCollector rc)
        {
            textTitle = rc.RcGetComponent<TMP_Text>("TextTitle");
            textContent = rc.RcGetComponent<TMP_Text>("TextContent");
            textBtn1 = rc.RcGetComponent<TMP_Text>("TextOne");
            textBtn2 = rc.RcGetComponent<TMP_Text>("TextTwo");
            textBtn3 = rc.RcGetComponent<TMP_Text>("TextThree");
            btn1 = rc.RcGetComponent<Button>("BtnOne");
            btn2 = rc.RcGetComponent<Button>("BtnTwo");
            btn3 = rc.RcGetComponent<Button>("BtnThree");

            btn1.onClick.AddListener(OnBtn1);
            btn2.onClick.AddListener(OnBtn2);
            btn3.onClick.AddListener(OnBtn3);
        }

        public void Open(string title, string content, Action closeAction = null,
            string btn1Text = "", Action action1 = null,
            string btn2Text = "", Action action2 = null,
            string btn3Text = "", Action action3 = null)
        {
            textTitle.text = title;
            textContent.text = content;
            close = closeAction;
            textBtn1.text = btn1Text;
            textBtn2.text = btn2Text;
            textBtn3.text = btn3Text;
            callback1 = action1;
            callback2 = action2;
            callback3 = action3;

            IsShowBtn1 = null != callback1 || (null != btn1Text && string.Empty != btn1Text);
            IsShowBtn2 = null != callback2 || (null != btn2Text && string.Empty != btn2Text);
            IsShowBtn3 = null != callback3 || (null != btn3Text && string.Empty != btn3Text);
        }

        public override void Close()
        {
            callback1 = null;
            callback2 = null;
            callback3 = null;
            close = null;
        }

        #region 回调
        /// <summary>
        /// 关闭界面
        /// </summary>
        private void OnClose()
        {
            close?.Invoke();
            UI_DialogFactory.Close();
        }

        private void OnBtn1()
        {
            callback1?.Invoke();
            OnClose();
        }

        private void OnBtn2()
        {
            callback2?.Invoke();
            OnClose();
        }

        private void OnBtn3()
        {
            callback3?.Invoke();
            OnClose();
        }
        #endregion
    }
}