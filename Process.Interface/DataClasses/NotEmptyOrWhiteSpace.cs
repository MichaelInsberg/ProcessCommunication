namespace Process.Interface.DataClasses;

/// <summary>
/// The not empty or white space record
/// </summary>
public readonly struct NotEmptyOrWhiteSpace
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Create a new instance of NotEmptyOrWhiteSpace
    /// </summary>
    public NotEmptyOrWhiteSpace(string value)
    {
        IsNullEmptyOrWhiteSpaceException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }
    /// <inheritdoc />

    public override string ToString()
    {
        return Value;
    }
}
