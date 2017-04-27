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

    public interface IManager
    {
        IServer getRing();
        Boolean checkIfKeyExists(String key);
        SerializableAttribute searchServersForObject(String key);
        Boolean ReplicateInformationBetweenServers(int id, String Key, SerializableAttribute val);
        int getServerId();
    }
}
