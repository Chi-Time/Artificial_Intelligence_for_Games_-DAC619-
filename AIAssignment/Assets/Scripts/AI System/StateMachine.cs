using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.AI_System
{
    //TODO: Consider a list of global states to keep track of.
    //TODO: Consider how you could define state transitions through the use of utility values.
    //TODO: Consider how to make atomic and composite states and treat states as just goals.
    public class StateMachine<T> : IListener where T : class
    {
        /// <summary>Reference to the owner of the machine.</summary>
        public T Agent { get; private set; }
        /// <summary>Reference to the current global state in the machine.</summary>
        public IState<T> _GlobalState { get; private set; }
        /// <summary>Reference to the currently active state in the machine.</summary>
        public IState<T> _CurrentState { get; private set; }
        /// <summary>Reference to the previous state of the machine.</summary>
        public IState<T> _PreviousState { get; private set; }

        /// <summary>Creates a new state machine instance and assign's it an owner.</summary>
        /// <param name="agent">The agent who owns the machine.</param>
        public StateMachine (T agent)
        {
            this.Agent = agent;
        }

        /// <summary>Updates the machine's states.</summary>
        public void Process ()
        {
            if (_GlobalState != null)
                _GlobalState.Process (Agent);

            if (_CurrentState != null)
                _CurrentState.Process (Agent);
        }

        /// <summary>Change the currently active state of the machine.</summary>
        /// <param name="newState">The new state for the machine to switch to.</param>
        public void ChangeState (IState<T> newState)
        {
            _PreviousState = _CurrentState;
            _CurrentState.Exit (Agent);

            _CurrentState = newState;
            _CurrentState.Enter (Agent);
        }

        /// <summary>Revert's the machine back to a previous state in memory.</summary>
        public void RevertToPreviousState ()
        {
            ChangeState (_PreviousState);
        }

        /// <summary>Returns true if the machine is in the given state.</summary>
        /// <param name="state">The state to test for.</param>
        public bool IsInState (IState<T> state)
        {
            if (_CurrentState.GetType () == state.GetType ())
                return true;

            return false;
        }

        /// <summary>Set's the current agent's state to that of the one provided.</summary>
        /// <param name="newState">The new state to mark and monitor as the current.</param>
        public void SetCurrentState (IState<T> newState) => _CurrentState = newState;

        /// <summary>Set's the current agent's global state to that of the one provided.</summary>
        /// <param name="newState">The new state to mark and monitor as the global.</param>
        public void SetGlobalState (IState<T> newState) => _GlobalState = newState;

        /// <summary>Set's the agent's previous state to that of the one provided.</summary>
        /// <param name="newState">The new state to mark as the previous.</param>
        public void SetPreviousState (IState<T> newState) => _PreviousState = newState;

        public bool HandleMessage (Message message)
        {
            if (_CurrentState != null && _CurrentState.HandleMessage (message))
            {
                return true;
            }

            if (_GlobalState != null && _GlobalState.HandleMessage (message))
            {
                return true;
            }

            return false;
        }
    }
}
