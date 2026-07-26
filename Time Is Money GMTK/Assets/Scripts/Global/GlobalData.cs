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
        DontDestroyOnLoad(gameObject);
        UnityEngine.Random.InitState(RandomNumberGenerator.GetInt32(int.MaxValue));
        rollerData.Initialise();
    }
    public void RollLuck()
    {

    }
}
