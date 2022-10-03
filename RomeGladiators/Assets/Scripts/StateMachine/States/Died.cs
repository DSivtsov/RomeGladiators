using System;
using UnityEngine;

public class Died : BaseState
{
    public Died(Gladiator gladiator) : base(gladiator)  { }

    public override void OnEnter()
    {
        SingletonGladiatorsManager.Instance.PutCurrentGladiatorToRemoveFromListGladiators();
        _gladiator.SetImDied();
        //_gladiator.Target.RemoveTarget();
        CountFrame.DebugLogUpdate($"{this} : after OnEnter()");
    }
}