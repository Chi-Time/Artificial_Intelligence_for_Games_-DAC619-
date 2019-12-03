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

        private float _Timer = 0.0f;
        private float _MemoryLength = 2.0f;
        private AI _Agent = null;

        public TargetingSystem (AI agent)
        {
            this._Agent = agent;
        }

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
                if (_Timer <= _MemoryLength)
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

        public GameObject SelectTarget ()
        {
            // If there is still a target selected then just return him.
            if (CurrentTarget != null)
                return CurrentTarget;

            // Find all enemies in view.
            var enemiesInView = _Agent.Senses.GetEnemiesInView ();

            // Only do the calculations if there are enemies in range.
            if (enemiesInView.Count > 0)
            {
                // Consider tracking how many times we've died by or killed an enemy and factor that into the equation.
                float shortestDistance = 100.0f;
                GameObject bestTarget = null;

                foreach (GameObject enemy in enemiesInView)
                {
                    float distance = Vector3.Distance (_Agent.transform.position, enemy.transform.position);
                    
                    if (distance <= shortestDistance)
                    {
                        bestTarget = enemy;
                        shortestDistance = distance;
                    }
                }

                CurrentTarget = bestTarget;

                return bestTarget;
            }

            return null;
        }
    }
}
