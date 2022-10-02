using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MoveFollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private NavMeshAgent agent;
    private Vector3 previousTargetPosition;
    private const float deltaTimeUpdatePath = 0.5f;
    private float sqrStoppingDistance;
    private float doubleStoppingDistance;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (_target)
        {
            InitFollowTraget(); 
        }
    }
    /// <summary>
    /// Init parameters for Coroutine FollowTarget by parameters of NavMeshAgent and _target
    /// </summary>
    private void InitFollowTraget()
    {
        agent.destination = _target.position;
        previousTargetPosition = _target.position;
        sqrStoppingDistance = agent.stoppingDistance * agent.stoppingDistance;
        doubleStoppingDistance = agent.stoppingDistance * 2;
        StartCoroutine(FollowTarget(_target));
    }

    private IEnumerator FollowTarget(Transform target)
    {
        do
        {
            yield return new WaitForSeconds(deltaTimeUpdatePath);
            //Debug.Log($"SqrMagnitude={Vector3.SqrMagnitude(transform.position - target.position)} remainingDistance={agent.remainingDistance}");
            UpdateTarget(target);
        } while (agent.enabled && agent.remainingDistance - doubleStoppingDistance > 0);
        do
        {
            yield return null;
            //Debug.Log($"remainingDistance={agent.remainingDistance}");
            UpdateTarget(target);
        } while (agent.enabled && agent.remainingDistance - agent.stoppingDistance > 0);
        agent.enabled = false;

        void UpdateTarget(Transform target)
        {
            if (Vector3.SqrMagnitude(previousTargetPosition - target.position) > sqrStoppingDistance)
            {
                agent.SetDestination(target.position);
                previousTargetPosition = target.position;
            }
        }
    }

    public void SetDestinantion(Transform target)
    {
        this._target = target;
        InitFollowTraget();
    }
}
