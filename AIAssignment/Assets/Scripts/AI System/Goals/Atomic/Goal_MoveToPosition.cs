using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System.Goals
{
    class Goal_MoveToPosition : Goal<AI>
    {
        private Vector3 _Position = Vector3.zero;

        public Goal_MoveToPosition (AI agent, Vector3 position) : base (agent)
        {
            this._Agent = agent;
            this._Position = position;
        }

        public override void AddSubGoal (Goal<AI> subGoal)
        {
            throw new NotImplementedException ();
        }

        public override void Activate ()
        {
            _CurrentStatus = GoalStatus.Active;

            Debug.Log ("Called");
        }

        public override GoalStatus Process ()
        {
            if (_CurrentStatus == GoalStatus.Inactive)
                Activate ();

            _Agent.Actions.MoveTo (_Position);

            Debug.Log ("Moving");

            if (_Agent.transform.position == _Position)
                _CurrentStatus = GoalStatus.Completed;

            return _CurrentStatus;
        }

        public override void Terminate () { }
    }
}
