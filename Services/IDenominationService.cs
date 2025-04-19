namespace RIA.API.Services;

public interface IDenominationService
{
    IEnumerable<Dictionary<int, int>> GetDenominations(int amount);
}