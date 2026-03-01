#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.IO;
using System;
using UnityEditor;
using YRFramework.Runtime.Manager;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace YRFramework.Runtime.DataTable
{
    /// <summary>
    /// 配置管理器-编辑器工具
    /// </summary>
    public sealed partial class DataTableManager : YRFrameworkManager
    {
        /// <summary>
        /// 组名
        /// </summary>
        private const string GROUP_NAME = "配置管理器-编辑器工具";

        /// <summary>
        /// bat名
        /// </summary>
        private string BAT_NAME = "gen.bat";
        /// <summary>
        /// 配置工具目录
        /// </summary>
        private static readonly string configDirPath = $"{Application.dataPath}/../Config/";

        [BoxGroup(GROUP_NAME), Button("导出配置")]
        public void ExportConfig()
        {
            EditorUtility.DisplayProgressBar("配置工具", "正在导出配置", 0);
            try
            {
                DirectoryInfo dirInfo = new(configDirPath);
                StartProcess($"{dirInfo.FullName}{BAT_NAME}", string.Empty, configDirPath);
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debug.LogError($"异常，信息：{e}");
            }

            EditorUtility.ClearProgressBar();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("生成配置完成");
        }

        /// <summary>
        /// 启动进程
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="arg">参数</param>
        /// <param name="workDir">工作目录</param>
        /// <returns></returns>
        public bool StartProcess(string fileName, string arg, string workDir)
        {
            bool isFail = false;

            ProcessStartInfo startInfo = new()
            {
                FileName = fileName,
                Arguments = arg,
                CreateNoWindow = true,   // 不创建新窗口
                UseShellExecute = false, // 不使用系统外壳程序启动
                RedirectStandardError = true,  // 重定向标准输出
                RedirectStandardOutput = true, // 重定向错误输出
            };

            if (!string.IsNullOrWhiteSpace(workDir))
                startInfo.WorkingDirectory = workDir;

            try
            {
                using Process process = Process.Start(startInfo);

                process.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                        Debug.Log(e.Data);
                };

                process.ErrorDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                        Debug.Log(e.Data);
                };

                process.EnableRaisingEvents = true;
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit(); // 等待批处理执行完成

                string name = null;
                string arguments = null;

                if (process.ExitCode != 0 && !isFail)
                {
                    isFail = true;
                    name = process.StartInfo.FileName;
                    arguments = process.StartInfo.Arguments;
                }

                if (isFail)
                    throw new Exception($"ExitCode:{process.ExitCode}]\n启动进程失败，FileName=[{name}]\nArg=[{arguments}\n");
            }
            catch (Exception e)
            {
                Debug.LogError($"错误: {e.Message}");
            }

            return !isFail;
        }
    }
}
#endif