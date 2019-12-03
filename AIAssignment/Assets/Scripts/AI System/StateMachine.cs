using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        public IState<T> GlobalState { get; private set; }
        /// <summary>Reference to the currently active state in the machine.</summary>
        public IState<T> CurrentState { get; private set; }
        /// <summary>Reference to the previous state of the machine.</summary>
        public IState<T> PreviousState { get; private set; }

        /// <summary>Regulator for controlling global state ticks.</summary>
        private Regulator _GlobalRegulator = new Regulator (AISystem.GlobalDelay);
        /// <summary>Regulator for controlling current state ticks.</summary>
        private Regulator _CurrentStateRegulator = new Regulator (AISystem.CurrentDelay);

        /// <summary>Creates a new state machine instance and assign's it an owner.</summary>
        /// <param name="agent">The agent who owns the machine.</param>
        public StateMachine (T agent)
        {
            this.Agent = agent;
        }

        /// <summary>Updates the machine's states.</summary>
        public void Process ()
        {
            // If there is a state available and the regulator says we can process it.
            if (GlobalState != null && _GlobalRegulator.CanProcess ())
                // Tick the state and perform it's logic.
                GlobalState.Process (Agent);

            if (CurrentState != null && _CurrentStateRegulator.CanProcess ())
                CurrentState.Process (Agent);
        }

        /// <summary>Change the currently active state of the machine.</summary>
        /// <param name="newState">The new state for the machine to switch to.</param>
        public void ChangeState (IState<T> newState)
        {
            // Reset our previous state to this one.
            PreviousState = CurrentState;
            // Exit out of the current state.
            CurrentState.Exit (Agent);

            // Switch our current state to the new one and enter it.
            CurrentState = newState;
            CurrentState.Enter (Agent);
        }

        /// <summary>Revert's the machine back to a previous state in memory.</summary>
        public void RevertToPreviousState ()
        {
            ChangeState (PreviousState);
        }

        /// <summary>Returns true if the machine is in the given state.</summary>
        /// <param name="state">The state to test for.</param>
        public bool IsInState (IState<T> state)
        {
            // Compare if the given state type is the same as our current one.
            if (CurrentState.GetType () == state.GetType ())
                return true;

            return false;
        }

        /// <summary>Set's the current agent's state to that of the one provided.</summary>
        /// <param name="newState">The new state to mark and monitor as the current.</param>
        public void SetCurrentState (IState<T> newState) => CurrentState = newState;

        /// <summary>Set's the current agent's global state to that of the one provided.</summary>
        /// <param name="newState">The new state to mark and monitor as the global.</param>
        public void SetGlobalState (IState<T> newState) => GlobalState = newState;

        /// <summary>Set's the agent's previous state to that of the one provided.</summary>
        /// <param name="newState">The new state to mark as the previous.</param>
        public void SetPreviousState (IState<T> newState) => PreviousState = newState;

        public bool HandleMessage (Message message)
        {
            // If there is a state available and it can process the message.
            if (CurrentState != null && CurrentState.HandleMessage (message))
            {
                // Return that the message was handled.
                return true;
            }

            if (GlobalState != null && GlobalState.HandleMessage (message))
            {
                return true;
            }

            return false;
        }
    }
}
