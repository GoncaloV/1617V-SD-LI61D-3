﻿using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;


namespace Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                       ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class RegisterService : IRegister
    {

        private readonly IDictionary<ChatUser, IUserCallback> onlineUsers = new Dictionary<ChatUser, IUserCallback>();

        private readonly object myLock = new object();

        public List<ChatUser> Subscribe(ChatUser user)
        {

            if (user == null)
                throw new FaultException("User information can be null!");

            List<ChatUser> users = new List<ChatUser>();
            List<ChatUser> alreadyGone = new List<ChatUser>();

            lock (myLock)
            {
                try
                {
                    users = onlineUsers.Keys.ToList();
                    IUserCallback userCb = OperationContext.Current.GetCallbackChannel<IUserCallback>();
                    foreach (ChatUser chatUser in onlineUsers.Keys)
                    {
                        try
                        {
                            onlineUsers[chatUser].NotifySubscribe(user);
                        }
                        catch (EndpointNotFoundException e)
                        {
                            alreadyGone.Add(chatUser);
                        }
                    }

                    onlineUsers.Add(user, userCb);
                }
                catch (TimeoutException e)
                {
                    Console.WriteLine("## ERROR ## CentralService - Subscribe: " + e.Message);
                    Console.WriteLine(e.StackTrace);
                }
                catch (Exception e)
                {
                    Console.WriteLine("## ERROR ## CentralService - Subscribe: " + e.Message);
                    Console.WriteLine(e.StackTrace);
                }

                alreadyGone.ForEach(i => onlineUsers.Remove(i));
                Console.WriteLine("User: " + user.name + " subscribed");
                return users;
            }
        }

        public void Unsubscribe(ChatUser user)
        {

            if (user == null)
                throw new FaultException("User information can be null!");

            ChatUser userChatInfo = null;
            List<ChatUser> alreadyGone = new List<ChatUser>();

            lock (myLock)
            {
                try
                {
                    foreach (ChatUser chatUser in onlineUsers.Keys)
                    {
                        if (chatUser.URI.Equals(user.URI) && chatUser.Binding.Equals(user.Binding))
                            userChatInfo = chatUser;
                    }

                    foreach (ChatUser chatUser in onlineUsers.Keys)
                    {
                        try
                        {
                            if (!onlineUsers[userChatInfo].Equals(onlineUsers[chatUser]))
                                onlineUsers[chatUser].NotifyUnsubscribe(userChatInfo);
                        }
                        catch (EndpointNotFoundException e)
                        {
                            alreadyGone.Add(chatUser);
                        }
                    }

                    alreadyGone.ForEach(i => onlineUsers.Remove(i));
                    onlineUsers.Remove(userChatInfo);
                }
                catch (TimeoutException e)
                {
                    Console.WriteLine("## ERROR ## CentralService - Unsubscribe: " + e.Message);
                    Console.WriteLine(e.StackTrace);
                }
                catch (Exception e)
                {
                    Console.WriteLine("## ERROR ## CentralService - Unsubscribe: " + e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }

            Console.WriteLine("User: " + user.name + " unsubscribed");
        }

    }

    class Program
    {

        static void Main(string[] args)
        {
            RunService();
        }

        private static void RunService()
        {
            using (ServiceHost host = new ServiceHost(typeof(RegisterService)))
            {
                Uri addr = new Uri("http://localhost:8676/RegisterService");
                Type serviceType = typeof(IRegister);
                WSDualHttpBinding bind = new WSDualHttpBinding(WSDualHttpSecurityMode.None);
                AddBehavior(host, addr);
                host.AddServiceEndpoint(serviceType, bind, addr);
                host.Open();

                Console.WriteLine("CentralService hosted. To close hosting, Press Enter. ");
                Console.ReadLine();
                host.Close();
                Console.WriteLine("Host closed.");
            }
        }

        private static void AddBehavior(ServiceHost svchost, Uri addr)
        {

            ServiceMetadataBehavior smb = svchost.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (smb != null)
            {
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = addr;
            }
            else
            {
                smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = addr;
                svchost.Description.Behaviors.Add(smb);
            }
        }
    }
}