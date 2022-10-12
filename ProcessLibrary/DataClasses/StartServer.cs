namespace ProcessCommunication.ProcessLibrary.DataClasses;

/// <summary>
/// The start server class
/// </summary>
public sealed class StartServer : BaseProcessCommunication
{
    /// <summary>
    /// Gets or sets the server types
    /// </summary>

    public ServerType ServerType { get; set; }
}
