using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageOperations;
using System.Xml.Serialization;
using System.IO;
using System.ServiceModel;

namespace key_value_WCF
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerSession)]
    class StorageOperations: IStorageOperations
    {
        
        // private UserList AuthorizedUsers = new UserList();
        public bool Authorized = false;
        public string GetValue(string key)
        {
            if(Authorized)
                return Storage.GetValue(key);
            return "";
        }
        public void AddKey(string key, string value)
        {
            if(Authorized)
                Storage.AddKey(key, value);
        }
        public bool Authorization(User user)
        {
            var userList = GetUserList();
            //var user = new User { Login = login, Password = password };
            //if (userList.Contains(user))
            //{
            //    AuthorizedUsers.Add(user);
            //    return user;
            //}
            foreach (var u in userList)//Why is method "Contains" not working?
            {
                if (u.Login == user.Login && u.Password == user.Password)
                {
           //         AuthorizedUsers.Add(user);
                    Authorized = true;
                    return true;
                }
            }
            return false;

        }

        public void SaveToXml()
        {
            Storage.SaveToXml();
        }

        public void Remove(string key)
        {
            Storage.Remove(key);
        }
        private UserList GetUserList()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserList));
            using (var fileStream = new FileStream("users.xml", FileMode.Open))
            {
                var userList = serializer.Deserialize(fileStream) as UserList;
                return userList;
            }
        }

    }
}
