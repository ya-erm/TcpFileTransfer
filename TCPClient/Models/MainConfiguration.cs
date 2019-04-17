using System.IO;
using System.Runtime.Serialization;
using TCPClient.Logic;

namespace TCPClient.Models
{
    [DataContract]
    public class MainConfiguration: AModel
    {
        [DataMember]
        public IpModel ClientAddress { get => Get<IpModel>(); set => Set(value); }


        public MainConfiguration()
        {
            ClientAddress = new IpModel("127.0.0.1", 1234);
        }


        #region -Save and Load-

        private static readonly string CONFIG_FILE = "Configuration.json";

        public void Save()
        {
            var json = JsonConverter.Serialize(this);
            File.WriteAllText(CONFIG_FILE, json);
        }

        public static MainConfiguration Load()
        {
            try
            {
                var json = File.ReadAllText(CONFIG_FILE);
                var config = JsonConverter.Deserialize<MainConfiguration>(json);
                return config;
            }
            catch
            {
                return new MainConfiguration();
            }
        }

        #endregion
    }
}
