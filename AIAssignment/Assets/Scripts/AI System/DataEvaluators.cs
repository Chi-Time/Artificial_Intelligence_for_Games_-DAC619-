using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System
{
    /// <summary>Used for accessing game data in a clean value range.</summary>
    class DataEvaluators
    {
        /// <summary>Evaluates the desirability of the distance to an object between a range of 0-1. 0 being most desirable and 1 being least.</summary>
        /// <param name="agent">The agent wanting the distance.</param>
        /// <param name="obj">The object in question.</param>
        /// <returns>A normalised value between a range of 0-1 representing the desirability.</returns>
        public static float DistanceToObject (GameObject agent, GameObject obj)
        {
            // If either target is null then return 1 as this isn't a desirable option.
            if (obj == null || agent == null)
                return 1.0f;

            return DistanceToPosition (agent, obj.transform.position);
        }

        /// <summary>Evaluates the desirability of the distance to an object between a range of 0-1. 0 being most desirable and 1 being least.</summary>
        /// <param name="agent">The agent wanting the distance.</param>
        /// <param name="position">The position in question.</param>
        /// <returns>A normalised value between a range of 0-1 representing the desirability.</returns>
        public static float DistanceToPosition (GameObject agent, Vector3 position)
        {
            // Find our distance and set up a basic range.
            const float minDistance = 0f;
            const float maxDistance = 75f;
            float distance = Vector3.Distance (agent.transform.position, position);

            // If we're further away than max distance than this option is undesirable.
            if (distance > maxDistance)
                return 1.0f;

            // Clamp and normalise the value and return it.
            float desirability = Helpers.GetDistribution (distance, minDistance, maxDistance);

            return desirability;
        }

        /// <summary>Get the current agent's health as a normalised value between 0-1. 0 Being least desirable and 1 being most.</summary>
        /// <param name="agent">The agent target.</param>
        /// <returns>A normalised value clamped between a 0-1 range.</returns>
        public static float Health (AI agent)
        {
            float desirability = Helpers.GetDistribution (agent.Data.CurrentHitPoints, 0.0f, agent.Data.MaxHitPoints);

            return desirability;
        }

        /// <summary>Evaluates the current number of enemies into a 0-1 range with 0 being desirable and 1 being undesirable.</summary>
        /// <param name="agent">The agent to use for determining enemies around them.</param>
        /// <returns>The clamped and normalised value between 0 - 1</returns>
        public static float NumberOfEnemiesInSight (AI agent)
        {
            // Constant for dividing the value in half so it doesn't weigh too heavily in equations.
            const float half = 0.5f;
            float desirability = 0.0f;

            int enemiesInView = agent.Senses.GetEnemiesInView ().Count;
            desirability = Helpers.GetDistribution (enemiesInView, 0, 3);

            return desirability * half;
        }

        /// <summary>Get's the current agent's strength as a normalised valued between 0-1. 0 being least desirable and 1 being most.</summary>
        /// <param name="agent">The agent target.</param>
        /// <returns>A normalised and clamped value between a 0-1 range.</returns>
        public static float Strength (AI agent)
        {
            float desirability = 0.0f;
            // Pre-defined tweaker value for adjusting strength.
            const float tweaker = 1.5f;
            // Get the agent's current attack damage.
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

            // Apply the tweaker to their damage and then get a normalised value from it and return.
            damage *= tweaker;
            desirability = Helpers.GetDistribution (damage, 0, 20);

            return desirability;
        }
    }
}
