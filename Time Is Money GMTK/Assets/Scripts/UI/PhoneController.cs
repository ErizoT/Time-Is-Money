using DG.Tweening;
using KLRB.Utility;
using UnityEngine;





public class PhoneController : MonoBehaviour
{
    public Transform moveTarget;
    public float moveDistance;


    public TweenAnimationCurve upCurve;
    public TweenAnimationCurve downCurve;



    private Tweener phoneTween;
    private Tweener phoneWobble;


    private bool phoneUp = false;
    private void Awake()
    {
        moveTarget.transform.localPosition = moveTarget.transform.localPosition.With(y: moveDistance);
        GlobalData.Instance.State.SM.SubscribeEnter(GameState.Shop, OnPhoneRaise);
        GlobalData.Instance.State.SM.SubscribeExit(GameState.Shop, OnPhoneLower);
    }

    void InitTweens()
    {
        if (phoneTween == null)
        {
            phoneTween = moveTarget.DOLocalMove( Vector3.up * moveDistance, 0f).SetAutoKill(false);
        }
    }
    void OnPhoneRaise()
    {
        InitTweens();
        phoneTween.ChangeEndValue(Vector3.zero, upCurve.duration, true).Restart();
    }


    void OnPhoneLower()
    {
        InitTweens();
        phoneTween.ChangeEndValue(Vector3.up * moveDistance, downCurve.duration, true).Restart();
    }
}
