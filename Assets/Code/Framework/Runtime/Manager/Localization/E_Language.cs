using Sirenix.OdinInspector;

namespace YRFramework.Runtime.Localization
{
    /// <summary>
    /// 语言类型
    /// </summary>
    public enum E_Language
    {
        /// <summary>
        /// 无效
        /// </summary>
        None = YRConsts.INVALID_INT,
        [LabelText("简体中文(zh)")]
        ZH,

        [LabelText("English(en)")]
        EN,
    }
}