using UnityEngine;

public class GlobalLuck : MonoBehaviour
{
    public static GlobalLuck Instance;

    

    public void Start()
    {
        if (Instance != null && Instance != this) return;
        Instance = this;
    }
    public void RollLuck()
    {

    }
}
