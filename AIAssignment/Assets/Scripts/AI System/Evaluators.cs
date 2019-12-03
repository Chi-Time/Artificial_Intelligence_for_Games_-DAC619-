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
        IState<T> GetState (T agent);
        float CalculateDesirability (T agent);
    }

    public class Evaluator_Wander : IEvaluator<AI>
    {
        public float CalculateDesirability (AI agent)
        {
            return 0.05f;
        }

        public IState<AI> GetState (AI agent)
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

                float desirability = tweaker * ((GlobalEvaluators.Evaluator_Health (agent)) ) * ( 1 - GlobalEvaluators.Evaluator_DistanceToObject (agent, enemiesInView[0]) ) * GlobalEvaluators.Evaluator_Strength (agent);

                desirability = Mathf.Clamp (desirability, 0.0f, 1.0f);

                Log.Desirability ("AttackEnemy", desirability, agent);
                return desirability;
            }

            //Log.Desirability ("AttackEnemy", 0.0f, agent);

            // No enemies are in sight so this goal isn't desirable.
            return 0.0f;
        }

        public IState<AI> GetState (AI agent)
        {
            var target = agent.Targeting.SelectTarget ();
            return new State_AttackEnemy (target);
        }
    }

    public class Evaluator_Heal : IEvaluator<AI>
    {
        public float CalculateDesirability (AI agent)
        {
            // Find and get the health item.
            var healthItem = GameObject.Find (Names.HealthKit);

            // If the item doesn't exist then return 0 as this goal isn't desirable.
            if (healthItem == null)
                return 0.0f;

            // Get the distance from here to the health item.
            float distance = GlobalEvaluators.Evaluator_DistanceToObject (agent, healthItem);
            // Get the health using a sigmoid logistic curve.
            float health = UtilityCurves.Logistic.Evaluate (GlobalEvaluators.Evaluator_Health (agent));

            // If the distance is greater than 1 then returon 0 as this goal isn't desirable.
            if (distance >= 1)
                return 0.0f;

            // Tweaker value for adjusting the curve.
            const float tweaker = .05f;

            // Calculate the desirablity of getting the health item and clamp it.
            float desirability = tweaker * (( 1 - GlobalEvaluators.Evaluator_Health (agent) ) / distance);

            desirability = Mathf.Clamp (desirability, 0.0f, 1.0f);

            Log.Desirability ("Heal", desirability, agent);

            return desirability;
        }

        public IState<AI> GetState (AI agent)
        {
            return new CompState_Heal ();
        }
    }
}
