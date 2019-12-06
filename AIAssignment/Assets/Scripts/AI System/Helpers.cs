using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System
{
    /// <summary>Debug color types for the Unity console.</summary>
    public enum ColorTypes { Red, Yellow, Blue, Cyan, Green, Pink, Purple, White, Black, Orange }

    /// <summary>Helper methods for math.</summary>
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

        /// <summary>Returns true if a position is near another within a minimum specified threshold.</summary>
        /// <param name="a">The first parameter to test for.</param>
        /// <param name="b">The second parameter to test for.</param>
        /// <param name="distance">The minimum distance between the two before we can consider them "near".</param>
        /// <returns>True if the two positions are near each other.</returns>
        public static bool IsNearPosition (Vector3 a, Vector3 b, float distance)
        {
            if (Vector3.Distance (a, b) <= distance)
                return true;

            return false;
        }
    }

    /// <summary>Extension of the Unity logger to aid with debugging of goals and utilities.</summary>
    public static class Log
    {
        /// <summary>Logs a formatted debug value out to the console.</summary>
        /// <param name="desirablity">The desirability to log to the console.</param>
        /// <param name="agent">The agent that called it.</param>
        public static void Desirability (float desirablity, AI agent)
        {
            //Disabled for submission.
            //Debug.Log ($"<Color={ColorTypes.White}>Desirability:</color> <Color={ColorTypes.Cyan}>{desirablity}</color> | {agent}");
        }

        /// <summary>Logs a formatted debug value out to the console.</summary>
        /// <param name="name">The name of the desirability value to log to the console.</param>
        /// <param name="desirablity">The desirability to log to the console.</param>
        /// <param name="agent">The agent that called it.</param>
        public static void Desirability (string name, float desirablity, AI agent)
        {
            //Disabled for submission.
            //Debug.Log ($"<Color={ColorTypes.White}>{name} Desirability:</color> <Color={ColorTypes.Cyan}>{desirablity}</color> | {agent}");
        }

        /// <summary>Logs a formatted entered goal announcement.</summary>
        /// <param name="goalName">The name of the goal that's logging.</param>
        /// <param name="agent">The agent who called it.</param>
        public static void EnteredGoal (string goalName, AI agent)
        {
            Goal (goalName, "Entered", ColorTypes.Yellow, agent);
        }

        /// <summary>Logs a formatted processing goal announcement.</summary>
        /// <param name="goalName">The name of the goal that's logging.</param>
        /// <param name="agent">The agent who called it.</param>
        public static void ProcessingGoal (string goalName, AI agent)
        {
            Goal (goalName, "Processing", ColorTypes.Green, agent);
        }

        /// <summary>Logs a formatted exited goal announcement.</summary>
        /// <param name="goalName">The name of the goal that's logging.</param>
        /// <param name="agent">The agent who called it.</param>
        public static void ExitedGoal (string goalName, AI agent)
        {
            Goal (goalName, "Exited", ColorTypes.Red, agent);
        }

        /// <summary>Logs a formatted goal announcement.</summary>
        /// <param name="goalName">The name of the goal that's logging.</param>
        /// <param name="goalValue">The value of the goal.</param>
        /// <param name="color">What color should the goal be printed out as?</param>
        /// <param name="agent">The agent who called it.</param>
        public static void Goal (string goalName, string goalValue, ColorTypes color, AI agent)
        {
            //Disabled for submission.
            //Debug.Log ($"<Color={ColorTypes.White}>Goal_{goalName}:</color> <color={color.ToString ()}> {goalValue} </color>| {agent}");
        }
    }
}
