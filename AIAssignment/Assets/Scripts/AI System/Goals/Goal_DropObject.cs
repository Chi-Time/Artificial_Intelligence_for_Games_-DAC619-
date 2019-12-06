using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.Goals
{
    class Goal_DropObject : IGoal<AI>
    {
        public GoalState CurrentState { get; private set; }

        private GameObject _Object = null;

        public Goal_DropObject (GameObject obj)
        {
            this._Object = obj;
        }

        public void Enter (AI agent)
        {
            Log.EnteredState ("DropObject", agent);

            agent.Actions.DropItem (_Object);
        }

        public GoalState Process (AI agent)
        {
            Log.ProcessingState ("DropObject", agent);

            if (agent.Inventory.HasItem (_Object.name))
            {
                agent.Actions.DropItem (_Object);
            }
            else
            {
                return CurrentState = GoalState.Complete;
            }

            return CurrentState = GoalState.Active;
        }

        public void Exit (AI agent)
        {
            Log.ExitedState ("DropObject", agent);

            _Object = null;
        }

        public bool HandleMessage (Message message)
        {
            return false;
        }

        public void AddSubGoal (IGoal<AI> subState) { throw new System.NotImplementedException (); }
    }
}
