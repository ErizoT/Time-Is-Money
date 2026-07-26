using System;
using DG.Tweening;
using KLRB.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;





public class PhoneController : MonoBehaviour
{
    public Transform moveTarget;
    public float moveDistance;

    public TextMeshProUGUI phoneText;


    public TweenAnimationCurve upCurve;
    public TweenAnimationCurve downCurve;



    private Tweener phoneTween;
    private Tweener phoneWobble;


    private bool phoneUp = false;

    private float phoneTime;
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
        phoneTime = 0;
        InitTweens();
        phoneTween.ChangeEndValue(Vector3.zero, upCurve.duration, true).Restart();
        phoneUp = true;
    }

    private void Update()
    {
        TimeSpan ts = TimeSpan.FromSeconds(phoneTime);
        phoneText.text = string.Format("{0}:{1:00}", (int)ts.Minutes, ts.Seconds);
        

        if (phoneUp)
        {
            phoneTime += Time.deltaTime;
        }
    }


    void OnPhoneLower()
    {
        InitTweens();
        phoneTween.ChangeEndValue(Vector3.up * moveDistance, downCurve.duration, true).Restart();
        phoneUp = false;
    }
}
