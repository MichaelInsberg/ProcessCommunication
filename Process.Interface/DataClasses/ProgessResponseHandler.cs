namespace Process.Interface.DataClasses;

public sealed class ProgessResponseHandler :IProgessResponseHandler
{
    public void HandleResponse(NotEmptyOrWhiteSpace command)
    {
        Console.WriteLine($"Response is {command}");
    }
}
