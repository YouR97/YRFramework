using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GamePlay.Runtime
{
    /// <summary>
    /// 通用函数
    /// </summary>
    public static partial class Utility
    {
        /// <summary>
        /// 截图相关功能
        /// </summary>
        public static class ScreenShot
        {
            private static RenderTexture screenTexture;
            private static RenderTexture screenTexture2;

            /// <summary>
            /// 截图
            /// </summary>
            public static async UniTask CaptureScreenshot()
            {
                await UniTask.NextFrame();
                
                if (!screenTexture)
                    screenTexture = new RenderTexture(Screen.width, Screen.height, 24);

                if (!screenTexture2)
                    screenTexture2 = new RenderTexture(Screen.width, Screen.height, 24);

                // 先渲图
                Camera camera = Game.Camera.UICamera;
                camera.targetTexture = screenTexture;
                camera.Render();
                camera.targetTexture = null;

                camera = Game.Camera.MainCamera;
                camera.targetTexture = screenTexture;
                camera.Render();
                camera.targetTexture = null;
            }
            
            public static Texture2D GetScreenTexture2D()
            {
                Texture2D texture2D = new(screenTexture.width, screenTexture.height);
                RenderTexture.active = screenTexture;
                texture2D.ReadPixels(new Rect(0f, 0f, screenTexture.width, screenTexture.height), 0, 0);
                texture2D.Apply();
                RenderTexture.active = null;

                return texture2D;
            }
            
            public static async UniTask<Texture2D> GetScreenTexture2DByGaussianBlur()
            {
                #region 高斯模糊处理
                Material material = await Game.Asset.LoadAssetAsync<Material>("UI_GaussianBlur");

                // 需要用一个缓存的rt,不然移动平台会出现方块
                Graphics.Blit(screenTexture, screenTexture2, material);
                Graphics.Blit(screenTexture2, screenTexture);
                #endregion
                
                Texture2D texture2D = new(screenTexture.width, screenTexture.height);
                RenderTexture.active = screenTexture;
                texture2D.ReadPixels(new Rect(0f, 0f, screenTexture.width, screenTexture.height), 0, 0);
                texture2D.Apply();
                RenderTexture.active = null;

                return texture2D;
            }
        }
    }
}