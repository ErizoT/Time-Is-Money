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
            var rollerSymbol1 = GlobalData.RollerData.MachineData.RollRandomSymbol();
            var rollerSymbol2 = GlobalData.RollerData.MachineData.RollRandomSymbol();
            var rollerSymbol3 = GlobalData.RollerData.MachineData.RollRandomSymbol();
        }
    }
}
