#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.IO;
using UnityEditor;
using UnityEngine;
using YRFramework.Runtime.Manager;

namespace YRFramework.Runtime.Storage
{
    /// <summary>
    /// 本地缓存管理器-编辑器
    /// </summary>
    public sealed partial class StorageManager : YRFrameworkManager, IInit, IUpdate
    {
        /// <summary>
        /// 组名
        /// </summary>
        private const string GROUP_NAME = "本地缓存管理器-编辑器工具";

        [SerializeField]
        [BoxGroup(GROUP_NAME), LabelText("Data脚本模板"), ShowInInspector]
        private TextAsset dataScripTemplate;

        [SerializeField]
        [BoxGroup(GROUP_NAME), LabelText("工厂脚本模板"), ShowInInspector]
        private TextAsset factoryScripTemplate;

        [SerializeField]
        [BoxGroup(GROUP_NAME), LabelText("创建Storage脚本路径"), ShowInInspector, FolderPath]
        private string createPath;

        [BoxGroup(GROUP_NAME), LabelText("Storage脚本名"), ShowInInspector]
        private string scriptName;

        [BoxGroup(GROUP_NAME), Button("创建Storage脚本")]
        private void CreateStorageScript()
        {
            if (null == dataScripTemplate)
            {
                EditorUtility.DisplayDialog(GROUP_NAME, "创建Storage脚本失败，空的Data脚本模板", "确定");
                return;
            }

            if (null == factoryScripTemplate)
            {
                EditorUtility.DisplayDialog(GROUP_NAME, "创建Storage脚本失败，空的Factory脚本模板", "确定");
                return;
            }

            if (string.IsNullOrEmpty(createPath))
            {
                EditorUtility.DisplayDialog(GROUP_NAME, "创建Storage脚本失败，未填写路径", "确定");
                return;
            }

            if (string.IsNullOrEmpty(scriptName))
            {
                EditorUtility.DisplayDialog(GROUP_NAME, "创建Storage脚本失败，未填写脚本名", "确定");
                return;
            }

            string path = $"{createPath}/Storage_{scriptName}Data.cs";
            string dataTemplate = dataScripTemplate.text;
            dataTemplate = dataTemplate.Replace("{ClassName}", scriptName);
            File.WriteAllText(path, dataTemplate);

            path = $"{createPath}/Storage_{scriptName}Factory.cs";
            string factoryTemplate = factoryScripTemplate.text;
            factoryTemplate = factoryTemplate.Replace("{ClassName}", scriptName);
            File.WriteAllText(path, factoryTemplate);

            Debug.Log($"[{nameof(StorageManager)}]:创建脚本成功");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
#endif