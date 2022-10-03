#undef TRACE
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/*
 * Main character class put to every character through Prefab
 * Contains:
 * - current chracterestic of character
 * - state, transation with conditions initialized for every character to reduce overhead at calling with parameters
 * - character is moving by navMeshAgent
 * - reactions on actions of other characters
 */
public class Gladiator : MonoBehaviour
{
    private const float DistanceToFight = 1.5f;
    private int _uid;
    private int _health;
    private int _attackForce;

    private StateMachine _stateMachine;
    public Gladiator Target { get; set; }

    public int Attack => _attackForce;
    public bool IsDied { get; private set; }

    public bool ReadyToFight { get; private set; }

    private void Awake()
    {
        var navMeshAgent = GetComponent<NavMeshAgent>();

        _stateMachine = new StateMachine();

        SearchForEnemy searchEnemy = new SearchForEnemy(this);
        MoveToEnemy moveToEnemy = new MoveToEnemy(this, navMeshAgent);
        Fight fight = new Fight(this);
        WaitReadyToFight waitReadyToFight = new WaitReadyToFight(this);
        Died died = new Died(this);
        //Exit died = new Exit(this);

        Func<bool> HasTarget() => () => Target != null;
        Func<bool> ReachedEnemy() => () =>
        {
            //DebugReachedEnemy();
            return Target != null &&
                Vector3.Distance(transform.position, Target.transform.position) < DistanceToFight;
        };
        //Func<bool> Victory() => () => Target == null;
        Func<bool> WasKilled() => () => _health == 0;
        // initially Func<bool> TargetReadyToFight() => () => Target.ReadyToFight;
        Func<bool> TargetReadyToFight() => () => Target != null && Target.ReadyToFight;
        Func<bool> TargetDied() => () => Target.IsDied;

        _stateMachine.AddTransition(searchEnemy, moveToEnemy, HasTarget(),"To=moveToEnemy, HasTarget()");

        _stateMachine.AddTransition(moveToEnemy, searchEnemy, TargetDied(), "To=fight, TargetDied()");
        _stateMachine.AddTransition(moveToEnemy, waitReadyToFight, ReachedEnemy(), "To=waitReadyToFight, ReachedEnemy()");

        _stateMachine.AddTransition(waitReadyToFight, fight, TargetReadyToFight(), "To=fight, TargetReadyToFight()");
        _stateMachine.AddTransition(waitReadyToFight, searchEnemy, TargetDied(), "To=searchEnemy, TargetDied()");

        //_stateMachine.AddTransition(fight, searchEnemy, Victory(), "To=searchEnemy, Victory()");
        _stateMachine.AddTransition(fight, searchEnemy, TargetDied(), "To=searchEnemy, TargetDied()");
        _stateMachine.AddTransition(fight, died, WasKilled(), "To=died, WasKilled()");

        _stateMachine.SetState(searchEnemy);
    }

    public void SetReadyToFight(bool value) => ReadyToFight = value;
    /// <summary>
    /// Calling from Enemy
    /// </summary>
    public void RemoveTarget() => Target = null;

    /// <summary>
    /// Calling from Enemy
    /// </summary>
    /// <param name="attack"></param>
    public void DecreaseHP(int attack)
    {
        _health -= attack;
        if (_health < 0)
            _health = 0;
    }

    private void DebugReachedEnemy()
    {
        if (Target != null)
        {
            CountFrame.DebugLogUpdate($"{this} : ReachedEnemy() : {Vector3.Distance(transform.position, Target.transform.position)} ");
        }
        else
            CountFrame.DebugLogUpdate($"{this} : ReachedEnemy() Target == null");
    }

    public void InitGladiator(int uid, int health, int attackForce)
    {
        this._uid = uid;
        this._stateMachine.SetUIDGladiator(uid);
        this._health = health;
        this._attackForce = attackForce;
    }
    public override string ToString()
    {
        return $"Gladiator[{_uid:00}] HP[{_health}] AT[{_attackForce}]";
    }

    public StateMachine GetStateMachine() => _stateMachine;

    public void SetImDied() => IsDied = true;
}

