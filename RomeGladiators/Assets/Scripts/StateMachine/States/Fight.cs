using UnityEngine;

internal class Fight : IState
{
    private readonly Gladiator _gladiator;
    private readonly Animator _animator;
    private float _resourcesPerSecond = 1;

    private float _nextTakeResourceTime;

    public Fight(Gladiator gladiator)
    {
        _gladiator = gladiator;
    }

    public void Tick()
    {
        Debug.LogError($"{this.GetType().Name}");
        if (_gladiator.Target != null)
        {
            if (_nextTakeResourceTime <= Time.time)
            {
                _nextTakeResourceTime = Time.time + (1f / _resourcesPerSecond);

            }
        }
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }
}