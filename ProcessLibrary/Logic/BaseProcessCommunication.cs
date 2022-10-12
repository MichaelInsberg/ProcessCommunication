namespace ProcessCommunication.ProcessLibrary.Logic
{
    /// <summary>
    /// The process communication base class
    /// </summary>
    public abstract class BaseProcessCommunication : ISerializeAble
    {
        private readonly SerializerHelper serializerHelper;

        /// <summary>
        /// Create a new instance of BaseProcessCommunication
        /// </summary>
        protected BaseProcessCommunication()
        {
            serializerHelper = new SerializerHelper();
        }

        /// <inheritdoc />
        public string Serialize()
        {
            return serializerHelper.Serialize(new NotNull<BaseProcessCommunication>(this));
        }

        /// <inheritdoc />
        public T DeSerialize<T>(NotEmptyOrWhiteSpace stringValue)
        {
            return serializerHelper.DeSerialize<T>(stringValue);
        }
    }
}
