using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.States
{
    class State_Wander : IState<AI>
    {
        private float _Timer = 0.0f;
        private float _DirectionDelay = 2.0f;

        public void Enter (AI agent)
        {
            Log.EnteredState ("Wander", agent);
            _Timer = _DirectionDelay;
        }

        public void Process (AI agent)
        {
            Log.ProcessingState ("Wander", agent);

            CalculateTimer ();

            if (CanChangeDirection ())
            {
                ChangeDirection (agent);
            }
        }

        private void CalculateTimer ()
        {
            _Timer += Time.deltaTime;
        }

        private bool CanChangeDirection ()
        {
            if (_Timer >= _DirectionDelay)
                return true;

            return false;
        }

        private void ChangeDirection (AI agent)
        {
            // Calculate a new direction for the agent to move to.
            var newDirection = Random.insideUnitSphere * agent.Data.Speed;

            // If the agent is able to move there.
            // Reset the timer as we have a found a new location.
            if (agent.Actions.MoveTo (newDirection))
                _Timer = 0.0f;
        }

        public void Exit (AI agent)
        {
            Log.ExitedState ("Wander", agent);
        }

        public bool HandleMessage (Message message)
        {
            return false;
        }
    }
}
