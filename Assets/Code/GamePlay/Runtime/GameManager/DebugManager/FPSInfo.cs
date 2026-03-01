using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace GamePlay.Runtime
{
    [Serializable]
    public sealed class FPSInfo
    {
        [LabelText("颜色")]
        public Color color;
        [LabelText("帧率")]
        public int frameRate;
    }
}