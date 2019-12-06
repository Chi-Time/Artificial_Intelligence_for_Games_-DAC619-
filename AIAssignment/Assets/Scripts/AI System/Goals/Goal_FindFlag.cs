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

        /// <summary>Reference to the name of the flag to find.</summary>
        private string _Flag = "";
        /// <summary>The minimum distance to determine if we're close enough to see the item.</summary>
        private const float _MinDistance = 5f;
        /// <summary>The last known position the flag was seen at.</summary>
        private Vector3 _FlagPosition = Vector3.zero;

        public Goal_FindFlag (string flag)
        {
            this._Flag = flag;
        }

        public void Enter (AI agent)
        {
            Log.EnteredGoal ("FindFlag", agent);

            // Grab the last known position of the flag and attempt to move there.
            _FlagPosition = WorldManager.Instance.GetLastKnownFlagPosition (_Flag);
            agent.Actions.MoveTo (_FlagPosition);
        }

        public GoalState Process (AI agent)
        {
            Log.ProcessingGoal ("FindFlag", agent);

            // If we've seen the flag on our travels, then we've completed our job.
            if (GetFlagInView (agent))
                return CurrentState = GoalState.Complete;

            // If there is no known position to find the flag at we've failed.
            if (_FlagPosition == Vector3.zero)
                return CurrentState = GoalState.Failed;

            // If we're near the location we think the flag is at then we've completed our job.
            if (agent.Actions.HasArrived (_MinDistance))
                return CurrentState = GoalState.Complete;

            return CurrentState = GoalState.Active;
        }

        /// <summary>Retrieve the flag in our sights if one exists.</summary>
        /// <param name="agent">The agent to use as our eyes.</param>
        /// <returns>The flag we can see if one exists, null if not.</returns>
        private GameObject GetFlagInView (AI agent)
        {
            // Grab an item in our view from our senses.
            GameObject viewedItem = agent.Senses.GetObjectInViewByName (_Flag);

            return viewedItem;
        }

        public void Exit (AI agent)
        {
            Log.ExitedGoal ("FindFlag", agent);

            _FlagPosition = Vector3.zero;
        }

        public void AddSubGoal (IGoal<AI> subState)
        {
            throw new NotImplementedException ();
        }
    }
}
