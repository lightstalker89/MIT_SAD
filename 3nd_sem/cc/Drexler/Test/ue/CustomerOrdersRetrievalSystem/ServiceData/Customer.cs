﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceData
{
    [DataContract]
    public class Customer
    {
        [DataMember]
        public string Name { get; set; }
    }
}
