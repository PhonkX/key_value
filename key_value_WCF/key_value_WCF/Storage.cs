using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using StorageOperations;

namespace key_value_WCF
{
    class Storage:  IStorageOperations
    {
        private Hashtable ht = new Hashtable();
        public string GetValue(string key)
        {
            if (ht.Contains(key))
                return (string)ht[key];
            else
                return default(string);
        }
        public void AddKey(string key, string value)
        {
            ht.Add(key, value);
        }
    }
}
