using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.States
{
    //TODO: Add in current state for each goal so that we can determine if they have passed or failed.
    class State_AttackEnemy : IState<AI>
    {
        private GameObject _Target = null;

        public State_AttackEnemy (GameObject target)
        {
            this._Target = target;
        }

        public void Enter (AI agent)
        {
            Log.EnteredState ("AttackEnemy", agent);
        }

        public StateType Process (AI agent)
        {
            Log.ProcessingState ("AttackEnemy", agent);

            //TODO: Determine if enemy was killed or lost from sight.
            if (agent.Targeting.IsTargetPresent () == false)
            {
                agent.Brain.ChangeState (new State_Wander ());
                return StateType.Failed;
            }

            if (agent.Senses.IsInAttackRange (_Target))
            {
                agent.Actions.AttackEnemy (_Target);
            }
            else
            {
                agent.Actions.MoveTo (_Target);
            }

            return StateType.Active;
        }

        public void Exit (AI agent)
        {
            Log.ExitedState ("AttackEnemy", agent);
        }

        public bool HandleMessage (Message message)
        {
            return false;
        }

        public void AddSubState (IState<AI> subState) { throw new System.NotImplementedException (); }
    }
}
