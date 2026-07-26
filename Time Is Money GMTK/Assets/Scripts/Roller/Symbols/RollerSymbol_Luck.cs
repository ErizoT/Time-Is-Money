using UnityEngine;

[CreateAssetMenu(fileName = "RollerSymbol_Luck", menuName = "Scriptable Objects/RollerSymbol_Luck")]
public class RollerSymbol_Luck : RollerSymbol, IWeightedListEntry
{
    public override void DoSingleEffect()
    {
        GlobalData.Instance.PlayerLuck *= SymbolSingleValue;
    }
    public override void DoTripleEffect()
    {
        GlobalData.Instance.PlayerLuck *= SymbolTripleValue;
    }
}
