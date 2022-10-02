#undef TRACE
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gladiator : MonoBehaviour
{
    private int _uid;
    private int _health;
    private int _attackForce;

    private StateMachine _stateMachine;
    public Gladiator Target { get; set; }

    private void Awake()
    {
        var navMeshAgent = GetComponent<NavMeshAgent>();

        _stateMachine = new StateMachine();

        SearchForEnemy searchEnemy = new SearchForEnemy(this, navMeshAgent);
        MoveToEnemy moveToEnemy = new MoveToEnemy(this, navMeshAgent);
        Fight fight = new Fight(this);

        Func<bool> HasTarget() => () => Target != null;
        Func<bool> StuckForOverASecond() => () => moveToEnemy.TimeStuck > 1f;
        Func<bool> ReachedEnemy() => () => Target != null &&
                                      Vector3.Distance(transform.position, Target.transform.position) < 1f;

        _stateMachine.AddTransition(searchEnemy, moveToEnemy, HasTarget());
        _stateMachine.AddTransition(moveToEnemy, searchEnemy, StuckForOverASecond());
        _stateMachine.AddTransition(moveToEnemy, fight, ReachedEnemy());

        _stateMachine.SetState(searchEnemy);
    }

    public void InitGladiator(int uid, int health, int attackForce)
    {
        this._uid = uid;
        this._health = health;
        this._attackForce = attackForce;
    }
    public override string ToString()
    {
        return $"Gladiator[{_uid}] HP[{_health}] AT[{_attackForce}]";
    }

    public StateMachine GetStateMachine() => _stateMachine;
}

