using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System
{
    public interface IListener
    {
        bool HandleMessage (Message message);
    }

    /// <summary>Compares two telegrams by their message delay length.</summary>
    public class ByDelayTime : IComparer<Message>
    {
        public int Compare (Message x, Message y)
        {
            return x.MessageDelay.CompareTo (y.MessageDelay);
        }
    }

    public class MessageDispatcher
    {
        public static readonly MessageDispatcher Instance = new MessageDispatcher ();

        private SortedSet<Message> _PriorityQueue = new SortedSet<Message> (new ByDelayTime ());

        static MessageDispatcher () { }
        private MessageDispatcher () { }

        public void DispatchMessage (IListener sender, IListener receiver, float delay = 0.0f, object information = null, MessageType type = MessageType.Normal)
        {
            var message = new Message ()
            {
                MessageDelay = delay,
                Information = information,
                Sender = sender,
                Receiver = receiver,
                MessageType = type
            };

            if (delay <= 0.0f)
            {
                Discharge (message);
            }
            else
            {
                float currentTime = Time.time;
                message.MessageDelay = currentTime + delay;
            }
        }

        public void DispatchDelayedMessages ()
        {
            float currentTime = Time.time;
            
            while (_PriorityQueue.First ().MessageDelay < currentTime && _PriorityQueue.First ().MessageDelay > 0.0f)
            {
                Message message = _PriorityQueue.First ();

                Discharge (message);

                _PriorityQueue.Remove (message);
            }
        }

        private void Discharge (Message message)
        {
            if (!message.Receiver.HandleMessage (message))
            {
                Debug.Log ("Message not handled by receiver.");
            }
        }
    }    
}
