using Interface;
using System;
using System.Collections.Generic;

namespace ServerClass
{
    class ServerClassImpl : MarshalByRefObject, IServer
    {
        private Dictionary<String, String> map;
        private int id;
        private IManagerServerSide ring;

        public ServerClassImpl()
        {
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
    }
}
