namespace ProcessCommunication.ProcessLibrary.DataClasses.Commands
{
    /// <summary>
    /// The start server class
    /// </summary>
    public sealed class CommandStartServer : CommandBase
    {
        /// <summary>
        /// Gets or sets the server types
        /// </summary>

        public ServerType ServerType { get; set; }
    }
}
