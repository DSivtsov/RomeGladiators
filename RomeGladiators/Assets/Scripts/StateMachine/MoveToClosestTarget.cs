using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public static class MoveToClosestTarget
{
    //private static Transform[] Targets;
    //private static NavMeshAgent Agent;
    private static bool firstCall;

    private static List<Gladiator> _listGladiators;
    private static Vector3[] tempArr;

    static MoveToClosestTarget()
    {
        _listGladiators = SingletonGladiatorsManager.Instance.ListGladiators;
        firstCall = true;
    }

    private static void InitTempArrays()
    {
        tempArr = new Vector3[_listGladiators.Count];
        for (int i = 0; i < tempArr.Length; i++)
        {
            tempArr[i] = _listGladiators[i].transform.position;
        }
    }

    public static Gladiator ChooseTarget(int idxObject, int agentAreaMask)
    {
        if (firstCall)
            InitTempArrays();
        Vector3 positionObject = firstCall ? tempArr[idxObject] : _listGladiators[idxObject].transform.position;
        float closestTargetDistance = float.MaxValue;
        NavMeshPath Path;
        //NavMeshPath ShortestPath = null;
        Gladiator closestTarget = _listGladiators[0];
        Path = new NavMeshPath();
        Vector3 position;
        for (int i = 0; i < _listGladiators.Count; i++)
        {
            if (i == idxObject)
                continue;
            Path.ClearCorners();
            position = firstCall ? tempArr[i] : _listGladiators[i].transform.position;
            if (NavMesh.CalculatePath(positionObject, position, agentAreaMask, Path))
            {
                float distance = Vector3.Distance(positionObject, Path.corners[0]);

                for (int j = 1; j < Path.corners.Length; j++)
                {
                    distance += Vector3.Distance(Path.corners[j - 1], Path.corners[j]);
                }

                if (distance < closestTargetDistance)
                {
                    closestTargetDistance = distance;
                    //ShortestPath = Path;
                    closestTarget = _listGladiators[i];
                }
            }
        }
        if (firstCall)
            firstCall = false;
        return closestTarget;
    }
}