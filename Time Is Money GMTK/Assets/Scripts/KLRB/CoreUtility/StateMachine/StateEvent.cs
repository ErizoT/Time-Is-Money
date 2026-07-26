using System;

namespace KLRB.Utility.StateMachine
{

    public class StateEvent<T> : IStateEvent where T : Delegate
    {
#nullable enable
        public T? Action;

        private void sub(Delegate action)
        {
            Action = (T?)Delegate.RemoveAll(Action, action);
            Action = (T?)Delegate.Combine(Action, action);
        }

        public void unsub(Delegate action)
        {
            Action = (T?)Delegate.RemoveAll(Action, action);
        }

        public void Subscribe(Delegate action)
        {
            sub(action);
        }

        public void Unsubscribe(Delegate action)
        {
            unsub(action);
        }
    }


    public class StateEventDelegates
    {
        public IStateEvent evt;
        public Delegate del;

        public StateEventDelegates(IStateEvent evt, Delegate del)
        {
            this.evt = evt;
            this.del = del;
        }
    }

    public interface IStateEvent
    {
        public void Subscribe(Delegate action);
        public void Unsubscribe(Delegate action);
    }
}

