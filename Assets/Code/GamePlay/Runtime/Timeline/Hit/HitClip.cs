using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace GamePlay.Runtime.YRTimeline
{
    public class HitClip : PlayableAsset
    {
        private HitBehaviour template;

        public ExposedReference<GameObject> exampleValue;

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
            ScriptPlayable<HitBehaviour> playable = ScriptPlayable<HitBehaviour>.Create(graph, template);
            HitBehaviour hitBehaviour = playable.GetBehaviour();
            hitBehaviour.exampleValue = exampleValue.Resolve(graph.GetResolver());

            return playable;
        }
    }
}