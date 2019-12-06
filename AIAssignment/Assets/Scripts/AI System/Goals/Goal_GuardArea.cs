using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.Goals
{
    class Goal_GuardArea : IGoal<AI>
    {
        public GoalState CurrentState { get; private set; }

        /// <summary>The position to stay around for guarding.</summary>
        private Vector3 _GuardPosition = Vector3.zero;
        
        public Goal_GuardArea (Vector3 position)
        {
            _GuardPosition = position;
        }

        public void Enter (AI agent)
        {
            MoveToNextPosition (agent);
        }

        public GoalState Process (AI agent)
        {
            // If we've arrived close to or at our position.
            if (agent.Actions.HasArrived (1f))
            {
                // Get a new one and move there.
                MoveToNextPosition (agent);
            }

            return CurrentState = GoalState.Active;
        }

        /// <summary>Get's a new location within the guard area and moves the agent there.</summary>
        /// <param name="agent">The agent to move.</param>
        private void MoveToNextPosition (AI agent)
        {
            Vector3 nextPosition = GetNextPosition ();
            agent.Actions.MoveTo (nextPosition);
        }

        /// <summary>Get's a new position within the guard area.</summary>
        /// <returns>A new random position.</returns>
        private Vector3 GetNextPosition ()
        {
            Vector3 nextPosition = _GuardPosition;
            nextPosition *= Random.Range (0.85f, 1.0f);

            return nextPosition;
        }

        public void Exit (AI agent)
        {
            _GuardPosition = Vector3.zero;
        }

        public bool HandleMessage (Message message)
        {
            return true;
        }

        public void AddSubGoal (IGoal<AI> subState) { }
    }
}
