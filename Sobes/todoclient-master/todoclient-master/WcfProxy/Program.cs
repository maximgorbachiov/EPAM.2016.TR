using System;
using System.Configuration;

namespace WcfProxy
{
    class Program
    {
        private const string address = "http://localhost:50000/WcfService";

        private static readonly string serviceApiUrl = ConfigurationManager.AppSettings["ToDoServiceUrl"];

        static void Main(string[] args)
        {
            var host = new WcfHost(address, serviceApiUrl);

            Console.WriteLine("Enter smth to start wcf or stop to end");

            using (host)
            {
                while (Console.ReadLine() != "stop")
                {
                    Console.WriteLine("Wcf starting");

                    host.OpenWcfService();

                    Console.WriteLine("Wcf started/ Press smth to close service");
                    Console.ReadLine();
                    Console.WriteLine("Wcf closing");

                    host.CloseWcfService();

                    Console.WriteLine("Wcf closed");
                    Console.WriteLine("Enter smth to start wcf or stop to end");
                }
            }
        }
    }
}
