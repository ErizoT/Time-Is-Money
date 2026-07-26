using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KLRB.Utility
{
    public static class YieldExtensions
    {
        public static IEnumerator WhenAllCoroutines(this MonoBehaviour host, IEnumerable<IEnumerator> coroutines)
        {
            int remaining = coroutines.Count();
            foreach (var c in coroutines)
                host.StartCoroutine(Run(c));
            yield return new WaitUntil(() => remaining == 0);

            IEnumerator Run(IEnumerator r)
            {
                yield return r;
                remaining--;
            }
        }
    }
}