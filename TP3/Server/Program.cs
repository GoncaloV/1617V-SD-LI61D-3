using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class RegisterService : IRegister
    {
        static void Main(string[] args)
        {
        }

        public List<ChatUser> getCurrentUsers(ChatUser myself)
        {
            throw new NotImplementedException();
        }

        public bool Subscribe(ChatUser user)
        {
            throw new NotImplementedException();
        }

        public bool Unsubscribe(ChatUser user)
        {
            throw new NotImplementedException();
        }
    }
}
