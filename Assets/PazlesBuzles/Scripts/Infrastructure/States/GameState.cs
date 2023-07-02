using System;
using System.Collections.Generic;
using UnityEngine;

internal class GameState : IGameState, IService
{
    public List<Sprite> PazzleSprites => _pazzleSprites;
    public Sprite OrigImage => _origImage;

    public PiecePlace SelectedPlace { get; set; }
    public bool PieceIsTaken { get; set; }

    private readonly GameStateMachine _stateMachine;
    private List<Sprite> _pazzleSprites;
    private Sprite _origImage;

    public GameState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        AllServices.RegisterService(this);
    }

    public void InitGameInfo(Sprite origImage, List<Sprite> pazzleSprites)
    {
        _origImage = origImage;
        _pazzleSprites = pazzleSprites;
    }

    public void Enter()
    {
        SoundManager.PlayAllMusicClips_();
    }

    public void Exit()
    {
    }
}
