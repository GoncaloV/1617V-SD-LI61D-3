using System;
using System.Threading.Tasks;

namespace Interface
{
    public interface IServer
    {
        void storePair(String key, String value);
        String readPair(String key);
        void deletePair(String key);
        void init(int serverID);
    }

    public interface IManagerServerSide
    {
        Boolean checkIfKeyExists(String key, int originServer);
        String searchServersForObject(String key);
        Boolean ReplicateInformationBetweenServers(int id, String Key, String val);
        void deleteInformation(String key, int id);
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
