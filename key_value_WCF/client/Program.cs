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
            var factory = new ChannelFactory<IStorageOperations>(binding, new EndpointAddress("net.tcp://localhost:61390/StorageService"));

            var channel = factory.CreateChannel();

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
