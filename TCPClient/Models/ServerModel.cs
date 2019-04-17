using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient.Models
{
    public class ServerModel: AModel
    {
        /// <summary>
        /// Адрес сервера
        /// </summary>
        public IpModel Address { get => Get<IpModel>(); set => Set(value); }

        /// <summary>
        /// Открыт ли порт сервера
        /// </summary>
        public bool IsPortOpen { get => Get<bool>(); set => Set(value); }

        /// <summary>
        /// Адрес отправителя
        /// </summary>
        public IpModel InputAddress { get => Get<IpModel>(); set => Set(value); }

        /// <summary>
        /// Принимаемый файл
        /// </summary>
        public FileModel InputFile { get => Get<FileModel>(); set => Set(value); }
        
        /// <summary>
        /// Количество принятых байт
        /// </summary>
        public long ReceivedBytes { get => Get<long>(); set => Set(value); }

        /// <summary>
        /// Процент прогресса отправки
        /// </summary>
        [DependsOn(nameof(ReceivedBytes), nameof(InputFile))]
        public double Persentage { get => ReceivedBytes / (InputFile?.Length ?? 1.0) * 100; }

        /// <summary>
        /// Время приёма файла
        /// </summary>
        public DateTime ReceivedTime { get => Get<DateTime>(); set => Set(value); }


        public ServerModel()
        {
            IsPortOpen = false;
            Address = new IpModel() { Ip = "127.0.0.1", Port = 1234 };
        }
    }
}
