using System.Collections;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public Transform startScreen;
    public Transform shop;
    public Transform switchButton;
    public Transform timer;
    public Transform money;
    public Transform gameOver;
  


    public bool timerIsActive => GlobalData.Instance.State.Current == GameState.Shop ||
                                 GlobalData.Instance.State.Current == GameState.Slots;

    public bool moneyIsActive => GlobalData.Instance.State.Current == GameState.Shop ||
                             GlobalData.Instance.State.Current == GameState.Slots;
    private void Awake()
    {
             
        GlobalData.Instance.State.SM.SubscribeEnter(GameState.StartScreen, () => startScreen.gameObject.SetActive(true));
        GlobalData.Instance.State.SM.SubscribeExit(GameState.StartScreen, () => startScreen.gameObject.SetActive(false));
        

        GlobalData.Instance.State.SM.SubscribeEnter(GameState.Shop, () =>
        {
            shop.gameObject.SetActive(true);
            switchButton.gameObject.SetActive(true);
        });
        
        
        GlobalData.Instance.State.SM.SubscribeExit(GameState.Shop, () =>
        {
            shop.gameObject.SetActive(false);
            switchButton.gameObject.SetActive(false);
        });


        GlobalData.Instance.State.SM.SubscribeEnter(GameState.Slots, () =>
        {
            switchButton.gameObject.SetActive(true);
        });


        GlobalData.Instance.State.SM.SubscribeExit(GameState.Slots, () =>
        {
            switchButton.gameObject.SetActive(false);
        });


        GlobalData.Instance.State.SM.SubscribeEnter(GameState.GameOver, () =>
        {
            gameOver.gameObject.SetActive(true);
        });


        GlobalData.Instance.State.SM.SubscribeExit(GameState.GameOver, () =>
        {
            gameOver.gameObject.SetActive(false);
        });


        GlobalData.Instance.State.SM.SubscribeEnterAny(OnEnterAny);
        GlobalData.Instance.State.SM.SubscribeExitAny(OnExitAny);



    }


    public void OnSwitchButtonPressed()
    {

        if(GlobalData.Instance.State.Current == GameState.Slots)
        {
            GlobalData.Instance.State.SM.ChangeState(GameState.Shop);
        }
        else if(GlobalData.Instance.State.Current == GameState.Shop)
        {
            GlobalData.Instance.State.SM.ChangeState(GameState.Slots);
        }
    }

    public void OnStartGamePressed()
    {
        GlobalData.Instance.StartGame();
    }

    void OnEnterAny(GameState state)
    {
        if(timerIsActive) timer.gameObject.SetActive(true);
        else timer.gameObject.SetActive(false);

        if (moneyIsActive) money.gameObject.SetActive(true);
        else money.gameObject.SetActive(false);
    }

   

    void OnExitAny(GameState state)
    {
       
    }




}
