using Process.Interface.DataClasses;

namespace Process.Interface
{
    /// <summary>
    /// The serialize able interface
    /// </summary>
    public interface ISerializeAble
    {
        /// <summary>
        /// Serialize the object to string
        /// </summary>
        /// <returns>The serialize object as string</returns>
        string Serialize();
        
        /// <summary>
        /// Deserialize the object from the string
        /// </summary>
        /// <param name="stringValue">The serialize object string</param>
        /// <typeparam name="T">The type to serialize</typeparam>
        /// <returns>The serialized object</returns>
        T DeSerialize<T>(NotEmptyOrWhiteSpace stringValue);
    }
}
