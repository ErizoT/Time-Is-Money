using System.Collections.Generic;
using UnityEngine;

namespace KLRB.Utility
{
    public abstract class UnitySerializedHashSet<TKey> : HashSet<TKey>, ISerializationCallbackReceiver
    {
        public UnitySerializedHashSet() : base() { }
        
        [SerializeField, HideInInspector]
        private List<TKey> keyData = new List<TKey>();
        
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.Clear();
            for (int i = 0; i < this.keyData.Count; i++)
            {
                this.Add(keyData[i]);
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            this.keyData.Clear();
            foreach (var item in this)
            {
                this.keyData.Add(item);
            }
        }
    }
}