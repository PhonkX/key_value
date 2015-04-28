using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using StorageOperations;
using System.Xml.Serialization;
using System.IO;


namespace key_value_WCF
{
    class Storage
    {
        
        private static Hashtable ht = new Hashtable();
        public static string GetValue(string key)
        {
            lock (ht)
            {
                if (ht.Contains(key))
                    return (string)ht[key];
                else
                    return default(string);
            }
            
        }
        public static void AddKey(string key, string value)
        {
            lock (ht)
            {
                ht.Add(key, value);
            }
        }
    }
    //public class User
    //{
    //    [XmlAttribute()]
    //    public string Login { get; set; }

    //    [XmlAttribute()]
    //    public string Password { get; set; }

    //}
    //public class UserList : List<User>
    //{
    //}
}
