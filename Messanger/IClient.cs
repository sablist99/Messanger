namespace Messanger
{
    public interface IClient
    {
        public StreamReader? Reader { get; }
        public StreamWriter? Writer { get; }

        public Task ReceiveMessageAsync(StreamReader reader);
        public Task SendMessageAsync(StreamWriter writer);
        public void Print(string message);
    }
}