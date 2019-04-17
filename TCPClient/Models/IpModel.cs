using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient.Models
{
    public class IpModel: AModel
    {
        public string Ip { get => Get<string>(); set => Set(value); }

        public int Port { get => Get<int>(); set => Set(value); }


        public IpModel() { }
        public IpModel(string ip, int port)
        {
            Ip = ip;
            Port = port;
        }


        public override string ToString()
        {
            return $"{Ip}:{Port}";
        }
    }
}
