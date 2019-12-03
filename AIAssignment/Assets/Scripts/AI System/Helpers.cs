using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System
{
    public enum ColorTypes { Red, Yellow, Blue, Cyan, Green, Pink, Purple, White, Black, Orange }

    class Helpers
    {
        /// <summary>Normalise a given value within the defined range.</summary>
        /// <param name="value">The value to normalise.</param>
        /// <param name="min">The minimum extent of the range.</param>
        /// <param name="max">The maximum extent of the range.</param>
        /// <returns>A nornalised value between the given range.</returns>
        public static float Normalise (float value, float min, float max)
        {
            return ( value - min ) / ( max - min );
        }

        /// <summary>Normalise a given value within the given range and then clamp it to either end.</summary>
        /// <param name="value">The value to normalise and clamp.</param>
        /// <param name="min">The minimum extent of the range.</param>
        /// <param name="max">The maximum extent of the range.</param>
        /// <returns>A normalised and clamped float between the given range.</returns>
        public static float GetDistribution (float value, float min, float max)
        {
            float normalisedValue = Normalise (value, min, max);

            return Mathf.Clamp (normalisedValue, 0.0f, 1.0f);
        }
    }

    public static class Log
    {
        public static void Desirability (float desirablity, AI agent)
        {
            Debug.Log ($"<Color={ColorTypes.White}>Desirability:</color> <Color={ColorTypes.Cyan}>{desirablity}</color> | {agent}");
        }

        public static void EnteredState (string stateName, AI agent)
        {
            State (stateName, "Entered", ColorTypes.Yellow, agent);
        }

        public static void ProcessingState (string stateName, AI agent)
        {
            State (stateName, "Processing", ColorTypes.Green, agent);
        }

        public static void ExitedState (string stateName, AI agent)
        {
            State (stateName, "Exited", ColorTypes.Red, agent);
        }

        public static void State (string stateName, string stateValue, ColorTypes color, AI agent)
        {
            Debug.Log ($"<Color={ColorTypes.White}>State_{stateName}:</color> <color={color.ToString ()}> {stateValue} </color>| {agent}");
        }
    }
}
