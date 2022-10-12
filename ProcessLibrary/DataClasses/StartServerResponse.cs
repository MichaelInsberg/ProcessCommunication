namespace ProcessCommunication.ProcessLibrary.DataClasses;

/// <summary>
/// The StartServerResponse class
/// </summary>
public sealed class StartServerResponse
{
    /// <summary>
    /// Gets or sets the is started indicating that the Server is started
    /// </summary>
    public bool IsStarted { get; set; }

    /// <summary>
    /// Gets or sets the IP address to reach the server 
    /// </summary>
    public string IpAddress { get; set; }

    /// <summary>
    /// Gets or sets the port to reach the server
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets the server type
    /// </summary>
    public ServerType ServerType { get; set; }
    
    /// <summary>
    /// Gets or sets the serial number of the server in available otherwise an empty string
    /// </summary>
    public string SerialNumber { get; set; }

    /// <summary>
    /// Create a new instance of StartServerResponse
    /// </summary>
    public StartServerResponse()
    {
        IsStarted = false;
        IpAddress = string.Empty;
        SerialNumber = string.Empty;
    }
}
