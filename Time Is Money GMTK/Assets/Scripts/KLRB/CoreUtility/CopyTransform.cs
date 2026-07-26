using System;
using UnityEngine;


namespace KLRB.Utility
{
    public enum UpdateMode
    {
        Update,
        FixedUpdate,
        LateUpdate
    }
    
    public class CopyTransform : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private UpdateMode updateMode = UpdateMode.LateUpdate;

        void Update()
        {
            if (updateMode == UpdateMode.Update) 
            {
                SyncPositionAndRotation();
            }
        }
        
        private void LateUpdate()
        {
            if (updateMode == UpdateMode.LateUpdate) 
            {
                SyncPositionAndRotation();
            }
            
        }

        private void FixedUpdate()
        {
            if (updateMode == UpdateMode.FixedUpdate) 
            {
                SyncPositionAndRotation();
            }
        }
        
        void SyncPositionAndRotation()
        {
            if (target != null)
            {
                transform.position = Vector3.Lerp(transform.position,target.position,0.2f);
                //transform.rotation = target.rotation;
            }
        }

        public void SetTarget(Transform newTarget,bool setPosition = true)
        {
            if (setPosition && newTarget != null)
            {
                transform.position = newTarget.position;
            }
            
            target = newTarget;
        }
       

    }
}
