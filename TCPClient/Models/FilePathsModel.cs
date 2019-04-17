using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient.Models
{
    public class FilePathsModel
    {
        /// <summary>
        /// Словарь мета-информации принятых файлов
        /// Ключ - MD5 хеш-сумма, Значение - Полный путь к файлу
        /// (Используется для докачки файлов)
        /// </summary>
        public Dictionary<string, string> ReceivedFilePaths = new Dictionary<string, string>();


        const string ConfigPath = "ReceivedFilePaths.txt";

        public void Save()
        {
            using (var file = new FileStream(ConfigPath, FileMode.Create))
            using (var writer = new StreamWriter(file))
            {
                foreach (var key in ReceivedFilePaths.Keys)
                {
                    writer.WriteLine(key);
                    writer.WriteLine(ReceivedFilePaths[key]);
                }
            }
        }
        
        public void Load()
        {
            ReceivedFilePaths = new Dictionary<string, string>();

            if (!File.Exists(ConfigPath)) return;
            using (var file = new FileStream(ConfigPath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(file))
            {
                while(!reader.EndOfStream)
                {
                    var key = reader.ReadLine();
                    var value = reader.ReadLine();
                    ReceivedFilePaths.Add(key, value);
                }
            }
        }

        public FilePathsModel()
        {
            Load();
        }
    }
}
