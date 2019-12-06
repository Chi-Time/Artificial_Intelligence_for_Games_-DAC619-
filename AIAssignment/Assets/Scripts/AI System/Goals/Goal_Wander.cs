using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.Goals
{
    class Goal_Wander : IGoal<AI>
    {
        public GoalState CurrentState { get; private set; }

        public void Enter (AI agent)
        {
            Log.EnteredState ("Wander", agent);
            agent.StartCoroutine (ChangeDirection (agent));
        }

        public GoalState Process (AI agent)
        {
            Log.ProcessingState ("Wander", agent);

            return CurrentState = GoalState.Active;
        }

        private IEnumerator ChangeDirection (AI agent)
        {
            // Calculate a new direction for the agent to move to.
            var newDirection = Random.insideUnitSphere * agent.Data.Speed;

            agent.Actions.MoveTo (newDirection);

            yield return new WaitForSeconds (2.0f);

            agent.StartCoroutine (ChangeDirection (agent));
        }

        public void Exit (AI agent)
        {
            Log.ExitedState ("Wander", agent);
            agent.StopCoroutine (ChangeDirection (agent));
        }

        public bool HandleMessage (Message message)
        {
            return false;
        }

        public void AddSubGoal (IGoal<AI> subState) { throw new System.NotImplementedException (); }
    }
}
