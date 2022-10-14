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

    /// <summary>
    /// Serialize the object
    /// </summary>
    /// <param name="data"></param>
    /// <returns>The Json sting</returns>
    public string Serialize(NotNull<object> data)
    {
        return JsonConvert.SerializeObject(data.Value, jsonSerializerSettings);
    }
    /// <summary>
    /// Deserialize the string
    /// </summary>
    /// <param name="stringValue">The string value</param>
    /// <typeparam name="T">The Type to retrun</typeparam>
    /// <returns>The deserialized object</returns>
    /// <exception cref="NotSupportedException"></exception>
    public T DeSerialize<T>(NotEmptyOrWhiteSpace stringValue)
    {
        var deserializeObject = JsonConvert.DeserializeObject<T>(stringValue.Value, jsonSerializerSettings);
        if (deserializeObject is null)
        {
            throw new NotSupportedException($"Unable to deserialize {stringValue.Value}");
        }
        return deserializeObject;
    }

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
