using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace GamePlay.Runtime.YRTimeline
{
    /// <summary>
    /// 受击轨道
    /// </summary>
    [TrackColor(0/255f, 0/255f, 0/255f)]
    [TrackBindingType(typeof(GameObject))]
    [TrackClipType(typeof(HitClip))]
    public class HitTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            // 这个是被HitMixer驱动的Playable，mixerPlayable.GetBehaviour() as HitMixer;就可以获得HitMixer
            ScriptPlayable<HitMixer> mixerPlayable = ScriptPlayable<HitMixer>.Create(graph);
            mixerPlayable.SetInputCount(inputCount);

            return mixerPlayable;
        }
    }
}