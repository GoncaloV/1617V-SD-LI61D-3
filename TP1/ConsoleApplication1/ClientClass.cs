using System;
using Interface;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Runtime.Serialization;
using System.Linq;
using System.Configuration;

namespace ClientClass
{
    public class ClientClassImpl : MarshalByRefObject, IClientInterface
    {

        private IServer associatedServer;
        private IManagerClientSide ringManager;
        private LinkedList<Student> students = new LinkedList<Student>();


        public ClientClassImpl()
        {
            Console.WriteLine("ClientClass construtor");
            createStudents();
            connectToRing();
        }

        private void createStudents()
        {
            List<String> studentOptions = ConfigurationManager.AppSettings.AllKeys.ToList();

            foreach(String key in studentOptions)
            {
                String value = ConfigurationManager.AppSettings.Get(key);

                String[] value_splitted = value.Split(':');
                int number = 0;
                Int32.TryParse(value_splitted[1], out number);

                Student s = new Student(value_splitted[0], number);

                String[] classes = value_splitted[2].Split(',');
                String[] semesters_tmp = value_splitted[3].Split(',');

                LinkedList<Turma> turmasObj = new LinkedList<Turma>();

                for(int i = 0; i < classes.Length; i++)
                {
                    int semester = 0;
                    Int32.TryParse(semesters_tmp[i], out semester);

                    turmasObj.AddLast(new Turma(classes[i], semester));
                 
                }

                s.classes = turmasObj.ToArray();

                //Save it.
                students.AddLast(s);
            }
        }

        private String connectToRing()
        {
            WellKnownClientTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
            WellKnownClientTypeEntry entry = entries[0];

            if (entry == null)
                throw new RemotingException("Type not found");

            ringManager = (IManagerClientSide)Activator.GetObject(entry.ObjectType, entry.ObjectUrl);

            return "Connected!";
        }

        public String associateWithServer()
        {
            try { 
                String url = ringManager.getRing();

                WellKnownClientTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
                WellKnownClientTypeEntry entry = entries[0];

                if (entry == null)
                    throw new RemotingException("Type not found");

                associatedServer = (IServer)Activator.GetObject(entry.ObjectType, url);

                return "Associated with server: " + url;
            }catch(Exception e)
            {
                return "Cannot connect to Ring";
            }
        }

        public String deletePairFromServer(String key)
        {
            try
            {
                associatedServer.deletePair(key);
                return "Deleted value for key: " + key;
            }catch(Exception e)
            {
                return "Cannot communicate with Server";
            }
        }

        public String readPairFromServer(String key)
        {
            try
            {
                return "Value for key: " + key + " is " + associatedServer.readPair(key);
            }
            catch (Exception e)
            {
                return "Cannot communicate with Server";
            }
        }

        public String storePairOnServer(String key, int student)
        {
            return student + "";

            try
            {
                Student toSend = students.ElementAt(student);
                String deserializedStudent = WriteFromObject(toSend);

                associatedServer.storePair(key, deserializedStudent);

                return "Pushed " + key + " to Server";
            }catch(Exception e)
            {
                return "Cannot communicate with Server";
            }
        }

        // Cria um objeto e serealiza-o em JSON para um memory stream.  
        public static String WriteFromObject(Student student)
        {
            //Cria um memory stream para serialize.  
            MemoryStream ms = new MemoryStream();

            // Serializer o objeto para a stream.  
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Student));
            ser.WriteObject(ms, student);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);

        }

        // Deserialize o JSON stream para um objeto.  
        public static Student ReadToObject(string json)
        {
            Student deserializedStudent = new Student();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedStudent.GetType());
            deserializedStudent = ser.ReadObject(ms) as Student;
            ms.Close();
            return deserializedStudent;
        }

        public class Turma
        {

            public Turma() { }
            public Turma(string name, int semester)
            {
                this.name = name;
                this.semester = semester;
            }
            public string name;
            public int semester;

        }

        [DataContract]
        public class Student
        {
            public Student() { }

            public Student(string name, int number)
            {
                this.name = name;
                this.number = number;
            }

            [DataMember]
            public string name;
            [DataMember]
            public int number;
            [DataMember]
            public Turma[] classes;
        }

    }
}
