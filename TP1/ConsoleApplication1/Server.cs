using System;
using Interface;
using System.Collections.Generic;

namespace Server
{
    class Server : MarshalByRefObject, IServer
    {

        Dictionary<SerializableAttribute,SerializableAttribute> Map;
        int Id;

        public Server(int Id)
        {
            this.Id = Id;
            Map = new Dictionary<SerializableAttribute, SerializableAttribute>();
        }

        public void setServerid(int Id)
        {
            this.Id = Id;
        }

        public int storePair(SerializableAttribute key, SerializableAttribute value)
        {
            
        }

        public object readPair(SerializableAttribute key)
        {
            throw new NotImplementedException();
        }

        public int deletePair(SerializableAttribute key)
        {
            throw new NotImplementedException();
        }

        static void Main(string[] args)
        {

        }
    }
}
