using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.Goals
{
    class Goal_GetHealthKit : IGoal<AI>
    {
        public GoalState CurrentState { get; private set; }

        private GameObject _HealthKit = null;

        public void Enter (AI agent)
        {
            Log.EnteredState ("GetHealthKit", agent);

            CurrentState = GoalState.Inactive;
        }

        public GoalState Process (AI agent)
        {
            // Check to see if the location still exists in the world.
            if (WorldManager.Instance.HealthKitSpawner == null)
            {
                // If it doesn't then we failed.
                return CurrentState = GoalState.Failed;
            }

            // Move towards the health kit.
            agent.Actions.MoveTo (WorldManager.Instance.HealthKitSpawner);

            // Are we in range for seeing the kit?
            if (IsInSightRange (agent))
            {
                if (_HealthKit == null)
                {
                    _HealthKit = GameObject.Find (Names.HealthKit);
                }

                // Can we see the health kit?
                if (CanSeeItem (agent))
                {
                    if (_HealthKit != null)
                    {
                        // If we can, then collect it and return to a default state.
                        agent.Actions.CollectItem (_HealthKit);

                        // Check if we collected, if we did then return that we did our job.
                        if (agent.Inventory.HasItem (Names.HealthKit))
                            return CurrentState = GoalState.Complete;
                    }
                }
                // If we can't then return to a default state.
                else
                {
                    return CurrentState = GoalState.Failed;
                }
            }

            return CurrentState = GoalState.Active;
        }

        private bool IsInSightRange (AI agent)
        {
            const float range = 3.0f;

            if (Vector3.Distance (agent.transform.position, WorldManager.Instance.HealthKitSpawner.transform.position) < range)
                return true;

            return false;
        }

        private bool CanSeeItem (AI agent)
        {
            var collectables = agent.Senses.GetCollectablesInView ();

            if (collectables.Count > 0)
                return true;

            return false;
        }

        public void Exit (AI agent)
        {
            Log.ExitedState ("GetHealthKit", agent);
            _HealthKit = null;
        }

        public bool HandleMessage (Message message)
        {
            return false;
        }

        public void AddSubGoal (IGoal<AI> subState) { throw new NotImplementedException (); }
    }
}
