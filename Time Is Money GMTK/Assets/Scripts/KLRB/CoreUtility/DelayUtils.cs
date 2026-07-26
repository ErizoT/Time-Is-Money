using System;
using System.Collections;
using UnityEngine;

namespace KLRB.Utility
{
    public static class DelayUtils
    {
        public static IEnumerator Delay_Action(this MonoBehaviour mono, WaitForSeconds time, Action action)
        {
            if (mono.enabled && mono.gameObject.activeInHierarchy)
            {
                var coro = DelayedAction(time, action);
                mono.StartCoroutine(coro);

                return coro;
            }
            return null;
        }
        private static IEnumerator DelayedAction(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action.Invoke();
        }
        private static IEnumerator DelayedAction(WaitForSeconds time, Action action)
        {
            yield return time;
            action.Invoke();
        }
    }
}