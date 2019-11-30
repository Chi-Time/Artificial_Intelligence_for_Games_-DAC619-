using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.Goals
{
    public class CompGoal_GetHealth : CompositeGoal<AI>
    {
        private GameObject _Item = null;

        public CompGoal_GetHealth (AI agent) : base (agent)
        {
            this._Agent = agent;
        }

        public override void Activate ()
        {
            _CurrentStatus = GoalStatus.Active;

            _Item = GameObject.Find (Names.HealthKit);

            if (_Item == null)
                _CurrentStatus = GoalStatus.Failed;
            else
                AddSubGoal (new Goal_MoveToPosition (_Agent, _Item.transform.position));
        }

        public override GoalStatus Process ()
        {
            if (_CurrentStatus == GoalStatus.Inactive)
                Activate ();

            if (_Item == null)
                _CurrentStatus = GoalStatus.Failed;

            return _CurrentStatus;
        }

        public override void Terminate ()
        {
        }
    }
}
