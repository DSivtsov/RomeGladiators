using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System;

/*
 * Support class used to calculated distance based on NavMesh Path, call from SearchForEnemy
 */
public static class MoveToClosestTarget
{
    //Std agentAreaMask
    private static int agentAreaMask = NavMesh.AllAreas;
    private static bool firstSMTick = false;

    private static List<Gladiator> _listGladiators;
    private static HashSet<int> _diedGladiatorsInCurrentSMTick;
    private static Vector3[] tempArr;

    static MoveToClosestTarget()
    {
        _listGladiators = SingletonGladiatorsManager.Instance.ListGladiators;
        _diedGladiatorsInCurrentSMTick = SingletonGladiatorsManager.Instance.DiedGladiatorsInCurrentSMTick;
    }

    public static void InitTempArraysForFirstStateMachineTick()
    {
        CountFrame.DebugLogUpdate("ChooseTarget() : InitTempArraysForFirstStateMachineTick()");
        tempArr = new Vector3[_listGladiators.Count];
        for (int i = 0; i < tempArr.Length; i++)
        {
            tempArr[i] = _listGladiators[i].transform.position;
        }
    }

    public static Gladiator ChooseTarget(int idxObject)
    {
        if (firstSMTick)
            CountFrame.DebugLogUpdate("ChooseTarget() : firstCall = true");
        else
        {
            CountFrame.DebugLogUpdate($"ChooseTarget() : firstCall = false" +
            $" CurrentGladiatorCount={_listGladiators.Count} diedGladiator={_diedGladiatorsInCurrentSMTick.Count}"); 
        }
        Vector3 positionObject = firstSMTick ? tempArr[idxObject] : _listGladiators[idxObject].transform.position;
        float closestTargetDistance = float.MaxValue;
        NavMeshPath Path;
        Gladiator closestTarget = null;
        Path = new NavMeshPath();
        Vector3 position;
        // idxGladiator - index gladiator in current state of _listGladiators
        for (int idxGladiator = 0; idxGladiator < _listGladiators.Count; idxGladiator++)
        {
            if ( idxGladiator == idxObject ||
                (_diedGladiatorsInCurrentSMTick.Count != 0 && _diedGladiatorsInCurrentSMTick.Contains(idxGladiator)) )
                continue;
            Path.ClearCorners();
            position = firstSMTick ? tempArr[idxGladiator] : _listGladiators[idxGladiator].transform.position;
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
                    closestTarget = _listGladiators[idxGladiator];
                }
            }
        }
        return closestTarget;
    }

    public static void SetFirstSMTick(bool value) => firstSMTick = value;

    //after dying of gladiators the _listGladiators will be updated
    public static void UpdateListGladiators() => _listGladiators = SingletonGladiatorsManager.Instance.ListGladiators;
}