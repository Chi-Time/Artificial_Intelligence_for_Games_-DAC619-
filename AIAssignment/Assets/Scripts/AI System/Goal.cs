using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.AI_System.Goals;

namespace Assets.Scripts.AI_System
{
    /// <summary>The state which the goal is currently in.</summary>
    public enum GoalState { Inactive, Active, Failed, Complete }

    /// <summary>Interface for all base goal behaviours.</summary>
    /// <typeparam name="T">The agent type to own this goal.</typeparam>
    public interface IGoal<T> where T : class
    {
        GoalState CurrentState { get; }

        void Enter (T agent);
        GoalState Process (T agent);
        void Exit (T agent);
        void AddSubGoal (IGoal<T> subState);
    }

    /// <summary>Base for all composite goals</summary>
    /// <typeparam name="T">The agent type to own this goal.</typeparam>
    public abstract class CompositeGoal<T> : IGoal<T> where T : class
    {
        public GoalState CurrentState { get; private set; }

        /// <summary>The various sub goals this composite goal has left to process.</summary>
        public Stack<IGoal<T>> SubGoals { get; private set; }

        /// <summary>The current sub goal we have selected from our stack.</summary>
        protected IGoal<T> _CurrentSubGoal = null;

        public CompositeGoal ()
        {
            // Initialise our sub gaols with a default value.
            SubGoals = new Stack<IGoal<T>> ();
        }

        public virtual void Enter (T agent)
        {
            // Get the first item from our sub goals and enter into it.
            _CurrentSubGoal = SubGoals.Pop ();
            _CurrentSubGoal.Enter (agent);
        }

        public virtual GoalState Process (T agent)
        {
            // Return the overall result of the sub goals processing.
            return ProcessSubGoals (agent);
        }

        protected virtual GoalState ProcessSubGoals (T agent)
        {
            // If there is no current sub goal then return complete as there's nothing left.
            if (_CurrentSubGoal == null)
                return GoalState.Complete;

            // If there are currently still sub goals left to process.
            if (SubGoals.Count > 0)
            {
                // Process the subgoal and if it has completed it's task.
                if (_CurrentSubGoal.Process (agent) == GoalState.Complete)
                {
                    // Exit the current goal.
                    _CurrentSubGoal.Exit (agent);
                    // Switch to the new goal by popping it off the stack.
                    _CurrentSubGoal = SubGoals.Pop ();
                    // Enter the new subgoal.
                    _CurrentSubGoal.Enter (agent);
                    // Return active as we're still processing.
                    return GoalState.Active;
                }
                // Process the subgoal and if it has failed.
                else if (_CurrentSubGoal.Process (agent) == GoalState.Failed)
                {
                    // Return that the entire composite goal has failed.
                    return GoalState.Failed;
                }
            }
            // Is there still currently a goal left to check?
            else if (_CurrentSubGoal != null)
            {
                // Process the subgoal and if it has completed it's task.
                if (_CurrentSubGoal.Process (agent) == GoalState.Complete)
                {
                    _CurrentSubGoal = null;
                    return GoalState.Complete;
                }
            }

            return GoalState.Active;
        }

        public virtual void Exit (T agent)
        {
            // Clean up after ourselves.
            _CurrentSubGoal = null;
            SubGoals.Clear ();
        }

        /// <summary>Add's a sub goal to this compoosite goal</summary>
        /// <param name="subGoal">The new sub goal to add as a child.</param>
        public virtual void AddSubGoal (IGoal<T> subGoal)
        {
            SubGoals.Push (subGoal);
        }
    }
}
