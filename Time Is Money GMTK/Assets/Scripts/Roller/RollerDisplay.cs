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
    public bool IsRolling {
        get => _isRolling; 
        set { 
            OnIsRollingChanged?.Invoke(value);
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
        if (_isRolling) { return; }
        if (GlobalData.Instance.PlayerMoney < GlobalData.RollerData.LuckPerMachine[SlotMachineId].RollCost) { return; }
        StartCoroutine(PerformRoll());
    }
    IEnumerator PerformRoll()
    {
        _isRolling = true;
        int rollCost = GlobalRollerData.Instance.LuckPerMachine[_slotMachineId].RollCost;
        if (GlobalData.Instance.PlayerMoney < rollCost)
        {
            animatorComponent.SetBool(doFailedRollEndHash, true);
            animatorComponent.SetBool(doFailedRollHash, true);
            while (animatorComponent.GetBool(doFailedRollEndHash))
            {
                yield return null;
            }
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
        roller1.StartSpin(rollerSymbol1);
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
        _isRolling = false;
    }
}
