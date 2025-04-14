namespace Neuroglia.A2A;

/// <summary>
/// Enumerates all types of parts
/// </summary>
public static class PartType
{

    /// <summary>
    /// Indicates a data part
    /// </summary>
    public const string Data = "data";
    /// <summary>
    /// Indicates a file part
    /// </summary>
    public const string File = "file";
    /// <summary>
    /// Indicates a text part
    /// </summary>
    public const string Text = "text";
  
    /// <summary>
    /// Gets a new <see cref="IEnumerable{T}"/> containing all supported values
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all supported values</returns>
    public static IEnumerable<string> AsEnumerable()
    {
        yield return Data;
        yield return File;
        yield return Text;
    }

}