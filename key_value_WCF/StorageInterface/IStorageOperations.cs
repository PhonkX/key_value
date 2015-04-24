﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StorageInterface
{
    
        [ServiceContract]
        public interface IStorageOperations
        {
            [OperationContract]

            V GetValue<K, V>(K key);
            void AddKey<K, V>(K key, V value);
        }
   

}