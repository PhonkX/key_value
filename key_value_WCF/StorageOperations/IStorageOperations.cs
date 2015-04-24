using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StorageOperations
{
    [ServiceContract]
    public interface IStorageOperations
    {
        [OperationContract]
        string GetValue(string key);

        [OperationContract]
        void AddKey(string key, string value);

      //  [OperationContract]

        //void Authotization(string login, string password);
    }
}
