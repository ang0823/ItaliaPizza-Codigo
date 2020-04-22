using System;
using System.Net.Sockets;
using System.ServiceModel;

namespace Host
{
    public static class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(ServidrorPizzaItaliana.ServicioPizzaItaliana)))
            {
                try
                {
                    host.Open();
                    Console.WriteLine("El servidor está arriba");
                    Console.ReadLine();
                }
                catch (SocketException)
                {
                    Console.WriteLine("Error de conexion");
                }
            }
        }

    }
}
