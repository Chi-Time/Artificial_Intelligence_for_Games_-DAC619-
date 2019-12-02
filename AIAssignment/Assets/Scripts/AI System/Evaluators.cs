using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.AI_System.States;

namespace Assets.Scripts.AI_System
{
    public interface IEvaluator<T> where T : class
    {
        IState<T> GetState ();
        float CalculateDesirability (T agent);
    }

    public class Evaluator_Wander : IEvaluator<AI>
    {
        public float CalculateDesirability (AI agent)
        {
            return 0.05f;
        }

        public IState<AI> GetState ()
        {
            return new State_Wander ();
        }
    }

    public class Evaluator_AttackEnemy : IEvaluator<AI>
    {
        public float CalculateDesirability (AI agent)
        {
            // Find all enemies in view.
            var enemiesInView = agent.Senses.GetEnemiesInView ();

            // Only do the calculations if there are enemies in range.
            if (enemiesInView.Count > 0)
            {
                float tweaker = 1.0f;

                float desirability = ( tweaker * Aspirations.Evaluator_Health (agent) ) * ( 1 - Aspirations.Evaluator_DistanceToObject (agent, enemiesInView[0]) ) * Aspirations.Evaluator_Strength (agent);

                return Mathf.Clamp (desirability, 0.0f, 1.0f);
            }

            // No enemies are in sight so this goal isn't desirable.
            return 0.0f;
        }

        public IState<AI> GetState ()
        {
            return new State_AttackEnemy ();
        }
    }

    public class Evaluator_GetHealthKit : IEvaluator<AI>
    {
        public float CalculateDesirability (AI agent)
        {
            // Find and get the health item.
            var healthItem = GameObject.Find (Names.HealthKit);

            // If the item doesn't exist then return 0 as this goal isn't desirable.
            if (healthItem == null)
                return 0.0f;

            // Get the distance from here to the health item.
            float distance = Aspirations.Evaluator_DistanceToObject (agent, healthItem);

            // If the distance is greater than 1 then returon 0 as this goal isn't desirable.
            if (distance >= 1)
                return 0.0f;

            // Tweaker value for adjusting the curve.
            const float tweaker = 0.2f;

            // Calculate the desirablity of getting the health item and clamp it.
            float desirability = tweaker * ( 1 - Aspirations.Evaluator_Health (agent) ) / distance;

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IState<AI> GetState ()
        {
            return new State_GetHealthKit ();
        }
    }
}
