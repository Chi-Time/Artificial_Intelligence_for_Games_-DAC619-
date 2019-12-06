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
        private const float _MinDistance = 2f;

        public void Enter (AI agent)
        {
            Log.EnteredState ("MoveToBase", agent);

            agent.Actions.MoveTo (agent.Data.FriendlyBase);
        }

        public GoalState Process (AI agent)
        {
            Log.ProcessingState ("MoveToBase", agent);

            if (Vector3.Distance (agent.transform.position, agent.Data.FriendlyBase.transform.position) <= _MinDistance)
                return CurrentState = GoalState.Complete;

            return CurrentState = GoalState.Active;
        }

        public void Exit (AI agent)
        {
            Log.ExitedState ("MoveToBase", agent);
        }

        public bool HandleMessage (Message message)
        {
            return false;
        }

        public void AddSubGoal (IGoal<AI> subState) { throw new System.NotImplementedException (); }
    }
}
