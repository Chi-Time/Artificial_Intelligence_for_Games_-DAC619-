using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.AI_System.States
{
    class CompState_Heal : CompositeState<AI>
    {
        //TODO: Find a way to process failures in the global state as currently when thinks fail nothing happens.
        public override void Enter (AI agent)
        {
            //TODO: Find a better way to set the current state as having to place this code before the base call is just error prone.
            Log.EnteredState ("Heal_Comp", agent);
            AddSubState (new State_UseHealthKit ());
            AddSubState (new State_GetHealthKit ());

            base.Enter (agent);
        }

        public override StateType Process (AI agent)
        {
            Log.ProcessingState ("Heal_Comp", agent);

            UnityEngine.Debug.Log (base.Process (agent));

            return base.Process (agent);
        }

        public override void Exit (AI agent)
        {
            base.Exit (agent);

            Log.ExitedState ("Heal_Comp", agent);
        }
    }
}
