using UnityEngine;

namespace YRFramework.Editor
{
    /// <summary>
    /// 纹理导入设置常量
    /// </summary>
    public static class TextureImportSettingConst
    {
        /// <summary>
        /// 纹理导入设置路径
        /// </summary>
        public static readonly string TextureImportSettingsPath = $"Assets/Settings/ImportSettings/TextureImportSettings.asset";

        /// <summary>
        /// 忽略的后缀名(拥有该后缀名不进行导入设置)
        /// </summary>
        public static string IgnorePostName = "_Ignore";
        /// <summary>
        /// 无效名
        /// </summary>
        public static string NoneName = "-";

        /// <summary>
        /// 前缀名
        /// </summary>
        public static readonly string[] PreNames =
        {
            NoneName,

            "ui_",
            "BG_",
            "role_"
        };

        /// <summary>
        /// 文件类型后缀
        /// </summary>
        public static readonly string[] EndDress =
        {
            NoneName,

            ".jpg",
            ".png"
        };

        /// <summary>
        /// 纹理最大大小
        /// </summary>
        public static readonly string[] TextureMaxSizes =
        {
            "32",
            "64",
            "128",
            "256",
            "512",
            "1024",
            "2048",
            "4096",
            "8192"
        };
    }
}