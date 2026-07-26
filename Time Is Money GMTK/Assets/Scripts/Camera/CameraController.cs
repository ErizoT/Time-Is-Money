using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Animator animator;



    private void Awake()
    {
 
        GlobalData.Instance.State.SM.SubscribeEnter(GameState.StartScreen, () => animator.Play("Start Screen"));
        GlobalData.Instance.State.SM.SubscribeEnter(GameState.Slots, () => animator.Play("Slot Machine"));
        GlobalData.Instance.State.SM.SubscribeEnter(GameState.Shop, () => animator.Play("Phone"));

    }


   

}
