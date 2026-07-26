using UnityEngine;

[CreateAssetMenu(fileName = "RollerSymbol_Multiplier", menuName = "Scriptable Objects/RollerSymbol_Multiplier")]
public class RollerSymbol_Multiplier : RollerSymbol, IWeightedListEntry
{
    public override void DoSingleEffect()
    {
        GlobalData.Instance.PlayerMoney *= SymbolSingleValue;
    }
    public override void DoTripleEffect()
    {
        GlobalData.Instance.PlayerMoney *= SymbolTripleValue;
    }
}
