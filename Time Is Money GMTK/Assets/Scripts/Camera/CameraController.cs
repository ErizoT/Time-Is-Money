using Unity.Cinemachine;
using UnityEditor.PackageManager;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Animator animator;



    private void Awake()
    {
 
        GlobalData.Instance.State.SM.SubscribeEnterAny(OnEnter);
 
    }

    void OnEnter(GameState state)
    {
      string animatorState = state switch {
          GameState.StartScreen => "Start Screen",
          GameState.TransitionToSlots => "Slot Machine",
          GameState.Slots => "Slot Machine",
          GameState.Shop => "Phone",
          _ => ""};
        if (string.IsNullOrEmpty(animatorState)) return;
        animator.Play(animatorState);
    }

   

}
