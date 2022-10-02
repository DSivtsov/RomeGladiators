using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchForEnemy : BaseState
{
    //private readonly Gladiator _gladiator;

    public SearchForEnemy(Gladiator gladiator) : base(gladiator) { }

    public override void Tick()
    {
        _gladiator.Target = MoveToClosestTarget.ChooseTarget(SingletonGladiatorsManager.Instance.IdxCurrentGladiator);
        CountFrame.DebugLogUpdate($"[{_gladiator.name}] my Enemy is {_gladiator.Target.name}");
    }

}
