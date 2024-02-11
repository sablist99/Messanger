using ConsoleServer;
using Messanger;

AbstractServer server = new ServerObject();// создаем сервер
await server.ListenAsync(); // запускаем сервер