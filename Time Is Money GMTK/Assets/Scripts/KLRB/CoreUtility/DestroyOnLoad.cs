using System;
using UnityEngine;


namespace KLRB.Utility
{

    public class DestroyOnLoad : MonoBehaviour
    {
        private void OnEnable() => Destroy(gameObject);
    }
}

