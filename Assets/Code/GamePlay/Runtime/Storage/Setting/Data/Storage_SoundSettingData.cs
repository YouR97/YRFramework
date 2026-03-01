using YRFramework.Runtime.Storage;

namespace GamePlay.Runtime.Storage
{
    /// <summary>
    /// 本地缓存-声音设置数据
    /// </summary>
    public sealed class Storage_SoundSettingData : StorageDataBase
    {
        #region 私有字段
        /// <summary>
        /// 所有音量
        /// </summary>
        private float allVolume;
        /// <summary>
        /// 音乐音量
        /// </summary>
        private float musicVolume;
        /// <summary>
        /// 音效音量
        /// </summary>
        private float soundVolume;
        /// <summary>
        /// 是否全部静音
        /// </summary>
        private bool isAllMute;
        /// <summary>
        /// 音乐是否禁音
        /// </summary>
        private bool isMusicMute;
        /// <summary>
        /// 音效是否禁音
        /// </summary>
        private bool isSoundMute;
        #endregion

        #region 属性
        /// <summary>
        /// 总音量
        /// </summary>
        public float AllVolume
        {
            get { return allVolume; }
            set { SetField(value, ref allVolume); }
        }

        /// <summary>
        /// 音乐音量
        /// </summary>
        public float MusicVolume
        {
            get { return musicVolume; }
            set { SetField(value, ref musicVolume); }
        }

        /// <summary>
        /// 音效音量
        /// </summary>
        public float SoundVolume
        {
            get { return soundVolume; }
            set { SetField(value, ref soundVolume); }
        }

        /// <summary>
        /// 是否全部静音
        /// </summary>
        public bool IsAllMute
        {
            get { return isAllMute; }
            set { SetField(value, ref isAllMute); }
        }

        /// <summary>
        /// 音乐是否静音
        /// </summary>
        public bool IsMusicMute
        {
            get { return isMusicMute; }
            set { SetField(value, ref isMusicMute); }
        }

        /// <summary>
        /// 音效是否静音
        /// </summary>
        public bool IsSoundMute
        {
            get { return isSoundMute; }
            set { SetField(value, ref isSoundMute); }
        }
        #endregion

        internal Storage_SoundSettingData()
        {
            allVolume = 1f;
            musicVolume = 1f;
            soundVolume = 1f;
            isAllMute = false;
            isMusicMute = false;
            isSoundMute = false;
        }
    }
}