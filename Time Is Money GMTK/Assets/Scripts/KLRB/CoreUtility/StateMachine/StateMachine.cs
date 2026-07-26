using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace KLRB.Utility.StateMachine
{


    public class StateMachine<T> : IDisposable where T : IComparable
    {
        protected T state;

        public T GetState()
        {
            return state;
        }

        private T previousState;

        public T GetPreviousState()
        {
            return previousState;
        }

        public bool UpdateStateEnabled = true;
        public bool LateUpdateStateEnabled = false;


        private bool isFirstState = true;

        //  public bool CanTransitionToSelf = false;

        public StateEvent<Action<T>> OnEnterAnyState = new();
        public StateEvent<Action<T>> OnExitAnyState = new();
        public StateEvent<Action<T>> OnActivateAnyState = new();
        public StateEvent<Action<T>> OnDeactivateAnyState = new();

        private AllStateActions ActionsOfStates;
        private bool changingState = false;

        public class StateProperties
        {
            public bool StateActive = false;
            public bool CanTransitionToSelf = false;
            public StateEvent<Action> OnEnter = new();
            public StateEvent<Action<T>> OnExitNext = new();
            public StateEvent<Action> OnExit = new();
            public StateEvent<Action> OnUpdate = new();
            public StateEvent<Action> OnLateUpdate = new();
            public StateEvent<Action> OnFixedUpdate = new();
        }

        protected class AllStateActions
        {
            public Dictionary<T, StateProperties> ActionDictionary = new();
            public T[] StatePriorityOrder;



            public AllStateActions(List<T> states)
            {
                states.Reverse();
                StatePriorityOrder = states.ToArray();
                foreach (var state in states)
                {
                    ActionDictionary.Add(state, new StateProperties());
                }
            }
        }

        public StateMachine()
        {
            Init();
        }

        public StateMachine(List<T> states)
        {
            Init(states);
        }




        public void Init(List<T> states = null)
        {

            if (states == null)
            {
                if (typeof(T).IsEnum)
                {
                    ActionsOfStates = new(EnumUtils.GetEnumValues<T>().ToList());
                }
            }
            else
            {
                ActionsOfStates = new(states);
            }

            InitializeParameters();
        }


        private void InitializeParameters()
        {
            state = GetLowestPriorityState();
        }

        public void InitUpdateEvent(bool updateState, bool fixedUpdateState)
        {
            if (updateState)
            {
                GlobalPersistentUpdater.Singleton().UpdateEvent.AddListener(EvaluateUpdateState);
            }

            if (fixedUpdateState)
            {
                GlobalPersistentUpdater.Singleton().FixedUpdateEvent.AddListener(EvaluateFixedUpdateState);
            }
        }

        public void EnableLateUpdateEvent()
        {
            LateUpdateStateEnabled = true;
        }


        public virtual void ChangeState(T _state)
        {
            if (!ActionsOfStates.ActionDictionary[_state].CanTransitionToSelf && _state.Equals(state) && !isFirstState)
            {
                return;
            }

            EvaluateExitState();
            EvaluateExitState(_state);
            if (!IsState(_state)) previousState = state;
            state = _state;
            EvaluateEnterState();
            isFirstState = false;
        }

        public bool IsState(T _state)
        {
            return state.Equals(_state);
        }




        private StateEvent<Action> GetUpdateState(T state)
        {
            if (ActionsOfStates.ActionDictionary.TryGetValue(state, out StateProperties actions))
            {
                return actions.OnUpdate;
            }

            throw new InvalidOperationException("Invalid State");
        }

        private StateEvent<Action> GetFixedUpdateState(T state)
        {
            if (ActionsOfStates.ActionDictionary.TryGetValue(state, out StateProperties actions))
            {
                return actions.OnFixedUpdate;
            }

            throw new InvalidOperationException("Invalid State");
        }


        private StateEvent<Action> GetLateUpdateState(T state)
        {
            if (ActionsOfStates.ActionDictionary.TryGetValue(state, out StateProperties actions))
            {
                return actions.OnLateUpdate;
            }

            throw new InvalidOperationException("Invalid State");
        }


        private StateEvent<Action> GetEnterState(T state)
        {
            if (ActionsOfStates.ActionDictionary.TryGetValue(state, out StateProperties actions))
            {
                return actions.OnEnter;
            }

            throw new InvalidOperationException("Invalid State");
        }

        private StateEvent<Action<T>> GetExitNextState(T state)
        {
            if (ActionsOfStates.ActionDictionary.TryGetValue(state, out StateProperties actions))
            {
                return actions.OnExitNext;
            }

            throw new InvalidOperationException("Invalid State");
        }

        private StateEvent<Action> GetExitState(T state)
        {

            if (ActionsOfStates.ActionDictionary.TryGetValue(state, out StateProperties actions))
            {
                return actions.OnExit;
            }

            throw new InvalidOperationException("Invalid State");
        }

        private void EvaluateUpdateState()
        {

            if (UpdateStateEnabled) GetUpdateState(state).Action?.Invoke();
            if (LateUpdateStateEnabled) GetLateUpdateState(state).Action?.Invoke();
        }

        private void EvaluateFixedUpdateState()
        {

            if (UpdateStateEnabled) GetFixedUpdateState(state).Action?.Invoke();

        }


        private void EvaluateEnterState()
        {
            GetEnterState(state).Action?.Invoke();
            OnEnterAnyState.Action?.Invoke(state);
        }

        private void EvaluateExitState(T nextState)
        {
            GetExitNextState(state).Action?.Invoke(nextState);
        }

        private void EvaluateExitState()
        {
            GetExitState(state).Action?.Invoke();
            OnExitAnyState.Action?.Invoke(state);
        }

        public int GetStatePriority(T state)
        {
            return (int)(object)state;
        }

        public void Activate_TryChangeState(T state)
        {
            ActivateState(state);
            TryChangeStatePriority(true, state);
        }

        public void Deactivate_TryChangeState(T state)
        {
            DeactivateState(state);
            TryChangeStatePriority();
        }

        public void ActivateState(T state)
        {
            ActionsOfStates.ActionDictionary[state].StateActive = true;
            OnActivateAnyState.Action?.Invoke(state);
        }

        public void DeactivateState(T state)
        {
            ActionsOfStates.ActionDictionary[state].StateActive = false;
            OnDeactivateAnyState.Action?.Invoke(state);
        }

        public bool IsStateActive(T state)
        {
            return ActionsOfStates.ActionDictionary[state].StateActive;
        }

        public T GetLowestPriorityState()
        {
            if (ActionsOfStates.ActionDictionary.Count <= 0)
            {
                Debug.LogError("State Machine has 0 States");
                return default;
            }


            return ActionsOfStates.StatePriorityOrder[ActionsOfStates.StatePriorityOrder.Length - 1];
        }

        public void SetStatePriorityOrder(T[] order)
        {
            ActionsOfStates.StatePriorityOrder = order;
        }

        public void TryChangeStatePriority(bool isActivatedState = false, T activatedState = default)
        {
            for (int i = 0; i < ActionsOfStates.StatePriorityOrder.Length; i++)
            {
                var lState = ActionsOfStates.StatePriorityOrder[i];
                if (IsStateActive(lState))
                {
                    bool canTransition =
                        (ActionsOfStates.ActionDictionary[lState].CanTransitionToSelf && isActivatedState &&
                         activatedState.Equals(lState)) || isFirstState;

                    if (lState.Equals(state) && !canTransition) return;
                    ChangeState(lState);
                    return;
                }
            }
        }

        public void SetTransitionToSelf(T setState, bool value)
        {
            ActionsOfStates.ActionDictionary[setState].CanTransitionToSelf = value;
        }

        public void SetTransitionToSelfAll(bool value)
        {
            foreach (var state in ActionsOfStates.ActionDictionary.Keys)
            {
                ActionsOfStates.ActionDictionary[state].CanTransitionToSelf = value;
            }
        }

        public void SubscribeEnter(T state, Action action) => Subscribe(GetEnterState(state), action);
        public void SubscribeEnterAny(Action<T> action) => Subscribe(OnEnterAnyState, action);
        public void SubscribeExitAny(Action<T> action) => Subscribe(OnExitAnyState, action);
        public void SubscribeActivateAny(Action<T> action) => Subscribe(OnActivateAnyState, action);
        public void SubscribeDeactivateAny(Action<T> action) => Subscribe(OnDeactivateAnyState, action);
        public void SubscribeExit(T state, Action action) => Subscribe(GetExitState(state), action);
        public void SubscribeUpdate(T state, Action action) => Subscribe(GetUpdateState(state), action);
        public void SubscribeFixedUpdate(T state, Action action) => Subscribe(GetFixedUpdateState(state), action);
        public void SubscribeLateUpdate(T state, Action action) => Subscribe(GetLateUpdateState(state), action);
        public void SubscribeExitNext(T state, Action<T> action) => Subscribe(GetExitNextState(state), action);
        
        
        
        public void UnsubscribeEnter(T state, Action action) => Unsubscribe(GetEnterState(state), action);
        public void UnsubscribeEnterAny(Action<T> action) => Unsubscribe(OnEnterAnyState, action);
        public void UnsubscribeExitAny(Action<T> action) => Unsubscribe(OnExitAnyState, action);
        public void UnsubscribeActivateAny(Action<T> action) => Unsubscribe(OnActivateAnyState, action);
        public void UnsubscribeDeactivateAny(Action<T> action) => Unsubscribe(OnDeactivateAnyState, action);
        public void UnsubscribeExit(T state, Action action) => Unsubscribe(GetExitState(state), action);
        public void UnsubscribeUpdate(T state, Action action) => Unsubscribe(GetUpdateState(state), action);
        public void UnsubscribeFixedUpdate(T state, Action action) => Unsubscribe(GetFixedUpdateState(state), action);
        public void UnsubscribeLateUpdate(T state, Action action) => Unsubscribe(GetLateUpdateState(state), action);
        public void UnsubscribeExitNext(T state, Action<T> action) => Unsubscribe(GetExitNextState(state), action);


        public IEnumerable<KeyValuePair<T, StateProperties>> EnumerateStates()
        {
            foreach (var kvp in ActionsOfStates.ActionDictionary) yield return kvp;
        }
        

        protected List<StateEventDelegates> actionDelegates = new();

        private void Subscribe<TDel>(StateEvent<TDel> action, TDel function) where TDel : Delegate
        {
            action.Subscribe(function);
            actionDelegates.Add(new StateEventDelegates(action, function));
        }
        
        private void Unsubscribe<TDel>(StateEvent<TDel> action, TDel function) where TDel : Delegate
        {
            action.Unsubscribe(function);
            for( int i = 0; i < actionDelegates.Count; i++)
            {
                if (actionDelegates[i].evt.Equals(action) && actionDelegates[i].del.Equals(function))
                {
                    actionDelegates.RemoveAt(i);
                    i--;
                }
            }
        }
        

        private void UnsubscribeFromAllEvents()
        {
            for (int i = 0; i < actionDelegates.Count; i++)
                actionDelegates[i].evt.Unsubscribe(actionDelegates[i].del);
        }

        public void Dispose()
        {
            UnsubscribeFromAllEvents();
        }

    }



    public class DynamicStateMachine
    {
        public State DefaultState;
        protected State state = new State();

        public State GetState()
        {
            return state;
        }

        private State previousState;

        public State GetPreviousState()
        {
            return previousState;
        }

        public StateEvent<Action<State>> OnEnterAnyState = new();


        public class State
        {
            private string name;

            public virtual string Name
            {
                get { return name; }
            }

            public virtual void OnEnter(DynamicStateMachine stateMachine)
            {
            }

            public virtual void OnExit(DynamicStateMachine stateMachine)
            {
            }

            public virtual void OnUpdate(DynamicStateMachine stateMachine)
            {
            }
        }



        public DynamicStateMachine(State defaultState)
        {
            DefaultState = defaultState;
            SetToDefaultState();
        }

        public void SetToDefaultState()
        {
            ChangeState(DefaultState);
        }


        public void InitUpdateEvent()
        {
            GlobalPersistentUpdater.Singleton().UpdateEvent.AddListener(EvaluateUpdateState);
        }

        public void ChangeState(State _state)
        {
            EvaluateExitState();
            previousState = state;
            state = _state;
            EvaluateEnterState();
        }

        public bool IsState(State _state)
        {
            return state.Equals(_state);
        }

        private void EvaluateUpdateState()
        {
            state.OnUpdate(this);
        }

        private void EvaluateEnterState()
        {
            state.OnEnter(this);
            OnEnterAnyState.Action?.Invoke(state);
        }

        private void EvaluateExitState()
        {
            state.OnExit(this);
        }


      
    }
}
     
