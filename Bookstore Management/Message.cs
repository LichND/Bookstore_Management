using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Management
{
    public interface IMessageEvent
    {
        event EventHandler<MessageEventArgs> IsHaveMessage;
    }
    public class Message
    {
        public object Sender { get; set; }
        public MessageType Type { get; set; } = MessageType.None;
        public enum MessageType
        {
            None, Quit, Back, OK, Logout, Search
        }
        public Message(MessageType t)
        {
            Type = t;
        }
        public Message() { }
    }
    public class MessageEventArgs
    {
        public Message Message { get; private set; }
        public MessageEventArgs(Message m)
        {
            Message = m;
        }
    }
}
