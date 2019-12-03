using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.States
{
    class State_Wander : IState<AI>
    {
        private StateType _CurrentStateType = StateType.Inactive;

        public void Enter (AI agent)
        {
            Log.EnteredState ("Wander", agent);
            agent.StartCoroutine (ChangeDirection (agent));
        }

        public StateType Process (AI agent)
        {
            Log.ProcessingState ("Wander", agent);

            _CurrentStateType = StateType.Active;

            return _CurrentStateType;
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

        public void AddSubState (IState<AI> subState) { throw new System.NotImplementedException (); }
    }
}
