using System;

public class BaseState : IState
{
    protected readonly Gladiator _gladiator;

    public BaseState(Gladiator gladiator)
    {
        _gladiator = gladiator;
    }

    public virtual void OnEnter()
    {
        //CountFrame.DebugLogUpdate($"{this} : BaseState.OnEnter()");
    }

    public virtual void OnExit()
    {
        //CountFrame.DebugLogUpdate($"{this} : BaseState.OnExit()");
    }

    public virtual void Tick()
    {
        //CountFrame.DebugLogUpdate($"{this} : BaseState.Tick()");
    }

    public override string ToString()
    {
        return $"{_gladiator} : {this.GetType().Name} Target={_gladiator.Target}";
    }
}