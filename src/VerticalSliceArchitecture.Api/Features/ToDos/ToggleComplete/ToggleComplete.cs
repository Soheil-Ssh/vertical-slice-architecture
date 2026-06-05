using VerticalSliceArchitecture.Api.Entities.ToDo;

namespace VerticalSliceArchitecture.Api.Features.ToDos.ToggleComplete;

public class ToggleComplete
{
    public sealed record Command(long Id) : IRequest<Result<bool>>;

    public class Handler(ApplicationDbContext context) : IRequestHandler<Command, Result<bool>>
    {
        public async Task<Result<bool>> Handle(Command request, CancellationToken cancellationToken)
        {
            var todo = await context.ToDos
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (todo is null)
                return ToDoErrors.NotFound;

            todo.IsCompleted = !todo.IsCompleted;
            todo.CompletedDate = todo.IsCompleted ? DateTime.UtcNow : null;

            context.ToDos.Update(todo);
            await context.SaveChangesAsync(cancellationToken);
            return todo.IsCompleted;
        }
    }

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/tasks/toggle-complete/{id:long}", async (long id, ISender sender) =>
            {
                var command = new Command(id);
                var result = await sender.Send(command);
                return result.ToHttpResult();
            });
        }
    }
}