using UnityEngine;

[CreateAssetMenu(fileName = "RollerSymbol_Time", menuName = "Scriptable Objects/RollerSymbol_Time")]
public class RollerSymbol_Time : RollerSymbol, IWeightedListEntry
{
    public override void DoSingleEffect()
    {
        GlobalData.Instance.PlayerTime *= SymbolSingleValue;
    }
    public override void DoTripleEffect()
    {
        GlobalData.Instance.PlayerTime *= SymbolTripleValue;
    }
}
