using Interface;
using System;
using System.Collections.Generic;

namespace ServerClass
{
    class ServerClass : MarshalByRefObject, IServer
    {
        private Dictionary<String, String> map;
        private int id;
        private IManagerServerSide ring;

        public ServerClass()
        {
            Console.WriteLine("ServerClass construtor");

        }

        public void setRingReference(IManagerServerSide ring)
        {
            this.ring = ring; 
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
    }
}
