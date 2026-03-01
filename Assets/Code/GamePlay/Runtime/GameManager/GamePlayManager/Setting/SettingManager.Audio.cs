using UnityEngine;
using YRFramework.Runtime.Manager;
using GamePlay.Runtime.Storage;
using YRFramework.Runtime.Utility;
using YRFramework.Runtime.Audio;

namespace GamePlay.Runtime.Setting
{
    /// <summary>
    /// 设置管理器-声音设置
    /// </summary>
    public sealed partial class SettingManager : YRFrameworkManager
    {
        #region 属性
        /// <summary>
        /// 声音设置
        /// </summary>
        private Storage_SoundSettingData soundSettingData;

        /// <summary>
        /// 所有音量
        /// </summary>
        public float AllVolume { get; set; }
        /// <summary>
        /// 音乐音量
        /// </summary>
        public float MusicVolume { get; set; }
        /// <summary>
        /// 音效音量
        /// </summary>
        public float SoundVolume { get; set; }
        /// <summary>
        /// 是否全部静音
        /// </summary>
        public bool IsAllMute { get; set; }
        /// <summary>
        /// 音乐是否禁音
        /// </summary>
        public bool IsMusicMute { get; set; }
        /// <summary>
        /// 音效是否禁音
        /// </summary>
        public bool IsSoundMute { get; set; }
        #endregion

        /// <summary>
        /// 加载声音设置
        /// </summary>
        private void LoadAudioSetting()
        {
            soundSettingData = Storage_SoundSettingFactory.GetData();
            if (null == soundSettingData)
            {
                Debug.LogError($"[{nameof(SettingManager)}]:声音设置加载错误，null");
                YRUtility.Game.ExitGame();
            }

            RestoreSoundSetting();
        }

        #region 保存设置
        /// <summary>
        /// 保存声音设置
        /// </summary>
        public void SaveAudioSetting(bool isImmediate = true)
        {
            soundSettingData.AllVolume = AllVolume;
            soundSettingData.MusicVolume = MusicVolume;
            soundSettingData.SoundVolume = SoundVolume;
            soundSettingData.IsAllMute = IsAllMute;
            soundSettingData.IsMusicMute = IsMusicMute;
            soundSettingData.IsSoundMute = IsSoundMute;

            if (isImmediate)
                Game.Storage.Save();
        }

        /// <summary>
        /// 还原声音设置
        /// </summary>
        public void RestoreSoundSetting()
        {
            AllVolume = soundSettingData.AllVolume;
            MusicVolume = soundSettingData.MusicVolume;
            SoundVolume = soundSettingData.SoundVolume;
            IsAllMute = soundSettingData.IsAllMute;
            IsMusicMute = soundSettingData.IsMusicMute;
            IsSoundMute = soundSettingData.IsSoundMute;
        }

        /// <summary>
        /// 应用所有声音设置
        /// </summary>
        public void ApplyAllAudioSetting()
        {
            Game.Audio.SetVolume(E_AudioType.Music, IsAllMute, MusicVolume);
            Game.Audio.SetVolume(E_AudioType.Sound, IsSoundMute, SoundVolume);
            Game.Audio.SetVolume(E_AudioType.All, IsAllMute, AllVolume);
        }
        #endregion
    }
}