using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace YRFramework.Editor
{
    /// <summary>
    /// 编辑器实用函数集
    /// </summary>
    public partial class UtilityEditor
    {
        /// <summary>
        /// 进程实用函数
        /// </summary>
        public static class ProcessEditor
        {
            /// <summary>
            /// 启动进程
            /// </summary>
            /// <param name="fileName">文件名</param>
            /// <param name="arg">参数</param>
            /// <param name="workDir">工作目录</param>
            /// <returns></returns>
            public static bool StartProcess(string fileName, string arg, string workDir)
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
}