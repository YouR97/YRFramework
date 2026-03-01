using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Runtime.Loading
{
    /// <summary>
    /// 加载表现类
    /// </summary>
    public sealed class LoadingDisplay
    {
        /// <summary>
        /// 进度变化的插值时间
        /// </summary>
        private float changeProgressTime;
        /// <summary>
        /// 上次获取时间
        /// </summary>
        private float preGetTime;
        /// <summary>
        /// 目标进度
        /// </summary>
        private float targerProgress;
        /// <summary>
        /// 开始进度
        /// </summary>
        private float beginProgress;

        /// <summary>
        /// 表现进度
        /// </summary>
        public float ShowProgress { get; private set; }

        public void SetInfo(float changeProgressTime)
        {
            this.changeProgressTime = changeProgressTime;

            preGetTime = Time.realtimeSinceStartup;
            beginProgress = 0f;
            targerProgress = 0f;
            ShowProgress = 0f;
        }

        public void Update(float realProgress)
        {
            float realtimeSinceStartup = Time.realtimeSinceStartup;

            if (0f >= changeProgressTime) // 没有插值时间
            {
                targerProgress = realProgress;
                preGetTime = realtimeSinceStartup;
                ShowProgress = targerProgress;

                return;
            }

            if (!Mathf.Approximately(targerProgress, realProgress))
            {
                preGetTime = realtimeSinceStartup;
                beginProgress = ShowProgress;
                targerProgress = realProgress;
            }

            float timer = realtimeSinceStartup - preGetTime;
            ShowProgress = Mathf.Lerp(beginProgress, targerProgress, Mathf.Clamp01(timer / changeProgressTime));
        }
    }
}