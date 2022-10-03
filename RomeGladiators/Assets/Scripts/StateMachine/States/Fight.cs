using UnityEngine;

public class Fight : BaseState
{
    //private readonly Gladiator _gladiator;

    public Fight(Gladiator gladiator) : base(gladiator) { }

    public override void Tick()
    {
        if (_gladiator.Target != null)
        {
            _gladiator.Target.DecreaseHP(_gladiator.Attack);
        }
        CountFrame.DebugLogUpdate($"{this} : after Tick()");
    }
    public override void OnExit()
    {
        _gladiator.SetReadyToFight(false);
        CountFrame.DebugLogUpdate($"{this} : after OnExit");
    }
}