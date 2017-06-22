using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Interfaces
{
    //Implemented by the Server Project
    [ServiceContract(Namespace = "http://ISEL.ADEETC.SD", CallbackContract = typeof(IUserCallback))]
    public interface IRegister
    {
        //Add the client 'user' to the server's connected list
        [OperationContract]
        List<ChatUser> Subscribe(ChatUser user);
        //Removes the client 'user' from the connected list
        [OperationContract]
        void Unsubscribe(ChatUser user);

    }

    public interface IUserCallback
    {
        [OperationContract]
        void NotifySubscribe(ChatUser info);
        [OperationContract]
        void NotifyUnsubscribe(ChatUser info);
    }

    //Implemented by the User Project
    [ServiceContract(Namespace = "http://ISEL.ADEETC.SD")]
    public interface IReceiver
    {
        //The method that the server will invocate to send messages to the client. 
        [OperationContract]
        void handleNewMessage(ChatMessage message);
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
