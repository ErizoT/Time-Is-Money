using System;
using UnityEngine;

namespace KLRB.Utility.StateMachine
{
    [Serializable]
    public struct StateKey : IEquatable<StateKey>, IComparable, IComparable<StateKey>
    {
        [SerializeField] private string state;
        private int? cachedHash;

        public StateKey(string name)
        {
            this.state = name;
            cachedHash = null;
        }

        public string Name => state;
        public int Hash => cachedHash ??= Animator.StringToHash(state); 

        public override int GetHashCode() => Hash;

        public override bool Equals(object obj) => obj is StateKey other && Equals(other);
        public bool Equals(StateKey other) => Hash == other.Hash;

        public override string ToString() => state;
       

        public static implicit operator StateKey(string s) => new StateKey(s);
        public static implicit operator string(StateKey k) => k.Name;

        public int CompareTo(StateKey other)
        { return Hash.CompareTo(other.Hash); }
        
        public int CompareTo(object obj)
        {
            if (obj is StateKey other)
            {
                return CompareTo(other);
            }
            throw new ArgumentException($"Cannot compare StateKey with {obj?.GetType().Name ?? "null"}");
        }
    }
}