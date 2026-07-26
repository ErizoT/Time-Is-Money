using System;
using System.Collections;
using UnityEngine;

namespace KLRB.Utility
{
    public static class CoroutineExtensions
    {
        public static void WaitForNextFrame(this MonoBehaviour mb, Action callback)
        {
            mb.StartCoroutine(Coroutine(callback));
            static IEnumerator Coroutine(Action callback)
            {
                yield return null;
                callback.Invoke();
            }
        }
        
      
            public static void RunInstant(IEnumerator routine)
            {
                while (routine.MoveNext())
                {
                    if (routine.Current is IEnumerator nested)
                        RunInstant(nested);
                }
            }
        
    }
}