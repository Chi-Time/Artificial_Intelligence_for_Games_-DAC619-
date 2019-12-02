using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.States
{
    class State_GetHealthKit : IState<AI>
    {
        private GameObject _HealthKit = null;
        private GameObject _HealthKitSpawner = null;

        public void Enter (AI agent)
        {
            Log.EnteredState ("GetHealthKit", agent);

            _HealthKitSpawner = GameObject.Find ("HealthKitSpawner");

            if (_HealthKitSpawner == null)
            {
                Debug.Log ("Couldn't find it");
                agent.Brain.ChangeState (new State_Wander ());
            }
        }

        public void Process (AI agent)
        {
            // Check to see if the kit still exists in the world.
            if (_HealthKitSpawner == null)
            {
                // If it doesn't then go back to an idle state.
                agent.Brain.ChangeState (new State_Wander ());
                return;
            }

            // Move towards the health kit.
            agent.Actions.MoveTo (_HealthKitSpawner);

            // Are we in range for picking up the kit?
            if (IsInRange (agent))
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
                        Debug.Log ("I can see it and I'm grabbin: " + agent.name);
                        // If we can, then collect it and return to a default state.
                        agent.Actions.CollectItem (_HealthKit);
                    }
                }
                // If we can't then return to a default state.
                else
                {
                    Debug.Log ("Can't see shit boss: " + agent.name);
                    agent.Brain.ChangeState (new State_Wander ());
                }
            }
        }

        private bool IsInRange (AI agent)
        {
            const float range = 3.0f;

            if (Vector3.Distance (agent.transform.position, _HealthKitSpawner.transform.position) < range)
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
    }
}
