using GamePlay.Runtime.Setting;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YRFramework.Runtime;
using YRFramework.Runtime.Audio;
using YRFramework.Runtime.UI;

namespace GamePlay.Runtime.UI
{
    /// <summary>
    /// 设置界面
    /// </summary>
    public sealed class UI_SettingController : UIControllerBase
    {
        #region 结构
        /// <summary>
        /// 页签
        /// </summary>
        private sealed class UI_Tap : UIGameObjectNode
        {
            /// <summary>
            /// 选中状态
            /// </summary>
            private readonly GameObject goSelect;
            /// <summary>
            /// 按钮
            /// </summary>
            private readonly Button btnTab;

            /// <summary>
            /// 设置类型
            /// </summary>
            private readonly E_SettingType settingType;
            /// <summary>
            /// 点击回调
            /// </summary>
            private Action<E_SettingType> clickAction;

            /// <summary>
            /// 是否选中
            /// </summary>
            public bool IsSelect
            {
                set { goSelect.IsScaleShow(value); }
            }

            public UI_Tap(GameObject root, E_SettingType eSettingType, Action<E_SettingType> click) : base(root, true)
            {
                settingType = eSettingType;
                clickAction = click;

                goSelect = rc.RcGetGameObject("ImageSelect");
                btnTab = GoRoot.GetComponent<Button>();

                IsSelect = false;

                Utility.UI.BindButton(btnTab, OnClickTab);
            }

            private void OnClickTab()
            {
                clickAction?.Invoke(settingType);
            }

            protected override void Dispose(bool isDisposing)
            {
                clickAction = null;

                base.Dispose(isDisposing);
            }
        }

        /// <summary>
        /// 设置UI基类
        /// </summary>
        private abstract class UI_SettingBase : UIGameObjectNode
        {
            /// <summary>
            /// 滑动列表
            /// </summary>
            protected ScrollRect scrollRect;

            /// <summary>
            /// 是否显示(缩放控制)
            /// </summary>
            public new bool IsScaleShow
            {
                get { return goRoot.transform; }
                set
                {
                    if (value)
                    {
                        goRoot.IsScaleShow(value);
                        scrollRect.enabled = true;
                    }
                    else
                    {
                        scrollRect.enabled = false;
                        goRoot.IsScaleShow(value);
                    }
                }
            }

            protected UI_SettingBase(GameObject root) : base(root, true)
            {
                scrollRect = rc.RcGetComponent<ScrollRect>("ScrollView");

                IsScaleShow = false;
            }

            public abstract void SetInfo();
        }

        /// <summary>
        /// 游戏设置
        /// </summary>
        private sealed class UI_GameSetting : UI_SettingBase
        {
            public UI_GameSetting(GameObject root) : base(root)
            {
            }

            public override void SetInfo()
            {
                IsScaleShow = true;
            }
        }

        /// <summary>
        /// 声音设置
        /// </summary>
        private sealed class UI_AudioSetting : UI_SettingBase
        {
            /// <summary>
            /// 声音音量
            /// </summary>
            private sealed class UI_AudioVolume : UIGameObjectNode
            {
                /// <summary>
                /// 音量滑动条
                /// </summary>
                private Slider sliderVolume;
                /// <summary>
                /// 音量文本
                /// </summary>
                private TMP_Text textVolume;

                public E_AudioType AudioType { get; private set; }

                public UI_AudioVolume(GameObject root, E_AudioType audioType) : base(root)
                {
                    AudioType = audioType;

                    sliderVolume = rc.RcGetComponent<Slider>("SliderVolume");
                    textVolume = rc.RcGetComponent<TMP_Text>("TextVolume");

                    sliderVolume.onValueChanged.AddListener(OnVolumeChange);
                }

                public void SetInfo()
                {
                    switch (AudioType)
                    {
                        case E_AudioType.All:
                            {
                                SetVolume(true, Game.Setting.AllVolume);
                            }
                            break;
                        case E_AudioType.Music:
                            {
                                SetVolume(true, Game.Setting.MusicVolume);
                            }
                            break;
                        case E_AudioType.Sound:
                            {
                                SetVolume(true, Game.Setting.SoundVolume);
                            }
                            break;
                        default:
                            {
                                Debug.LogError($"[{nameof(UI_AudioVolume)}]:未处理类型{AudioType}");
                            }
                            break;
                    }

                    IsScaleShow = true;
                }

