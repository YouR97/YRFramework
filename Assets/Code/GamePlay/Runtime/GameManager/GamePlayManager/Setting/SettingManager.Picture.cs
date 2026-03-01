using UnityEngine;
using YRFramework.Runtime.Manager;
using GamePlay.Runtime.Storage;
using YRFramework.Runtime.Utility;
using YRFramework.Runtime.Audio;
using UnityEngine.Playables;

namespace GamePlay.Runtime.Setting
{
    /// <summary>
    /// 设置管理器-图像设置
    /// </summary>
    public sealed partial class SettingManager : YRFrameworkManager
    {
        #region 属性
        /// <summary>
        /// 图像设置
        /// </summary>
        private Storage_PictureSettingData pictureSettingData;

        /// <summary>
        /// 帧率
        /// </summary>
        public int FrameRate { get; set; }
        #endregion

        /// <summary>
        /// 加载图像设置
        /// </summary>
        private void LoadPictureSetting()
        {
            pictureSettingData = Storage_PictureSettingFactory.GetData();
            if (null == pictureSettingData)
            {
                Debug.LogError($"[{nameof(SettingManager)}]:图像设置加载错误，null");
                YRUtility.Game.ExitGame();
            }

            RestorePictureSetting();
        }

        #region API
        /// <summary>
        /// 保存图像设置
        /// </summary>
        public void SavePictureSetting(bool isImmediate = true)
        {
            pictureSettingData.FrameRate = FrameRate;

            if (isImmediate)
                Game.Storage.Save();
        }

        /// <summary>
        /// 还原图像设置
        /// </summary>
        public void RestorePictureSetting()
        {
            FrameRate = pictureSettingData.FrameRate;
        }

        /// <summary>
        /// 应用所有图像设置
        /// </summary>
        public void ApplyAllPictureSetting()
        {
            Application.targetFrameRate = FrameRate;
        }
        #endregion
    }
}