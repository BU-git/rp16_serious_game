using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class HostConfiguration
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string HostName { get; set; }
    }
}
