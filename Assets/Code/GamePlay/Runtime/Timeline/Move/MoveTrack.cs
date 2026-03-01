using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace GamePlay.Runtime.YRTimeline
{
    /// <summary>
    /// 移动轨道
    /// </summary>
    [TrackColor(0/255f, 0/255f, 0/255f)]
    [TrackBindingType(typeof(GameObject))]
    [TrackClipType(typeof(MoveClip))]
    public class MoveTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            ScriptPlayable<MoveMixer> mixerPlayable = ScriptPlayable<MoveMixer>.Create(graph);
            mixerPlayable.SetInputCount(inputCount);

            return mixerPlayable;
        }

        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            return base.CreatePlayable(graph, gameObject, clip);
        }
    }
}