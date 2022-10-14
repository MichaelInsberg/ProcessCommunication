using ProcessCommunication.ProcessLibrary.Logic.CommunicationHandler.Server;

const string IP_ADDRESS = "127.0.0.1";
const int PORT = 58174;

var logger = new DebugLogger();
using var server = new ProcessServer(
    new NotNull<ILogger>(logger),
    new NotNull<ISerializerHelper>(new SerializerHelper()),
    new NotEmptyOrWhiteSpace(IP_ADDRESS), 
    PORT);
using var cts = new CancellationTokenSource();

Func<IProcessServerCommunicationHandler> func = () => new ProcessServerCommunicationHandler();

server.Start(func,cts.Token);

logger.Log(new NotEmptyOrWhiteSpace($"Server started: {server.IsStarted}"));
logger.Log(new NotEmptyOrWhiteSpace("Press CTRL C to finish the application"));
var isCanceled = false;
Console.CancelKeyPress += (_, _) =>
{
    isCanceled = true;
};

while (!isCanceled)
{
    // 13.10.2022 MInsberg  : Do Nothing     
}







