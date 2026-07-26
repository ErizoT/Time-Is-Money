using System;
using UnityEngine;

namespace KLRB.Utility
{

    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static T _singleton;
        public static T Singleton => _singleton;

        protected virtual void Awake()
        {
            if (_singleton == null)
            {
                _singleton = this as T;
            }
            else
            {
                if (this != _singleton)
                {
                    Destroy(this);
                }
            }
        }

        private void OnApplicationQuit()
        {
            _singleton = null;
        }
    }
}