using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using ClientServer;
using Microsoft.Win32;

namespace BroadcastMessengerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("127.0.0.1",8000);
            server.Start();
            Task task = new Task(()=>server.ConnectionUpdate());
            task.Start();
            while (true)
            {
                if (server.handler.Count != 0)
                {

                    try
                    {

                    
                    Console.WriteLine("1.Get User Apps\n2.Start Users' App");
                    Console.Write("MSG: ");
                    string str = Console.ReadLine();
                    switch (str) {
                        case "1":
                            for (int i = 0; i < server.handler.Count; i++)
                            {
                                Console.WriteLine(i + ". " + server.handler[i].socket.RemoteEndPoint.ToString());
                            }
                                Console.Write("Enter user index: ");
                                str = Console.ReadLine();
                                server.Send(Server.FromStringToBytes("--GET--"),int.Parse(str));
                                string listofapps = Server.FromBytesToString(server.Get(int.Parse(str)));
                                Console.WriteLine(listofapps);
                               
                                str = "--end--";
                            break;
                        case "2":
                            Console.Write("Enter App Name: ");
                            var app = Console.ReadLine();
                            for (int i = 0; i < server.handler.Count; i++)
                            {
                                Console.WriteLine(i + ". " + server.handler[i].socket.RemoteEndPoint.ToString());
                            }
                            Console.Write("Enter User Index: ");
                            var index = Console.ReadLine();
                            server.Send(Server.FromStringToBytes("--START-- "+ app), int.Parse(index));
                            str = "--end--";
                            break;
                        default:
                            str = "--end--";
                            break;
                    }
                    if (str == "--end--") {
                        continue;
                    }
                    for (int i = 0; i < server.handler.Count; i++)
                    {
                        server.Send(Server.FromStringToBytes(str), i);
                    }
                    }
                    catch (Exception ex)
                    {

                        continue;
                    }
                }

            }
            Console.ReadLine();

            server.Close();
        }




    }




}

public class Mouse {
    [DllImport("user32.dll")]
    public static extern long SetCursorPos(int x, int y);
    [DllImport("user32.dll")]
    public static extern long ClientToScreen(IntPtr hWnd, ref POINT point);
    [DllImport("user32.dll", SetLastError = false)]
    public static extern IntPtr GetDesktopWindow();
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT {
        public int x;
        public int y;
        public POINT(int X,int Y)
        {
            x = X;
            y = Y;

        }
    }


}