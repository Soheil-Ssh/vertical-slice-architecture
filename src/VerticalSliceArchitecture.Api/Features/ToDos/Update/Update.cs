using VerticalSliceArchitecture.Api.Entities.ToDo;

namespace VerticalSliceArchitecture.Api.Features.ToDos.Update;

public static class Update
{
    // ReSharper disable once MemberCanBePrivate.Global
    public sealed record Request(long Id, string Title, string? Description);

    public sealed record Command(long Id, string Title, string? Description) : IRequest<Result>;

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
            var todo = await context.ToDos.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (todo is null)
                return ToDoErrors.NotFound;

            todo.Title = request.Title;
            todo.Description = request.Description;
            todo.UpdateDate = DateTime.UtcNow;

            context.ToDos.Update(todo);
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/tasks", async (Request request, ISender sender) =>
            {
                var command = request.Adapt<Command>();
                var result = await sender.Send(command);
                return result.ToHttpResult();
            });
        }
    }
}