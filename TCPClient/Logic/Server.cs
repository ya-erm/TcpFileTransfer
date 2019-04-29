using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using Microsoft.Win32;

namespace TCPClient.Logic
{
    using Models;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Threading;

    [DataContract]
    public class Server
    {
        ServerModel vm = MainModel.Instance.Server;

        TcpListener server;

        DispatcherTimer timer;

        bool IsServerRunning = false;

        public void Start()
        {
            server = new TcpListener(IPAddress.Any, vm.Address.Port);
            IsServerRunning = true;
            Task.Factory.StartNew(() =>
            {
                while (IsServerRunning)
                {
                    try
                    {
                        server.Start();
                        vm.IsPortOpen = true;

                        using (var client = server.AcceptTcpClient())
                        using (var stream = client.GetStream())
                        {
                            var remote = client.Client.RemoteEndPoint as IPEndPoint;
                            vm.InputAddress = new IpModel(remote?.Address.ToString(), remote.Port);
                            // Получаем информацию о файле
                            vm.InputFile = Common.ReadFileHeader(stream);
                            var fileMode = FileMode.Create;
                            var filePath = string.Empty;
                            if (vm.InputFile.MD5 != null && fp.ReceivedFilePaths.ContainsKey(vm.InputFile.MD5))
                            {
                                filePath = fp.ReceivedFilePaths[vm.InputFile.MD5];
                                if (File.Exists(filePath))
                                {
                                    var fileInfo = new FileInfo(filePath);
                                    vm.InputFile.Offset = fileInfo.Length + 1;
                                    vm.ReceivedBytes = fileInfo.Length;
                                }
                            }
                            else
                            {
                                vm.ReceivedBytes = 0;
                                vm.ReceivedTime = new TimeSpan();
                                vm.Speed.Clear();
                            }
                            Common.WriteFileHeader(stream, vm.InputFile);
                            // Запрашиваем место сохранения, если файл ранее не скачивался
                            var saveFileDialog = new SaveFileDialog() { FileName = vm.InputFile.Name, Filter = "Все файлы (*.*)|*.*" };
                            if (string.IsNullOrEmpty(filePath))
                                if (saveFileDialog.ShowDialog() == true)
                                {
                                    filePath = saveFileDialog.FileName;
                                    if (vm.InputFile.MD5 != null)
                                        fp.ReceivedFilePaths.Add(vm.InputFile.MD5, filePath);
                                    fp.Save();
                                }
                                else
                                    return;
                            else
                                fileMode = FileMode.Append;

                            using (var file = new FileStream(filePath, fileMode))
                            {
                                // Получаем сам файл
                                timer?.Start();
                                vm.ReceivedTime = new TimeSpan();
                                vm.Speed.Clear();
                                var buf = new byte[32*1024];
                                do
                                {
                                    var count = stream.Read(buf, 0, buf.Length);
                                    file.Write(buf, 0, count);
                                    vm.ReceivedBytes += count;
                                }
                                while (vm.ReceivedBytes < vm.InputFile.Length);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        timer?.Stop();
                        MessageBox.Show(ex.Message, "При получении файла возникла ошибка");
                    }
                }

            });

            var tick = 0;
            // Настраиваем таймер
            timer = new DispatcherTimer(
                TimeSpan.FromMilliseconds(100),
                DispatcherPriority.Normal,
                new EventHandler((o, e) =>
                {
                    if (vm.ReceivedBytes < vm.InputFile?.Length)
                    {
                        vm.ReceivedTime = vm.ReceivedTime.Add(timer.Interval);
                        if (++tick % 10 == 0)
                            vm.Speed.AddPoint(vm.ReceivedTime, vm.ReceivedBytes);
                    }
                }),
                Dispatcher.CurrentDispatcher
            )
            { IsEnabled = false };
        }


        public void Stop()
        {
            server?.Stop();
            vm.IsPortOpen = false;
            vm.ReceivedBytes = 0;
            vm.InputFile = null;
            IsServerRunning = false;
        }

        FilePathsModel fp = new FilePathsModel();

    }
}
