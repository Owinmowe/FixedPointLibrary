using FixedPoint.SubTypes;
using UnityEditor;
using UnityEngine;

namespace FixedPoint.Editor 
{
    [CustomPropertyDrawer(typeof(Vector3Fp))]
    public class Vector3FpPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty xSerializedProperty = property.FindPropertyRelative("x").FindPropertyRelative("RawValue");
            float xValue = (float)new Fp(xSerializedProperty.longValue);

            SerializedProperty ySerializedProperty = property.FindPropertyRelative("y").FindPropertyRelative("RawValue");
            float yValue = (float)new Fp(ySerializedProperty.longValue);

            SerializedProperty zSerializedProperty = property.FindPropertyRelative("z").FindPropertyRelative("RawValue");
            float zValue = (float)new Fp(zSerializedProperty.longValue);

            Vector3 vector3 = EditorGUI.Vector3Field(position, label, new Vector3(xValue, yValue, zValue));

            xSerializedProperty.longValue = (long)(vector3.x * Fp.InternalOne);
            ySerializedProperty.longValue = (long)(vector3.y * Fp.InternalOne);
            zSerializedProperty.longValue = (long)(vector3.z * Fp.InternalOne);
        }
    }
}
