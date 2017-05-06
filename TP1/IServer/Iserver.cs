using System;


namespace Interface
{
    public interface IServer
    {
        int storePair(String key, String value);
        String readPair(String key);
        int deletePair(String key);
        void init(int serverID);
    }

    public interface IManagerServerSide
    {
        Boolean checkIfKeyExists(String key, int originServer);
        SerializableAttribute searchServersForObject(String key);
        Boolean ReplicateInformationBetweenServers(int id, String Key, String val);
    }

    public interface IManagerClientSide
    {
        String getRing();
    }

    public interface IClientInterface
    {
        String storePairOnServer(String key, int student);
        String readPairFromServer(String key);
        String deletePairFromServer(String key);
        String associateWithServer();

    }


}