                /// <summary>
                /// 音量变化回调
                /// </summary>
                /// <param name="value"></param>
                private void OnVolumeChange(float value)
                {
                    SetVolume(false, value);
                }

                /// <summary>
                /// 设置音量
                /// </summary>
                private void SetVolume(bool isInit, float value)
                {
                    if (isInit)
                        sliderVolume.value = value;
                    else
                    {
                        switch (AudioType)
                        {
                            case E_AudioType.All:
                                {
                                    Game.Setting.AllVolume = value;
                                }
                                break;
                            case E_AudioType.Music:
                                {
                                    Game.Setting.MusicVolume = value;
                                }
                                break;
                            case E_AudioType.Sound:
                                {
                                    Game.Setting.SoundVolume = value;
                                }
                                break;
                            default:
                                {
                                    Debug.LogError($"[{nameof(UI_AudioVolume)}]:未处理类型{AudioType}");
                                }
                                break;
                        }

                        Game.Setting.ApplyAllAudioSetting();
                    }

                    textVolume.text = $"{(int)(value * 100)}";
                }
            }

            /// <summary>
            /// 音量控制数组
            /// </summary>
            private UI_AudioVolume[] audioVolumes;

            public UI_AudioSetting(GameObject root) : base(root)
            {
                audioVolumes = new UI_AudioVolume[3]
                {
                    new(rc.RcGetGameObject("MainVolumeRoot"), E_AudioType.All),
                    new(rc.RcGetGameObject("MusicVolumeRoot"), E_AudioType.Music),
                    new(rc.RcGetGameObject("SoundVolumeRoot"), E_AudioType.Sound)
                };
            }

            public override void SetInfo()
            {
                foreach (UI_AudioVolume audioVolume in audioVolumes)
                {
                    audioVolume.SetInfo();
                }

                IsScaleShow = true;
            }

            protected override void Dispose(bool isDisposing)
            {
                if (null != audioVolumes)
                {
                    foreach (var i in audioVolumes)
                    {
                        i.Dispose();
                    }

                    audioVolumes = null;
                }

                base.Dispose(isDisposing);
            }
        }

        /// <summary>
        /// 语言设置
        /// </summary>
        private sealed class UI_LanguageSetting : UI_SettingBase
        {
            public UI_LanguageSetting(GameObject root) : base(root)
            {
            }

            public override void SetInfo()
            {
                IsScaleShow = true;
            }
        }

        /// <summary>
        /// 画面设置
        /// </summary>
        private sealed class UI_PictureSetting : UI_SettingBase
        {
            /// <summary>
            /// 帧率滑动条
            /// </summary>
            private Slider sliderFrameRate;
            /// <summary>
            /// 帧率文本
            /// </summary>
            private TMP_Text textFrameRate;

            public UI_PictureSetting(GameObject root) : base(root)
            {
                sliderFrameRate = rc.RcGetComponent<Slider>("SliderFrameRate");
                textFrameRate = rc.RcGetComponent<TMP_Text>("TextFrameRate");

                sliderFrameRate.onValueChanged.AddListener(OnFrameRateChange);
            }

            public override void SetInfo()
            {
                SetFrameRate(true, Game.Setting.FrameRate);

                IsScaleShow = true;
            }

            /// <summary>
            /// 帧率变化回调
            /// </summary>
            /// <param name="value"></param>
            private void OnFrameRateChange(float value)
            {
                SetFrameRate(false, value);
            }

            /// <summary>
            /// 设置帧率
            /// </summary>
            private void SetFrameRate(bool isInit, float value)
            {
                if (isInit)
                    sliderFrameRate.value = value;
                else
                {
                    Game.Setting.FrameRate = (int)value;

                    Game.Setting.ApplyAllPictureSetting();
                }

                textFrameRate.text = $"{(int)value}";
            }
        }

