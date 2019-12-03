using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.AI_System.States
{
    class State_UseHealthKit : IState<AI>
    {
        public void AddSubState (IState<AI> subState) { }

        public void Enter (AI agent)
        {
            Log.EnteredState ("UseHealthKit", agent);
        }

        public StateType Process (AI agent)
        {
            Log.ProcessingState ("UseHealthKit", agent);

            if (agent.Inventory.HasItem (Names.HealthKit))
            {
                UnityEngine.Debug.Log ("I have it.");
                var item = agent.Inventory.GetItem (Names.HealthKit);
                agent.Actions.UseItem (item);

                return StateType.Complete;
            }

            return StateType.Active;
        }

        public void Exit (AI agent)
        {
            Log.ExitedState ("UseHealthKit", agent);
        }

        public bool HandleMessage (Message message)
        {
            return false;
        }
    }
}
