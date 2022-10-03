//#undef TRACE
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Ramdomly placed chracters on field and select the parameters for them (HP/ATK)
 */
public class GladiatorsFactory
{
    private int _numGladiators;
    private BattleField _battleField;
    private GladiatorSettingSO _gladiatorSetting;

    public GladiatorsFactory(int numGladiators, int seedCreatePosGladiators)
    {
        this._numGladiators = numGladiators;
        _gladiatorSetting = SingletonGladiatorsManager.Instance.GladiatorSetting;
        _battleField = new BattleField();
    }
    /// <summary>
    /// Placed chracters on field and select the parameters for them
    /// </summary>
    /// <returns></returns>
    public (List<Gladiator>, List<StateMachine>) CreateGladiators()
    {
        List<StateMachine> _listGladiatorStateMachine = new List<StateMachine>(_numGladiators);
        List<Gladiator> _listGladiators = new List<Gladiator>(_numGladiators);
        _battleField.allAttemps = 0;
        for (int uid = 0; uid < _numGladiators; uid++)
        {
            Gladiator newGladiator = MonoBehaviour.Instantiate(SingletonGladiatorsManager.Instance.GladiatorPrefab,
                _battleField.GetNewGladiatorPos(), Quaternion.identity);
            SetNameGladiatorGO(newGladiator, uid);
            newGladiator.InitGladiator(uid, _gladiatorSetting.GetRandomHealth(), _gladiatorSetting.GetRandomAttackForce());
            _listGladiators.Add(newGladiator);
            _listGladiatorStateMachine.Add(newGladiator.GetStateMachine());
            CountFrame.DebugLogUpdate($"CreateGladiators() : {newGladiator}");
        }
        //CountFrame.DebugLogUpdate($"CreateGladiators() : GetNewGladiatorPos() : allAttemps={_battleField.allAttemps}");
        return (_listGladiators, _listGladiatorStateMachine);
    }

    [System.Diagnostics.Conditional("TRACE")]
    private void SetNameGladiatorGO(Gladiator newGladiator, int uid)
    {
        newGladiator.name += $"[{uid:00}]";
    }
}

