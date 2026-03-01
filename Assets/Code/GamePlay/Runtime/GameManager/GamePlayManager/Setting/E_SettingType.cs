using YRFramework.Runtime;

namespace GamePlay.Runtime.Setting
{
    /// <summary>
    /// 设置类型
    /// </summary>
    public enum E_SettingType : sbyte
    {
        None = YRConsts.INVALID_INT,

        /// <summary>
        /// 游戏
        /// </summary>
        Game,
        /// <summary>
        /// 声音
        /// </summary>
        Audio,
        /// <summary>
        /// 语言
        /// </summary>
        Language,
        /// <summary>
        /// 图像
        /// </summary>
        Picture,
        /// <summary>
        /// 控制
        /// </summary>
        Controller,

        /// <summary>
        /// 数量
        /// </summary>
        Count,
    }
}