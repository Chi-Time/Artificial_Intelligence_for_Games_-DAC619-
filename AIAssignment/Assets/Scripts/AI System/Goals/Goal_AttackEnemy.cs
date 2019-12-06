using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.Goals
{
    class Goal_AttackEnemy : IGoal<AI>
    {
        public GoalState CurrentState { get; private set; }

        public void Enter (AI agent)
        {
            Log.EnteredGoal ("AttackEnemy", agent);
            CurrentState = GoalState.Inactive;
        }

        public GoalState Process (AI agent)
        {
            Log.ProcessingGoal ("AttackEnemy", agent);

            // If we don't have a target in sight, then get a new one.
            if (agent.Targeting.IsTargetPresent () == false)
            {
                agent.Targeting.SelectTarget ();
            }

            // If the target is in range to be attacked, then let's start hitting.
            if (agent.Senses.IsInAttackRange (agent.Targeting.CurrentTarget))
            {
                agent.Actions.AttackEnemy (agent.Targeting.CurrentTarget);
            }
            // If not, let's get closer to em.
            else
            {
                agent.Actions.MoveTo (agent.Targeting.CurrentTarget);
            }

            return CurrentState = GoalState.Active;
        }

        public void Exit (AI agent)
        {
            Log.ExitedGoal ("AttackEnemy", agent);
            CurrentState = GoalState.Inactive;
        }

        public void AddSubGoal (IGoal<AI> subState) { throw new System.NotImplementedException (); }
    }
}
