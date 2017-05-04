using Interface;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace ServerClass
{
    class ServerClassImpl : MarshalByRefObject, IServer
    {
        private Dictionary<String, String> map;
        private int id;
        private IManagerServerSide ring;

        public ServerClassImpl() { 
            WellKnownClientTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
            WellKnownClientTypeEntry entry = entries[0];

            if (entry == null)
                throw new RemotingException("Type not found");

            ring = (IManagerServerSide)Activator.GetObject(entry.ObjectType, entry.ObjectUrl);

            Console.WriteLine("ServerClass construtor");

        }

        public int deletePair(SerializableAttribute key)
        {
            throw new NotImplementedException();
        }

        public object readPair(SerializableAttribute key)
        {
            throw new NotImplementedException();
        }

        public void setServerid()
        {
            throw new NotImplementedException();
        }

        public int storePair(SerializableAttribute key, SerializableAttribute value)
        {
            throw new NotImplementedException();
        }

        public string test()
        {
            return "ola";
        }
    }
}
