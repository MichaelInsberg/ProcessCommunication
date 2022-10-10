// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using ProcessCommunication.ProcessLibrary.DataClasses;

const string IP_ADDRESS = "127.0.0.1";
const int PORT = 58174;


Console.WriteLine("Hello, World!");
var ipPAddress = IPAddress.Parse(IP_ADDRESS);

var client = new TcpClient();
client.Connect(ipPAddress, PORT);

var networkStream = client.GetStream();
var streamWriter = new StreamWriter(networkStream, System.Text.Encoding.Unicode);
var startServer = new StartServer();
var stringValue = JsonSerializer.Serialize(startServer);
streamWriter.WriteLine(stringValue);
streamWriter.Flush();

Console.WriteLine("Sent data");
Console.ReadLine();
