using UnityEngine;

namespace YRFramework.Runtime
{
    /// <summary>
    /// 常量定义
    /// </summary>
    public partial class YRConsts
    {
        /// <summary>
        /// Layer定义
        /// </summary>
        public class Layer
        {
            /// <summary>
            /// 默认Layer名
            /// </summary>
            public const string DEFAULT_NAME = "Default";
            /// <summary>
            /// UI Layer名
            /// </summary>
            public const string UI_NAME = "UI";

            /// <summary>
            /// 默认Layer id
            /// </summary>
            public static readonly int Default = LayerMask.NameToLayer(DEFAULT_NAME);
            /// <summary>
            /// UILayer id
            /// </summary>
            public static readonly int UI = LayerMask.NameToLayer(UI_NAME);
        }
    }
}