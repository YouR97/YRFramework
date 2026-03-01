using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace YRFramework.Editor
{
    /// <summary>
    /// 编辑器实用函数集
    /// </summary>
    public partial class UtilityEditor
    {
        public static class GuiLayout
        {
            /// <summary>
            /// 弹出框
            /// </summary>
            /// <param name="label"></param>
            /// <param name="selected"></param>
            /// <param name="displayOption"></param>
            /// <returns></returns>
            public static string EditorGUILayoutPopup(string label, string selected, string[] displayOption)
            {
                try
                {
                    int index = EditorGUILayout.Popup(label, FindIndex(displayOption, selected), displayOption);

                    return displayOption[index >= 0 ? index : 0];
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }

                return string.Empty;
            }

            /// <summary>
            /// 查找索引
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="readList"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            private static int FindIndex<T>(IReadOnlyList<T> readList, T value)
            {
                if (null == readList)
                    return -1;

                for (int i = 0; i < readList.Count; ++i)
                {
                    if (readList[i].Equals(value))
                        return i;
                }

                return -1;
            }
        }
    }
}