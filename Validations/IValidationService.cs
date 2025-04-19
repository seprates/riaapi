namespace RIA.API.Validations;

public interface IValidationService
{
    bool TryValidate<M, V>(M model, out string[] errors)
        where M : class
        where V : BaseValidator<M>, new();
}