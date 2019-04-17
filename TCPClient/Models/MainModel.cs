using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient.Models
{
    public class MainModel : AModel
    {
        #region -Singletone-
        public static MainModel Instance => instance;
        private static MainModel instance { get; set; } = new MainModel();
        private MainModel()
        {
            Config = MainConfiguration.Load();

            Client = new ClientModel() { DestinationAddress = Config.ClientAddress };
            Server = new ServerModel();
        }
        #endregion
        
        public ClientModel Client { get => Get<ClientModel>(); set => Set(value); }

        public ServerModel Server { get => Get<ServerModel>(); set => Set(value); }


        private MainConfiguration Config { get => Get<MainConfiguration>(); set => Set(value); }

        public void SaveConfig()
        {
            Config.ClientAddress = Client.DestinationAddress;
            Config.Save();
        }
    }
}
