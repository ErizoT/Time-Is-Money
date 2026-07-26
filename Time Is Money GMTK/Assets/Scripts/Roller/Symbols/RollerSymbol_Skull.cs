using UnityEngine;

[CreateAssetMenu(fileName = "RollerSymbol_Skull", menuName = "Scriptable Objects/RollerSymbol_Skull")]
public class RollerSymbol_Skull : RollerSymbol, IWeightedListEntry
{
    public override string SymbolDescription
    {
        get
        {
            return symbolDescription + '\n' +
            $"On single rolled: takes away ${SymbolSingleValue}" + '\n' +
            $"On triple rolled: set money to ${SymbolTripleValue}";
        }
    }
    public override void DoSingleEffect()
    {
        GlobalData.Instance.PlayerMoney -= (int)SymbolSingleValue;
    }
    public override void DoTripleEffect()
    {
        GlobalData.Instance.PlayerMoney = (int)SymbolTripleValue;
    }
}
