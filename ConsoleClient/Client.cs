using Messanger;
using System.Net.Sockets;

namespace ConsoleClient
{
    public class Client : IClient
    {
        private readonly string? userName;
        private const string host = "127.0.0.1";
        private const int port = 8888;

        public StreamReader? Reader { get; }
        public StreamWriter? Writer { get; }

        public Client()
        {
            using TcpClient client = new TcpClient();
            Console.Write("Введите свое имя: ");
            userName = Console.ReadLine();
            Console.WriteLine($"Добро пожаловать, {userName}");

            try
            {
                client.Connect(host, port); //подключение клиента
                Reader = new StreamReader(client.GetStream());
                Writer = new StreamWriter(client.GetStream());
                run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Writer?.Close();
                Reader?.Close();
            }
        }

        private async void run()
        {
            try
            {
                if (Writer is null || Reader is null) return;
                // запускаем новый поток для получения данных
                Task.Run(() => ReceiveMessageAsync(Reader));
                // запускаем ввод сообщений
                await SendMessageAsync(Writer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task SendMessageAsync(StreamWriter writer)
        {
            // сначала отправляем имя
            await writer.WriteLineAsync(userName);
            await writer.FlushAsync();
            Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");

            while (true)
            {
                string? message = Console.ReadLine();
                await writer.WriteLineAsync(message);
                await writer.FlushAsync();
            }
        }

        public async Task ReceiveMessageAsync(StreamReader reader)
        {
            while (true)
            {
                try
                {
                    // считываем ответ в виде строки
                    string? message = await reader.ReadLineAsync();
                    // если пустой ответ, ничего не выводим на консоль
                    if (string.IsNullOrEmpty(message)) continue;
                    Print(message);//вывод сообщения
                }
                catch
                {
                    break;
                }
            }
        }

        public void Print(string message)
        {
            if (OperatingSystem.IsWindows())    // если ОС Windows
            {
                var position = Console.GetCursorPosition(); // получаем текущую позицию курсора
                int left = position.Left;   // смещение в символах относительно левого края
                int top = position.Top;     // смещение в строках относительно верха
                                            // копируем ранее введенные символы в строке на следующую строку
                Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
                // устанавливаем курсор в начало текущей строки
                Console.SetCursorPosition(0, top);
                // в текущей строке выводит полученное сообщение
                Console.WriteLine(message);
                // переносим курсор на следующую строку
                // и пользователь продолжает ввод уже на следующей строке
                Console.SetCursorPosition(left, top + 1);
            }
            else Console.WriteLine(message);
        }
    }
}
