using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMTools;
using System;

public class SingletonGladiatorsManager : SingletonController<SingletonGladiatorsManager>
{
    [Header("Game Setting")]
    [SerializeField] private int _gladiatorsNumber;
    [Header("Data")]
    [SerializeField] private Transform _groundTransform;
    [SerializeField] private BattfleFieldSO _battfleFieldData;
    [SerializeField] private GladiatorSettingSO _gladiatorSetting;
    [SerializeField] private Gladiator _gladiatorPrefab;
    [SerializeField] private Material _gladiatorDying;
    [Header("DataTest")]
    [Tooltip("Seed for RND position gladiators, will be random if set 0")]
    [SerializeField] private int _seedPos;
    [Tooltip("Seed for RND characterestics gladiators, will be random if set 0")]
    [SerializeField] private int _seedCharacter;

    public int RandomSeedPos => _seedPos;
    public BattfleFieldSO BattfleFieldData => _battfleFieldData;
    public GladiatorSettingSO GladiatorSetting => _gladiatorSetting;
    public Gladiator GladiatorPrefab => _gladiatorPrefab;
    public Material GladiatorDyingMaterial => _gladiatorDying;
    public HashSet<int> DiedGladiatorsInCurrentSMTick => _diedGladiatorsInCurrentSMTick;
    public List<Gladiator> ListGladiators => _listGladiators;

    private GladiatorsFactory gladiators;

    private List<StateMachine> _listGladiatorStateMachine;
    private List<Gladiator> _listGladiators;
    private int _idxCurrentGladiator;
    public int IdxCurrentGladiator => _idxCurrentGladiator;
    private bool firstSMTick = true;

    //private Dictionary<int, GameObject> _gameobjectGarbage = new Dictionary<int, GameObject>();
    private HashSet<int> _diedGladiatorsInCurrentSMTick = new HashSet<int>();

    private GameManager _gameManager;

    protected override void Awake()
    {
        base.Awake();
        _battfleFieldData.SetWordldPosZerro(_groundTransform);
        _gladiatorSetting.Init(_seedCharacter);
        //_arrGladiators = new GameObject[_gladiatorsNumber];
        gladiators = new GladiatorsFactory(_gladiatorsNumber, _seedCharacter);
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        (_listGladiators, _listGladiatorStateMachine)  = gladiators.CreateGladiators();
    }

    private void Update()
    {
        if (firstSMTick)
        {
            MoveToClosestTarget.InitTempArraysForFirstStateMachineTick();
            MoveToClosestTarget.SetfirstSMTick(true);
        }
        for (_idxCurrentGladiator = 0; _idxCurrentGladiator < _listGladiatorStateMachine.Count; _idxCurrentGladiator++)
        {
            _listGladiatorStateMachine[_idxCurrentGladiator].StateMachineTick();
        }
        if (firstSMTick)
        {
            firstSMTick = false;
            MoveToClosestTarget.SetfirstSMTick(false);
        }
        if (_diedGladiatorsInCurrentSMTick.Count != 0)
            RemodeRecordsDiedGladiators();
    }

    private void RemodeRecordsDiedGladiators()
    {
        foreach (int idx in _diedGladiatorsInCurrentSMTick)
        {
            //To get possibility to stop StateMachine at any time when it will be reasoble
            _gameManager.CallCoroutineGladiatorDying(_listGladiators[idx].gameObject);
            _listGladiators.RemoveAt(idx);
            _listGladiatorStateMachine.RemoveAt(idx);
        }
        CountFrame.DebugLogUpdate($"Removed Gladiators records = {_diedGladiatorsInCurrentSMTick.Count}");
        _diedGladiatorsInCurrentSMTick.Clear();
    }

    public void PutCurrentGladiatorToRemoveFromListGladiators()
    {
        _diedGladiatorsInCurrentSMTick.Add(_idxCurrentGladiator);
        CountFrame.DebugLogUpdate($"Gladiators[{_listGladiatorStateMachine[_idxCurrentGladiator].UIDGladiator}] Died");
        if (_listGladiators.Count - _diedGladiatorsInCurrentSMTick.Count == 1)
        {
            CountFrame.DebugErrorLogUpdate($"Gladiators[{_listGladiatorStateMachine[_idxCurrentGladiator].UIDGladiator}]" +
                $" is Duncan MacLeod");
            _gameManager.FinishGame(); 
        }
    }
}
