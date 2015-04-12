using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;

namespace key_value
{
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
    class Storage
    {
        private Hashtable ht = new Hashtable();
        public Storage() { }
        public void Add<K, V>(K key, V value)
        {
            ht.Add(key, value);
        }
        public V Search<K, V>(K key)
        {
            if (ht.Contains(key))
                return (V)ht[key];
            else
                return default(V);
        }
        public void Delete<K>(K key)
        {
            try
            {
                ht.Remove(key);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
    class Server
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private const int SRV_PORT = 61390;
        private const int CLIENTS_NUMBER = 10;
        private static IPAddress ipAddr = IPAddress.Any;
        private static Socket srvSock = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        private static IPEndPoint srvEP = new IPEndPoint(ipAddr, SRV_PORT);
        private static Storage store = new Storage();
        // private static List<Socket> clients = new List<Socket>();
        public Server()
        {
            store.Add("ololo", "yeah:)");
            store.Add("sdfsdf", "y:)");
            Console.WriteLine("Server");
            Console.WriteLine(ipAddr.ToString());
        }
        public void StartServer()
        {
            try
            {
                srvSock.Bind(srvEP);
                srvSock.Listen(CLIENTS_NUMBER);
                while (true)
                {
                    allDone.Reset();                    // Set the event to nonsignaled state.
                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Waiting for a connection...");
                    srvSock.BeginAccept(new AsyncCallback(AcceptCallback), srvSock);
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            try
            {
                allDone.Set();

                // Get the socket that handles the client request.
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);
                // clients.Add(handler);
                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = handler;

                while (true)
                {
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                      new AsyncCallback(ReadCallback), state);
                }
                /*  while (true)
                  {
                      Receive(handler);
                  }*/
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        /*   public static void Receive(Socket sock)
           {
               try
               {
                   StateObject state = new StateObject();
                   state.workSocket = sock;


                   sock.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                       new AsyncCallback(ReadCallback), state);
               }
               catch (Exception e)
               {
                   Console.WriteLine(e.ToString());
               }
            
           }*/
        public static void ReadCallback(IAsyncResult ar)
        {
            try
            {
                String content = String.Empty;

                // Retrieve the state object and the handler socket
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                // Read data from the client socket. 
                int bytesRead = handler.EndReceive(ar);

                //   if (bytesRead > 0)
                //   {
                var rcvData = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);
                // There  might be more data, so store the data received so far.
                //  state.sb.Append(rcvData);

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                //    content = state.sb.ToString();
                // if (content.IndexOf("<EOF>") > -1)
                //    {
                // All the data has been read from the 
                // client. Display it on the console.
                Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                    content.Length, content);
                var data = store.Search<string, string>(rcvData);
                if (data == default(string))
                    data = "There is no such key.";
                Console.WriteLine("{0}[{1}]", rcvData, data);
                // Echo the data back to the client.
                Send(handler, data);
                //      }
                //   }
                //     else
                //     {
                // Not all data received. Get more.
                //        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                //        new AsyncCallback(ReadCallback), state);
                // }
            }
            catch (System.Net.Sockets.SocketException e)
            {
                Console.WriteLine("Client has disconnected");
                //  throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Send(Socket handler, String data)
        {
            try
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                //  handler.Shutdown(SocketShutdown.Both);
                //  handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


    }
    class Program
    {
        static void Main(string[] args)
        {
            Server srv = new Server();
            srv.StartServer();
        }
    }
}
