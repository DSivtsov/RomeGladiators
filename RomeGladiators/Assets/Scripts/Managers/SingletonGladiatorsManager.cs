using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMTools;
using System.Linq;

/*
 * Main module:
 * - Contains the main parameters and settings
 * - Made as Singleton
 * - Partial data stored in coresponding ScriptableObject (for compact transfer to correspondent classes)
 * - Contain data for testing (seeds)
 * - Manage all StateMachine
 * - Decision on finish Game
 */
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
    public int NumGladiators => _gladiatorsNumber;

    private GladiatorsFactory gladiators;

    private List<StateMachine> _listGladiatorStateMachine;
    private List<Gladiator> _listGladiators;
    private int _idxCurrentGladiator;
    public int IdxCurrentGladiator => _idxCurrentGladiator;
    private bool firstSMTick = true;

    private HashSet<int> _diedGladiatorsInCurrentSMTick = new HashSet<int>();

    private GameManager _gameManager;
    private System.Random random = new System.Random();

    protected override void Awake()
    {
        base.Awake();
        _battfleFieldData.SetWordldPosZerro(_groundTransform);
        _seedCharacter = _seedCharacter != 0 ? _seedCharacter : random.Next();
        _gladiatorSetting.Init(_seedCharacter);
        _seedPos = _seedPos != 0 ? _seedPos : random.Next();
        gladiators = new GladiatorsFactory();
        WaintingOnArena.InitShiftForUIDGladiator();
        _gameManager = FindObjectOfType<GameManager>();
        Debug.LogWarning($"_seedPos={_seedPos} _seedCharacter={_seedCharacter}");
    }

    private void Start()
    {
        (_listGladiators, _listGladiatorStateMachine)  = gladiators.CreateGladiators();
    }
    //Single poiny to call every StateMachine to reduce numbers of Update()
    private void Update()
    {
        //At first call will made the mass calculation distance between all obejcts to reduce overhead the resolving references
        if (firstSMTick)
        {
            MoveToClosestTarget.InitTempArraysForFirstStateMachineTick();
            MoveToClosestTarget.SetFirstSMTick(true);
        }
        for (_idxCurrentGladiator = 0; _idxCurrentGladiator < _listGladiatorStateMachine.Count; _idxCurrentGladiator++)
        {
            _listGladiatorStateMachine[_idxCurrentGladiator].StateMachineTick();
        }
        if (firstSMTick)
        {
            firstSMTick = false;
            MoveToClosestTarget.SetFirstSMTick(false);
        }
        if (_diedGladiatorsInCurrentSMTick.Count != 0)
            RemodeRecordsDiedGladiators();
    }
    /// <summary>
    /// Update the List<> after finish the StateMachine Tick
    /// </summary>
    private void RemodeRecordsDiedGladiators()
    {
        int newNumGladiators = _listGladiators.Count - _diedGladiatorsInCurrentSMTick.Count;
        List<StateMachine> updatedListGladiatorStateMachine =  new List<StateMachine>(newNumGladiators);
        List<Gladiator> updatedLstGladiators = new List<Gladiator>(newNumGladiators);
        //Debug.LogWarning($"newNumGladiators={newNumGladiators} OldCount={_listGladiators.Count} DiedCount={_diedGladiatorsInCurrentSMTick.Count}");
        //Debug.Log($"delIdx=[{string.Join(" ", _diedGladiatorsInCurrentSMTick)}]"); 
        for (int oldIdx = 0; oldIdx < _listGladiators.Count; oldIdx++)
        {
            if (!_diedGladiatorsInCurrentSMTick.Contains(oldIdx))
            {
                updatedListGladiatorStateMachine.Add(_listGladiatorStateMachine[oldIdx]);
                updatedLstGladiators.Add(_listGladiators[oldIdx]);
            }
            else
            {
                //To get possibility to stop StateMachine at any time when it will be reasoble
                _gameManager.CallCoroutineGladiatorDying(_listGladiators[oldIdx].gameObject);
            }
        }
        _listGladiatorStateMachine = updatedListGladiatorStateMachine;
        _listGladiators = updatedLstGladiators;
        MoveToClosestTarget.UpdateListGladiators();
        CountFrame.DebugLogUpdate($"SingletonGladiatorsManager : Removed Gladiators records =" +
            $" {_diedGladiatorsInCurrentSMTick.Count}");
        _diedGladiatorsInCurrentSMTick.Clear();
    }
    /// <summary>
    /// The List<> with object will stay unchageable till finish StateMachine Tick
    /// </summary>
    public void PutCurrentGladiatorToRemoveFromListGladiators()
    {
        _diedGladiatorsInCurrentSMTick.Add(_idxCurrentGladiator);
        CountFrame.DebugLogUpdate($"SingletonGladiatorsManager :" +
            $" Gladiators[{_listGladiatorStateMachine[_idxCurrentGladiator].UIDGladiator:00}] Died");
        //After Tick when died the last opponet StateMachine is stopped - Game finish
        if (_listGladiators.Count - _diedGladiatorsInCurrentSMTick.Count == 1)
        {
            DuncanMacLeod();
        }
    }

    private void DuncanMacLeod()
    {
        CountFrame.DebugErrorLogUpdate(
            $"Gladiators[{_listGladiatorStateMachine[(_idxCurrentGladiator == 1) ? 0 : 1].UIDGladiator:00}]" +
            $" is Duncan MacLeod");
        _gameManager.FinishGame();
        Debug.LogWarning("StateMachine continue current cycle till end can be used the throw Exeception to evecuate?");
    }
}
