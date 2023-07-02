using UnityEngine;

public class Boostraper : MonoBehaviour, IService
{
    public GameStateMachine StateMachine => _gameStateMachine;

    [Tooltip("Wieving current game state")]
    [SerializeField]
    private string CurrentGameState;
    [Header("Prefabs")]
    [SerializeField]
    private PiecePlace PiecePlacePrefab;
    [SerializeField]
    private SoundManager SoundManagerPrefab;

    private GameStateMachine _gameStateMachine;

    void Awake()
    {
        if (AllServices.GetService<Boostraper>() != null)
        {
            Debug.Log("Boostraper already exist");
            Destroy(gameObject);
            return;
        }

        Init();
        RegisterServices();

        StateMachine.StateSwitch<BoostraperState>();
    }

    private void Init()
    {
        AllServices.RegisterService(this);
        _gameStateMachine = new GameStateMachine();
        DontDestroyOnLoad(gameObject);

        SoundManager soundManager = Instantiate(SoundManagerPrefab, transform.parent);
        soundManager.Init();
    }

    private void RegisterServices()
    {
        AllServices.RegisterService(new FactoryPazzlePiece());
        AllServices.RegisterService(new FactoryPiecePlace(PiecePlacePrefab));
    }

    private void Update()
    {
        CurrentGameState = StateMachine.CurrentState;
    }
}
