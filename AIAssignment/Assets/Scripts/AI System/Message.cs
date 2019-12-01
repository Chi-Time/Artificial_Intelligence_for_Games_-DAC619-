using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System
{
    public enum MessageType { Normal, Weird }

    public struct Message
    {
        public IListener Sender;
        public IListener Receiver;
        public float MessageDelay;
        public object Information;
        public MessageType MessageType;
    }
}
