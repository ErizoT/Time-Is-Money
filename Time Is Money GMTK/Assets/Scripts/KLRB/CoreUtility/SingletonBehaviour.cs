using System;
using UnityEngine;

namespace KLRB.Utility
{

    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static T _singleton;
        public static T Instance
        {
            get
            {
                if (_singleton == null) _singleton = GameObject.FindAnyObjectByType<T>();
                return _singleton;
            }
        }


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