using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using StorageOperations;
using System.Threading;
using System.IO;
using System.Xml.Serialization;


namespace key_value_WCF
{
    class Program
    {
        static void Main(string[] args)
        { 
            using (var fileStream = new FileStream("db.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(UserList));
                serializer.Serialize(fileStream, new UserList()
                {
                    new User() { Login = "PhonkX", Password = "ololo" },
                    new User() { Login = "asar", Password = "2sdf" },
                    new User() { Login = "Vasya2", Password = "3" },
                    new User() { Login = "Vasya4", Password = "4" }
                });
            }
            ServiceHost storageHost = new ServiceHost(typeof(StorageOperations));
            var syncBinding = new NetTcpBinding();
            storageHost.AddServiceEndpoint(typeof(IStorageOperations), syncBinding, "net.tcp://localhost:10503/StorageService");
            storageHost.Open();
          
            while(true)
            {
                Thread.Sleep(1000);
            }
            storageHost.Close();
            
        }
    }
}
