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

        /// <summary>Reference to the object to drop.</summary>
        private GameObject _Object = null;

        /// <summary>Creates a new goal with an object drop.</summary>
        /// <param name="obj">The object to drop in this goal.</param>
        public Goal_DropObject (GameObject obj)
        {
            this._Object = obj;
        }

        public void Enter (AI agent)
        {
            Log.EnteredGoal ("DropObject", agent);

            // Attempt to drop the item.
            agent.Actions.DropItem (_Object);
        }

        public GoalState Process (AI agent)
        {
            Log.ProcessingGoal ("DropObject", agent);

            // Do we still have the item? If so, try and drop it.
            if (agent.Inventory.HasItem (_Object.name))
            {
                agent.Actions.DropItem (_Object);
            }
            // If not, return that we finished our job.
            else
            {
                return CurrentState = GoalState.Complete;
            }

            return CurrentState = GoalState.Active;
        }

        public void Exit (AI agent)
        {
            Log.ExitedGoal ("DropObject", agent);

            // Clean up after ourselves.
            _Object = null;
        }

        public void AddSubGoal (IGoal<AI> subState) { throw new System.NotImplementedException (); }
    }
}
