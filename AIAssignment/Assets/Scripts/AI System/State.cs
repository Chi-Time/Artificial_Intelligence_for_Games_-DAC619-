using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.AI_System.States;

namespace Assets.Scripts.AI_System
{
    /// <summary>The type which the state is currently in.</summary>
    public enum StateType { Inactive, Active, Failed, Complete }

    /// <summary>Interface for all base state behaviours.</summary>
    /// <typeparam name="T">The class who owns this state.</typeparam>
    public interface IState<T> : IListener where T : class
    {
        void Enter (T agent);
        StateType Process (T agent);
        void Exit (T agent);
        void AddSubState (IState<T> subState);
    }

    public abstract class CompositeState<T> : IState<T> where T : class
    {
        /// <summary>The various sub states this composite state has left to process.</summary>
        public Stack<IState<T>> SubStates { get; private set; }

        protected IState<T> _CurrentSubState = null;

        public CompositeState ()
        {
            SubStates = new Stack<IState<T>> ();
        }

        public virtual void Enter (T agent)
        {
            _CurrentSubState = SubStates.Pop ();
            _CurrentSubState.Enter (agent);
        }

        public virtual StateType Process (T agent)
        {
            // Return the overall result of the sub states processing.
            return ProcessSubStates (agent);
        }

        protected virtual StateType ProcessSubStates (T agent)
        {
            // If there is no current sub state then return complete as there's nothing left.
            if (_CurrentSubState == null)
                return StateType.Complete;

            // If there are currently still sub states left to process.
            if (SubStates.Count > 0)
            {
                // Process the substate and if it has completed it's task.
                if (_CurrentSubState.Process (agent) == StateType.Complete)
                {
                    // Exit the current state.
                    _CurrentSubState.Exit (agent);
                    // Switch to the new state by popping it off the stack.
                    _CurrentSubState = SubStates.Pop ();
                    // Enter the new substate.
                    _CurrentSubState.Enter (agent);
                    // Return active as we're still processing.
                    return StateType.Active;
                }
                // Process the substate and if it has failed.
                else if (_CurrentSubState.Process (agent) == StateType.Failed)
                {
                    // Return that the entire composite state has failed.
                    return StateType.Failed;
                }
            }
            // Is there still currently a state left to check?
            else if (_CurrentSubState != null)
            {
                // Process the substate and if it has completed it's task.
                if (_CurrentSubState.Process (agent) == StateType.Complete)
                {
                    return StateType.Complete;
                }
            }

            return StateType.Active;
        }

        public virtual void Exit (T agent)
        {
            _CurrentSubState = null;
            SubStates.Clear ();
        }

        public virtual void AddSubState (IState<T> subState)
        {
            SubStates.Push (subState);
        }

        public virtual bool HandleMessage (Message message)
        {
            return false;
        }
    }
}
