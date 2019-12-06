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

        private string _ItemName = "";
        private const float _MinDistance = 2f;
        private GameObject _ItemLocation = null;

        public Goal_GetItem (GameObject itemLocation, string itemName)
        {
            this._ItemLocation = itemLocation;
            this._ItemName = itemName;
        }

        public void Enter (AI agent)
        {
            Log.EnteredState ("GetItem: " + _ItemLocation.name, agent);
        }

        public GoalState Process (AI agent)
        {
            Log.ProcessingState ("GetItem: " + _ItemLocation.name, agent);
            
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
                    Debug.Log ("<color=cyan>ITS NOT THERE</color>");
                    return CurrentState = GoalState.Failed;
                }
            }

            return CurrentState = GoalState.Active;
        }

        // Determine if we're close to the object by a minimum distance.
        private bool IsInRange (AI agent)
        {
            if (Vector3.Distance (agent.transform.position, _ItemLocation.transform.position) <= _MinDistance)
                return true;

            return false;
        }

        // Loop through every seen item and check if it's the one we're looking for.
        private bool CanSeeItem (AI agent)
        {
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

        public bool HandleMessage (Message message)
        {
            return true;
        }

        public void AddSubGoal (IGoal<AI> state) { }
    }
}
