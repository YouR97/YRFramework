using UnityEditor;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.Extension;

namespace YRFramework.Editor
{
    /// <summary>
    /// 纹理导入处理
    /// </summary>
    public partial class TextureImportProcessor : AssetPostprocessor
    {
        private static TextureImportSettings config;

        private void OnPreprocessTexture()
        {
            GetTextureFormatSettings();

            if (null == config)
            {
                Debug.LogError($"{nameof(TextureImportSettings)} is null");
                return;
            }

            TextureImporter textureImporter = assetImporter as TextureImporter;
            string fileName = GetFileName(textureImporter.assetPath); // 文件名

            if (fileName.EndsWith(TextureImportSettingConst.IgnorePostName))
                return;

            fileName = CheckName(fileName);

            TextureImportSetting settings = config.ListSetting.Find(set =>
                !set.PreName.YREquals(TextureImportSettingConst.NoneName) && fileName.StartsWith(set.PreName)
            );

            if (null == settings)
                return;

            int maxSize = settings.MaxSize;  // size
            textureImporter.textureType = settings.ImporterType;  // Type
            textureImporter.spriteImportMode = settings.SpriteMode; // Mode
            textureImporter.textureShape = settings.ImporterShape; // Shape

            #region Physics Shape
            TextureImporterSettings importerSettings = new TextureImporterSettings();
            textureImporter.ReadTextureSettings(importerSettings);
            importerSettings.spriteGenerateFallbackPhysicsShape = settings.GeneratePhysicShape;
            textureImporter.SetTextureSettings(importerSettings);
            #endregion

            #region Alpha
            if (settings.ForceAlphaSetting)
                textureImporter.alphaSource = settings.Alpha;
            else
            {
                if (textureImporter.DoesSourceTextureHaveAlpha())
                {
                    textureImporter.alphaSource = TextureImporterAlphaSource.FromInput;
                    textureImporter.alphaIsTransparency = true;
                }
                else
                    textureImporter.alphaSource = TextureImporterAlphaSource.None;
            }
            #endregion

            TextureImporterPlatformSettings settingDefault = textureImporter.GetPlatformTextureSettings("DefaultTexturePlatform");
            TextureImporterPlatformSettings settingAndroid = textureImporter.GetPlatformTextureSettings("Android");
            TextureImporterPlatformSettings settingIPhone = textureImporter.GetPlatformTextureSettings("iPhone");
            TextureImporterPlatformSettings settingStandalone = textureImporter.GetPlatformTextureSettings("Standalone");
            settingAndroid.overridden = true;
            settingIPhone.overridden = true;
            settingStandalone.overridden = false;

            settingIPhone.maxTextureSize = maxSize;
            settingAndroid.maxTextureSize = maxSize;
            settingStandalone.maxTextureSize = settings.StandaloneMaxSize;

            bool isHasAlpha = textureImporter.DoesSourceTextureHaveAlpha();
            settingDefault.textureCompression = TextureImporterCompression.Uncompressed; 
            settingAndroid.format = settings.GetFormatAndroid(isHasAlpha);
            settingIPhone.format = settings.GetFormatIos(isHasAlpha);
            settingStandalone.format = settings.GetFormatStandalone(isHasAlpha);

            textureImporter.SetPlatformTextureSettings(settingDefault);
            textureImporter.SetPlatformTextureSettings(settingAndroid);
            textureImporter.SetPlatformTextureSettings(settingIPhone);
            textureImporter.SetPlatformTextureSettings(settingStandalone);

            textureImporter.mipmapEnabled = settings.MipMap;
            textureImporter.wrapMode = settings.WrapMode;
            textureImporter.isReadable = settings.IsReadable;
            textureImporter.filterMode = settings.FilterMode;
            textureImporter.npotScale = settings.NPOT;
        }

        /// <summary>
        /// 获取TextureFormatSettings
        /// </summary>
        private void GetTextureFormatSettings()
        {
            if (null == config || TextureImportSettings.IsDirtyLock)
                config = FrameworkGameEnter.Asset.LoadAsset<TextureImportSettings>(TextureImportSettingConst.TextureImportSettingsPath);
        }
    }
}