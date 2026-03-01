using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using UnityEngine;
using UnityEngine.Playables;

namespace GamePlay.Runtime.YRTimeline
{
    /// <summary>
    /// 移动Clip逻辑
    /// </summary>
    public class MoveBehaviour : PlayableBehaviour
    {
        public Transform TsTarget;

        private Transform tsPlayer;
        private float moveSpeed = 10f;
        
        /// <summary>
        /// Clip开始执行
        /// </summary>
        /// <param name="playable"></param>
        /// <param name="info"></param>
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            Debug.Log($"移动OnBehaviourPlay");
        }

        /// <summary>
        /// Clip暂停和退出执行
        /// </summary>
        /// <param name="playable"></param>
        /// <param name="info"></param>
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            Debug.Log($"移动OnBehaviourPause");
        }

        public override void OnPlayableCreate(Playable playable)
        {
            Debug.Log($"移动OnPlayableCreate");
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            Debug.Log($"移动OnPlayableDestroy");
        }
        
        public override void OnGraphStart(Playable playable)
        {
            Debug.Log($"移动OnGraphStart");
        }

        public override void OnGraphStop(Playable playable)
        {
            Debug.Log($"移动OnGraphStop");
        }

        public override void PrepareData(Playable playable, FrameData info)
        {
            Debug.Log($"移动PrepareData");
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            Debug.Log($"移动PrepareFrame");
        }
        
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (null == tsPlayer)
            {
                if (playerData is not GameObject goPlayer)
                {
                    Debug.LogError("(移动轨道)空绑定");
                    return;
                }
                
                tsPlayer = goPlayer.transform;
            }

            tsPlayer.position += new Vector3(moveSpeed * info.deltaTime, 0f, 0f);
            Debug.Log($"移动PrepareFrame，{playerData}");
        }
    }
}