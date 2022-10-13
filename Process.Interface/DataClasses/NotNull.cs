namespace Process.Interface.DataClasses
{
    /// <summary>
    /// The Valid record
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct NotNull<T>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotNull{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException">value</exception>
        /// <exception cref="IsNullEmptyOrWhiteSpaceException">value</exception>
        public NotNull(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Value = value;
        }

        /// <inheritdoc />
        public override string? ToString()
        {
            return Value.ToString();
        }
    }
}
