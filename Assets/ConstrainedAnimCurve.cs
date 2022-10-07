using UnityEngine;
using System;

namespace GridSpace
{

    [Serializable]
    public class ConstrainedAnimCurve
    {
        [CurveConstraitns]
        [SerializeField] private AnimationCurve Curve;

        public AnimationCurve curve {

            get 
            { 
                return Curve; 
            }
            set 
            {
                Curve = value;
                Curve.keys = CurveConstraitnsAttribute.SetCurveConstraints(Curve);
            } 
        
        }
    }
}