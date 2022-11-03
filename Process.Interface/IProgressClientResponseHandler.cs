using Process.Interface.DataClasses;

namespace Process.Interface;

/// <summary>
/// The progress client response handler interface
/// </summary>
public interface IProgressClientResponseHandler
{
    /// <summary>
    /// The handle command method
    /// </summary>
    /// <param name="command">The receive command</param>
    void HandleResponse(string command);
}
