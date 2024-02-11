using ConsoleServer;
using Messanger;

AbstractServer server = new ConsoleServer.ConsoleServer();// создаем сервер
await server.ListenAsync(); // запускаем сервер