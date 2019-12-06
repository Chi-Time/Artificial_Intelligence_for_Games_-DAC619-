using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.Goals
{
    class Goal_MoveToBase : IGoal<AI>
    {
        public GoalState CurrentState { get; private set; }

        /// <summary>Minimum distance until we can count as "in" the base.</summary>
        private const float _MinDistance = 2f;

        public void Enter (AI agent)
        {
            Log.EnteredGoal ("MoveToBase", agent);

            // Move to our home base.
            agent.Actions.MoveTo (agent.Data.FriendlyBase);
        }

        public GoalState Process (AI agent)
        {
            Log.ProcessingGoal ("MoveToBase", agent);

            // If we're in reach our home base then return that we did our job.
            if (Helpers.IsNearPosition (agent.transform.position, agent.Data.FriendlyBase.transform.position, _MinDistance))
                return CurrentState = GoalState.Complete;

            return CurrentState = GoalState.Active;
        }

        public void Exit (AI agent)
        {
            Log.ExitedGoal ("MoveToBase", agent);
        }

        public void AddSubGoal (IGoal<AI> subState) { throw new System.NotImplementedException (); }
    }
}
