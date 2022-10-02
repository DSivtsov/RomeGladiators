using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;
using System.Linq.Expressions;

public class StateMachine
{
    private class Transition
    {
        private string name;
        public Func<bool> Condition { get; }
        public IState To { get; }

        public Transition(IState to, Func<bool> condition,string name)
        {
            To = to;
            Condition = condition;
            this.name = name;
        }

        public override string ToString() => name;
    }
    private int _uidGladiator;
    private IState _currentState;
   
    private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type,List<Transition>>();
    private List<Transition> _currentTransitions = new List<Transition>();

    private readonly Type TypeFinalState = typeof(Died);
    public void StateMachineTick()
    {
        var transition = GetTransition();
        if (transition != null)
            SetState(transition.To);
      
        _currentState?.Tick();
    }

    public void SetState(IState state)
    {
        if (state == _currentState)
            return;
      
        _currentState?.OnExit();
        _currentState = state;

        Type currentTypeState = _currentState.GetType();
        if (currentTypeState != TypeFinalState)
        {
            _transitions.TryGetValue(currentTypeState, out _currentTransitions);
            if (_currentTransitions == null)
            {
                CountFrame.DebugErrorLogUpdate($"StateMachine : _uid[{_uidGladiator}] _currentTransitions == null");
            } 
        }
        _currentState.OnEnter();
    }

   public void AddTransition(IState from, IState to, Func<bool> predicate, string name)
   {
        if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            _transitions[from.GetType()] = transitions;
        }
        transitions.Add(new Transition(to, predicate,name));
   }

    private Transition GetTransition()
    {
        foreach (var transition in _currentTransitions)
        {
            //CountFrame.DebugLogUpdate($"_uid[{_uidGladiator}] {transition}");
            if (transition.Condition())
                return transition; 
        }
        return null;
    }

    public void SetUIDGladiator(int uid) => _uidGladiator = uid;
    public int UIDGladiator => _uidGladiator;
}
