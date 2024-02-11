using Messanger;
using System.Net.Sockets;

namespace ConsoleServer
{
    public class ClientOnServerSideObject : AbstractClientOnServerSide
    {
        public ClientOnServerSideObject(TcpClient tcpClient, AbstractServer serverObject) : base(tcpClient, serverObject) { }

        public override async Task ProcessAsync()
        {
            try
            {
                string? userName = await Reader.ReadLineAsync();
                string? message = $"{userName} вошел в чат";
                await server.BroadcastMessageAsync(message, Id);
                Console.WriteLine(message);
                while (true)
                {
                    try
                    {
                        message = await Reader.ReadLineAsync();
                        if (message == null) continue;
                        message = $"{userName}: {message}";
                        Console.WriteLine(message);
                        await server.BroadcastMessageAsync(message, Id);
                    }
                    catch
                    {
                        message = $"{userName} покинул чат";
                        Console.WriteLine(message);
                        await server.BroadcastMessageAsync(message, Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                server.RemoveConnection(Id);
            }
        }
    }
}
