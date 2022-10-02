using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMTools;

[CreateAssetMenu(fileName = "GladiatorCharacterSO", menuName = "SO/GladiatorCharacterSO")]
public class GladiatorSettingSO : ScriptableObject
{
    private const int MinHealth = 5;
    private const int MaxHealth = 100;
    private const int MinAttackForce = 1;
    private const int MaxAttackForce = 50;

    [Header("Gladiators Setting")]
    [Range(MinHealth, MaxHealth), SerializeField] private int _minHealth = 15;
    [Range(MinHealth, MaxHealth), SerializeField] private int _maxHealth = 75;
    [Range(MinAttackForce, MaxAttackForce), SerializeField] private int _minAttackForce = 5;
    [Range(MinAttackForce, MaxAttackForce), SerializeField] private int _maxAttackForce = 15;

    private int _prevMaxHealth;
    private int _prevMaxAttack;
    private System.Random _random;

    public (int min, int max) GladiatorHealth => (_minHealth, _maxHealth);
    public (int min, int max) GladiatorAttackForce => (_minAttackForce, _maxAttackForce);

    private void Awake()
    {
        _prevMaxHealth = _maxHealth;
        _prevMaxAttack = _maxAttackForce;
    }

    public void Init(int seedCreateCharacterGladiators)
    {
        if (seedCreateCharacterGladiators != 0)
            this._random = new System.Random(seedCreateCharacterGladiators);
        else
            this._random = new System.Random();
    }

    public int GetRandomHealth() => GetRandomValueInRange(GladiatorHealth);
    public int GetRandomAttackForce() => GetRandomValueInRange(GladiatorAttackForce);

    private int GetRandomValueInRange((int min, int max) range) => _random.Next(range.min, range.max + 1);

    private void OnValidate()
    {
        CheckRange(ref _minHealth, ref _maxHealth, ref _prevMaxHealth);
        CheckRange(ref _minAttackForce, ref _maxAttackForce, ref _prevMaxAttack);
    }

    private void CheckRange(ref int minValue, ref int maxValue, ref int prevMaxValue)
    {
        if (maxValue < minValue)
        {
            if (prevMaxValue != maxValue)
            {
                minValue = maxValue;
            }
            else
            {
                maxValue = minValue;
            }
        }
        prevMaxValue = maxValue;
    }
}
