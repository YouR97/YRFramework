using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace YRFramework.Editor
{
    [Serializable]
    [CreateAssetMenu(fileName = "TextureImportSettings", menuName = "AssetsImportSettings/TextureImportSettings", order = 0)]
    public class TextureImportSettings : ScriptableObject
    {
        private static bool isDirtyLock = true;
        public List<TextureImportSetting> ListSetting;

        public static bool IsDirtyLock
        {
            get
            {
                bool r = isDirtyLock;
                isDirtyLock = false;
                return r;
            }
            set { isDirtyLock = value; }
        }
    }

    [Serializable]
    public class TextureImportSetting
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 前缀名
        /// </summary>
        public string PreName;
        /// <summary>
        /// 文件类型后缀
        /// </summary>
        public string EndDress;

        public TextureImporterType ImporterType;
        public SpriteImportMode SpriteMode;
        public TextureImporterShape ImporterShape = TextureImporterShape.Texture2D;
        public bool GeneratePhysicShape;
        public int MaxSize;
        public int StandaloneMaxSize;
        public bool MipMap;
        public bool ForceAlphaSetting = true;
        public TextureImporterAlphaSource Alpha;
        public TextureWrapMode WrapMode;

        public bool IsReadable;

        public FilterMode FilterMode = FilterMode.Bilinear;
        public TextureImporterNPOTScale NPOT = TextureImporterNPOTScale.ToNearest;

        public TextureImporterFormat FormatIos = TextureImporterFormat.ASTC_5x5;
        public TextureImporterFormat FormatAndroid = TextureImporterFormat.ASTC_5x5;
        public TextureImporterFormat FormatIosAlpha = TextureImporterFormat.ASTC_5x5;
        public TextureImporterFormat FormatAndroidAlpha = TextureImporterFormat.ASTC_5x5;
        public TextureImporterFormat FormatStandalone = TextureImporterFormat.RGB24;
        public TextureImporterFormat FormatStandaloneAlpha = TextureImporterFormat.RGBA32;


        public bool InspectorFoldOut;


        public TextureImporterFormat GetFormatIos(bool hasAlpha)
        {
            return hasAlpha ? FormatIosAlpha : FormatIos;
        }

        public TextureImporterFormat GetFormatAndroid(bool hasAlpha)
        {
            return hasAlpha ? FormatAndroidAlpha : FormatAndroid;
        }

        public TextureImporterFormat GetFormatStandalone(bool hasAlpha)
        {
            return hasAlpha ? FormatStandaloneAlpha : FormatStandalone;
        }
    }
}