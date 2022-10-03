using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaintingOnArena : BaseState
{
    private static int _shiftForUIDGladiator;
    public WaintingOnArena(Gladiator gladiator) : base(gladiator) { }

    public override void OnEnter()
    {
        _gladiator.SetReadyToFight(true);
    }

    public static void InitShiftForUIDGladiator()
    {
        _shiftForUIDGladiator = GetBitDepth(SingletonGladiatorsManager.Instance.NumGladiators);
    }

    private static int GetBitDepth(int numGladiators)
    {
        int bitDepth = 0;
        do
        {
            bitDepth++;
            numGladiators >>= 1;
        } while (numGladiators > 0);
        return bitDepth;
    }
    /// <summary>
    /// Arena Number format "uid2-uid1"
    /// </summary>
    /// <param name="uid1"></param>
    /// <param name="uid2"></param>
    /// <returns></returns>
    private int GetArenaNumber(int uid1, int uid2)
    {
        int numArena;
        if (uid1 > uid2)
            numArena = uid1 << _shiftForUIDGladiator + uid2;
        else
            numArena = uid2 << _shiftForUIDGladiator + uid1;
        return numArena;
    }
}