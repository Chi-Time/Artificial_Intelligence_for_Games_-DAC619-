using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.AI_System.Goals;

namespace Assets.Scripts.AI_System
{
    public abstract class Evaluator
    {
        protected float _CharacterBias;

        public abstract float CalculateDesirability (AI agent);
        public abstract void SetGoal (AI agent);
    }

    public class GetHealthEvaluator : Evaluator
    {
        public override float CalculateDesirability (AI agent)
        {
            var distance = Features.DistanceToItem (agent, Names.HealthKit);

            if (distance == 1)
                return 0.0f;
            else
            {
                const float tweaker = 0.2f;

                float desirablity = tweaker * ( ( 1 - 75 ) / distance );

                Debug.Log ("Desirablitity: " + desirablity);
                return desirablity;
            }
        }

        public override void SetGoal (AI agent)
        {
            agent.Brain.AddGoal_Health (new CompGoal_GetHealth (agent));
        }
    }

    class Features
    {
        public static float Normalise (float value, float min, float max)
        {
            return Mathf.Abs (( value - min ) / ( max - min ));
        }

        public static float DistanceToItem (AI agent, string name)
        {
            var item = GameObject.Find (name);

            if (item == null)
                return 1.0f;

            // Figure out how to use these later.
            const float maxDistance = 500f;
            const float minDistance = 0f;

            float distance = Vector3.Distance (agent.transform.position, item.transform.position);

            //Debug.Log (distance);
            //Debug.Log (Normalise (distance, minDistance, maxDistance));

            return Normalise (distance, 0.0f, 1.0f);
        }
    }


}
