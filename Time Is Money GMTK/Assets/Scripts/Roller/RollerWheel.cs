using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RollerWheel : MonoBehaviour
{
    [HideInInspector]
    public int MachineId;
    [SerializeField] float rollTime;
    [SerializeField] float rollDelay;
    [SerializeField] RollerIconDisplay[] rollerIconDisplays;
    [SerializeField] RollerIconDisplay importantDisplay;
    [SerializeField] GameObject rollerParent;
    private float DegreeTarget;
    private Quaternion rollerInitialRotation;
    [SerializeField] float perStepRotation;
    [SerializeField] float rotationSpeed;
    public bool IsSpinning => _isSpinning;
    private bool _isSpinning = false;
    [HideInInspector]
    public bool continueSpinning;
    [HideInInspector]
    public RollerSymbol endingSymbol;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var roller in rollerIconDisplays)
        {
            roller.forwardsVector = this.transform.forward;
        }
        rollerInitialRotation = rollerParent.transform.localRotation;
        DegreeTarget = rollerParent.transform.localEulerAngles.x;
    }
    public void StartSpin(RollerSymbol result)
    {
        if (IsSpinning) return;
        importantDisplay.QueueRollerSymbol(result);
        foreach (var roller in rollerIconDisplays)
        {
            if (roller == importantDisplay) continue;
            roller.QueueRollerSymbol(GlobalData.Instance.rollerData.LuckPerMachine[MachineId].RollRandomSymbol());
        }
        StartCoroutine(BeginSpin());
    }
    public void StopSpin()
    {
        continueSpinning = false;
    }
    IEnumerator BeginSpin()
    {
        _isSpinning = true;
        continueSpinning = true;

        Vector3 spinStep = Vector3.zero;
        while (continueSpinning)
        {
            float stepMagnitude = Time.deltaTime * perStepRotation;
            foreach (var roller in rollerIconDisplays)
            {
                roller.RollStep(stepMagnitude);
            }
            spinStep = new Vector3(stepMagnitude, 0, 0);
            rollerParent.transform.localRotation *= Quaternion.Euler(spinStep);
            yield return null;
        }
        while (rollerParent.transform.localEulerAngles.x < DegreeTarget)
        {
            yield return null;
            spinStep = new Vector3(Time.deltaTime * perStepRotation, 0, 0);
            rollerParent.transform.localRotation *= Quaternion.Euler(spinStep);
        }
        rollerParent.transform.localRotation = rollerInitialRotation;
        _isSpinning = false;
        continueSpinning = false;
    }
}
