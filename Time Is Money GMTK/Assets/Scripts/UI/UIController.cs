using UnityEngine;

public class UIController : MonoBehaviour
{

    public Transform startScreen;
    public Transform shop;
    public Transform switchButton;


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
 
    public void OnStartGamePressed() => GlobalData.Instance.State.SM.ChangeState(GameState.Slots);


    void OnEnterAny(GameState state)
    {
     
    }

    void OnExitAny(GameState state)
    {
       
    }

  



}
