using Microsoft.Win32;
using System.IO;
using System.Net;
using System.Windows;
using System.Net.Sockets;
using System.Threading.Tasks;
using TCPClient.Models;
using TCPClient.Logic;
using System.Threading;
using System.Windows.Threading;
using System;
using System.Reflection;

namespace TCPClient
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Основная ViewModel 
        /// </summary>
        MainModel vm = MainModel.Instance;

        Server server = new Server();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = vm;

            configureVersionLabel();
        }

        private void configureVersionLabel()
        {
            var v = Assembly.GetExecutingAssembly().GetName().Version;
            textBlockVersion.Text = "Версия программы: " + v.ToString();
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            Client.Send();

            vm.SaveConfig();
        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Client.Cancel();
        }
        private void button_Browse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { CheckFileExists = true };
            if (dialog.ShowDialog() == true) {
                vm.Client.FileToSend = new FileInfo(dialog.FileName);
                vm.Client.SentBytes = 0;
            }
        }

        private void button_OpenPort_Click(object sender, RoutedEventArgs e)
        {
            server.Start();
        }

        private void button_ClosePort_Click(object sender, RoutedEventArgs e)
        {
            server.Stop();
        }

        private void ButtonCopyClientMD5_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(vm.Client.MD5))
                Clipboard.SetText(vm.Client.MD5);
        }
        private void ButtonCopyServerMD5_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(vm.Server.InputFile?.MD5))
                Clipboard.SetText(vm.Server.InputFile?.MD5);
        }
    }
}

