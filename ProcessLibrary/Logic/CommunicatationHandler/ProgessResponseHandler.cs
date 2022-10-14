namespace ProcessCommunication.ProcessLibrary.Logic.CommunicatationHandler;

/// <summary>
/// The progess response handler class
/// </summary>
public sealed class ProgessResponseHandler :IProgessResponseHandler
{
    public void HandleResponse(NotEmptyOrWhiteSpace command)
    {
        Console.WriteLine($"Response is {command}");
    }
}
