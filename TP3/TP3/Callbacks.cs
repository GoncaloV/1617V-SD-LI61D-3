using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TP3
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class Callbacks : IUserCallback
    {
        public List<ChatUser> userList { get; set;}
        private readonly object nLock = new object();

        public Callbacks()
        {
            this.userList = new List<ChatUser>();
        }

        public void NotifySubscribe(ChatUser info)
        {
            lock (nLock)
            {
                if(userList.Find((s) => s.Binding.Equals(info.Binding)) == null)
                    userList.Add(info);
            }
        }

        public void NotifyUnsubscribe(ChatUser info)
        {

            lock (nLock)
                userList.Remove(userList.Single(u => u.Binding.Equals(info.Binding)));
        }
    }
}
