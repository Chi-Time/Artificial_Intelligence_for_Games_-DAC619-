﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System
{
    public enum ColorTypes { Red, Yellow, Blue, Cyan, Green, Pink, Purple, White, Black, Orange }

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
