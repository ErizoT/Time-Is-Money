using System;
using System.Security.Cryptography;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance;
    [SerializeField] public static GlobalRollerData RollerData;

    public int playerTime;
    public int playerMoney;
    public float timeTickRate;
    public float playerLuck;
    public float PlayerLuck { get => playerLuck; 
        set 
        {
            OnPlayerLuckChanged?.Invoke(value);
            playerLuck = value; 
        } 
    }
    public Action<float> OnPlayerLuckChanged;

    public void Start()
    {
        if (Instance != null && Instance != this) return;
        Instance = this;
        DontDestroyOnLoad(gameObject);
        UnityEngine.Random.InitState(RandomNumberGenerator.GetInt32(int.MaxValue));
        RollerData.Initialise();
    }
    public void RollLuck()
    {

    }
}
