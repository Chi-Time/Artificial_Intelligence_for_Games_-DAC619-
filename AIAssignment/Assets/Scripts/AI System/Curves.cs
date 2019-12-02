using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System
{
    class Curves : MonoBehaviour
    {
        public static Curves Instance { get; private set; }

        private void Awake ()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy (this);
        }

        public AnimationCurve StepCurve = new AnimationCurve ();
        public AnimationCurve EaseOutCurve = new AnimationCurve ();
        public AnimationCurve ExponentialCurve = new AnimationCurve ();
        public AnimationCurve LogisticCurve = new AnimationCurve ();
    }
}
