using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.Goals
{
    class Goal_Think : IGoal<AI>
    {
        public GoalState CurrentState { get; private set; }

        /// <summary>Reference to all of the evaluators we should check.</summary>
        private List<IEvaluator<AI>> _Evaluators = new List<IEvaluator<AI>> ();

        /// <summary>Add an evaluator to the list to be checked each tick.</summary>
        /// <param name="evaluator">The evaluator to add.</param>
        public void AddEvaluator (IEvaluator<AI> evaluator)
        {
            _Evaluators.Add (evaluator);
        }

        public void Enter (AI agent)
        {
            Log.EnteredState ("Globals", agent);
        }

        public GoalState Process (AI agent)
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

                // If the desirability of this given evaluator is higher than a previous.
                if (desirability >= bestDesirability)
                {
                    // It then becomes our next best one.
                    bestDesirability = desirability;
                    bestEvaluator = evaluator;
                }
            }

            // Grab the desired goal from our best evaluator.
            var desiredState = bestEvaluator.GetState (agent);
            

            // Check to see if we are already in the desired goal. If we're not, then switch to the newly desired goal.
            if (agent.Brain.IsInGoal (desiredState) == false)
            {
                agent.Brain.ChangeGoal (desiredState);
                return CurrentState = GoalState.Active;
            }
            // If we are, then check to see if it's finished yet.
            else
            {
                // If it has finished go back to a default goal.
                IGoal<AI> currentGoal = agent.Brain.CurrentGoal;

                if (currentGoal.CurrentState == GoalState.Failed || currentGoal.CurrentState == GoalState.Complete)
                    agent.Brain.ChangeGoal (new Goal_Search ());

                // IF it has hasn't then wait till next time.
                return CurrentState = GoalState.Active;
            }
        }

        public void Exit (AI agent)
        {
            Log.ExitedState ("Globals", agent);
        }

        public void AddSubGoal (IGoal<AI> subState) { throw new NotImplementedException (); }
    }
}
