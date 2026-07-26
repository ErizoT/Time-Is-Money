using UnityEngine;
using KLRB.Utility;
using KLRB.Utility.StateMachine;

public  class GlobalGameState : MonoBehaviour
{
    public enum GameState
    {
        StartScreen,
        Slots, 
        Shop
    }

    public StateMachine<GameState> GameStateMahine;


}
