using System;
using System.Collections.Generic;
using UnityEngine;

internal class GameState : IGameState
{
    private readonly GameStateMachine _stateMachine;
    private List<Sprite> _pazzleSprites;
    private Sprite _origImage;

    public List<Sprite> PazzleSprites => _pazzleSprites;
    public Sprite OrigImage => _origImage;

    public GameState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void InitGameInfo(Sprite origImage, List<Sprite> pazzleSprites)
    {
        _origImage = origImage;
        _pazzleSprites = pazzleSprites;
    }

    public void Enter()
    {

    }

    public void Exit()
    {
    }

    internal void InitGameInfo(object v)
    {
        throw new NotImplementedException();
    }
}
