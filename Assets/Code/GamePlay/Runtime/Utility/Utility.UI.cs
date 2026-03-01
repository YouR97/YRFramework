using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GamePlay.Runtime
{
    /// <summary>
    /// 通用函数
    /// </summary>
    public static partial class Utility
    {
        /// <summary>
        /// UI相关
        /// </summary>
        public static class UI
        {
            /// <summary>
            /// 绑定按钮
            /// </summary>
            /// <param name="button">按钮</param>
            /// <param name="action">回调</param>
            /// <returns></returns>
            public static void BindButton(Button button, UnityAction action)
            {
                if (null == button)
                {
                    Debug.LogError("绑定UI按钮为空");
                    return;
                }

                button.onClick.AddListener(() =>
                {
                    action?.Invoke();
                });
            }
        }
    }
}