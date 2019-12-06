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

        public static bool IsNearPosition (Vector3 a, Vector3 b, float distance)
        {
            if (Vector3.Distance (a, b) <= distance)
                return true;

            return false;
        }
    }

    public static class Log
    {
        public static void Desirability (float desirablity, AI agent)
        {
            Debug.Log ($"<Color={ColorTypes.White}>Desirability:</color> <Color={ColorTypes.Cyan}>{desirablity}</color> | {agent}");
        }

        public static void Desirability (string name, float desirablity, AI agent)
        {
            Debug.Log ($"<Color={ColorTypes.White}>{name} Desirability:</color> <Color={ColorTypes.Cyan}>{desirablity}</color> | {agent}");
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

    /// <summary>Custom queue for handling frequent remove operations but keeping a FIFO system.</summary>
    /// <typeparam name="T">The type of data to hold.</typeparam>
    public class CustomQueue<T>
    {
        public int Count { get { return list.Count; } }

        private LinkedList<T> list = new LinkedList<T> ();

        public void Enqueue (T t)
        {
            list.AddLast (t);
        }

        public T Dequeue ()
        {
            var result = list.First.Value;
            list.RemoveFirst ();
            return result;
        }

        public T Peek ()
        {
            return list.First.Value;
        }

        public bool Remove (T t)
        {
            return list.Remove (t);
        }
    }
}
