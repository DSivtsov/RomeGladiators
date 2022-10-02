using UnityEngine;

internal class WaitReadyToFight : BaseState
{

    public WaitReadyToFight(Gladiator gladiator) : base(gladiator)
    {
        //_gladiator = gladiator;
    }

    public override void OnEnter()
    {
        _gladiator.SetReadyToFight(true);
    }
}