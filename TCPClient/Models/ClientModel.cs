using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPClient.Logic.Utils;

namespace TCPClient.Models
{
    public class ClientModel : AModel
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
        public string MD5 {
            get {

                if (md5 == null)
                {
                    new Task(() =>
                    {
                        md5 = MD5Hash.FileHash(FileToSend?.FullName);
                        Set(md5, nameof(MD5));
                    }).Start();

                    return MD5Hash.STRING_COMPUTING;
                }

                return md5;
            }
        }
        public string md5 = null;

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
        public TimeSpan SendingTime { get => Get<TimeSpan>(); set => Set(value); }

        /// <summary>
        /// Скорость передачи файла
        /// </summary>
        public SpeedModel Speed { get => Get<SpeedModel>(); set => Set(value); }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ClientModel()
        {
            DestinationAddress = new IpModel() { Ip = "0.0.0.0", Port = 1234 };
            Speed = new SpeedModel();
        }
    }
}
