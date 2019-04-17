using System;
using System.IO;
using System.Windows;
using System.Threading;
using System.Reflection;

namespace TCPClient
{
    public class Program
    {
        [STAThreadAttribute]
        public static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
            App.Main();
        }

        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            try
            {
                Assembly parentAssembly = Assembly.GetExecutingAssembly();
                string assebleName = args.Name.Substring(0, args.Name.IndexOf(','));
                string[] resources = parentAssembly.GetManifestResourceNames();
                string foundedResourceName = null;
                for (int i = 0; i < resources.Length; i++)
                {
                    if (resources[i].EndsWith(assebleName + ".dll"))
                    {
                        foundedResourceName = resources[i];
                        break;
                    }
                }
                if (!string.IsNullOrWhiteSpace(foundedResourceName))
                {
                    using (Stream stream = parentAssembly.GetManifestResourceStream(foundedResourceName))
                    {
                        byte[] block = new byte[stream.Length];
                        stream.Read(block, 0, block.Length);
                        return Assembly.Load(block);
                    }
                }
                else
                {
                    //MessageBox.Show($"Не удалоь найти следующую сборку в ресурсах программы:\n{args.Name}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                return null;
            }
        }
    }
}
