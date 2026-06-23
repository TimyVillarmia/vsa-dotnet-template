using ErrorOr;

namespace Project.API.Common;

public static class ErrorOrExtensions
{
    public static IResult ToProblem(this List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Results.Problem();
        }

        var firstError = errors[0];

        var statusCode = firstError.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        return Results.Problem(
            statusCode: statusCode,
            title: firstError.Description,
            extensions: new Dictionary<string, object?>
            {
                ["errors"] = errors.Select(error => new
                {
                    error.Code,
                    error.Description
                })
            });
    }
}