using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StorageOperations
{
    [ServiceContract]
    public interface IStorageOperations
    {
        [OperationContract]
        string GetValue(string key);

        [OperationContract]
        void AddKey(string key, string value);

        [OperationContract]

        bool Authorization(User user);

        //[OperationContract]

        //void SaveToXml();

        [OperationContract]

        void Remove(string key);

    }
    public class User
    {
        [XmlAttribute()]
        public string Login { get; set; }

        [XmlAttribute()]
        public string Password { get; set; }

    }
    public class UserList : List<User>
    {
    }
}
