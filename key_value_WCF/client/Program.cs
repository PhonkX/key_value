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
        static void ShowHelp()
        {
            Console.WriteLine("Type (without pluses):");
            Console.WriteLine("\"Read\" or \"read\" + key to get a value; or");
            Console.WriteLine("\"Write\" or \"write\" + key + value to write new key-value pair to storage");
            Console.WriteLine("Type \"Save\" to save database to disk");
            Console.WriteLine("Type \"Help\" to get help.");
        }
        static void Main(string[] args)
        {
            try
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
                ShowHelp();
                while (true)
                {
                    //Console.WriteLine("Enter key");
                    //string key = Console.ReadLine();
                    //Console.WriteLine("Enter value");
                    //string value = Console.ReadLine();
                    //channel.AddKey(key, value);
                    //Console.WriteLine("\n{0}", channel.GetValue(key));
                    var data = Console.ReadLine().Split(' ');
                    if (data[0].ToLower() == "read")
                    {
                        if (data.GetLength(0) >= 2)
                        {
                            string rcvData = channel.GetValue(data[1]);
                            if (rcvData == "")
                                Console.WriteLine("No such key.");
                            else
                                Console.WriteLine("Key: {0}, value: {1}", data[1], rcvData);
                        }
                        else
                        {
                            Console.WriteLine("Too few arguments. Please, enter command and key.");
                        }
                        // Echo the data back to the client.

                    }
                    else if (data[0].ToLower() == "write")
                    {
                        if (data.GetLength(0) >= 3) //command + key + value
                        {
                            channel.AddKey(data[1], data[2]);
                            Console.WriteLine("Record's done.");
                        }
                        else
                        {
                            Console.WriteLine("Too few arguments. Please, enter command and key with value.");
                        }
                    }
                    //else if (data[0].ToLower() == "save") //command
                    //{
                    //    channel.SaveToXml();
                    //}
                    else if (data[0].ToLower() == "remove")
                    {
                        if (data.GetLength(0) >= 2)//command + key
                        {
                            channel.Remove(data[1]);
                            Console.WriteLine("Record's removed.");
                        }
                        else
                        {
                            Console.WriteLine("Too few arguments. Please, enter command and key.");
                        }
                    }
                    else if (data[0].ToLower() == "help") //command
                    {
                        ShowHelp();
                    }
                    else
                    {
                        Console.WriteLine("Wrong command.");
                    }

                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("Wrong address.");
            }
            //TODO: Think about parametrization (generics); 
        }
    }
}
