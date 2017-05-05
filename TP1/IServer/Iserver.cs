using System;


namespace Interface
{
    public interface IServer
    {
        int storePair(SerializableAttribute key, SerializableAttribute value);
        Object readPair(SerializableAttribute key);
        int deletePair(SerializableAttribute key);
        void init(int serverID);

        String test();
    }

    public interface IManagerServerSide
    {
        Boolean checkIfKeyExists(String key, int originServer);
        SerializableAttribute searchServersForObject(String key);
        Boolean ReplicateInformationBetweenServers(int id, String Key, SerializableAttribute val);
    }

    public interface IManagerClientSide
    {
        String getRing();
    }

    public interface IClientInterface
    {
        String storePairOnServer(String key, String value);
        String readPairFromServer(String key);
        String deletePairFromServer(String key);
        String associateWithServer();

    }


}
