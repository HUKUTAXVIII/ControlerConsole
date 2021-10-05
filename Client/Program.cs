using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ClientServer;
using Microsoft.Win32;

namespace Clients
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = "127.0.0.1";


       


            Client client = new Client(ip,8000);
            client.Connect();
            //client.Send(Client.FromStringToBytes(client.ToString()));
            while (true)
            {
                var data = Client.FromBytesToString(client.Get());
                if (data.Split(" ").Length >= 1) {
                    var components = data.Split(" ");
                    if (components[0] == "--START--") {
                        var reference = data.Remove(0,components[0].Length).TrimStart();
                        var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + reference.ToString();
                        if (File.Exists(path))
                        {
                            var p = new Process();
                            p.StartInfo = new ProcessStartInfo(path)
                            {
                                UseShellExecute = true
                            };
                            p.Start();
                           
                        }
                        continue;
                    }
                    if (components[0] == "--GET--")
                    {
                        string listoffiles = string.Empty;
                        Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToList().Where(file=>new FileInfo(file).Extension==".lnk").ToList().ForEach((file)=> {
                            listoffiles += new FileInfo(file).Name + "\n";
                        });

                        var tosend = Client.FromStringToBytes(listoffiles);
                        Console.WriteLine(Client.FromBytesToString(tosend));
                        client.Send(tosend);

                        continue;
                    }

                }
                Console.WriteLine(data);
            }
            Console.ReadLine();

            client.Close();
        }
    }
    public class Mouse
    {
        [DllImport("user32.dll")]
        public static extern long SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern long ClientToScreen(IntPtr hWnd, ref POINT point);
        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
            public POINT(int X, int Y)
            {
                x = X;
                y = Y;

            }
        }


    }
}
