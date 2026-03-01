namespace YRFramework.Runtime.Utility
{
    /// <summary>
    /// 通用函数
    /// </summary>
    public static partial class YRUtility
    {
        /// <summary>
        /// 游戏通用函数
        /// </summary>
        public static class Game
        {
            /// <summary>
            /// 退出游戏
            /// </summary>
            public static void ExitGame()
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
    }
}