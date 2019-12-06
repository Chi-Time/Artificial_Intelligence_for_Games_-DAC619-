using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System
{
    class UtilityCurves : MonoBehaviour
    {
        public static AnimationCurve Step { get; private set; }
        public static AnimationCurve Linear { get; private set; }
        public static AnimationCurve EaseOut { get; private set; }
        public static AnimationCurve Logistic { get; private set; }
        public static AnimationCurve Exponential { get; private set; }
        public static AnimationCurve ReverseExponential { get; private set; }
        
        [SerializeField] private AnimationCurve _Step = null;
        [SerializeField] private AnimationCurve _Linear = null;
        [SerializeField] private AnimationCurve _EaseOut = null;
        [SerializeField] private AnimationCurve _Logistic = null;
        [SerializeField] private AnimationCurve _Exponential = null;
        [SerializeField] private AnimationCurve _ReverseExponential = null;
        

        private void Awake ()
        {
            SetStaticFields ();
        }

        private void SetStaticFields ()
        {
            // Set the static fields with the curves made in the inspector.
            Step = _Step;
            Linear = _Linear;
            EaseOut = _EaseOut;
            Logistic = _Logistic;
            Exponential = _Exponential;
            ReverseExponential = _ReverseExponential;

            // Set the fields back to null so that we're not holding onto the inspector vars anymore.
            _Step = null;
            _Linear = null;
            _EaseOut = null;
            _Logistic = null;
            _Exponential = null;
            _ReverseExponential = null;
        }
    }
}
