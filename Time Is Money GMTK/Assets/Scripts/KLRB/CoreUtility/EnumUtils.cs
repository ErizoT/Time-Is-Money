using System;

namespace KLRB.Utility
{


    public static class EnumUtils
    {
        public static T[] GetEnumValues<T>()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("GetValues<T> can only be called for types derived from System.Enum", "T");
            }

            return (T[])Enum.GetValues(typeof(T));
        }
    }
}

