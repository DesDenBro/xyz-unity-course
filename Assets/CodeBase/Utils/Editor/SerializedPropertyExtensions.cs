using System;
using UnityEditor;

namespace PixelCrew.Utils.Editor
{
    public static class SerializedPropertyExtensions
    {
        public static bool GetEnum<TEnumType>(this SerializedProperty prop, out TEnumType enumType)
            where TEnumType : Enum
        {
            enumType = default;
            var names = prop.enumNames;

            if (names == null || names.Length == 0) return false;

            var enumName = names[prop.enumValueIndex];
            enumType = (TEnumType)Enum.Parse(typeof(TEnumType), enumName);
            return true;
        }
    }
}
