using VerticalSliceArchitecture.Api.Entities.ToDo;

namespace VerticalSliceArchitecture.Api.Features.ToDos.GetAll;

public static class GetAll
{
    public sealed record Request(string? SearchQuery, bool? IsComplete, int Page = 1, int Take = 10);

    public sealed record Response(long Id,
        string Title,
        string? Description,
        bool IsComplete,
        DateTime? CompleteDate,
        DateTime CreateDate,
        DateTime? UpdateDate);

    public sealed record Query(string? SearchQuery, bool? IsComplete, int Page = 1, int Take = 10)
        : IRequest<Result<Pagination<Response>>>;

    public class Handler(ApplicationDbContext context) : IRequestHandler<Query, Result<Pagination<Response>>>
    {
        public async Task<Result<Pagination<Response>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var todosQuery = context.ToDos;

            int take = request.Take <= 0 ? 10 : request.Take;
            int currentPage = request.Page <= 0 ? 1 : request.Page;

            int count = await todosQuery.CountAsync(cancellationToken: cancellationToken);
            if (count == 0)
                return ToDoErrors.NotFound;

            int skip = (currentPage - 1) * take;
            if (skip >= count)
            {
                currentPage = (int)Math.Ceiling(count / (double)take);
                skip = (currentPage - 1) * take;
            }

            var items = await todosQuery.Skip(skip)
                .Take(take)
                .ProjectToType<Response>()
                .ToListAsync(cancellationToken: cancellationToken);
            return new Pagination<Response>(items, currentPage, count, take);
        }
    }

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/tasks", async ([AsParameters] Request request, ISender sender) =>
            {
                var query = request.Adapt<Query>();
                var result = await sender.Send(query);
                return result.ToHttpResult();
            });
        }
    }
}