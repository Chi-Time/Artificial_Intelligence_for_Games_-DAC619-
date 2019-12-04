using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.States
{
    class Goal_Search : IState<AI>
    {
        /// <summary>The current location we'll be heading towards.</summary>
        private GameObject _Location = null;
        /// <summary>Minimum distance to define if we're "at" the location.</summary>
        private const float _MinDistance = 2f;

        public void Enter (AI agent)
        {
            Log.EnteredState ("Search", agent);

            // If there is no location target then get one.
            if (_Location == null)
            {
                _Location = WorldManager.Instance.GetLocation ();
            }
        }

        public StateType Process (AI agent)
        {
            Log.ProcessingState ("Search", agent);

            // If during the course of the goal we've already reached or lost the target, get a new one.
            if (_Location == null)
            {
                MoveToLocation (agent);
            }

            // If we've reached our location, go to a new one.
            if (IsAtLocation (agent))
            {
                MoveToLocation (agent);
            }

            return StateType.Active;
        }

        private bool IsAtLocation (AI agent)
        {
            if (Vector3.Distance (agent.transform.position, _Location.transform.position) <= _MinDistance)
            {
                return true;
            }

            return false;
        }

        private void MoveToLocation (AI agent)
        {
            //TODO: Pick a random location near the location chosen using random insideunitsphere or other means.
            _Location = WorldManager.Instance.GetLocation ();
            agent.Actions.MoveTo (_Location);
        }

        public void Exit (AI agent)
        {
            Log.ExitedState ("Search", agent);
            _Location = null;
        }

        public bool HandleMessage (Message message)
        {
            return true;
        }

        public void AddSubState (IState<AI> subState) { }
    }
}
