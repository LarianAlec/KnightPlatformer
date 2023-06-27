using UnityEngine;

namespace KnightPlatformer.Utils
{
    public static class GameObjectExtensions
    {
        // Check: Is this gameObject located in layer?
        public static bool IsInLayer(this GameObject gameObj, LayerMask layer)
        {
            // Побитовый сдвиг
            // 0001 go.layer
            // 0110 layer
            // result:
            // 0111
            return layer == (layer | 1 << gameObj.layer);
        }
    }
}
