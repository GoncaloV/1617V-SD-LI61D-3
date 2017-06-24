using System.Collections.Generic;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Interfaces
{
    [ServiceContract(Namespace = "http://ISEL.ADEETC.SD")]
    public interface IReceiver
    {
        [OperationContract]
        void ReceiveMessage(ChatMessage msg);
    }

    [ServiceContract(Namespace = "http://ISEL.ADEETC.SD", CallbackContract = typeof(IUserCallback))]
    public interface IRegister
    {
        [OperationContract]
        List<ChatUser> Subscribe(ChatUser info);
        [OperationContract]
        void Unsubscribe(ChatUser info);
    }

    public interface IUserCallback {

        [OperationContract]
        void NotifySubscribe(ChatUser info);
        [OperationContract]
        void NotifyUnsubscribe(ChatUser info);
    }

    //Class that represents one user
    [DataContract]
    public class ChatUser
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string language { get; set; }
        [DataMember]
        public string URI { get; set; }
        [DataMember]
        public string Binding { get; set; }
    }

    //Class that represents a message
    public class ChatMessage
    {
        //Represents the language of the user that sent this message.
        //We need the language so that the client that receives it, decide if it need to be translated or not
        [DataMember]
        public string language { get; set; }

        //The message
        [DataMember]
        public string message { get; set; }

        //The user that sent it
        [DataMember]
        public string username { get; set; }
    }
}
