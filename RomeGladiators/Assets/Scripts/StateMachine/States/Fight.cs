using UnityEngine;

internal class Fight : BaseState
{
    //private readonly Gladiator _gladiator;

    public Fight(Gladiator gladiator) : base(gladiator) { }

    public override void Tick()
    {
        base.Tick();
        if (_gladiator.Target != null)
        {
            _gladiator.Target.DecreaseHP(_gladiator.Attack);
        }
    }
    public override void OnExit()
    {
        _gladiator.SetReadyToFight(false);
    }
}