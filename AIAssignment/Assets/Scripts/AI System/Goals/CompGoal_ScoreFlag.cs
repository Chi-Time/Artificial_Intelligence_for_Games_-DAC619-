using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.Goals
{
    class CompGoal_ScoreFlag : CompositeGoal<AI>
    {
        /// <summary>Reference to the flag we have.</summary>
        private GameObject _Flag = null;

        /// <summary>Crreates a new goal going home and dropping the given flag.</summary>
        /// <param name="flag">The flag to drop off..</param>
        public CompGoal_ScoreFlag (GameObject flag)
        {
            this._Flag = flag;
        }

        public override void Enter (AI agent)
        {
            // Setup our composite goal in reverse order so that our stack works correctly.
            Log.EnteredState ("Get Flag", agent);
            AddSubGoal (new Goal_DropObject  (_Flag));
            AddSubGoal (new Goal_MoveToBase ());

            base.Enter (agent);
        }

        public override GoalState Process (AI agent)
        {
            Log.ProcessingState ("Get Flag", agent);

            // Run the base sub goals processor to execute the logic in the sub goals.
            return base.Process (agent);
        }

        public override void Exit (AI agent)
        {
            base.Exit (agent);

            Log.ExitedState ("Get Flag", agent);
        }
    }
}
