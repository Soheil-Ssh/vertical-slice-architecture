using VerticalSliceArchitecture.Api.Entities.ToDo;

namespace VerticalSliceArchitecture.Api.Features.ToDos.Delete;

public static class Delete
{
    public sealed record Command(long Id) : IRequest<Result>;

    public class Handler(ApplicationDbContext context) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var todo = await context.ToDos
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (todo is null)
                return ToDoErrors.NotFound;

            context.ToDos.Remove(todo);
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/tasks/{id:long}", async (long id, ISender sender) =>
            {
                var command = new Command(id);
                var result = await sender.Send(command);
                return result.ToHttpResult();
            });
        }
    }
}