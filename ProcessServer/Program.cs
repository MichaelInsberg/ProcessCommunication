// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using ProcessCommunication.ProcessLibrary.DataClasses;

const string IP_ADDRESS = "127.0.0.1";
const int PORT = 58174;


var ipPAddress = IPAddress.Parse(IP_ADDRESS);
var server = new TcpListener(ipPAddress, PORT);
server.Start();

while (true)
{
    var client = server.AcceptTcpClient();
    Task.Factory.StartNew(() =>
    {
        DoCommunication(client);
    });
}


static void DoCommunication(TcpClient tcpClient)
{
    var networkStream = tcpClient.GetStream();
    var streamReader = new StreamReader(networkStream, System.Text.Encoding.Unicode);
    var result = streamReader.ReadLine();
    var obj = JsonSerializer.Deserialize<StartServer>(result);
    Console.WriteLine(obj);
}





