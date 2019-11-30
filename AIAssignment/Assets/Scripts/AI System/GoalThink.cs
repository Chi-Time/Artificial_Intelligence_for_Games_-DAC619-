using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.AI_System.Goals;

namespace Assets.Scripts.AI_System
{
    public class GoalThink : CompositeGoal<AI>
    {
        protected List<Evaluator> _Evaluators = new List<Evaluator> ();

        public GoalThink (AI agent) : base (agent)
        {
            this._Agent = agent;

            _Evaluators.Add (new GetHealthEvaluator ());
        }

        public override void Activate ()
        {
            Arbitrate ();
            _CurrentStatus = GoalStatus.Active;
        }

        public override GoalStatus Process ()
        {
            if (IsInactive)
                _CurrentStatus = GoalStatus.Active;

            var subGoalStatus = ProcessSubgoals ();

            if (subGoalStatus == GoalStatus.Completed || subGoalStatus == GoalStatus.Failed)
            {
                _CurrentStatus = GoalStatus.Inactive;
            }

            return _CurrentStatus;
        }

        public override void Terminate () {}

        public virtual void Arbitrate ()
        {
            float best = -1.0f;

            Evaluator desirable = null;

            foreach (Evaluator evaluator in _Evaluators)
            {
                float desirablity = evaluator.CalculateDesirability (_Agent);

                if (desirablity >= best)
                {
                    best = desirablity;
                    desirable = evaluator;
                }
            }

            desirable.SetGoal (_Agent);
        }

        public void AddGoal_Health (CompGoal_GetHealth healthGoal)
        {
            RemoveAllSubGoals ();
            AddSubGoal (healthGoal);
        }
    }
}
