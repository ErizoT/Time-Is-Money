using System;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class RollerDisplay : MonoBehaviour
{
    [SerializeField]
    int _slotMachineId;
    int SlotMachineId => _slotMachineId;
    [SerializeField]
    float RollerDelay = 8f;
    [SerializeField]
    float CountdownDelay = 0.75f;
    bool _isRolling = false;
    [SerializeField]
    RollerWheel roller1;
    [SerializeField]
    RollerWheel roller2;
    [SerializeField]
    RollerWheel roller3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void DoRoll()
    {
        Debug.Log("Beginning roll!");
        if (_isRolling) { return; }
        StartCoroutine(PerformRoll());
    }
    IEnumerator PerformRoll()
    {
        RollerSymbol rollerSymbol1, rollerSymbol2, rollerSymbol3;
        rollerSymbol1 = GlobalData.RollerData.LuckPerMachine[SlotMachineId].RollRandomSymbol();
        roller1.StartSpin(rollerSymbol1);
        Debug.Log("Beginning spin 1!");
        yield return null;
        rollerSymbol2 = GlobalData.RollerData.LuckPerMachine[SlotMachineId].RollRandomSymbol();
        roller2.StartSpin(rollerSymbol2);
        yield return null;
        rollerSymbol3 = GlobalData.RollerData.LuckPerMachine[SlotMachineId].RollRandomSymbol();
        roller3.StartSpin(rollerSymbol3);
        yield return new WaitForSeconds(RollerDelay * GlobalData.Instance.timeTickRate);
        float countdownDelay = CountdownDelay * GlobalData.Instance.timeTickRate;
        roller1.StopSpin();
        yield return new WaitForSeconds(countdownDelay);
        roller2.StopSpin();
        yield return new WaitForSeconds(countdownDelay);
        roller3.StopSpin();
    }
}
