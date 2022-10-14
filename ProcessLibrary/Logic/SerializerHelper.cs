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
    public string Serialize(NotNull<object> data)
    {
        return JsonConvert.SerializeObject(data.Value, jsonSerializerSettings);
    }

    /// <inheritdoc />
    public T DeSerialize<T>(NotEmptyOrWhiteSpace stringValue)
    {
        var deserializeObject = JsonConvert.DeserializeObject<T>(stringValue.Value, jsonSerializerSettings);
        if (deserializeObject is null)
        {
            throw new NotSupportedException($"Unable to deserialize {stringValue.Value}");
        }
        return deserializeObject;
    }

    /// <inheritdoc />
    public object DeSerialize(NotEmptyOrWhiteSpace stringValue, Type type)
    {
        var deserializeObject = JsonConvert.DeserializeObject(stringValue.Value, type, jsonSerializerSettings);
        if (deserializeObject is null)
        {
            throw new NotSupportedException($"Unable to deserialize {stringValue.Value}");
        }
        return deserializeObject;
    }

}
