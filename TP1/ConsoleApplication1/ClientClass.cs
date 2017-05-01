using System;
using Interface;
using System.Collections.Generic;

namespace ClientClass
{
    class ClientClass : MarshalByRefObject, IClientInterface
    {

        private IServer associatedServer;
        private IManagerClientSide ringManager;

        public ClientClass()
        {
            Console.WriteLine("ClientClass construtor");
        }

        public void associateWithServer()
        {
            throw new NotImplementedException();
        }

        public void deletePairFromServer(SerializableAttribute key)
        {
            throw new NotImplementedException();
        }

        public void readPairFromServer(SerializableAttribute key)
        {
            throw new NotImplementedException();
        }

        public void storePairOnServer(SerializableAttribute key, SerializableAttribute value)
        {
            throw new NotImplementedException();
        }
    }
}
