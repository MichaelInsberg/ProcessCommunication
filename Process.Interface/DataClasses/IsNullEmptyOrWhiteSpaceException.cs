using System.Globalization;

namespace Process.Interface.DataClasses;

/// <summary>
/// The exception is null or empty class
/// </summary>
/// <seealso cref="System.Exception" />
[Serializable]
public class IsNullEmptyOrWhiteSpaceException : Exception
{
    /// <summary>
    /// The internal message
    /// </summary>
    private const string INTERNAL_MESSAGE = "Value can not be null empty or whitespace. {0}";

    /// <summary>
    /// Initializes a new instance of the <see cref="IsNullEmptyOrWhiteSpaceException" /> class.
    /// </summary>
    public IsNullEmptyOrWhiteSpaceException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IsNullEmptyOrWhiteSpaceException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public IsNullEmptyOrWhiteSpaceException(string? message) : base(string.Format(CultureInfo.InvariantCulture, INTERNAL_MESSAGE, message))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IsNullEmptyOrWhiteSpaceException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
    public IsNullEmptyOrWhiteSpaceException(string message, Exception innerException) : base(string.Format(CultureInfo.InvariantCulture, message, message), innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IsNullEmptyOrWhiteSpaceException"/> class.
    /// </summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    protected IsNullEmptyOrWhiteSpaceException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>Throws an <see cref="ThrowIfNullOrWhiteSpace"/> if <paramref name="argument"/> is null empty or whitespace.</summary>
    /// <param name="argument">The reference type argument to validate.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    public static void ThrowIfNullOrWhiteSpace(string argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (string.IsNullOrWhiteSpace(argument))
        {
            Throw(paramName);
        }
    }

    [DoesNotReturn]
    private static void Throw(string? paramName) =>
        throw new IsNullEmptyOrWhiteSpaceException(paramName);

}
