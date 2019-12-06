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
            Log.EnteredState ("AttackEnemy", agent);
            CurrentState = GoalState.Inactive;
        }

        public GoalState Process (AI agent)
        {
            Log.ProcessingState ("AttackEnemy", agent);

            if (agent.Targeting.IsTargetPresent () == false)
            {
                agent.Targeting.SelectTarget ();
            }

            if (agent.Senses.IsInAttackRange (agent.Targeting.CurrentTarget))
            {
                agent.Actions.AttackEnemy (agent.Targeting.CurrentTarget);
            }
            else
            {
                agent.Actions.MoveTo (agent.Targeting.CurrentTarget);
            }

            return CurrentState = GoalState.Active;
        }

        public void Exit (AI agent)
        {
            Log.ExitedState ("AttackEnemy", agent);
            CurrentState = GoalState.Inactive;
        }

        public bool HandleMessage (Message message)
        {
            return false;
        }

        public void AddSubGoal (IGoal<AI> subState) { throw new System.NotImplementedException (); }
    }
}
