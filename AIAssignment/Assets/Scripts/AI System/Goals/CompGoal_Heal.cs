using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.Goals
{
    /// <summary>Responsible for making an agent find and then use a health kit item.</summary>
    class CompGoal_Heal : CompositeGoal<AI>
    {
        public override void Enter (AI agent)
        {
            // Setup our composite goal in reverse order so that our stack works correctly.
            Log.EnteredGoal ("Heal_Comp", agent);
            AddSubGoal (new Goal_UseItem (Names.HealthKit));
            AddSubGoal (new Goal_GetItem (WorldManager.Instance.HealthKitSpawner, Names.HealthKit));

            base.Enter (agent);
        }

        public override GoalState Process (AI agent)
        {
            Log.ProcessingGoal ("Heal_Comp", agent);

            // Run the base sub goals processor to execute the logic in the sub goals.
            return base.Process (agent);
        }

        public override void Exit (AI agent)
        {
            base.Exit (agent);

            Log.ExitedGoal ("Heal_Comp", agent);
        }
    }
}
