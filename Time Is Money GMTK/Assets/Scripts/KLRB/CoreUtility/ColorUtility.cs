using UnityEngine;

namespace KLRB.Utility
{
    public static class ColorUtility
    {
        public static Color WithAlpha(this Color color, float alpha) {
            color.a = alpha;
            return color;
        }

    }
}