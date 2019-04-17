using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPClient.Logic.Utils;

namespace TCPClient.Models
{
    public class ClientModel: AModel
    {
        /// <summary>
        /// Адрес назначения
        /// </summary>
        public IpModel DestinationAddress { get => Get<IpModel>(); set => Set(value); }

        /// <summary>
        /// Файл для отправки
        /// </summary>
        public FileInfo FileToSend { get => Get<FileInfo>(); set => Set(value); }
        
        /// <summary>
        /// Хеш-сумма файла
        /// </summary>
        [DependsOn(nameof(FileToSend))]
        public string MD5 { get => MD5Hash.FileHash(FileToSend?.FullName); }

        /// <summary>
        /// Выбран ли файл для отправки
        /// </summary>
        [DependsOn(nameof(FileToSend))]
        public bool IsFileSelected { get => FileToSend != null; }

        /// <summary>
        /// Количество отправленных байт
        /// </summary>
        public long SentBytes { get => Get<long>(); set => Set(value); }
        
        /// <summary>
        /// Процент прогресса отправки
        /// </summary>
        [DependsOn(nameof(SentBytes), nameof(FileToSend))]
        public double Persentage { get => SentBytes / (FileToSend?.Length ?? 1.0) * 100; }

        /// <summary>
        /// Время передачи файла
        /// </summary>
        public DateTime SendingTime { get => Get<DateTime>(); set => Set(value); }


        /// <summary>
        /// Конструктор
        /// </summary>
        public ClientModel()
        {
            DestinationAddress = new IpModel() { Ip = "127.0.0.1", Port = 1234 };
        }
    }
}
