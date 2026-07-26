using UnityEngine;

namespace KLRB.Utility
{
    public static class BoundsUtils
    {
        public static void GetWorldCorners(Transform t,Bounds bounds, ref Vector3[] cornerArray)
        {
            int i = 0;
            for (int x = -1; x <= 1; x += 2)
            for (int y = -1; y <= 1; y += 2)
            for (int z = -1; z <= 1; z += 2)
            {
                Vector3 localCorner = bounds.center + Vector3.Scale(bounds.extents, new Vector3(x, y, z));
                cornerArray[i++] = t.TransformPoint(localCorner);
            }
        }
    }
}