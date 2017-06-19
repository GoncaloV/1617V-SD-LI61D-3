using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    //Implemented by the Server Project
    public interface IRegister
    {
        //Add the client 'user' to the server's connected list
        bool Subscribe(ChatUser user);
        //Removes the client 'user' from the connected list
        bool Unsubscribe(ChatUser user);
        //Gives the current list of users connected to the server - client 'myself' 
        List<ChatUser> getCurrentUsers(ChatUser myself);

    }

    //Implemented by the User Project
    public interface IReceiver
    {
        //The method that the server will invocate to send messages to the client. 
        void handleNewMessage(ChatMessage message);
    }


    //Class that represents one user
    public class ChatUser
    {
        public string name { get; set; }

        public string language { get; set; }

        public string URI { get; set; }
    }

    //Class that represents a message
    public class ChatMessage
    {
        //Represents the language of the user that sent this message.
        //We need the language so that the client that receives it, decide if it need to be translated or not
        public string language { get; set; }

        //The message
        public string message { get; set; }
        
        //The user that sent it
        public string username { get; set; }
    }
}
