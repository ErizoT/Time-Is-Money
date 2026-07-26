using UnityEngine;
using KLRB.Utility;
using KLRB.Utility.StateMachine;

public enum GameState
{
    StartScreen,
    Slots,
    Shop
}

public  class GlobalGameState : MonoBehaviour
{

    private StateMachine<GameState> _sm;
    public GameState Current => SM.GetState();
    public StateMachine<GameState> SM
    {
        get 
        { 
           if(_sm == null) _sm = new StateMachine<GameState>(); 
           return _sm; 
        }  
    }

    private void Start()
    {
        SM.ChangeState(GameState.StartScreen);
    }

}
