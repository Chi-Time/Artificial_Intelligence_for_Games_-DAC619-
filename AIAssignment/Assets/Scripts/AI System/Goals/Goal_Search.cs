using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.Goals
{
    class Goal_Search : IGoal<AI>
    {
        public GoalState CurrentState { get; private set; }

        /// <summary>The current location we'll be heading towards.</summary>
        private Vector3 _Location = Vector3.zero;

        public void Enter (AI agent)
        {
            Log.EnteredGoal ("Search", agent);

            // If there is no location target then get one.
            if (_Location == Vector3.zero)
            {
                _Location = GetLocation (agent);
            }
        }

        public GoalState Process (AI agent)
        {
            Log.ProcessingGoal ("Search", agent);

            // If we've reached our location or lost it, get a new one and go there.
            if (IsAtLocation (agent) || _Location == Vector3.zero)
            {
                MoveToLocation (agent);
            }

            return CurrentState = GoalState.Active;
        }

        private void MoveToLocation (AI agent)
        {
            // Get a position to move to and then let's head there.
            _Location = GetLocation (agent);
            agent.Actions.MoveTo (_Location);
        }

        private Vector3 GetLocation (AI agent)
        {
            // Get a random important location from the world to search for something there.
            Vector3 location = WorldManager.Instance.GetImportantLocation ().transform.position;

            // Minimum distance from the current location to consider it as already "in" it.
            const float minDistance = 5.0f;
            // Recursively search until we get a location that is isn't our current one.
            if (Vector3.Distance (location, _Location) < minDistance)
                return GetLocation (agent);
            
            // To make the AI seem a little less stiff let's apply a random offset to their chosen location.
            float offset = Random.Range (0.85f, 1.0f);
            location *= offset;

            return location;
        }

        private bool IsAtLocation (AI agent)
        {
            // Check if we're close to our destination to consider us as having reached it.
            const float minDistance = 2f;

            if (agent.Actions.HasArrived (minDistance))
                return true;

            //if (Helpers.IsNearPosition (agent.transform.position, _Location, minDistance))
            //{
            //    return true;
            //}

            return false;
        }

        public void Exit (AI agent)
        {
            Log.ExitedGoal ("Search", agent);
        }

        public void AddSubGoal (IGoal<AI> subState) { }
    }
}
