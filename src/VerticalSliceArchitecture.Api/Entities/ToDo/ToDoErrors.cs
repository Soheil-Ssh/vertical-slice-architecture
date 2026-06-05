using VerticalSliceArchitecture.Api.Common.Error;

namespace VerticalSliceArchitecture.Api.Entities.ToDo;

public static class ToDoErrors
{
    /// <summary>
    /// Not found to do by id error
    /// </summary>
    public static readonly Error NotFound = new("ToDo.NotFound", "To Do not found by id.", ErrorType.NotFound);
}