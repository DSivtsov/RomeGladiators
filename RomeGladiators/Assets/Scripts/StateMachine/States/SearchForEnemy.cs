using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchForEnemy : IState
{
    private readonly Gladiator _gladiator;
    private readonly NavMeshAgent _navMeshAgent;

    public SearchForEnemy(Gladiator gladiator, NavMeshAgent navMeshAgent)
    {
        _gladiator = gladiator;
        _navMeshAgent = navMeshAgent;
    }
    public void Tick()
    {
        Debug.Log($"IndexOf(_gladiator)={SingletonGladiatorsManager.Instance.ListGladiators.IndexOf(_gladiator)}");
        _gladiator.Target =
            MoveToClosestTarget.ChooseTarget(SingletonGladiatorsManager.Instance.ListGladiators.IndexOf(_gladiator),
            _navMeshAgent.areaMask);
        Debug.Log($"[{_gladiator.name}] my Enemy is {_gladiator.Target.name}");
    }

    public void OnEnter() { }
    public void OnExit() { }
}