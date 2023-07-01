using UnityEngine;

public class Boostraper : MonoBehaviour, IService
{
    public GameStateMachine StateMachine => _gameStateMachine;
    public LayerMask TableMask;

    [Tooltip("Wieving current game state")]
    [SerializeField]
    private string CurrentGameState;
    [Tooltip("Prefabs")]
    [SerializeField]
    private PiecePlace PiecePlacePrefab;

    private GameStateMachine _gameStateMachine;

    void Awake()
    {
        if (AllServices.Instance.GetService<Boostraper>() != null)
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
        AllServices.Instance.RegisterService(this);
        _gameStateMachine = new GameStateMachine();
        DontDestroyOnLoad(gameObject);
    }

    private void RegisterServices()
    {
        AllServices.Instance.RegisterService(new FactoryPazzlePiece());
        AllServices.Instance.RegisterService(new FactoryPiecePlace(PiecePlacePrefab));
    }

    private void Update()
    {
        CurrentGameState = StateMachine.CurrentState;
    }
}
