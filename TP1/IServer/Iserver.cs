using System;

namespace Interface
{
    public interface IServer
    {
        Boolean storePair(SerializableAttribute key, SerializableAttribute value);
        Object readPair(SerializableAttribute key);
        Boolean deletePair(SerializableAttribute key);
    }

    public interface IManager
    {


    }
}
