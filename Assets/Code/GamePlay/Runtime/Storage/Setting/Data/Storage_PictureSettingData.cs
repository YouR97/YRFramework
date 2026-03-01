using YRFramework.Runtime.Storage;

namespace GamePlay.Runtime.Storage
{
    /// <summary>
    /// 本地缓存-图像设置数据
    /// </summary>
    public sealed class Storage_PictureSettingData : StorageDataBase
    {
        #region 私有字段
        /// <summary>
        /// 帧率
        /// </summary>
        private int frameRate;
        #endregion

        #region 属性
        /// <summary>
        /// 帧率
        /// </summary>
        public int FrameRate
        {
            get { return frameRate; }
            set { SetField(value, ref frameRate); }
        }
        #endregion

        internal Storage_PictureSettingData()
        {
            frameRate = 60;
        }
    }
}