using Process.Interface.DataClasses;

namespace Process.Interface;

/// <summary>
/// The ILogger interface
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Log the message
    /// </summary>
    /// <param name="logMessage"></param>
    void Log(NotEmptyOrWhiteSpace logMessage);

    /// <summary>
    /// Log the message and the extension
    /// </summary>
    /// <param name="logMessage"></param>
    /// <param name="exception"></param>
    void LogException(NotEmptyOrWhiteSpace logMessage, Exception? exception);
}
