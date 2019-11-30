using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.AI_System.Goals.Composite
{
    class CompGoal_Explore : CompositeGoal<AI>
    {
        public CompGoal_Explore (AI agent) : base (agent)
        {
            this._Agent = agent;
        }

        public override void Activate ()
        {
            _CurrentStatus = GoalStatus.Active;
        }

        public override GoalStatus Process ()
        {
            if (_CurrentStatus == GoalStatus.Inactive)
                Activate ();

            //AddSubGoal (new Goal_MoveToPosition (_Agent, item.transform.position));

            return _CurrentStatus;
        }

        public override void Terminate ()
        {
        }
    }
}
