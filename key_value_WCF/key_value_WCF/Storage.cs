using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using StorageOperations;
using System.Xml.Serialization;
using System.IO;
using Sop.Collections.Generic.BTree;
using System.Xml;
using System.Runtime.Serialization;

namespace key_value_WCF
{
    public class DataItem
    {
        public string Key;

        public string Value;

        public DataItem(string key, string value)
        {
            Key = key.ToString();
            Value = value.ToString();
        }
        public DataItem() { }
    }
    class Storage
    {
        
         private static Hashtable ht = new Hashtable();
       // private static BTreeDictionary<string, string> bt = new BTreeDictionary<string, string>();

        public static string GetValue(string key)
        {
            try
            {
                lock (ht)
                {
                    if (ht.Contains(key))
                        return (string)ht[key];
                    else
                        return "";
                }
                //lock (bt)
                //{
                //    if (bt.Search(key))
                //        return (string)bt[key];
                //    else
                //        return "";
                //}
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("Wrong key.");
                return "";
            }
            
        }
        public static void AddKey (string key, string value)
        {
            try
            {
                lock (ht)
                {
                    ht.Add(key, value);
                   // bt.Add(key, value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Wrong key/value.");
            }
        }

        public static void SaveToXml()
        {
            try
            {
                lock (ht)
                {
                    List<DataItem> tempdataitems = new List<DataItem>(ht.Count);

                    foreach (string key in ht.Keys)
                    {
                        tempdataitems.Add(new DataItem(key, ht[key].ToString()));
                    }

                    XmlSerializer serializer = new XmlSerializer(typeof(List<DataItem>));
                    var fileStream = new FileStream("db.xml", FileMode.Create);
                    serializer.Serialize(fileStream, tempdataitems);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Remove(string key)
        {
            try
            {
                lock(ht)
                {
                    ht.Remove(key);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Wrong key.");
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
