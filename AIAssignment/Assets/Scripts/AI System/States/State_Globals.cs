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
            Log.EnteredState ("Globals", agent);
        }

        public StateType Process (AI agent)
        {
            Log.ProcessingState ("Globals", agent);

            // Default values for both desirability and evaluator.
            float bestDesirability = 0.0f;
            IEvaluator<AI> bestEvaluator = null;

            // Loop through every evaluator.
            foreach (IEvaluator<AI> evaluator in _Evaluators)
            {
                //Calculate the desirability of the current task.
                float desirability = evaluator.CalculateDesirability (agent);
                
                Log.Desirability (desirability, agent);

                // If the desirability of this given evaluator is higher than a previous.
                if (desirability >= bestDesirability)
                {
                    // It then becomes our next best one.
                    bestDesirability = desirability;
                    bestEvaluator = evaluator;
                }
            }

            // Grab the desired state from our best evaluator.
            var desiredState = bestEvaluator.GetState ();

            // Check to see if we are already in the desired state. If we're not, then switch to the newly desired state.
            if (agent.Brain.IsInState (desiredState) == false)
            {
                agent.Brain.ChangeState (desiredState);
                return StateType.Active;
            }
            // If we are, then leave as there's nothing more to do.
            else
            {
                return StateType.Active;
            }
        }

        public void Exit (AI agent)
        {
            Log.ExitedState ("Globals", agent);
        }

        public bool HandleMessage (Message message)
        {
            return false;
        }

        public void AddSubState (IState<AI> subState) { throw new NotImplementedException (); }
    }
}
