using System;
using System.IO;
using TCPClient.Models;

namespace TCPClient.Logic
{
    public class Common
    {
        public static FileModel ReadFileHeader(Stream stream)
        {
            // Получаем длину заголовка
            var fileHeaderLengthBytes = new byte[4];
            stream.Read(fileHeaderLengthBytes, 0, 4);
            var fileHeaderLength = BitConverter.ToInt32(fileHeaderLengthBytes, 0);
            // Получаем заголовок
            var fileHeaderBytes = new byte[fileHeaderLength];
            stream.Read(fileHeaderBytes, 0, fileHeaderLength);
            return JsonConverter.DeserializeFromBytes<FileModel>(fileHeaderBytes);
        }

        public static void WriteFileHeader(Stream stream, FileModel fileHeader)
        {
            var fileHeaderBytes = JsonConverter.SerializeToBytes(fileHeader);
            var fileHeaderLength = fileHeaderBytes.Length;
            var fileHeaderLengthBytes = BitConverter.GetBytes(fileHeaderLength);
            // Передаём 4 байта, в которых содержится длина заголовка
            stream.Write(fileHeaderLengthBytes, 0, 4);
            // Передаём сам заголовок
            stream.Write(fileHeaderBytes, 0, fileHeaderLength);
        }
    }
}
