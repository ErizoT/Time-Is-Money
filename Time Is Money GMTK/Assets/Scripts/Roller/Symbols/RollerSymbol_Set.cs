using UnityEngine;

[CreateAssetMenu(fileName = "RollerSymbol_Set", menuName = "Scriptable Objects/RollerSymbol_Set")]
public class RollerSymbol_Set : RollerSymbol, IWeightedListEntry
{
    public override void DoSingleEffect()
    {
        GlobalData.Instance.PlayerMoney = SymbolSingleValue;
    }
    public override void DoTripleEffect()
    {
        GlobalData.Instance.PlayerMoney = SymbolTripleValue;
    }
}
