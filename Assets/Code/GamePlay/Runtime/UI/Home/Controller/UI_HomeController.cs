using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using YRFramework.Runtime;
using YRFramework.Runtime.Audio;
using YRFramework.Runtime.UI;
using YRFramework.Runtime.Utility;

namespace GamePlay.Runtime.UI
{
    /// <summary>
    /// 主页界面
    /// </summary>
    public class UI_HomeController : UIControllerBase
    {
        #region UI
        /// <summary>
        /// 开始游戏按钮
        /// </summary>
        private Button btnBeginGame;
        /// <summary>
        /// 设置
        /// </summary>
        private Button btnSetting;
        /// <summary>
        /// 图鉴按钮
        /// </summary>
        private Button btnIllustration;
        /// <summary>
        /// 退出游戏
        /// </summary>
        private Button btnExitGame;
        #endregion

        protected override void Awake(ReferenceCollector rc)
        {
            btnBeginGame = rc.RcGetComponent<Button>("BtnBeginGame");
            btnSetting = rc.RcGetComponent<Button>("BtnSetting");
            btnIllustration = rc.RcGetComponent<Button>("BtnIllustration");
            btnExitGame = rc.RcGetComponent<Button>("BtnExitGame");

            Utility.UI.BindButton(btnBeginGame, OnBeginGame);
            Utility.UI.BindButton(btnSetting, OnSetting);
            Utility.UI.BindButton(btnIllustration, OnIllustration);
            Utility.UI.BindButton(btnExitGame, OnExitGame);
        }

        public void Open()
        {
            Game.Audio.PlayByKey("Music_2", E_AudioType.Music); // TODO 测试,Music_KeepOn
        }

        public override void Close()
        {
            Game.Audio.StopAllByType(E_AudioType.All);
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        private void ExitGame()
        {
            YRUtility.Game.ExitGame();
        }

        #region 回调方法
        /// <summary>
        /// 开始游戏
        /// </summary>
        private void OnBeginGame()
        {
            Game.Audio.PlayByKey(Consts.Audio.SOUND_UI_COMMON_CLICK, E_AudioType.Sound);

            OnClick(async () =>
            {
                isAcceptInput = false;

                await Game.Fight.EnterFight();

                isAcceptInput = true;
            });
        }

        /// <summary>
        /// 设置
        /// </summary>
        private void OnSetting()
        {
            Game.Audio.PlayByKey(Consts.Audio.SOUND_UI_COMMON_CLICK, E_AudioType.Sound);

            OnClick(async () =>
            {
                isAcceptInput = false;

                await UI_SettingFactory.Open();

                isAcceptInput = true;
            });
        }

        /// <summary>
        /// 图鉴
        /// </summary>
        private void OnIllustration()
        {
            Game.Audio.PlayByKey(Consts.Audio.SOUND_UI_COMMON_CLICK, E_AudioType.Sound);

            OnClick(async () =>
            {
                isAcceptInput = false;

                await UI_DialogFactory.Open("提示", "图鉴未开发", null, "确定"); // TODO 本地化

                isAcceptInput = true;
            });
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        private void OnExitGame()
        {
            Game.Audio.PlayByKey(Consts.Audio.SOUND_UI_COMMON_CLICK, E_AudioType.Sound);

            OnClick(async () =>
            {
                isAcceptInput = false;

                await UI_DialogFactory.Open("提示", "是否退出游戏？", null, "确定", ExitGame, null, null, "取消"); // TODO 本地化

                isAcceptInput = true;
            });
        }
        #endregion
    }
}