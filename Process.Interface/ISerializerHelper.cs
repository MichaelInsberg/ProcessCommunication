using Process.Interface.DataClasses;

namespace Process.Interface
{
    /// <summary>
    /// The serializer helper interface
    /// </summary>
    public interface ISerializerHelper
    {
        /// <summary>
        /// Serialize the object
        /// </summary>
        /// <param name="data"></param>
        /// <returns>The serialized sting</returns>
        string Serialize(object data);

        /// <summary>
        /// Deserialize the string
        /// </summary>
        /// <param name="stringValue">The string value</param>
        /// <typeparam name="T">The type to return</typeparam>
        /// <returns>The deserialized object</returns>
        T DeSerialize<T>(string stringValue);

        /// <summary>
        /// Deserialize the string
        /// </summary>
        /// <param name="stringValue">The string value</param>
        /// <param name="type">The type to return</param>
        /// <returns>The deserialized object</returns>
        object DeSerialize(string stringValue, Type type);
    }
}
