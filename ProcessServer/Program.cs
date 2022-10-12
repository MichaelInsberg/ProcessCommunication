// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Process.Interface;
using Process.Interface.DataClasses;
using ProcessCommunication.ProcessLibrary;
using ProcessCommunication.ProcessLibrary.DataClasses;
using ProcessCommunication.ProcessLibrary.Logic;

const string IP_ADDRESS = "127.0.0.1";
const int PORT = 58174;

var logger = new DebugLogger();
var server = new ProcessManager(new NotNull<ILogger>(logger), new NotEmptyOrWhiteSpace(IP_ADDRESS), PORT);
var cts = new CancellationTokenSource();
server.Start(cts.Token);

Console.WriteLine($"Server started: {server.IsStarted}");
while (true)
{
    
}







