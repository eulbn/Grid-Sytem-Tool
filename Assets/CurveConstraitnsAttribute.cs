using UnityEngine;
using UnityEditor;

namespace GridSpace
{ 
    internal class CurveConstraitnsAttribute: PropertyAttribute
    {
        internal CurveConstraitnsAttribute() {}

        static internal Keyframe[] SetCurveConstraints(AnimationCurve animationCurve)
        {
            Keyframe[] temKeyframe = animationCurve.keys;

            if (temKeyframe.Length >= 2)
            {
                temKeyframe[temKeyframe.Length - 1].value = 1;
                temKeyframe[temKeyframe.Length - 1].time = 1;

                temKeyframe[0].value = 0;
                temKeyframe[0].time = 0;
            }
            else
            {
                Keyframe[] newKeyframe = new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) };
                temKeyframe = newKeyframe;
            }

            return temKeyframe;
        }
    }

    [CustomPropertyDrawer(typeof(CurveConstraitnsAttribute))]
    internal class CCAttributeDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
            CurveConstraitnsAttribute constraints = attribute as CurveConstraitnsAttribute;
            if (property.propertyType == SerializedPropertyType.AnimationCurve)
            {
                AnimationCurve animationCurve = EditorGUI.CurveField(position, property.animationCurveValue);

                animationCurve.keys = CurveConstraitnsAttribute.SetCurveConstraints(animationCurve);
                property.animationCurveValue = animationCurve;

            }

        }

        

    }
}