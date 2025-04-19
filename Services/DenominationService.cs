namespace RIA.API.Services;

public class DenominationService(int[] cartridges) : IDenominationService
{
    private readonly int[] Cartridges = cartridges;
    private readonly IList<int> _state = [];
    private readonly IList<Dictionary<int, int>> _solution = [];

    public IEnumerable<Dictionary<int, int>> GetDenominations(int amount)
    {
        GetDenominations(amount, 0);
        return _solution;
    }

    #region Private Methods
    private void GetDenominations(int amount, int start)
    {
        if (amount == 0 && _state.Count > 0) {
            Dictionary<int, int> temp = [];
            foreach (int c in Cartridges)
                temp[c] = 0;

            foreach (var v in _state)
                temp[v]++;
            _solution.Add(temp);
        }
        if (amount < 0 || start >= Cartridges.Length) return;
        for (int i = start; i < Cartridges.Length; i++)
        {
            _state.Add(Cartridges[i]);
            GetDenominations(amount-Cartridges[i], i);
            _state.RemoveAt(_state.Count-1);
        }
    }
    #endregion Private Methods
}