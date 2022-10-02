//#undef TRACE
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GladiatorsFactory
{
    private int _numGladiators;
    private System.Random _random;
    private BattleField _battleField;
    private GladiatorSettingSO _gladiatorSetting;

    public GladiatorsFactory(int numGladiators,
        int seedCreatePosGladiators, int seedCreateCharacterGladiators)
    {
        this._numGladiators = numGladiators;
        _gladiatorSetting = SingletonGladiatorsManager.Instance.GladiatorSetting;
        _battleField = new BattleField(seedCreatePosGladiators);
        if (seedCreateCharacterGladiators != 0)
            this._random = new System.Random(seedCreateCharacterGladiators);
        else
            this._random = new System.Random();
    }

    public (List<Gladiator>, List<StateMachine>) CreateGladiators()
    {
        List<StateMachine> _listGladiatorStateMachine = new List<StateMachine>(_numGladiators);
        List<Gladiator> _listGladiators = new List<Gladiator>(_numGladiators);
        _battleField.allAttemps = 0;
        for (int i = 0; i < _numGladiators; i++)
        {
            Gladiator newGladiator = MonoBehaviour.Instantiate(SingletonGladiatorsManager.Instance.GladiatorPrefab,
                _battleField.GetNewGladiatorPos(), Quaternion.identity);
            NamedGladiator(newGladiator, i);
            newGladiator.InitGladiator(i, _gladiatorSetting.GetRandomHealth(), _gladiatorSetting.GetRandomAttackForce());
            _listGladiators.Add(newGladiator);
            _listGladiatorStateMachine.Add(newGladiator.GetStateMachine());
            CountFrame.DebugLogUpdate($"CreateGladiators() : {newGladiator}");
        }
        CountFrame.DebugLogUpdate($"CreateGladiators() : GetNewGladiatorPos() : allAttemps={_battleField.allAttemps}");
        return (_listGladiators, _listGladiatorStateMachine);
    }

    [System.Diagnostics.Conditional("TRACE")]
    private void NamedGladiator(Gladiator newGladiator, int i)
    {
        newGladiator.name += $"[{i:00}]";
    }

    private int GetRandomValueInRange((int min, int max) range) => _random.Next(range.min, range.max + 1);
}

