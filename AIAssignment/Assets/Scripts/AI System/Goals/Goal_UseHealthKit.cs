using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.AI_System.Goals
{
    class Goal_UseHealthKit : IGoal<AI>
    {
        public GoalState CurrentState { get; private set; }

        public void Enter (AI agent)
        {
            Log.EnteredState ("UseHealthKit", agent);

            CurrentState = GoalState.Inactive;
        }

        public GoalState Process (AI agent)
        {
            Log.ProcessingState ("UseHealthKit", agent);

            if (agent.Inventory.HasItem (Names.HealthKit))
            {
                var item = agent.Inventory.GetItem (Names.HealthKit);
                agent.Actions.UseItem (item);

                return CurrentState = GoalState.Complete;
            }
            else
            {
                return CurrentState = GoalState.Failed;
            }
        }

        public void Exit (AI agent)
        {
            Log.ExitedState ("UseHealthKit", agent);

            CurrentState = GoalState.Inactive;
        }

        public void AddSubGoal (IGoal<AI> subState) { }
    }
}
