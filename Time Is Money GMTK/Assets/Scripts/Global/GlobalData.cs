using KLRB.Utility;
using System;
using System.Security.Cryptography;
using UnityEngine;

public class GlobalData : SingletonBehaviour<GlobalData>
{
   
    public static GlobalRollerData RollerData => Instance.rollerData;
    public GlobalRollerData rollerData;
    public static GlobalGameState GameState => Instance.State;
    public GlobalGameState State;

    private int playerTime;
    public int PlayerTime
    {
        get => playerTime;
        set
        {
            OnPlayerTimeChanged?.Invoke(value);
            playerTime = value;
        }
    }
    public Action<int> OnPlayerTimeChanged;
    private int playerMoney;
    public int PlayerMoney
    {
        get => playerMoney;
        set 
        { 
            OnPlayerMoneyChanged?.Invoke(value);
            playerMoney = value;
        }
    }
    public Action<int> OnPlayerMoneyChanged;
    public float timeTickRate = 1;
    private float playerLuck;
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
        DontDestroyOnLoad(gameObject);
        UnityEngine.Random.InitState(RandomNumberGenerator.GetInt32(int.MaxValue));
        rollerData.Initialise();
    }
    public void RollLuck()
    {

    }
}
