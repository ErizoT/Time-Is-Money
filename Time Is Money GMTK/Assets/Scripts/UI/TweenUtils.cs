using UnityEngine;
using DG.Tweening;


    public abstract class TweenAnimationBase
    {
        public float duration = 1;
    }

    [System.Serializable]
    public class TweenAnimationCurve : TweenAnimationBase
    {
        public AnimationCurve upCurve = AnimationCurve.Linear(0, 0, 1, 1);

    }

    [System.Serializable]
    public class TweenAnimationEase : TweenAnimationBase
    {
        public Ease ease = Ease.Linear;
    }


