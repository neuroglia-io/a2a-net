namespace A2A;

/// <summary>
/// Represents an A2A-specific exception.
/// </summary>
public sealed class A2AException
    : Exception
{

    /// <summary>
    /// Initializes a new instance of the <see cref="A2AException"/> class.
    /// </summary>
    /// <param name="code">The error code associated with the exception.</param>
    /// <param name="message">The error message, if any, associated with the exception.</param>
    public A2AException(int code, string? message = null)
        : base(message)
    {

    }

    /// <summary>
    /// Gets the error code associated with the exception.
    /// </summary>
    public int Code { get; }

}
