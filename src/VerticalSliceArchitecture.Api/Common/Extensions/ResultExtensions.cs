using MapsterMapper;
using VerticalSliceArchitecture.Api.Common.Contracts;

namespace VerticalSliceArchitecture.Api.Common.Extensions;

/// <summary>
/// Provides extension methods to convert domain results into HTTP responses.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts a non-generic <see cref="Result"/> into an HTTP result.
    /// </summary>
    /// <param name="result">The domain result to convert.</param>
    /// <returns>
    /// An <see cref="IResult"/> representing either a successful OK response with an empty API response,
    /// or a problem details response based on the error.
    /// </returns>
    public static IResult ToHttpResult(this Result.Result result)
        => result.IsSuccess
            ? Results.Ok(ApiResponse.Success())
            : CreateProblem(result.Error);

    extension<T>(Result<T> result)
    {
        /// <summary>
        /// Converts a generic <see cref="Result{T}"/> into an HTTP result containing the data on success.
        /// </summary>
        /// <returns>
        /// An <see cref="IResult"/> with a successful OK response wrapping <see cref="ApiResponse{T}"/>,
        /// or a problem details response on failure.
        /// </returns>
        public IResult ToHttpResult()
            => result.IsSuccess
                ? Results.Ok(ApiResponse<T>.Success(result.Data))
                : CreateProblem(result.Error);

        /// <summary>
        /// Converts a generic <see cref="Result{T}"/> into an HTTP result after mapping the data using an AutoMapper mapper.
        /// </summary>
        /// <typeparam name="TResponse">The target type after mapping.</typeparam>
        /// <param name="mapper">The mapper instance used to transform the result data.</param>
        /// <returns>
        /// An <see cref="IResult"/> with a successful OK response containing the mapped data inside <see cref="ApiResponse{TResponse}"/>,
        /// or a problem details response if the result indicates failure.
        /// </returns>
        public IResult ToHttpResult<TResponse>(IMapper mapper)
        {
            if (!result.IsSuccess)
                return CreateProblem(result.Error);

            var mapped = mapper.Map<TResponse>(result.Data);

            return Results.Ok(ApiResponse<TResponse>.Success(mapped));
        }
    }

    /// <summary>
    /// Creates an HTTP problem details response from a domain error.
    /// </summary>
    /// <param name="error">The domain error containing code, description, and status code mapping.</param>
    /// <returns>An <see cref="IResult"/> configured with the problem details.</returns>
    private static IResult CreateProblem(Error.Error error)
    {
        var statusCode = error.GetStatusCode();

        var problem = new ApiProblemDetails
        {
            Title = error.Code,
            Detail = error.Description,
            Status = statusCode
        };

        return Results.Problem(
            title: problem.Title,
            detail: problem.Detail,
            statusCode: statusCode);
    }
}