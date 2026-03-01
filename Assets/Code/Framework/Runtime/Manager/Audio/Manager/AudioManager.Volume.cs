using UnityEngine;
using UnityEngine.Audio;
using YRFramework.Runtime.Manager;

namespace YRFramework.Runtime.Audio
{
    /// <summary>
    /// 音频管理器，声音管理
    /// </summary>
    public sealed partial class AudioManager : YRFrameworkManager, IInit
    {
        #region 私有
        /// <summary>
        /// 最小音量
        /// </summary>
        private const float MIN_VOLUME = 0f;
        /// <summary>
        /// 主AudioMixer音量参数名
        /// </summary>
        private const string MAIN_MIXER_VOLUME_PARAM_NAME = "MainVolume";
        /// <summary>
        /// 音乐Mixer音量参数名
        /// </summary>
        private const string MUSIC_MIXER_VOLUME_PARAM_NAME = "MusicVolume";
        /// <summary>
        /// 音效Mixer音量参数名
        /// </summary>
        private const string SOUND_MIXER_VOLUME_PARAM_NAME = "SoundVolume";

        /// <summary>
        /// 总Mixer
        /// </summary>
        [SerializeField]
        private AudioMixer mainMixer;
        /// <summary>
        /// 音乐Mixer
        /// </summary>
        [SerializeField]
        private AudioMixer musicMixer;
        /// <summary>
        /// 音效Mixer
        /// </summary>
        [SerializeField]
        private AudioMixer soundMixer;
        #endregion

        #region 属性
        /// <summary>
        /// 总音量
        /// </summary>
        public float AllVolume { get; private set; }
        /// <summary>
        /// 音乐音量
        /// </summary>
        public float MusicVolume { get; private set; }
        /// <summary>
        /// 音效音量
        /// </summary>
        public float SoundVolume { get; private set; }
        /// <summary>
        /// 全部音量禁音
        /// </summary>
        public bool IsAllMute { get; private set; }
        /// <summary>
        /// 音乐是否禁音
        /// </summary>
        public bool IsMusicMute { get; private set; }
        /// <summary>
        /// 音效是否禁音
        /// </summary>
        public bool IsSoundMute { get; private set; }
        #endregion

        /// <summary>
        /// 初始化声音管理
        /// </summary>
        private void InitVolume()
        {
            AllVolume = 1f;
            MusicVolume = 1f;
            SoundVolume = 1f;
            IsAllMute = false;
            IsMusicMute = false;
            IsSoundMute = false;

            SetVolume(E_AudioType.All, IsAllMute, AllVolume);
        }

        /// <summary>
        /// 释放
        /// </summary>
        private void ReleaseVolume()
        { }

        #region API
        /// <summary>
        /// 设置音量
        /// </summary>
        public void SetVolume(E_AudioType audioType, bool isMute, float volume)
        {
            switch (audioType)
            {
                case E_AudioType.Music:
                    {
                        SetMusicVolume(isMute, volume);
                    }
                    break;
                case E_AudioType.Sound:
                    {
                        SetSoundVolume(isMute, volume);
                    }
                    break;
                case E_AudioType.All:
                    {
                        SetAllVolume(isMute, volume);
                    }
                    break;
                default:
                    {
                        Debug.LogError($"[{nameof(AudioManager)}]:设置音量失败，未处理类型{audioType}");
                    }
                    break;
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 设置主音量
        /// </summary>
        /// <param name="isMute"></param>
        /// <param name="volume"></param>
        private void SetAllVolume(bool isMute, float volume)
        {
            IsAllMute = isMute;
            AllVolume = volume;
            if (IsAllMute)
                mainMixer.SetFloat(MAIN_MIXER_VOLUME_PARAM_NAME, Remap01ToDB(MIN_VOLUME));
            else
                mainMixer.SetFloat(MAIN_MIXER_VOLUME_PARAM_NAME, Remap01ToDB(AllVolume));
        }

        /// <summary>
        /// 设置音乐音量
        /// </summary>
        /// <param name="isMute"></param>
        /// <param name="volume"></param>
        private void SetMusicVolume(bool isMute, float volume)
        {
            IsMusicMute = isMute;
            MusicVolume = volume;
            if (IsMusicMute)
                mainMixer.SetFloat(MUSIC_MIXER_VOLUME_PARAM_NAME, Remap01ToDB(MIN_VOLUME));
            else
                mainMixer.SetFloat(MUSIC_MIXER_VOLUME_PARAM_NAME, Remap01ToDB(MusicVolume));
        }

        /// <summary>
        /// 设置音效音量
        /// </summary>
        /// <param name="isMute"></param>
        /// <param name="volume"></param>
        private void SetSoundVolume(bool isMute, float volume)
        {
            IsSoundMute = isMute;
            SoundVolume = volume;
            if (IsSoundMute)
                mainMixer.SetFloat(SOUND_MIXER_VOLUME_PARAM_NAME, Remap01ToDB(MIN_VOLUME));
            else
                mainMixer.SetFloat(SOUND_MIXER_VOLUME_PARAM_NAME, Remap01ToDB(SoundVolume));
        }

        /// <summary>
        /// 百分比映射到DB
        /// </summary>
        /// <param name="value">0%~1%</param>
        /// <returns>-80~0</returns>
        private float Remap01ToDB(float value)
        {
            if (value <= 0f)
                value = 0.0001f;

            return Mathf.Log10(value) * 20f;
        }
        #endregion
    }
}