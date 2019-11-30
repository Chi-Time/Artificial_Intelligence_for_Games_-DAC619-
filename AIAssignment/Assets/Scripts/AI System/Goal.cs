using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System
{
    /// <summary>The various states that a goal can be in at any one time.</summary>
    public enum GoalStatus
    {
        Inactive = 0,
        Active,
        Completed,
        Failed
    }

    public abstract class Goal<T> where T : class
    {
        #region Properties
        /// <summary>Returns true if the goal is currently in an active state.</summary>
        public bool IsActive {
            get {
                return _CurrentStatus == GoalStatus.Active;
            }
        }

        /// <summary>Returns true if the goal is currently in an inactive state.</summary>
        public bool IsInactive {
            get {
                return _CurrentStatus == GoalStatus.Inactive;
            }
        }

        /// <summary>Returns true if the goal has been completed.</summary>
        public bool IsComplete {
            get {
                return _CurrentStatus == GoalStatus.Completed;
            }
        }

        /// <summary>Returns true if the goal has failed.</summary>
        public bool HasFailed {
            get {
                return _CurrentStatus == GoalStatus.Failed;
            }
        }

        /// <summary>Returns a reference to the agent who owns this goal.</summary>
        public T Agent {
            get {
                return _Agent;
            }
        }
        #endregion

        #region Members
        /// <summary>Reference to the current agent who owns this goal.</summary>
        protected T _Agent = null;
        /// <summary>The current status of the goal and what it's doing.</summary>
        protected GoalStatus _CurrentStatus = GoalStatus.Inactive;
        #endregion

        #region Public Methods
        /// <summary>Constructs a new goal and gives it an owner agent.</summary>
        /// <param name="agent">The agent to own the goal.</param>
        public Goal (T agent)
        {
            this._Agent = agent;
        }

        /// <summary>Activates the goal and sets it up for use.</summary>
        public abstract void Activate ();

        /// <summary>Processes the goal and it's actions.</summary>
        /// <returns>The current goal's status during it's action.</returns>
        public abstract GoalStatus Process ();

        /// <summary>Terminates the goal and cleans it up.</summary>
        public abstract void Terminate ();

        /// <summary>Adds a sub goal to this goal.</summary>
        /// <param name="subGoal">The goal to add as a child of this one.</param>
        public abstract void AddSubGoal (Goal<T> subGoal);
        #endregion

        protected void ActivateIfInactive ()
        {
            if (IsInactive)
                _CurrentStatus = GoalStatus.Active;
        }

        protected void ActivateIfFailed ()
        {
            if (HasFailed)
                _CurrentStatus = GoalStatus.Active;
        }
    }

    public abstract class CompositeGoal<T> : Goal<T> where T : class
    {
        protected Stack<Goal<T>> _SubGoals = new Stack<Goal<T>> ();

        public CompositeGoal (T agent) : base (agent)
        {
            this._Agent = agent;
        }

        public override void AddSubGoal (Goal<T> subGoal)
        {
            _SubGoals.Push (subGoal);
        }

        public virtual GoalStatus ProcessSubgoals ()
        {
            RemoveFinishedGoals ();

            // If there are still sub goals left.
            if (_SubGoals.Count > 0)
            {
                Debug.Log ("Looping Goals");
                // Process the one at the top of the stack.
                GoalStatus frontGoalStatus = _SubGoals.Peek ().Process ();

                Debug.Log ("Processing Front Goal: " + frontGoalStatus);

                // Test to see if we have completed the current goal and that there are still goals left.
                if (frontGoalStatus == GoalStatus.Completed && _SubGoals.Count == 1)
                {
                    Debug.Log ("We're still processing");
                    // Return active as there are still sub goals to process.
                    return GoalStatus.Active;
                }

                // Return the current status of our sub goal.
                return frontGoalStatus;
            }
            // No goals left? We've completed our composite.
            else
            {
                Debug.Log ("All sub goals done!");
                return GoalStatus.Completed;
            }
        }

        protected void RemoveFinishedGoals ()
        {
            while (IsGoalsStillToBeRemoved ())
            {
                var goal = _SubGoals.Peek ();

                if (goal.IsComplete || goal.HasFailed)
                {
                    goal.Terminate ();
                    _SubGoals.Pop ();
                }
            }
        }

        protected bool IsGoalsStillToBeRemoved ()
        {
            if (_SubGoals.Count > 0 && ( _SubGoals.Peek ().IsComplete || _SubGoals.Peek ().HasFailed ))
                return true;

            return false;
        }

        public virtual void RemoveAllSubGoals ()
        {
            foreach (Goal<T> goal in _SubGoals)
                goal.Terminate ();

            _SubGoals.Clear ();
        }

        ~CompositeGoal ()
        {
            RemoveAllSubGoals ();
        }
    }
}
