using System.Net.Sockets;

namespace Messanger
{
    public abstract class AbstractClientOnServerSide
    {
        public Guid Id { get; }
        public StreamWriter Writer { get; }
        public StreamReader Reader { get; }

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

        public abstract Task ProcessAsync();
        public void Close()
        {
            Writer.Close();
            Reader.Close();
            client.Close();
        }
    }
}
