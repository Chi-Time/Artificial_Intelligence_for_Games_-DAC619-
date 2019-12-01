using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.AI_System
{
    public interface IState<T> : IListener where T : class
    {
        void Enter (T agent);
        void Process (T agent);
        void Exit (T agent);
    }
}
