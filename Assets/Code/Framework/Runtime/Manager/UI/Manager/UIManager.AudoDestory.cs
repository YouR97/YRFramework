using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI管理器（自动销毁）
    /// </summary>
    public sealed partial class UIManager
    {
        /// <summary>
        /// 最大UI数量，达到该数量清理UI
        /// </summary>
        [LabelText("最大缓存UI数量")]
        [SerializeField]
        private int maxUICount = 10;
        /// <summary>
        /// 检查UI使用时间间隔，超过该时间的UI自动销毁
        /// </summary>
        [LabelText("自动销毁时间间隔(秒)")]
        [SerializeField]
        private float autoDestroyInterval = 60f;

        /// <summary>
        /// 上次销毁时间
        /// </summary>
        private float preDestoryTimer;
        /// <summary>
        /// 自动销毁忽略的UI集合
        /// </summary>
        private HashSet<string> setIgnoreUI;

        /// <summary>
        /// 自动销毁初始化
        /// </summary>
        private void AutoDestoryInit()
        {
            preDestoryTimer = Time.realtimeSinceStartup;
            setIgnoreUI = new HashSet<string>();
        }

        /// <summary>
        /// 自动销毁释放
        /// </summary>
        private void AutoDestoryRelease()
        {
            if (null != setIgnoreUI)
            {
                setIgnoreUI.Clear();
                setIgnoreUI = null;
            }
        }

        /// <summary>
        /// 检查自动销毁时间
        /// </summary>
        /// <param name="realtimeSinceStartup"></param>
        private void CheckAutoDestoryByTime(float realtimeSinceStartup)
        {
            if (realtimeSinceStartup - preDestoryTimer > autoDestroyInterval)
            {
                preDestoryTimer = realtimeSinceStartup;
                AutoDestoryByTime();
            }
        }

        /// <summary>
        /// 根据时间自动销毁
        /// </summary>
        private void AutoDestoryByTime()
        {
            setIgnoreUI.Clear();

            foreach (UIEntity uiEntity in dicAllUI.Values)
            {
                if (E_UIState.Close != uiEntity.CurState)
                {
                    setIgnoreUI.Add(uiEntity.Info.Type);
                    continue;
                }
                
                float timeSpan = Time.realtimeSinceStartup - uiEntity.preCloseTime;
                if (timeSpan < autoDestroyInterval)
                    setIgnoreUI.Add(uiEntity.Info.Type);
            }

            if (setIgnoreUI.Count < dicAllUI.Count)
                DestroyAllUI(setIgnoreUI);
        }

        /// <summary>
        /// 根据数量自动销毁
        /// </summary>
        private void AutoDestoryByCount()
        {
            float dateTime = float.MaxValue;
            UIEntity destroyUIEntity = null;

            foreach (UIEntity uiEntity in dicAllUI.Values)
            {
                if (E_UIState.Close != uiEntity.CurState)
                    continue;

                if (uiEntity.preCloseTime < dateTime)
                {
                    destroyUIEntity = uiEntity;
                    dateTime = destroyUIEntity.preCloseTime;
                }
            }

            if (null != destroyUIEntity)
                DestroyUI(destroyUIEntity.Info.Type);
        }
    }
}