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
        private GameObject _Flag = null;

        public CompGoal_ScoreFlag (GameObject flag)
        {
            this._Flag = flag;
        }

        //TODO: Find a way to process failures in the global state as currently when thinks fail nothing happens.
        public override void Enter (AI agent)
        {
            //TODO: Find a better way to set the current state as having to place this code before the base call is just error prone.
            Log.EnteredState ("Get Flag", agent);
            AddSubGoal (new Goal_DropObject  (_Flag));
            AddSubGoal (new Goal_MoveToBase ());

            base.Enter (agent);
        }

        public override GoalState Process (AI agent)
        {
            Log.ProcessingState ("Get Flag", agent);

            return base.Process (agent);
        }

        public override void Exit (AI agent)
        {
            base.Exit (agent);

            Log.ExitedState ("Get Flag", agent);
        }
    }
}
