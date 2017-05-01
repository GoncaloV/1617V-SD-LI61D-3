using System;


namespace Interface
{
    public interface IServer
    {
        void setServerid();
        int storePair(SerializableAttribute key, SerializableAttribute value);
        Object readPair(SerializableAttribute key);
        int deletePair(SerializableAttribute key);
    }

    public interface IManagerServerSide
    {
        Boolean checkIfKeyExists(String key);
        SerializableAttribute searchServersForObject(String key);
        Boolean ReplicateInformationBetweenServers(int id, String Key, SerializableAttribute val);
        int getServerId();
    }

    public interface IManagerClientSide
    {
        IServer getRing();
    }

    public interface IClientInterface
    {
        void storePairOnServer(SerializableAttribute key, SerializableAttribute value);
        void readPairFromServer(SerializableAttribute key);
        void deletePairFromServer(SerializableAttribute key);
        void associateWithServer();

    }


}
