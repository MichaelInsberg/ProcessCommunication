namespace ProcessCommunication.ProcessLibrary.Logic;

/// <summary>
/// The serializer helper class
/// </summary>
public sealed class SerializerHelper : ISerializerHelper
{
    private readonly JsonSerializerSettings? jsonSerializerSettings;

    /// <summary>
    /// Create a new instance of SerializerHelper
    /// </summary>
    public SerializerHelper()
    {
        jsonSerializerSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Error,
            TypeNameHandling = TypeNameHandling.Auto,
            
        };
    }

    /// <inheritdoc />
    public string Serialize(object data)
    {
        return JsonConvert.SerializeObject(data, jsonSerializerSettings);
    }

    /// <inheritdoc />
    public T DeSerialize<T>(string stringValue)
    {
        var deserializeObject = JsonConvert.DeserializeObject<T>(stringValue, jsonSerializerSettings);
        if (deserializeObject is null)
        {
            throw new NotSupportedException($"Unable to deserialize {stringValue}");
        }
        return deserializeObject;
    }

    /// <inheritdoc />
    public object DeSerialize(string stringValue, Type type)
    {
        var deserializeObject = JsonConvert.DeserializeObject(stringValue, type, jsonSerializerSettings);
        if (deserializeObject is null)
        {
            throw new NotSupportedException($"Unable to deserialize {stringValue}");
        }
        return deserializeObject;
    }

}
