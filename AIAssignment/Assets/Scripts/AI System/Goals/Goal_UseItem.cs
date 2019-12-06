using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.Goals
{
    class Goal_UseItem : IGoal<AI>
    {
        public GoalState CurrentState { get; private set; }
        private string _ItemName = "";

        public Goal_UseItem (string itemName)
        {
            this._ItemName = itemName;
        }

        public void Enter (AI agent)
        {
            Log.EnteredState ("UseItem", agent);
        }

        public GoalState Process (AI agent)
        {
            Log.ProcessingState ("UseItem", agent);

            // Do we have the item? If so, try and use it.
            if (agent.Inventory.HasItem (_ItemName))
            {
                GameObject item = agent.Inventory.GetItem (_ItemName);
                agent.Actions.UseItem (item);

                return CurrentState = GoalState.Complete;
            }
            // If we don't then we've failed this goal.
            else
            {
                return CurrentState = GoalState.Failed;
            }
        }

        public void Exit (AI agent)
        {
            Log.ExitedState ("UseItem", agent);
        }

        public bool HandleMessage (Message message)
        {
            return true;
        }

        public void AddSubGoal (IGoal<AI> state) { }
    }
}
