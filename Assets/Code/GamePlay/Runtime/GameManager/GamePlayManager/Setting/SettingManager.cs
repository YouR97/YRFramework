using UnityEngine;
using YRFramework.Runtime.Manager;

namespace GamePlay.Runtime.Setting
{
    /// <summary>
    /// 设置管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRFramework/SettingManager")]
    public sealed partial class SettingManager : YRFrameworkManager
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.Setting;
        #endregion

        /// <summary>
        /// 加载设置
        /// </summary>
        public void LoadSetting()
        {
            LoadAudioSetting();
            LoadPictureSetting();
        }

        /// <summary>
        /// 应用所有设置
        /// </summary>
        public void ApplyAllSetting()
        {
            ApplyAllAudioSetting();
            ApplyAllPictureSetting();
        }

        /// <summary>
        /// 保存所有设置
        /// </summary>
        public void SaveAllSetting()
        {
            SaveAudioSetting(false);
            SavePictureSetting(false);

            Game.Storage.Save();
        }
    }
}