namespace Neuroglia.A2A.Server;

/// <summary>
/// Defines extensions for <see cref="TaskRecord"/>s
/// </summary>
public static class TaskRecordExtensions
{

    /// <summary>
    /// Converts the <see cref="TaskRecord"/> into a new <see cref="Models.Task"/>
    /// </summary>
    /// <param name="taskRecord">The <see cref="TaskRecord"/> to convert</param>
    /// <returns>A new <see cref="Models.Task"/></returns>
    public static Models.Task AsTask(this TaskRecord taskRecord) => new()
    {
        Id = taskRecord.Id,
        SessionId = taskRecord.SessionId,
        Status = taskRecord.Status,
        History = taskRecord.History,
        Metadata = taskRecord.Metadata,
    };

}
