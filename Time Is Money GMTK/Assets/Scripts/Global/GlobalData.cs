using KLRB.Utility;
using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class GlobalData : SingletonBehaviour<GlobalData>
{
   
    public static GlobalRollerData RollerData => Instance.rollerData;
    public GlobalRollerData rollerData;
    public GlobalGameState State;

    private float playerTime;
    public float PlayerTime
    {
        get => playerTime;
        set
        {
            OnPlayerTimeChanged?.Invoke(value);
            playerTime = value;
        }
    }
    public Action<float> OnPlayerTimeChanged;
    [SerializeField] private int playerMoney;

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
    [SerializeField] private int paidDebt = 0;
    public int PaidDebt
    {
        get => paidDebt;
        set
        {
            OnDebtPaid?.Invoke(value);
            paidDebt = value;
        }
    }
    public Action<int> OnDebtPaid;
    public float timeTickRate = 1;
    public float initialRoundDuration = 300;
    private float playerLuck;
    public float PlayerLuck { get => playerLuck; 
        set 
        {
            OnPlayerLuckChanged?.Invoke(value);
            playerLuck = value; 
        } 
    }
    public Action<float> OnPlayerLuckChanged;

    private bool gameCommenced;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        UnityEngine.Random.InitState(RandomNumberGenerator.GetInt32(int.MaxValue));
        rollerData.Initialise();

    }

  
    public void StartGame()
    {
        State.SM.ChangeState(GameState.TransitionToSlots);
        StartCoroutine(WaitForStart());
    }

    public IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(3);
        GlobalData.Instance.State.SM.ChangeState(GameState.Slots);
        StartTimer();
    }

    public void StartTimer()
    {
        gameCommenced = true;
        PlayerTime = initialRoundDuration;
    }


    void Update()
    {
        Tick();
    }

    void Tick()
    {
        if (!gameCommenced || State.Current == GameState.Shop) return;
        PlayerTime -= Time.deltaTime * timeTickRate;
        if(PlayerTime <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameCommenced = false;
        State.SM.ChangeState(GameState.GameOver);
    }



    public void RollLuck()
    {

    }
}
