using System;
using UnityEngine;
using UnityEngine.Events;


namespace KLRB.Utility
{

    public class GlobalPersistentUpdater : MonoBehaviour
    {

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Init()
        {
            SingletonInitialized = false;
        }

        public static bool SingletonInitialized { get; private set; } = false;
        private static GlobalPersistentUpdater _instance;

        public static GlobalPersistentUpdater Singleton()
        {
            if (!SingletonInitialized)
            {

                var uo = new GameObject();
                uo.name = "PersistentUpdater";
                DontDestroyOnLoad(uo);
                _instance = uo.AddComponent<GlobalPersistentUpdater>();
                SingletonInitialized = true;
            }

            return _instance;
        }

        public UnityEvent UpdateEvent = new UnityEvent();
        public UnityEvent FixedUpdateEvent = new UnityEvent();
        public UnityEvent LateUpdateEvent = new UnityEvent();
        public UnityEvent OnApplicationQuitEvent = new UnityEvent();

        public void SetReference(UnityAction update, UnityAction applicationQuit)
        {
            UpdateEvent.AddListener(update);
            OnApplicationQuitEvent.AddListener(applicationQuit);

        }

        private void Update()
        {
                UpdateEvent?.Invoke();
        }

        private void FixedUpdate()
        {
            FixedUpdateEvent?.Invoke();
        }

        private void LateUpdate()
        {
            LateUpdateEvent?.Invoke();
        }

        public void OnApplicationQuit()
        {
            OnApplicationQuitEvent?.Invoke();
        }


    }
}

