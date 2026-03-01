using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace YRFramework.Editor
{
    /// <summary>
    /// 纹理导入设置Inspector面板
    /// </summary>
    [CustomEditor(typeof(TextureImportSettings))]
    public class TextureFormatSettingInspector : UnityEditor.Editor
    {
        /// <summary>
        /// 纹理导入设置
        /// </summary>
        private TextureImportSettings targetSettings;

        public void OnEnable()
        {
            targetSettings = target as TextureImportSettings;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("排除后缀为", TextureImportSettingConst.IgnorePostName);

            for (int i = 0; i < targetSettings.ListSetting.Count; i++)
            {
                TextureImportSetting setting = targetSettings.ListSetting[i];
                using (new EditorGUILayout.HorizontalScope())
                {
                    setting.Name = EditorGUILayout.TextField("种类", setting.Name);
                    if (GUILayout.Button("↑") && i > 0)
                    {
                        targetSettings.ListSetting[i] = targetSettings.ListSetting[i - 1];
                        targetSettings.ListSetting[i - 1] = setting;
                    }

                    if (GUILayout.Button("↓") && i < targetSettings.ListSetting.Count - 1)
                    {
                        targetSettings.ListSetting[i] = targetSettings.ListSetting[i + 1];
                        targetSettings.ListSetting[i + 1] = setting;
                    }

                    if (GUILayout.Button("删"))
                    {
                        for (var j = i + 1; j < targetSettings.ListSetting.Count; j++)
                        {
                            targetSettings.ListSetting[j - 1] = targetSettings.ListSetting[j];
                        }
                        targetSettings.ListSetting.RemoveAt(targetSettings.ListSetting.Count - 1);
                        setting = targetSettings.ListSetting[i];
                    }
                }

                setting.InspectorFoldOut = EditorGUILayout.Foldout(setting.InspectorFoldOut, "");
                if (setting.InspectorFoldOut)
                {
                    setting.PreName = UtilityEditor.GuiLayout.EditorGUILayoutPopup("前缀", setting.PreName, TextureImportSettingConst.PreNames);
                    setting.EndDress = UtilityEditor.GuiLayout.EditorGUILayoutPopup("文件类型", setting.EndDress, TextureImportSettingConst.EndDress);

                    setting.ImporterType = (TextureImporterType)EditorGUILayout.EnumPopup("Texture Type", setting.ImporterType);
                    setting.SpriteMode = (SpriteImportMode)EditorGUILayout.EnumPopup("Sprite Mode", setting.SpriteMode);
                    setting.ImporterShape = (TextureImporterShape)EditorGUILayout.EnumPopup("Texture Shape", setting.ImporterShape);
                    setting.GeneratePhysicShape = EditorGUILayout.Toggle("Generate Physic Shape", setting.GeneratePhysicShape);
                    setting.MaxSize = int.Parse(UtilityEditor.GuiLayout.EditorGUILayoutPopup("Max Size", setting.MaxSize.ToString(),
                        TextureImportSettingConst.TextureMaxSizes));
                    setting.StandaloneMaxSize = int.Parse(UtilityEditor.GuiLayout.EditorGUILayoutPopup("standalone max size",
                        setting.StandaloneMaxSize.ToString(), TextureImportSettingConst.TextureMaxSizes));
                    setting.MipMap = EditorGUILayout.Toggle("Mip Maps", setting.MipMap);
                    setting.ForceAlphaSetting = EditorGUILayout.Toggle("force alpha", setting.ForceAlphaSetting);
                    setting.Alpha = (TextureImporterAlphaSource)EditorGUILayout.EnumPopup("alpha source", setting.Alpha);
                    setting.WrapMode = (TextureWrapMode)EditorGUILayout.EnumPopup("wrap mode", setting.WrapMode);
                    setting.IsReadable = EditorGUILayout.Toggle("is readable", setting.IsReadable);
                    setting.FilterMode = (FilterMode)EditorGUILayout.EnumPopup("filter mode", setting.FilterMode);
                    setting.NPOT = (TextureImporterNPOTScale)EditorGUILayout.EnumPopup("Non-Power of 2", setting.NPOT);

                    setting.FormatIos = (TextureImporterFormat)EditorGUILayout.EnumPopup("IOS Format", setting.FormatIos);
                    setting.FormatAndroid = (TextureImporterFormat)EditorGUILayout.EnumPopup("Android Format", setting.FormatAndroid);
                    setting.FormatIosAlpha = (TextureImporterFormat)EditorGUILayout.EnumPopup("IOS Alpha Format", setting.FormatIosAlpha);
                    setting.FormatAndroidAlpha = (TextureImporterFormat)EditorGUILayout.EnumPopup("Android Alpha Format", setting.FormatAndroidAlpha);
                    setting.FormatStandalone = (TextureImporterFormat)EditorGUILayout.EnumPopup("Standalone Format", setting.FormatStandalone);
                    setting.FormatStandaloneAlpha = (TextureImporterFormat)EditorGUILayout.EnumPopup("Standalone Alpha Format", setting.FormatStandaloneAlpha);
                }

                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
            }

            if (GUILayout.Button("+")) targetSettings.ListSetting.Add(new TextureImportSetting { Name = "未知" });

            TextureImportSettings.IsDirtyLock = true;
            EditorUtility.SetDirty(targetSettings);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("保存设置"))
                {
                }

                if (GUILayout.Button("重新导入所有纹理"))
                {
                }
            }
        }
    }
}