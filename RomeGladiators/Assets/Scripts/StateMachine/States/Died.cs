using System;
using UnityEngine;

internal class Died : BaseState
{
    //private int DyingTimeLeftTicks = 5;

    //private Vector3 _deltaDown;

    public Died(Gladiator gladiator) : base(gladiator)
    {
        //_deltaDown = 2 * Vector3.down / DyingTimeLeftTicks;
    }

    //public override void Tick()
    //{
    //    _gladiator.transform.position += _deltaDown;
    //    DyingTimeLeftTicks--;
    //}

    //public bool isDied() => DyingTimeLeftTicks < 0;

    public override void OnEnter()
    {
        base.OnEnter();
        SingletonGladiatorsManager.Instance.PutCurrentGladiatorToRemoveFromListGladiators();
        _gladiator.Target.RemoveTarget();
        //_gladiator.GetComponent<MeshRenderer>().material = SingletonGladiatorsManager.Instance.GladiatorDyingMaterial;
    }

    //public override void OnExit()
    //{
    //    base.OnExit();
    //    SingletonGladiatorsManager.Instance.PutCurrentGladiatorToRemoveFromStateMachinAndGameObject();
    //}
}