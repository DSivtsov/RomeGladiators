using UnityEngine;

public class WaitReadyToFight : BaseState
{

    public WaitReadyToFight(Gladiator gladiator) : base(gladiator) { }

    public override void OnEnter()
    {
        _gladiator.SetReadyToFight(true);
        CountFrame.DebugLogUpdate($"{this} : after OnEnter");
    }
}