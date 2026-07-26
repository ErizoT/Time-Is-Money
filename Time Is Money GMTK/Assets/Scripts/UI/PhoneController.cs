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
        moveTarget.transform.localPosition = moveTarget.transform.localPosition.With(y: -moveDistance);
        GlobalData.Instance.State.SM.SubscribeEnter(GameState.Shop, OnPhoneRaise);
        GlobalData.Instance.State.SM.SubscribeExit(GameState.Shop, OnPhoneLower);
    }

    void InitTweens()
    {
        if (phoneTween == null)
        {
            phoneTween = moveTarget.DOLocalMoveY(-moveDistance, 0f).SetAutoKill(false);
        }
    }
    void OnPhoneRaise()
    {
        InitTweens();
        phoneTween.ChangeEndValue(0f,upCurve.duration, true).Restart();
    }


    void OnPhoneLower()
    {
        InitTweens();
        phoneTween.ChangeEndValue(-moveDistance, downCurve.duration, true).Restart();
    }
}
