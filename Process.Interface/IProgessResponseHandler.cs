using Process.Interface.DataClasses;

namespace Process.Interface;

public interface IProgessResponseHandler
{
    void HandleResponse(NotEmptyOrWhiteSpace command);
}
