using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using TCPClient.Logic.Utils;

namespace TCPClient.Models
{
    /// <summary>
    /// Модель заголовка передаваемого файла
    /// </summary>
    [DataContract]
    public class FileModel : AModel
    {
        /// <summary>
        /// Название файла
        /// </summary>
        [DataMember]
        public string Name { get => Get<string>(); set => Set(value); }

        /// <summary>
        /// Размер файла
        /// </summary>
        [DataMember]
        public long Length { get => Get<long>(); set => Set(value); }


        /// <summary>
        /// Количество отправленных байт
        /// Используется для "докачки"
        /// </summary>
        [DataMember]
        public long Offset { get => Get<long>(); set => Set(value); }

        /// <summary>
        /// Хеш-сумма файла
        /// </summary>
        [DataMember]
        public string MD5 { get => Get<string>(); set => Set(value); }

        /// <summary>
        /// Конструктор модели передаваемого файла
        /// </summary>
        /// <param name="name">Название файла</param>
        /// <param name="length">Размер файла</param>
        public FileModel(string name, long length)
        {
            Name = name;
            Length = length;
        }


        /// <summary>
        /// Конструктор модели передаваемого файла
        /// </summary>
        /// <param name="fileInfo">Информация о файле</param>
        public FileModel(FileInfo fileInfo, string md5)
        {
            Name = fileInfo.Name;
            Length = fileInfo.Length;
            MD5 = (md5 == MD5Hash.STRING_COMPUTING)
                ? MD5Hash.FileHash(fileInfo.FullName)
                : md5;
        }
    }
}
