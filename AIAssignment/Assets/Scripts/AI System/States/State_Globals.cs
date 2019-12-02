﻿using System;
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

        public void Process (AI agent)
        {
            Log.ProcessingState ("Globals", agent);

            // Default values for both desirabiltiy and evaluator.
            float bestDesirability = 0.0f;
            IEvaluator<AI> bestEvaluator = null;

            // Loop through every evaluator.
            foreach (IEvaluator<AI> evaluator in _Evaluators)
            {
                //Calculate the desirability of the current task.
                float desirability = evaluator.CalculateDesirability (agent);

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

            // Check to see if we are already in the desired state. If we are, then leave as there's nothing more to do.
            if (agent.Brain.IsInState (desiredState) == false)
                return;
            // If we're not, then switch to the newly desired state.
            else
                agent.Brain.ChangeState (desiredState);
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
