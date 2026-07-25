using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance;

    

    public void Start()
    {
        if (Instance != null && Instance != this) return;
        Instance = this;
    }
    public void RollLuck()
    {

    }
}
