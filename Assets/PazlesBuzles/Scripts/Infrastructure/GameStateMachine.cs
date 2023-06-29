using System.Collections.Generic;
using System.Linq;

public class GameStateMachine : IStateSwitcher<IGameState>
{
    public string CurrentState => _currentState.GetType().ToString();

    private IGameState _currentState;
    private readonly List<IGameState> _states = new List<IGameState>();

    public GameStateMachine()
    {
        _states = new List<IGameState>
        {
            new BoostraperState(this),
            new MainMenuState(this),
            new LoadLevelState(this),
            new GameState(this),

        };
    }

    public TState GetGameState<TState>() where TState : IGameState
        => (TState)_states.FirstOrDefault(state => state is TState);

    public void StateSwitch<TState>() where TState : IGameState
    {
        _currentState?.Exit();
        _currentState = _states.FirstOrDefault(state => state is TState);
        _currentState.Enter();
    }
}