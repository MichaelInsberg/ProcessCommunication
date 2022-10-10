namespace ProcessCommunication.ProcessLibrary.DataClasses;

public sealed class StartServerResponse
{
    public bool IsStarted { get; set; }

    public string IpAdress { get; set; }
    
    public string SerialNumber { get; set; }

    public StartServerResponse()
    {
        IsStarted = false;
        IpAdress = string.Empty;
        SerialNumber = string.Empty;
    }
}
