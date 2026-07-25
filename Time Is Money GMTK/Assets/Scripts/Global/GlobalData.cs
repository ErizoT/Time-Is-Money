using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance;

    public int playerTime;
    public int playerMoney;
    public float timeTickRate;
    public float playerLuck;

    public void Start()
    {
        if (Instance != null && Instance != this) return;
        Instance = this;
    }
    public void RollLuck()
    {

    }
}
