using UnityEngine;

[CreateAssetMenu(fileName = "RollerSymbol_Time", menuName = "Scriptable Objects/RollerSymbol_Time")]
public class RollerSymbol_Time : RollerSymbol, IWeightedListEntry
{
    public override string SymbolDescription
    {
        get
        {
            return symbolDescription + '\n' +
            $"On single rolled: {GainOrLoseSingle} {SymbolSingleValue} seconds of remaining time" + '\n' +
            $"On triple rolled: {GainOrLoseTriple} {SymbolTripleValue} seconds of remaining time";
        }
    }
    public override void DoSingleEffect()
    {
        GlobalData.Instance.PlayerTime += SymbolSingleValue;
    }
    public override void DoTripleEffect()
    {
        GlobalData.Instance.PlayerTime += SymbolTripleValue;
    }
}
