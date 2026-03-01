using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace GamePlay.Runtime.YRTimeline
{
    /// <summary>
    /// 移动轨道
    /// </summary>
    public class MoveClip : PlayableAsset
    {
        private MoveBehaviour template;
        
        public ExposedReference<Transform> TsTarget;

        /// <summary>
        /// 混合类型
        /// </summary>
        public ClipCaps clipCaps
        {
            get { return ClipCaps.Blending; }
        }

        /// <summary>
        /// Clip相当于戏的剧本，需要创建一个Playable作为戏来演出
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<MoveBehaviour> playable = ScriptPlayable<MoveBehaviour>.Create(graph, template);
            MoveBehaviour behaviour = playable.GetBehaviour();
            behaviour.TsTarget = TsTarget.Resolve(graph.GetResolver());

            return playable;
        }
    }
}