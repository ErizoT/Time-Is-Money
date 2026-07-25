using System;
using System.Security.Cryptography;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance;
    public static GlobalRollerData RollerData => Instance.rollerData;
    public GlobalRollerData rollerData;

    public int playerTime;
    public int playerMoney;
    public float timeTickRate = 1;
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
        rollerData.Initialise();
    }
    public void RollLuck()
    {

    }
}
