using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.AI_System
{
    /// <summary>Regulates a task by only allowing it to fire at set intervals.</summary>
    class Regulator
    {
        /// <summary>The current time that has elapsed for this regulator.</summary>
        private float _Timer = 0.0f;
        /// <summary>The delay between task calls.</summary>
        private float _ProcessDelay = 0.0f;

        /// <summary>Creates a new regulator which will only allow a task to fire every few seconds.</summary>
        /// <param name="delay">The delay (in seconds) before the task can happen.</param>
        public Regulator (float delay)
        {
            _ProcessDelay = delay;
        }

        /// <summary>Determines if a task can process.</summary>
        /// <returns>True if the task is allowed to process.</returns>
        public bool CanProcess ()
        {
            // Increase timer every frame, if it's above the threshold allow a tick and reset it.
            _Timer += UnityEngine.Time.deltaTime;

            if (_Timer >= _ProcessDelay)
            {
                _Timer = 0.0f;
                return true;
            }

            return false;
        }
    }
}
