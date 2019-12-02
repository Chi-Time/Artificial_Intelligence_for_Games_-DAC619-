using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.States
{
    class State_Globals : IState<AI>
    {
        private List<IEvaluator<AI>> _Evaluators = new List<IEvaluator<AI>> ();

        public void AddEvaluator (IEvaluator<AI> evaluator)
        {
            _Evaluators.Add (evaluator);
        }

        public void Enter (AI agent)
        {

        }

        public void Process (AI agent)
        {
            // Default values for both desirabiltiy and evaluator.
            float bestDesirability = 0.0f;
            IEvaluator<AI> bestEvaluator = null;

            // Loop through every evaluator.
            foreach (IEvaluator<AI> evaluator in _Evaluators)
            {
                //
                float desirability = evaluator.CalculateDesirability (agent);

                if (desirability >= bestDesirability)
                {
                    bestDesirability = desirability;
                    bestEvaluator = evaluator;
                }
            }

            // Check to see if we are already in the desired state. If we are, then leave as there's nothing more to do.
            if (agent.Brain.IsInState (bestEvaluator.GetState ()) == false)
                return;
            // If we're not, then switch to the newly desired state.
            else
                agent.Brain.ChangeState (bestEvaluator.GetState ());

            //Log.Desirability (Aspirations.Evaluator_AttackEnemy (agent), agent);

            /** Get all evaluators
             *  Find evalutor with highest desirabiliy
             *  Find evaluator relevent state
             *  Change state to evaluator relevent state.
             */
        }

        public void Exit (AI agent)
        {

        }

        public bool HandleMessage (Message message)
        {
            return false;
        }
    }
}
