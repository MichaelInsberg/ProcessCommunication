using System.Net.Sockets;

namespace Process.Interface;

/// <summary>
/// The process client interface
/// </summary>
public interface IProcessTcpClient : IDisposable
{
    // Returns the stream used to read and write data to the remote host.
    NetworkStream GetStream();
}
