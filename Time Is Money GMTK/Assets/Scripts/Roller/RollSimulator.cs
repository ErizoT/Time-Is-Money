using UnityEngine;

public class RollSimulator : MonoBehaviour
{
    [SerializeField] public int rollCount = 100;
    [SerializeField] public float luck = 0.0f;
    [SerializeField] public float timeperroll = 5f;
    [SerializeField] public int startingMoney = 100;
    public void DoRolls()
    {
        for (int i = 0; i < rollCount; i++)
        {
            GlobalData.RollerData.MachineData.Recalculate();
            var rollerSymbol1 = GlobalData.RollerData.MachineData.RollRandomSymbol();
            var rollerSymbol2 = GlobalData.RollerData.MachineData.RollRandomSymbol();
            var rollerSymbol3 = GlobalData.RollerData.MachineData.RollRandomSymbol();
            if (rollerSymbol1.SymbolId == rollerSymbol2.SymbolId && rollerSymbol2.SymbolId == rollerSymbol3.SymbolId)
            {
                rollerSymbol1.DoTripleEffect();
            }
            else
            {
                rollerSymbol1.DoSingleEffect();
                rollerSymbol2.DoSingleEffect();
                rollerSymbol3.DoSingleEffect();
            }
            Debug.Log($"Roll {i}: rolled {rollerSymbol1.SymbolId}, {rollerSymbol2.SymbolId}, {rollerSymbol3.SymbolId}. Money count is {GlobalData.Instance.PlayerMoney}");
        }
    }
    public void DumpPool()
    {
        GlobalData.RollerData.MachineData.Recalculate(GlobalData.RollerData.MachineData.RollerLuck);
        GlobalData.RollerData.MachineData.RollerSymbolsWeightedList.DumpContents((GlobalRollerData.LuckData.RollerOption option) => { return GlobalData.RollerData.RollerSymbols[option.SymbolId].SymbolId; });
    }
}
