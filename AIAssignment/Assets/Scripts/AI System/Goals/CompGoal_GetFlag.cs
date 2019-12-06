using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.AI_System.Goals
{
    class CompGoal_GetFlag : CompositeGoal<AI>
    {
        /// <summary>Reference to the name of the flag to get.</summary>
        private string _Flag = "";

        /// <summary>Crreates a new goal for finding and grabbing the given flag.</summary>
        /// <param name="flag">The flag to find and get.</param>
        public CompGoal_GetFlag (string flag)
        {
            this._Flag = flag;
        }

        public override void Enter (AI agent)
        {
            // Setup our composite goal in reverse order so that our stack works correctly.
            Log.EnteredState ("Get Flag", agent);
            AddSubGoal (new Goal_CollectFlag (_Flag));
            AddSubGoal (new Goal_FindFlag (_Flag));

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
