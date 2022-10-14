using ProcessCommunication.ProcessLibrary.Logic.CommunicationHandler.Client;

const string IP_ADDRESS = "127.0.0.1";
const int PORT = 58174;

var logger = new DebugLogger();
using var processClient = new ProcessClient(
    new NotNull<ILogger>(logger),
    new NotNull<ISerializerHelper>(new SerializerHelper()),
    new NotEmptyOrWhiteSpace(IP_ADDRESS),
    PORT);
using var cts = new CancellationTokenSource();
Func<IProgessResponseHandler> func = () => new ProcessServerClientCommunicationHandler();

processClient.Connect(cts.Token, func);

Console.WriteLine($"Client connected {processClient.IsConnected}");

var startServer = new CommandStartServer();
await processClient.WriteCommandAsync(startServer).ConfigureAwait(false);
Console.WriteLine("Sent data 1");
await processClient.WriteCommandAsync(startServer).ConfigureAwait(false);
Console.WriteLine("Sent data 2");
_ = Console.ReadLine();

