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
    bool _isRolling = false;
    public bool IsRolling
    {
        get => _isRolling;
        set
        {
            OnIsRollingChanged?.Invoke(value);
            GlobalRollerData.Instance.NotifyRollerStateChanged(value);
            _isRolling = value;
        }
    }
    public Action<bool> OnIsRollingChanged;
    [SerializeField]
    Animator animatorComponent;
    [SerializeField]
    RollerWheel roller1;
    [SerializeField]
    RollerWheel roller2;
    [SerializeField]
    RollerWheel roller3;

    private static int doRollHash = Animator.StringToHash("doRoll");
    private static int doFailedRollHash = Animator.StringToHash("doFailedRoll");
    private static int doRollEndHash = Animator.StringToHash("doRollEnd");
    private static int doFailedRollEndHash = Animator.StringToHash("doFailedRollEnd");
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
        if (IsRolling)
        {
            animatorComponent.SetBool(doFailedRollHash, true);
            return;
        }
        StartCoroutine(PerformRoll());
    }
    IEnumerator PerformRoll()
    {
        GlobalData.RollerData.LuckPerMachine[SlotMachineId].Recalculate();
        IsRolling = true;
        int rollCost = GlobalRollerData.Instance.LuckPerMachine[_slotMachineId].RollCost;
        if (GlobalData.Instance.PlayerMoney < rollCost && false)
        {
            while (animatorComponent.GetBool(doFailedRollEndHash))
            {
                yield return null;
            }
            IsRolling = false;
            yield break;
        }
        GlobalData.Instance.PlayerMoney -= rollCost;
        animatorComponent.SetBool(doRollEndHash, true);
        animatorComponent.SetBool(doRollHash, true);
        while (animatorComponent.GetBool(doRollHash))
        {
            yield return null;
        }
        RollerSymbol rollerSymbol1, rollerSymbol2, rollerSymbol3;
        rollerSymbol1 = GlobalData.RollerData.LuckPerMachine[SlotMachineId].RollRandomSymbol();
        rollerSymbol2 = GlobalData.RollerData.LuckPerMachine[SlotMachineId].RollRandomSymbol();
        rollerSymbol3 = GlobalData.RollerData.LuckPerMachine[SlotMachineId].RollRandomSymbol();
        if (rollerSymbol1.SymbolId == rollerSymbol3.SymbolId)
        {
            (rollerSymbol3, rollerSymbol2) = (rollerSymbol2, rollerSymbol3); //gotta be the scummiest code I've ever written
        }
        else if (rollerSymbol2.SymbolId == rollerSymbol3.SymbolId) 
        {
            (rollerSymbol3, rollerSymbol1) = (rollerSymbol1, rollerSymbol3); //fixed case where 2 and 3 were equal
        }
        roller1.StartSpin(rollerSymbol1);
        roller2.StartSpin(rollerSymbol2);
        roller3.StartSpin(rollerSymbol3);
        yield return new WaitForSeconds(GlobalRollerData.Instance.MachineData.RollerDelay);
        float countdownDelay = GlobalRollerData.Instance.MachineData.CountdownDelay;
        roller1.StopSpin();
        yield return new WaitForSeconds(countdownDelay);
        roller2.StopSpin();
        if (rollerSymbol1.SymbolId == rollerSymbol2.SymbolId)
        {
            yield return new WaitForSeconds(countdownDelay * 1.5f);
        }
        else
        {
            yield return new WaitForSeconds(countdownDelay);
        }

        roller3.StopSpin();
        if (rollerSymbol1.SymbolId == rollerSymbol2.SymbolId && rollerSymbol2.SymbolId == rollerSymbol3.SymbolId)
        {
            rollerSymbol1.DoTripleEffect();
            Debug.Log($"Rolled a triple {rollerSymbol1.SymbolId}!");
        }
        else
        {
            rollerSymbol1.DoSingleEffect();
            rollerSymbol2.DoSingleEffect();
            rollerSymbol3.DoSingleEffect();
            Debug.Log($"Rolled a single {rollerSymbol1.SymbolId}!");
            Debug.Log($"Rolled a single {rollerSymbol2.SymbolId}!");
            Debug.Log($"Rolled a single {rollerSymbol3.SymbolId}!");
        }
        IsRolling = false;
    }
}