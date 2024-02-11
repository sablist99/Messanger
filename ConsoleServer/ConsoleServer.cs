using Messanger;
using System.Net;
using System.Net.Sockets;

namespace ConsoleServer
{
    public class ConsoleServer : AbstractServer
    {
        public ConsoleServer()
        {
            tcpListener = new TcpListener(IPAddress.Any, 8888); ;
            clients = new List<AbstractClientOnServerSide>();
        }

        public  override async Task ListenAsync()
        {
            try
            {
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                    ConsoleClientOnServerSide clientObject = new(tcpClient, this);
                    clients.Add(clientObject);
                    Task.Run(clientObject.ProcessAsync);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }
    }
}
