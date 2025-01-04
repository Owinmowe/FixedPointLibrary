using UnityEditor;
using UnityEngine;

namespace FixedPoint.Editor
{
    [CustomPropertyDrawer(typeof(Fp))]
    public class FpPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty rawValueSerializedProperty = property.FindPropertyRelative("RawValue");
            float value = EditorGUILayout.FloatField(label, (float)new Fp(rawValueSerializedProperty.longValue));
            rawValueSerializedProperty.longValue = (long)(value * Fp.InternalOne);
        }
    }
}