        /// <summary>
        /// 控制设置
        /// </summary>
        private sealed class UI_ControllerSetting : UI_SettingBase
        {
            public UI_ControllerSetting(GameObject root) : base(root)
            {
            }

            public override void SetInfo()
            {
                IsScaleShow = true;
            }
        }
        #endregion

        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button btnBack;

        /// <summary>
        /// UI页签字典
        /// </summary>
        private Dictionary<E_SettingType, UI_Tap> dicUITap;
        /// <summary>
        /// UI设置字典
        /// </summary>
        private Dictionary<E_SettingType, UI_SettingBase> dicUISetting;
        /// <summary>
        /// 当前选中设置类型
        /// </summary>
        private E_SettingType curSelectSettingType;

        #region 生命周期
        protected override void Awake(ReferenceCollector rc)
        {
            btnBack = rc.RcGetComponent<Button>("BtnBack");

            dicUITap = new Dictionary<E_SettingType, UI_Tap>((int)E_SettingType.Count)
            {
                { E_SettingType.Game, new(rc.RcGetGameObject("TabGame"), E_SettingType.Game, OnTab) },
                { E_SettingType.Audio, new(rc.RcGetGameObject("TabAudio"), E_SettingType.Audio, OnTab) },
                { E_SettingType.Language, new(rc.RcGetGameObject("TabLanguage"), E_SettingType.Language, OnTab) },
                { E_SettingType.Picture, new(rc.RcGetGameObject("TabPicture"), E_SettingType.Picture, OnTab) },
                { E_SettingType.Controller, new(rc.RcGetGameObject("TabController"), E_SettingType.Controller, OnTab) },
            };

            dicUISetting = new Dictionary<E_SettingType, UI_SettingBase>((int)E_SettingType.Count)
            {
                { E_SettingType.Game, new UI_GameSetting(rc.RcGetGameObject("ContentGame")) },
                { E_SettingType.Audio, new UI_AudioSetting(rc.RcGetGameObject("ContentAudio")) },
                { E_SettingType.Language, new UI_LanguageSetting(rc.RcGetGameObject("ContentLanguage")) },
                { E_SettingType.Picture, new UI_PictureSetting(rc.RcGetGameObject("ContentPicture")) },
                { E_SettingType.Controller, new UI_ControllerSetting(rc.RcGetGameObject("ContentController")) },
            };

            curSelectSettingType = E_SettingType.None;

            btnBack.onClick.AddListener(OnClose);
        }

        /// <summary>
        /// 入口
        /// </summary>
        public void Open()
        {
            OnTab(E_SettingType.Game);
        }

        public override void Close()
        {
        }

        public override void Dispose()
        {
            if (null != dicUITap)
            {
                foreach (var i in dicUITap.Values)
                {
                    i.Dispose();
                }

                dicUITap.Clear();
                dicUITap = null;
            }

            if (null != dicUISetting)
            {
                foreach (var i in dicUISetting.Values)
                {
                    i.Dispose();
                }

                dicUISetting.Clear();
                dicUISetting = null;
            }

            base.Dispose();
        }
        #endregion

        /// <summary>
        /// 关闭界面
        /// </summary>
        private void OnClose()
        {
            Game.Setting.SaveAllSetting();

            UI_SettingFactory.Close();
        }

        private void OnTab(E_SettingType settingType)
        {
            if (curSelectSettingType == settingType)
                return;

            if (dicUITap.TryGetValue(curSelectSettingType, out UI_Tap oldUITap))
                oldUITap.IsSelect = false;

            if (dicUISetting.TryGetValue(curSelectSettingType, out UI_SettingBase oldUISettingBase))
                oldUISettingBase.IsScaleShow = false;

            curSelectSettingType = settingType;

            if (dicUITap.TryGetValue(curSelectSettingType, out UI_Tap newUITap))
                newUITap.IsSelect = true;

            if (dicUISetting.TryGetValue(curSelectSettingType, out UI_SettingBase newUISettingBase))
                newUISettingBase.SetInfo();
        }
    }
}