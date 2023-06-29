internal class MainMenuState : IGameState
{
    readonly GameStateMachine _stateMachine;

    public MainMenuState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }
}
