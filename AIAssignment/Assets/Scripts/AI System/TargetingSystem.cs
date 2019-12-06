using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System
{
    public class TargetingSystem
    {
        /// <summary>The currently selected target for attacking.</summary>
        public GameObject CurrentTarget { get; private set; }

        /// <summary>The timer for how long it's been since we last saw our target.</summary>
        private float _Timer = 0.0f;
        /// <summary>The agent who owns this system.</summary>
        private AI _Agent = null;

        /// <summary>Creates a new instance of the targeting systema and assigns the agent as owner.</summary>
        /// <param name="agent">The agent who owns this system.</param>
        public TargetingSystem (AI agent)
        {
            this._Agent = agent;
        }

        /// <summary>Determines if the target is still present in our sight or memory.</summary>
        /// <returns>True if they are still present, false if not.</returns>
        public bool IsTargetPresent ()
        {
            // Update our memory timer.
            _Timer += Time.deltaTime;

            // Do we still have sight of our current target?
            if (IsTargetStillInSight ())
            {
                return true;
            }
            // We don't, see if he's in memory.
            else
            {
                // If we still have short term memory of the target return true.
                if (_Timer <= AISystem.TargetMemoryLength)
                {
                    return true;
                }
                // If we don't then remove the target from the system and reset our memory.
                else
                {
                    _Timer = 0.0f;
                    CurrentTarget = null;
                }
            }

            // Return false as no target is present anymore.
            return false;
        }

        /// <summary>Determines if a target is still in our sight radius.</summary>
        /// <returns>True if we can still see them, false if we cant.</returns>
        private bool IsTargetStillInSight ()
        {
            // Find all enemies in view.
            var enemiesInView = _Agent.Senses.GetEnemiesInView ();

            // Loop through all of the enemies in our sight.
            foreach (GameObject enemy in enemiesInView)
            {
                // If we can see our current target, reset out short term memory as it's been refreshed.
                if (enemy == CurrentTarget)
                {
                    _Timer = 0.0f;
                    return true;
                }
            }

            return false;
        }

        /// <summary>Selects a target based on proximity to agent.</summary>
        /// <returns>Target if one is found, null if none can be.</returns>
        public GameObject SelectTarget ()
        {
            // Find all enemies in view.
            var enemiesInView = _Agent.Senses.GetEnemiesInView ();

            // Only do the calculations if there are enemies in range.
            if (enemiesInView.Count > 0)
            {
                // Default values for finding our best target.
                GameObject bestTarget = null;
                float shortestDistance = 100.0f;

                // Loop through and keep picking the target with the lowest distance from us.
                foreach (GameObject enemy in enemiesInView)
                {
                    float distance = Vector3.Distance (_Agent.transform.position, enemy.transform.position);
                    
                    if (distance <= shortestDistance)
                    {
                        bestTarget = enemy;
                        shortestDistance = distance;
                    }
                }

                // Return this target as they're the closest.
                CurrentTarget = bestTarget;

                return bestTarget;
            }

            return null;
        }
    }
}
