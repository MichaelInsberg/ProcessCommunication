namespace ProcessCommunication.ProcessLibrary.ViewModel;

/// <summary>
/// The monitor server view model class
/// </summary>
public sealed class MonitorServerViewModel
{
    private readonly ILogger logger;

    /// <summary>
    /// Create a new instance of MonitorServerViewModel
    /// </summary>
    public MonitorServerViewModel(ILogger logger)
    {
        this.logger = logger;
    }
}
