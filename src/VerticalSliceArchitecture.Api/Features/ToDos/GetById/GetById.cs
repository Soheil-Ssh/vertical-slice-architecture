using VerticalSliceArchitecture.Api.Entities.ToDo;

namespace VerticalSliceArchitecture.Api.Features.ToDos.GetById;

public static class GetById
{
    public sealed record Response(long Id,
        string Title,
        string? Description,
        bool IsComplete,
        DateTime? CompleteDate,
        DateTime CreateDate,
        DateTime? UpdateDate);

    public sealed record Query(long Id) : IRequest<Result<Response>>;

    public class Handler(ApplicationDbContext context) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var todo = await context.ToDos
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (todo is null)
                return ToDoErrors.NotFound;

            return todo.Adapt<Response>();
        }
    }

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/tasks/{id:long}", async (long id, ISender sender) =>
            {
                var query = new Query(id);
                var result = await sender.Send(query);
                return result.ToHttpResult();
            });
        }
    }
}