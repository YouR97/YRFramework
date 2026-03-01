using UnityEngine.UI;

namespace YRFramework.Runtime.Tool.UI
{
    public class EmptyForRaycast : MaskableGraphic
    {
        protected EmptyForRaycast()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}