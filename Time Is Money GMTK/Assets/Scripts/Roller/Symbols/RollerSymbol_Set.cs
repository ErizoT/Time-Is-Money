using UnityEngine;

[CreateAssetMenu(fileName = "RollerSymbol_Set", menuName = "Scriptable Objects/RollerSymbol_Set")]
public class RollerSymbol_Set : RollerSymbol, IWeightedListEntry
{
    public override string SymbolDescription
    {
        get
        {
            return symbolDescription + '\n' +
            $"On single rolled: set money to ${SymbolSingleValue}" + '\n' +
            $"On triple rolled: set money to ${SymbolTripleValue}";
        }
    }
    public override void DoSingleEffect()
    {
        GlobalData.Instance.PlayerMoney = SymbolSingleValue;
    }
    public override void DoTripleEffect()
    {
        GlobalData.Instance.PlayerMoney = SymbolTripleValue;
    }
}
