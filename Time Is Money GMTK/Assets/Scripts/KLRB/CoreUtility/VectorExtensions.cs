using UnityEngine;

namespace KLRB.Utility
{
    public static class VectorExtensions
    {
        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null) {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }
        
        public static Vector2 With(this Vector2 vector, float? x = null, float? y = null) {
            return new Vector2(x ?? vector.x, y ?? vector.y);
        }
        
        public static Vector2 XZ(this Vector3 vector) {
            return new Vector2(vector.x, vector.z);
        }
        public static Vector2 XY(this Vector3 vector) {
            return new Vector2(vector.x, vector.y);
        }
        public static Vector2 YZ(this Vector3 vector) {
            return new Vector2(vector.y, vector.z);
        }
        
        public static Vector3 XZ(this Vector2 vector) {
            return new Vector3(vector.x,0, vector.y);
        }
        
        public static Vector3 XY(this Vector2 vector) {
            return new Vector3(vector.x, vector.y, 0);
        }
        
        public static Vector3 YZ(this Vector2 vector) {
            return new Vector3(0, vector.x, vector.y);
        }
        
        public static Vector3 Abs(this Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        public static bool IsValidDirection(this Vector3 direction)
        {
            if (direction.sqrMagnitude > 0.0001f && direction == direction)
            {
                return true;
            }
            return false;
        }
        
        
        public static bool IsValidDirection(this Vector2 direction)
        {
            if (direction.sqrMagnitude > 0.0001f && direction == direction)
            {
                return true;
            }
            return false;
        }

    }
}