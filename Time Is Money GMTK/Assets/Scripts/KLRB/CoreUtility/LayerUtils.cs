using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KLRB.Utility
{

    public static class LayerUtils
    {
        public static bool MaskContainsLayer(LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        public static void SetGameLayerRecursive(GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            foreach (Transform child in gameObject.transform)
            {
                SetGameLayerRecursive(child.gameObject, layer);
            }
        }
    }

}