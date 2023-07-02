public interface IStateSwitcher<StateType>
{
    void StateSwitch<Tstate>() where Tstate : StateType;
}

public interface IService { }

public interface IState
{
    void Enter();
    void Exit();
}

public interface IGameState : IState { };