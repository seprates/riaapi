using System.Text.Json;
namespace RIA.API.Validations;

public class ValidationService : IValidationService
{
    //private readonly ILogger _logger;

    //public ValidationService(ILogger logger) => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public bool TryValidate<M, V>(M model, out string[] errors)
        where M : class
        where V : BaseValidator<M>, new()
    {
        if (model is null)
        {
            errors = [$"{typeof(M).Name} must not be null or empty."];
            return false;
        }

        try
        {
            errors = null;
            var result = new V().Validate(model);
            if (!result.IsValid)
            {
                errors = result.Errors.Select(e => $"{e.ErrorCode}: {e.ErrorMessage}").ToArray();
            }
            return result.IsValid;
        }
        catch (Exception e)
        {
            var modelInJson = string.Empty;
            try
            {
                modelInJson = JsonSerializer.Serialize(model);
            }
            catch (Exception ex)
            {
                //_logger.Error(ex, "--> Failed to serialize the model");
            }
            var message = $"--> {nameof(ValidationService)} failed to validate {typeof(M).FullName}";
            //_logger.Error(e, message);

            errors = ["Validation failed due to some serious errors. For more details, refer to logs."];
            return false;
        }
    }
}