using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.Goals
{
    class Goal_GetItem : IGoal<AI>
    {
        public GoalState CurrentState { get; private set; }

        /// <summary>The name of the item to collect.</summary>
        private string _ItemName = "";
        /// <summary>The minimum distance until we can tell if we can see it.</summary>
        private const float _MinDistance = 2f;
        /// <summary>The location the item might be at.</summary>
        private GameObject _ItemLocation = null;

        /// <summary>Creates a new goal to go to an item location and try to get it if it's there.</summary>
        /// <param name="itemLocation">The location to check for.</param>
        /// <param name="itemName">The name of the item to get.</param>
        public Goal_GetItem (GameObject itemLocation, string itemName)
        {
            this._ItemLocation = itemLocation;
            this._ItemName = itemName;
        }

        public void Enter (AI agent)
        {
            Log.EnteredState ("GetItem: " + _ItemLocation.name, agent);

            CurrentState = GoalState.Inactive;
        }

        public GoalState Process (AI agent)
        {
            Log.ProcessingState ("GetItem: " + _ItemLocation.name, agent);
            
            // If the location of the item is null then we've failed to get it as there's nowhere to go.
            if (_ItemLocation == null)
            {
                return CurrentState = GoalState.Failed;
            }

            // Move towards the item positon.
            agent.Actions.MoveTo (_ItemLocation);

            // Are we close enough to begin seeing the item?
            if (IsInRange (agent))
            {
                // Is it actually there? If so, try and grab it.
                var item = agent.Senses.GetObjectInViewByName (_ItemName);

                if (item != null)
                {
                    agent.Actions.CollectItem (item);

                    // Have we managed to pick it up?
                    if (agent.Inventory.HasItem (_ItemName))
                        return CurrentState = GoalState.Complete;
                }
                // Is not there? Then we've failed.
                else
                {
                    return CurrentState = GoalState.Failed;
                }
            }

            return CurrentState = GoalState.Active;
        }

        // Determine if we're close to the object by a minimum distance.
        private bool IsInRange (AI agent)
        {
            if (agent.Actions.HasArrived (_MinDistance))
                return true;

            return false;
        }

        /// <summary>Can we see the item here?</summary>
        /// <param name="agent">The agent to use.</param>
        /// <returns>True if we can see the item, false if not.</returns>
        private bool CanSeeItem (AI agent)
        {
            // Loop through every seen item and check if it's the one we're looking for.
            var viewedItems = agent.Senses.GetCollectablesInView ();
            
            foreach (GameObject item in viewedItems)
            {
                if (item.name.Equals (_ItemName))
                    return true;
            }
            return false;
        }

        public void Exit (AI agent)
        {
            Log.ExitedState ("GetItem: " + _ItemLocation.name, agent);
            _ItemLocation = null;
            _ItemName = "";
        }

        public void AddSubGoal (IGoal<AI> state) { }
    }
}
