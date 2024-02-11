using System.Net.Sockets;

namespace Messanger
{
    public abstract class AbstractServer
    {
        protected TcpListener tcpListener { get; set; }
        protected IList<AbstractClientOnServerSide> clients { get; set; }


        public void RemoveConnection(Guid id)
        {
            AbstractClientOnServerSide? client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                clients.Remove(client);
            }
            client?.Close();
        }

        public async Task BroadcastMessageAsync(string message, Guid id)
        {
            foreach (var client in clients)
            {
                if (client.Id != id)
                {
                    await client.Writer.WriteLineAsync(message);
                    await client.Writer.FlushAsync();
                }
            }
        }

        public void Disconnect()
        {
            foreach (var client in clients)
            {
                client.Close();
            }
            tcpListener.Stop();
        }

        public abstract Task ListenAsync();
    }
}
