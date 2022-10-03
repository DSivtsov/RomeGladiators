using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MoveToEnemy : BaseState
{
    private readonly NavMeshAgent _navMeshAgent;
    private Vector3 _lastPosition = Vector3.zero;

    private Vector3 previousTargetPosition;
    private const float deltaTimeUpdatePath = 0.5f;
    private float sqrStoppingDistance;
    private float doubleStoppingDistance;

    public float TimeStuck;

    public MoveToEnemy(Gladiator gladiator, NavMeshAgent navMeshAgent) : base (gladiator)
    {
        _navMeshAgent = navMeshAgent;
        sqrStoppingDistance = _navMeshAgent.stoppingDistance * _navMeshAgent.stoppingDistance;
        doubleStoppingDistance = _navMeshAgent.stoppingDistance * 2;
    }

    public override void Tick()
    {
        _lastPosition = _gladiator.transform.position;
        UpdateTargetIfDemanding(_gladiator.Target.transform);
    }

    public override void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(_gladiator.Target.transform.position);
        previousTargetPosition = _gladiator.Target.transform.position;
        CountFrame.DebugLogUpdate($"{this} : after OnEnter()");
    }

    public override void OnExit()
    {
        _navMeshAgent.enabled = false;
        CountFrame.DebugLogUpdate($"{this} : after OnExit()");
    }

    void UpdateTargetIfDemanding(Transform target)
    {
        if (Vector3.SqrMagnitude(previousTargetPosition - target.position) > sqrStoppingDistance)
        {
            _navMeshAgent.SetDestination(target.position);
            previousTargetPosition = target.position;
        }
    }
    //private IEnumerator FollowTarget(Transform target)
    //{
    //    do
    //    {
    //        yield return new WaitForSeconds(deltaTimeUpdatePath);
    //        //Debug.Log($"SqrMagnitude={Vector3.SqrMagnitude(transform.position - target.position)} remainingDistance={agent.remainingDistance}");
    //        UpdateTarget(target);
    //    } while (_navMeshAgent.enabled && _navMeshAgent.remainingDistance - doubleStoppingDistance > 0);
    //    do
    //    {
    //        yield return null;
    //        //Debug.Log($"remainingDistance={agent.remainingDistance}");
    //        UpdateTarget(target);
    //    } while (_navMeshAgent.enabled && _navMeshAgent.remainingDistance - _navMeshAgent.stoppingDistance > 0);
    //    _navMeshAgent.enabled = false;
    //}
}