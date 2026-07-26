using DG.Tweening;
using UnityEngine;





public class PhoneController : MonoBehaviour
{



    public TweenAnimationCurve upCurve;
    public TweenAnimationCurve downCurve;



    private Tween phoneTween;
    private void Awake()
    {
      //  GlobalData.Instance.State.SM.SubscribeEnter(GameState.Shop, () => animator.Play("Phone"));
      //  GlobalData.Instance.State.SM.SubscribeExit(GameState.Shop, () => animator.Play("Phone"));
    }
}
