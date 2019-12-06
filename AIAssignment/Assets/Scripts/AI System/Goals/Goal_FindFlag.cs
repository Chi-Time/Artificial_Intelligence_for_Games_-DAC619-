using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.Goals
{
    class Goal_FindFlag : IGoal<AI>
    {
        public GoalState CurrentState { get; private set; }

        private string _Flag = "";
        private const float _MinDistance = 5f;
        private Vector3 _FlagPosition = Vector3.zero;

        public Goal_FindFlag (string flag)
        {
            this._Flag = flag;
        }

        public void Enter (AI agent)
        {
            Log.EnteredState ("FindFlag", agent);

            //TODO: Make it so that the flag can be "spotted" by teammates in short term memory
            //TODO: So that they can search there for it too.
            // Get the last known location of the enemy team's flag.
            if (agent.CompareTag (Tags.RedTeam))
                _FlagPosition = WorldManager.Instance.BlueBase.transform.position;
            else if (agent.CompareTag (Tags.BlueTeam))
                _FlagPosition = WorldManager.Instance.RedBase.transform.position;

            agent.Actions.MoveTo (_FlagPosition);
        }

        public GoalState Process (AI agent)
        {
            Log.ProcessingState ("FindFlag", agent);

            // If we've seen the flag on our travels, then we've completed our job.
            if (GetFlagInView (agent))
                return CurrentState = GoalState.Complete;

            // If there is no known position to find the flag at we've failed.
            if (_FlagPosition == Vector3.zero)
                return CurrentState = GoalState.Failed;

            // If we're near the location we think the flag is at then we've completed our job.
            if (Vector3.Distance (agent.transform.position, _FlagPosition) <= _MinDistance)
                return CurrentState = GoalState.Complete;

            return CurrentState = GoalState.Active;
        }

        private GameObject GetFlagInView (AI agent)
        {
            GameObject viewedItem = agent.Senses.GetObjectInViewByName (_Flag);

            return viewedItem;
        }

        public void Exit (AI agent)
        {
            Log.ExitedState ("FindFlag", agent);

            _FlagPosition = Vector3.zero;
        }

        public bool HandleMessage (Message message)
        {
            return true;
        }

        public void AddSubGoal (IGoal<AI> subState)
        {
            throw new NotImplementedException ();
        }
    }
}
