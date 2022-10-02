using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class StateMachine
{
    private class Transition
    {
        public Func<bool> Condition { get; }
        public IState To { get; }

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    private IState _currentState;
   
    private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type,List<Transition>>();
    private List<Transition> _currentTransitions = new List<Transition>();


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
      
      _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
      if (_currentTransitions == null)
        {
            Debug.LogError($"StateMachine : _currentTransitions == null");
        }
      _currentState.OnEnter();
   }

   public void AddTransition(IState from, IState to, Func<bool> predicate)
   {
      if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
      {
         transitions = new List<Transition>();
         _transitions[from.GetType()] = transitions;
      }
      
      transitions.Add(new Transition(to, predicate));
   }

   private Transition GetTransition()
   {
      foreach (var transition in _currentTransitions)
         if (transition.Condition())
            return transition;
      return null;
   }
}