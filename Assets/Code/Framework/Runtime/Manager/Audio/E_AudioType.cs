namespace YRFramework.Runtime.Audio
{
    /// <summary>
    /// 音频类型
    /// </summary>
    public enum E_AudioType : sbyte // 8位有符号整数(-128~127)
    {
        /// <summary>
        /// 音乐
        /// </summary>
        Music,
        /// <summary>
        /// 音效
        /// </summary>
        Sound,

        /// <summary>
        /// 包含所有
        /// </summary>
        All,
    }
}