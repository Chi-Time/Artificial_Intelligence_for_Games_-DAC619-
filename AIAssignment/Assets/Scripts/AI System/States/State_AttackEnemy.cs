using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.States
{
    class State_AttackEnemy : IState<AI>
    {
        private AI _Enemy = null;

        public void Enter (AI agent)
        {
            Log.EnteredState ("AttackEnemy", agent);

            var enemiesInSight = agent.Senses.GetEnemiesInView ();

            if (enemiesInSight.Count > 0)
            {
                _Enemy = enemiesInSight[Random.Range (0, enemiesInSight.Count)].GetComponent<AI> ();

                if (_Enemy == null)
                    agent.Brain.ChangeState (new State_Wander ());
            }
            else
            {
                agent.Brain.ChangeState (new State_Wander ());
            }
        }

        public StateType Process (AI agent)
        {
            Log.ProcessingState ("AttackEnemy", agent);

            if (_Enemy == null)
            {
                agent.Brain.ChangeState (new State_Wander ());
                return StateType.Failed;
            }

            agent.Actions.AttackEnemy (_Enemy.gameObject);

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
