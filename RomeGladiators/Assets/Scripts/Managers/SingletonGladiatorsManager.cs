using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMTools;
public class SingletonGladiatorsManager : SingletonController<SingletonGladiatorsManager>
{
    [Header("Game Setting")]
    [SerializeField] private int _gladiatorsNumber;
    [Header("Data")]
    [SerializeField] private Transform _groundTransform;
    [SerializeField] private BattfleFieldSO _battfleFieldData;
    [SerializeField] private GladiatorSettingSO _gladiatorSetting;
    [SerializeField] private Gladiator _gladiatorPrefab;
    [Header("DataTest")]
    [Tooltip("Seed for RND position gladiators, will be random if set 0")]
    [SerializeField] private int _seedPos;
    [Tooltip("Seed for RND characterestics gladiators, will be random if set 0")]
    [SerializeField] private int _seedChracter;

    public BattfleFieldSO BattfleFieldData => _battfleFieldData;

    public GladiatorSettingSO GladiatorSetting => _gladiatorSetting;

    public Gladiator GladiatorPrefab => _gladiatorPrefab;
    public List<StateMachine> ListGladiatorStateMachine => _listGladiatorStateMachine;
    public List<Gladiator> ListGladiators => _listGladiators;

    private GladiatorsFactory gladiators;

    private List<StateMachine> _listGladiatorStateMachine;
    private List<Gladiator> _listGladiators;

    protected override void Awake()
    {
        base.Awake();
        _battfleFieldData.SetWordldPosZerro(_groundTransform);
        _gladiatorSetting.Init(_seedChracter);
        //_arrGladiators = new GameObject[_gladiatorsNumber];
        gladiators = new GladiatorsFactory(_gladiatorsNumber, _seedPos, _seedChracter);
    }


    private void Start()
    {
        (_listGladiators, _listGladiatorStateMachine)  = gladiators.CreateGladiators();
    }
    int count = 10;
    private void Update()
    {
        if (count-- < 0)
            return;

        for (int i = 0; i < _listGladiatorStateMachine.Count; i++)
        {
            _listGladiatorStateMachine[i].StateMachineTick();
        }
    }
}
