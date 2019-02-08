using System.Collections.Generic;
using UnityEngine;

namespace TestCompany.Common
{
    public class MonoBehaviourFiniteStateMachine<T> : MonoBehaviour
    {
        public event EventHandler<T> OnStateChange;

        public T CurrentState { get; private set; }

        private readonly Dictionary<T, EventHandler> _transitions = new Dictionary<T, EventHandler>();

        protected void AddTransition(T state, EventHandler stateChangeHandler)
        {
            if (!_transitions.ContainsKey(state))
                _transitions.Add(state, stateChangeHandler);
        }

        protected void SetState(T state)
        {
            CurrentState = state;
            OnStateChange?.Invoke(CurrentState);

            if (_transitions.TryGetValue(state, out var callback))
                callback();
        }
    }
}