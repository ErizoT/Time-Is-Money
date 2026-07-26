using UnityEngine;
using KLRB.Utility;
using KLRB.Utility.StateMachine;

public enum GameState
{
    StartScreen,
    TransitionToSlots,
    Slots,
    Shop,
    GameOver
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
        _sm.ChangeState(GameState.StartScreen);
    }

}
