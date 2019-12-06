using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.Goals
{
    class Goal_CollectFlag : IGoal<AI>
    {
        public GoalState CurrentState { get; private set; }

        /// <summary>Reference to the name of the flag to grab.</summary>
        private string _Flag = "";
        
        public Goal_CollectFlag (string flag)
        {
            this._Flag = flag;
        }

        public void Enter (AI agent)
        {
            Log.EnteredGoal ("CollectFlag", agent);
        }

        public GoalState Process (AI agent)
        {
            Log.ProcessingGoal ("CollectFlag", agent);

            // Can we see the flag in our sight?
            GameObject flag = GetFlagInView (agent);

            // If we can then move towards and collect it.
            if (flag != null)
            {
                agent.Actions.MoveTo (flag);
                agent.Actions.CollectItem (flag);

                // If we've managed to pick up the flag then return that our task is completed.
                if (agent.Inventory.HasItem (flag.name))
                {
                    return CurrentState = GoalState.Complete;
                }
            }
            // Return that our goal has failed as there's no flag in sight and we don't know where it is.
            else
            {
                return CurrentState = GoalState.Failed;
            }

            return CurrentState = GoalState.Active;
        }

        /// <summary>Retrieve the flag in our sights if one exists.</summary>
        /// <param name="agent">The agent to use as our eyes.</param>
        /// <returns>The flag we can see if one exists, null if not.</returns>
        private GameObject GetFlagInView (AI agent)
        {
            // Grab an item in our view from our senses.
            GameObject viewedItem = agent.Senses.GetObjectInViewByName (_Flag);

            return viewedItem;
        }

        public void Exit (AI agent)
        {
            Log.ExitedGoal ("CollectFlag", agent);
        }

        public void AddSubGoal (IGoal<AI> subState) { throw new System.NotImplementedException (); }
    }
}
