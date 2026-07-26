using System.Collections.Generic;

namespace KLRB.Utility
{
    public class SetStack<T>
    {
        private readonly Stack<T> _stack = new();
        private readonly HashSet<T> _set = new();

        public int Count => _stack.Count;

        public bool Push(T item)
        {
            if (_set.Add(item)) 
            {
                _stack.Push(item);
                return true;
            }
            return false;
        }

        public T Pop()
        {
            var item = _stack.Pop();
            _set.Remove(item);
            return item;
        }

        public T Peek() => _stack.Peek();

        public bool Contains(T item) => _set.Contains(item);

        public void Clear()
        {
            _stack.Clear();
            _set.Clear();
        }

        public IEnumerable<T> AsEnumerable() => _stack;
    }
}