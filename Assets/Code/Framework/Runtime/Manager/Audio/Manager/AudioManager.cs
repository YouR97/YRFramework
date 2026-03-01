using cfg.Audio;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using YRFramework.Runtime.Manager;

namespace YRFramework.Runtime.Audio
{
    /// <summary>
    /// 音频管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRFramework/AudioManager")]
    public sealed partial class AudioManager : YRFrameworkManager, IInit
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.Audio;
        #endregion

        #region 私有
        /// <summary>
        /// 音频播放器字典
        /// </summary>
        private Dictionary<E_AudioType, List<AudioSource>> dicSource;
        /// <summary>
        /// 音乐音频播放器列表
        /// </summary>
        [SerializeField]
        private List<AudioSource> listMusicAudioSource;
        /// <summary>
        /// 音效音频播放器列表
        /// </summary>
        [SerializeField]
        private List<AudioSource> listSoundAudioSource;
        #endregion

        async UniTask IInit.OnInit()
        {
            dicSource = new Dictionary<E_AudioType, List<AudioSource>>
            {
                { E_AudioType.Music, listMusicAudioSource },
                { E_AudioType.Sound, listSoundAudioSource },
            };

            InitVolume();

            await UniTask.CompletedTask;
        }

        void IInit.OnRelease()
        {
            ReleaseVolume();

            if (null != dicSource)
            {
                dicSource.Clear();
                dicSource = null;
            }
        }

        #region API
        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="audioCfgId"></param>
        public void PlayByKey(string audioCfgKey, E_AudioType audioType)
        {
            if (E_AudioType.All == audioType)
            {
                Debug.LogWarning($"[{nameof(AudioManager)}]:播放失败，无效的播放类型：{E_AudioType.All}");
                return;
            }

            AudioCfg cfg = FrameworkGameEnter.DataTable.AudioConfig.GetOrDefault(audioCfgKey);
            if (null == cfg)
                return;

            Play(cfg.Path, cfg.Volume, audioType);
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        /// <param name="audioCfgKey"></param>
        public void StopAllByType(E_AudioType audioType) 
        {
            if (E_AudioType.All == audioType)
            {
                foreach (List<AudioSource> temp in dicSource.Values)
                {
                    foreach (AudioSource audioSource in temp)
                    {
                        StopByAudioSource(audioSource);
                    }
                }
                return;
            }

            if (!dicSource.TryGetValue(audioType, out List<AudioSource> listAudioSource))
                return;

            foreach (AudioSource audioSource in listAudioSource)
            {
                StopByAudioSource(audioSource);
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="path"></param>
        private async void Play(string path, float volume, E_AudioType audioType)
        {
            AudioClip audioClip = await FrameworkGameEnter.Asset.LoadAssetAsync<AudioClip>(path);
            FrameworkGameEnter.Asset.Unload(path);
            PlayAudio(audioType, audioClip, volume);
        }

        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="audioType"></param>
        /// <param name="audioClip"></param>
        private void PlayAudio(E_AudioType audioType, AudioClip audioClip, float volume)
        {
            List<AudioSource> listAudioSource = dicSource[audioType];
            foreach (AudioSource audioSource in listAudioSource)
            {
                if (audioSource.isPlaying)
                    continue;

                audioSource.clip = audioClip;
                audioSource.volume = volume;
                audioSource.Play();
                break;
            }
        }

        /// <summary>
        /// 停止AudioSource
        /// </summary>
        /// <param name="audioSource"></param>
        private void StopByAudioSource(AudioSource audioSource)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
        #endregion
    }
}