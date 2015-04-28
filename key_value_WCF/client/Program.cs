using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using StorageOperations;

namespace Сlient
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            var factory = new ChannelFactory<IStorageOperations>(binding, new EndpointAddress("net.tcp://localhost:10503/StorageService"));

            var channel = factory.CreateChannel();
            string login, password;
            Console.WriteLine("Enter login.");
            login = Console.ReadLine();
            Console.WriteLine("Enter password.");
            password = Console.ReadLine();
            User user = new User { Login = login, Password = password };
            while (!channel.Authorization(user))
            {
                Console.WriteLine("You are not authorized.");
                Console.WriteLine("Enter login.");
                login = Console.ReadLine();
                Console.WriteLine("Enter password.");
                password = Console.ReadLine();
                user = new User { Login = login, Password = password };
            }
            while (true)
            {
                Console.WriteLine("Enter key");
                string key = Console.ReadLine();
                Console.WriteLine("Enter value");
                string value = Console.ReadLine();
                channel.AddKey(key, value);
                Console.WriteLine("\n{0}", channel.GetValue(key));
            }
        }
    }
}
