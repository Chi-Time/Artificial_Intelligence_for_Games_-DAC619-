using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.AI_System.States;

namespace Assets.Scripts.AI_System
{
    public static class Aspirations
    {
        public static float Normalise (float value, float min, float max)
        {
            return ( value - min ) / ( max - min );
        }

        public static float GetDistribution (float value, float min, float max)
        {
            float normalisedValue = Normalise (value, min, max);

            return Mathf.Clamp (normalisedValue, 0.0f, 1.0f);
        }

        /// <summary>Evaluates the desirability of the distance to an object between a range of 0-1</summary>
        /// <param name="agent">The current agent wanting the object.</param>
        /// <param name="obj">The object in question.</param>
        /// <returns>A normalised value between a range of 0-1 representing the desirability.</returns>
        public static float Evaluator_DistanceToObject (AI agent, GameObject obj)
        {
            //TODO: FIND ACTUAL VALUES FOR THIS SHIT
            const float minDistance = 0f;
            const float maxDistance = 100f;
            float distance = Vector3.Distance (agent.transform.position, obj.transform.position);

            if (distance > maxDistance)
                return 0.0f;

            return GetDistribution (distance, minDistance, maxDistance);
        }

        public static float Evaluator_Health (AI agent)
        {
            return GetDistribution (agent.Data.CurrentHitPoints, 0.0f, agent.Data.MaxHitPoints);
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
                desirability = GetDistribution (damage, 0, 20);
                
                return Mathf.Clamp (desirability, 0.0f, 1.0f);
            }

            damage *= tweaker;
            desirability = GetDistribution (damage, 0, 20);
            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }
    }
}
