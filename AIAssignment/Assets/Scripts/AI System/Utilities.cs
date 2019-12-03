using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System
{
    class Utilities
    {
        public static float Normalise (float value, float min, float max)
        {
            return ( value - min ) / ( max - min );
        }

        public static float GetDistribution (float value, float min, float max)
        {
            float normalisedValue = Normalise (value, min, max);

            return Mathf.Clamp (normalisedValue, 0.0f, 1.0f);
        }
    }
}
