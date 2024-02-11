using System.Net.Sockets;

namespace Messanger
{
    public abstract class AbstractClientOnServerSide
    {
        public Guid Id { get; }
        protected StreamWriter Writer { get; }
        protected StreamReader Reader { get; }

        protected TcpClient client { get; }
        protected AbstractServer server { get; }

        public AbstractClientOnServerSide(TcpClient tcpClient, AbstractServer serverObject)
        {
            Id = Guid.NewGuid();
            client = tcpClient;
            server = serverObject;
            var stream = client.GetStream();
            Reader = new StreamReader(stream);
            Writer = new StreamWriter(stream);
        }

        public async Task SendMessage(string message)
        {
            await Writer.WriteLineAsync(message);
            await Writer.FlushAsync();
        }

        public abstract Task ProcessAsync();
        public void Close()
        {
            Writer.Close();
            Reader.Close();
            client.Close();
        }
    }
}
