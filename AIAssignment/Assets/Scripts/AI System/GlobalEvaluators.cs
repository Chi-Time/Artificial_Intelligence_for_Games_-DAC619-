using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System
{
    class GlobalEvaluators
    {
        /// <summary>Evaluates the desirability of the distance to an object between a range of 0-1</summary>
        /// <param name="agent">The current agent wanting the object.</param>
        /// <param name="obj">The object in question.</param>
        /// <returns>A normalised value between a range of 0-1 representing the desirability.</returns>
        public static float Evaluator_DistanceToObject (AI agent, GameObject obj)
        {
            const float minDistance = 0f;
            const float maxDistance = 50f;
            float distance = Vector3.Distance (agent.transform.position, obj.transform.position);

            if (distance > maxDistance)
                return 0.0f;

            float desirability = Helpers.GetDistribution (distance, minDistance, maxDistance);

            return UtilityCurves.Exponential.Evaluate (desirability);
        }

        public static float Evaluator_Health (AI agent)
        {
            float desirability = Helpers.GetDistribution (agent.Data.CurrentHitPoints, 0.0f, agent.Data.MaxHitPoints);

            return UtilityCurves.Linear.Evaluate (desirability);
        }

        public static float Evaluator_Strength (AI agent)
        {
            float desirability = 0.0f;
            const float tweaker = 1.5f;
            float damage = agent.Data.NormalAttackDamage;

            // If the agent has a powerup.
            if (agent.Data.IsPoweredUp)
            {
                // Apply the powerup to his normal damage.
                damage = agent.Data.PowerUpAmount * agent.Data.NormalAttackDamage;
                // Find out the desirability of his powered up damage.
                desirability = Helpers.GetDistribution (damage, 0, 20);

                return desirability;
            }

            damage *= tweaker;
            desirability = Helpers.GetDistribution (damage, 0, 20);
            return desirability;
        }
    }
}
