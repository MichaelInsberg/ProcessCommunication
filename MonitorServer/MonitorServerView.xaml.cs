namespace ProcessCommunication.MonitorServer;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public sealed partial class MonitorServerView : Window
{
    /// <summary>
    /// Create a new instance of MainWindow
    /// </summary>
    public MonitorServerView()
    {
        var model = new MonitorServerViewModel();
        DataContext = model;
        InitializeComponent();
    }
}
