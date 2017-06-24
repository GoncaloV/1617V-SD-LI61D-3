using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Interfaces;

namespace User{

    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class Callbacks : IUserCallback
    {

        public List<ChatUser> userList { get; set; }
        private readonly object nLock = new object();


        public Callbacks()
        {
            this.userList = new List<ChatUser>();
        }

        public void NotifySubscribe(ChatUser user)
        {
            lock (nLock)
            {
                if (userList.Find((u) => u.Binding.Equals(user.Binding)) == null)
                    userList.Add(user);
            }
        }

        public void NotifyUnsubscribe(ChatUser user)
        {
            lock (nLock)
            {
                userList.RemoveAll(u => u.Binding.Equals(user.Binding));
            }
        }
    }
}
