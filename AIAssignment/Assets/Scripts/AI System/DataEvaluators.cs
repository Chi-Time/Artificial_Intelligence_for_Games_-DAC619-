using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System
{
    class DataEvaluators
    {
        /// <summary>Evaluates the desirability of the distance to an object between a range of 0-1</summary>
        /// <param name="agent">The current agent wanting the object.</param>
        /// <param name="obj">The object in question.</param>
        /// <returns>A normalised value between a range of 0-1 representing the desirability.</returns>
        public static float DistanceToObject (GameObject agent, GameObject obj)
        {
            if (obj == null || agent == null)
                return 1.0f;

            const float minDistance = 0f;
            const float maxDistance = 75f;
            float distance = Vector3.Distance (agent.transform.position, obj.transform.position);

            if (distance > maxDistance)
                return 1.0f;

            float desirability = Helpers.GetDistribution (distance, minDistance, maxDistance);

            //Debug.Log ("Distance Utility: " + UtilityCurves.Linear.Evaluate (desirability));

            return UtilityCurves.Linear.Evaluate (desirability);
        }

        /// <summary>Evaluates the desirability of the distance to an object between a range of 0-1</summary>
        /// <param name="agent">The current agent wanting the object.</param>
        /// <param name="position">The position in question.</param>
        /// <returns>A normalised value between a range of 0-1 representing the desirability.</returns>
        public static float DistanceToPosition (GameObject agent, Vector3 position)
        {
            if (position == Vector3.zero)
                return 0.0f;

            const float minDistance = 0f;
            const float maxDistance = 75f;
            float distance = Vector3.Distance (agent.transform.position, position);

            if (distance > maxDistance)
                return 1.0f;

            float desirability = Helpers.GetDistribution (distance, minDistance, maxDistance);

            //Debug.Log ("Distance Utility: " + UtilityCurves.Linear.Evaluate (desirability));

            return UtilityCurves.Linear.Evaluate (desirability);
        }

        public static float TeamScore (AI agent)
        {
            if (agent.Data.FriendlyScore < agent.Data.EnemyScore)
                return Helpers.GetDistribution (agent.Data.FriendlyScore, 0.0f, agent.Data.EnemyScore);
            else
                return 0.01f;
        }

        public static float Health (AI agent)
        {
            float desirability = Helpers.GetDistribution (agent.Data.CurrentHitPoints, 0.0f, agent.Data.MaxHitPoints);

            //Debug.Log ("Health Utility: " + UtilityCurves.Logistic.Evaluate (desirability));

            // Use a sigmoid logistic curve to get the overall desirability for health.
            return desirability;
        }

        /// <summary>Evaluates the current number of enemies into a 0-1 range with 0 being desirable and 1 being undesirable.</summary>
        /// <param name="agent">The agent to use for determining enemies around them.</param>
        /// <returns>The clamped and normalised value between 0 - 1</returns>
        public static float NumberOfEnemiesInSight (AI agent)
        {
            const float half = 0.5f;
            float desirability = 0.0f;

            int enemiesInView = agent.Senses.GetEnemiesInView ().Count;

            desirability = Helpers.GetDistribution (enemiesInView, 0, 3);

            return desirability * half;
        }

        /// <summary>Evaluates the current number of friendlies into a 0-1 range with 1 being desirable and 0 being undesirable.</summary>
        /// <param name="agent">The agent to use for determining friendlies around them.</param>
        /// <returns>The clamped and normalised value between 0 - 1</returns>
        public static float NumberOfFriendliesInSight (AI agent)
        {
            const float half = 0.5f;
            float desirability = 0.0f;

            int friendliesInView = agent.Senses.GetFriendliesInView ().Count;

            desirability = Helpers.GetDistribution (friendliesInView, 0, 2);

            return desirability * half;
        }

        public static float NumberOfFriendliesAtPosition (AI agent, Vector3 position)
        {
            const float half = 0.5f;
            float desirability = 0.0f;
            const float minDistance = 7.5f;

            var friendlies = WorldManager.Instance.GetTeammates (agent);
            int friendliesAtPosition = 0;

            foreach (AI member in friendlies)
            {
                if (member == agent)
                    continue;

                if (Helpers.IsNearPosition (member.transform.position, position, minDistance))
                    friendliesAtPosition++;
            }

            desirability = Helpers.GetDistribution (friendliesAtPosition, 0, 2);

            return desirability;
        }

        public static float Strength (AI agent)
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

            //Debug.Log ("Strength Utility: " + desirability);
            return desirability;
        }
    }
}
