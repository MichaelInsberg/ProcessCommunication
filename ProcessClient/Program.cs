using ProcessCommunication.ProcessLibrary.Logic.CommunicationHandler.Client;

const string IP_ADDRESS = "127.0.0.1";
const int PORT = 58174;

var logger = new DebugLogger();
using var processClient = new ProcessClient(logger, new SerializerHelper(), IP_ADDRESS, PORT);
using var cts = new CancellationTokenSource();
Func<IProgressClientResponseHandler> func = () => new ProcessServerClientCommunicationHandler(logger);

processClient.Connect(func, cts.Token);

Console.WriteLine($"Client connected {processClient.IsConnected}");

var startServer = new CommandStartServer();
await processClient.WriteCommandAsync(startServer).ConfigureAwait(false);
Console.WriteLine("Sent data 1");
await processClient.WriteCommandAsync(startServer).ConfigureAwait(false);
Console.WriteLine("Sent data 2");
_ = Console.ReadLine();

