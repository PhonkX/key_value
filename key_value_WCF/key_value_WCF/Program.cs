using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using StorageOperations;

namespace key_value_WCF
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost storageHost = new ServiceHost(typeof(Storage));
            var syncBinding = new NetTcpBinding();
            storageHost.AddServiceEndpoint(typeof(IStorageOperations), syncBinding, "net.tcp://localhost:61390/StorageService");
            storageHost.Open();
            while(true)
            {

            }
            storageHost.Close();
            
        }
    }
}
