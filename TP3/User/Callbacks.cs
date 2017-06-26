using Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using User.RegisterService;

namespace User{

    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class Callbacks : IRegisterCallback
    {

        //Handles all connected users in the multicast service
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
                if (userList.Find((u) => u.URI.Equals(user.URI)) == null)
                    userList.Add(user);
            }
        }

        public void NotifyUnsubscribe(ChatUser user)
        {
            lock (nLock)
            {
                userList.RemoveAll(u => u.URI.Equals(user.URI));
            }
        }
    }
}
