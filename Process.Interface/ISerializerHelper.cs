using Process.Interface.DataClasses;

namespace Process.Interface
{
    public interface ISerializerHelper
    {
        /// <summary>
        /// Serialize the object
        /// </summary>
        /// <param name="data"></param>
        /// <returns>The Json sting</returns>
        string Serialize(NotNull<object> data);

        /// <summary>
        /// Deserialize the string
        /// </summary>
        /// <param name="stringValue">The string value</param>
        /// <typeparam name="T">The Type to retrun</typeparam>
        /// <returns>The deserialized object</returns>
        /// <exception cref="NotSupportedException"></exception>
        T DeSerialize<T>(NotEmptyOrWhiteSpace stringValue);

        object DeSerialize(NotEmptyOrWhiteSpace stringValue, Type type);
    }
}
