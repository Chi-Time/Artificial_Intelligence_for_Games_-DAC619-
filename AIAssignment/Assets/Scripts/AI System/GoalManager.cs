using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.AI_System.Goals;

namespace Assets.Scripts.AI_System
{
    /// <summary>Processes and manages goals</summary>
    /// <typeparam name="T">Data type to own this manager.</typeparam>
    public class GoalManager<T> where T : class
    {
        /// <summary>Reference to the owner of the manager.</summary>
        public T Agent { get; private set; }
        /// <summary>Reference to the current global goal in the manager.</summary>
        public IGoal<T> GlobalGoal { get; private set; }
        /// <summary>Reference to the currently active goal in the manager.</summary>
        public IGoal<T> CurrentGoal { get; private set; }
        /// <summary>Reference to the previous goal of the manager.</summary>
        public IGoal<T> PreviousGoal { get; private set; }

        /// <summary>Regulator for controlling global goal ticks.</summary>
        private Regulator _GlobalRegulator = new Regulator (AISystem.GlobalDelay);
        /// <summary>Regulator for controlling current goal ticks.</summary>
        private Regulator _CurrentGoalRegulator = new Regulator (AISystem.CurrentDelay);

        /// <summary>Creates a new goal manager instance and assign's it an owner.</summary>
        /// <param name="agent">The agent who owns the manager.</param>
        public GoalManager (T agent)
        {
            this.Agent = agent;
        }

        /// <summary>Updates the manager's goals.</summary>
        public void Process ()
        {
            // If there is a goal available and the regulator says we can process it.
            if (GlobalGoal != null && _GlobalRegulator.CanProcess ())
            {
                // Tick the goal and perform it's logic.
                GlobalGoal.Process (Agent);
            }

            if (CurrentGoal != null && _CurrentGoalRegulator.CanProcess ())
            {
                CurrentGoal.Process (Agent);
            }
        }

        /// <summary>Change the currently active goal of the manager.</summary>
        /// <param name="newGoal">The new goal for the manager to switch to.</param>
        public void ChangeGoal (IGoal<T> newGoal)
        {
            // Reset our previous goal to this one.
            PreviousGoal = CurrentGoal;
            // Exit out of the current goal.
            CurrentGoal.Exit (Agent);

            // Switch our current goal to the new one and enter it.
            CurrentGoal = newGoal;
            CurrentGoal.Enter (Agent);
        }

        /// <summary>Revert's the manager back to a previous goal in memory.</summary>
        public void RevertToPreviousGoal ()
        {
            ChangeGoal (PreviousGoal);
        }

        /// <summary>Returns true if the manager is in the given goal.</summary>
        /// <param name="goal">The goal to test for.</param>
        public bool IsInGoal (IGoal<T> goal)
        {
            // Compare if the given goal type is the same as our current one.
            if (CurrentGoal.GetType () == goal.GetType ())
                return true;

            return false;
        }

        /// <summary>Set's the current agent's goal to that of the one provided.</summary>
        /// <param name="newGoal">The new goal to mark and monitor as the current.</param>
        public void SetCurrentGoal (IGoal<T> newGoal) => CurrentGoal = newGoal;

        /// <summary>Set's the current agent's global goal to that of the one provided.</summary>
        /// <param name="newGoal">The new goal to mark and monitor as the global.</param>
        public void SetGlobalGoal (IGoal<T> newGoal) => GlobalGoal = newGoal;

        /// <summary>Set's the agent's previous goal to that of the one provided.</summary>
        /// <param name="newGoal">The new goal to mark as the previous.</param>
        public void SetPreviousGoal (IGoal<T> newGoal) => PreviousGoal = newGoal;
    }
}
