using UnityEngine;
using UnityEngine.AI;
using System.Collections;

internal class MoveToEnemy : IState
{
    private readonly Gladiator _gladiator;
    private readonly NavMeshAgent _navMeshAgent;
    private Vector3 _lastPosition = Vector3.zero;

    private Vector3 previousTargetPosition;
    private const float deltaTimeUpdatePath = 0.5f;
    private float sqrStoppingDistance;
    private float doubleStoppingDistance;

    public float TimeStuck;


    public MoveToEnemy(Gladiator gladiator, NavMeshAgent navMeshAgent)
    {
        _gladiator = gladiator;
        _navMeshAgent = navMeshAgent;
    }

    public void Tick()
    {
        if (Vector3.Distance(_gladiator.transform.position, _lastPosition) <= 0f)
            TimeStuck += Time.deltaTime;

        _lastPosition = _gladiator.transform.position;
        UpdateTarget(_gladiator.Target.transform);

    }

    public void OnEnter()
    {
        TimeStuck = 0f;
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(_gladiator.Target.transform.position);

        previousTargetPosition = _gladiator.Target.transform.position;
        sqrStoppingDistance = _navMeshAgent.stoppingDistance * _navMeshAgent.stoppingDistance;
        doubleStoppingDistance = _navMeshAgent.stoppingDistance * 2;
        //StartCoroutine(FollowTarget(_target));
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
        //_animator.SetFloat(Speed, 0f);
    }

    private IEnumerator FollowTarget(Transform target)
    {
        do
        {
            yield return new WaitForSeconds(deltaTimeUpdatePath);
            //Debug.Log($"SqrMagnitude={Vector3.SqrMagnitude(transform.position - target.position)} remainingDistance={agent.remainingDistance}");
            UpdateTarget(target);
        } while (_navMeshAgent.enabled && _navMeshAgent.remainingDistance - doubleStoppingDistance > 0);
        do
        {
            yield return null;
            //Debug.Log($"remainingDistance={agent.remainingDistance}");
            UpdateTarget(target);
        } while (_navMeshAgent.enabled && _navMeshAgent.remainingDistance - _navMeshAgent.stoppingDistance > 0);
        _navMeshAgent.enabled = false;
    }
    void UpdateTarget(Transform target)
    {
        if (Vector3.SqrMagnitude(previousTargetPosition - target.position) > sqrStoppingDistance)
        {
            _navMeshAgent.SetDestination(target.position);
            previousTargetPosition = target.position;
        }
    }
}