using VerticalSliceArchitecture.Api.Entities.ToDo;

namespace VerticalSliceArchitecture.Api.Features.ToDos.Create;

public static class Create
{
    // ReSharper disable once MemberCanBePrivate.Global
    public sealed record Request(string Title, string? Description);

    public sealed record Command(string Title, string? Description) : IRequest<Result>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Description)
                .MaximumLength(1000);
        }
    }

    public class Handler(ApplicationDbContext context) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var todo = request.Adapt<ToDo>();
            todo.CreateDate = DateTime.UtcNow;
            await context.ToDos.AddAsync(todo, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/tasks", async (Request request, ISender sender) =>
            {
                var command = request.Adapt<Command>();
                var result = await sender.Send(command);
                return result.ToHttpResult();
            });
        }
    }
}